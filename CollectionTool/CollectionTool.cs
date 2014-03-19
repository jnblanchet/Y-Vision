using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Y_Vision.BlobDescriptor;
using Y_Vision.Configuration;
using Y_Vision.DetectionAPI;
using Y_Vision.PipeLine;
using Y_Visualization.Drawing;

namespace CollectionTool
{
    public partial class MainForm : Form
    {
        private readonly HumanDetectorPipeline _detector;
        private readonly SensorConfig _config;
        private readonly BitmapCreator _bmpCreator;
        private readonly string _path;
        private readonly StreamWriter _writer;
        private readonly HoGManager _hoGManager;

        public MainForm()
        {
            // Setup global culture for float.toString
            var customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            // Prepare folder and CVS output file
            _path = Directory.CreateDirectory(DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss")).FullName;
            _writer = File.AppendText(_path + "\\blobs.txt");
            _writer.AutoFlush = true;
            _writer.WriteLine("sampleId,Width,Height,Distance,Surface" + Enumerable.Range(1, HoGManager.BinCount).Aggregate("", (agg, next) => agg + ",bin" + next));

            // Load config (and print error if config not found)
            var configManager = new ConfigurationManager();
            var configs = configManager.GetSensorId();
            if(configs.Length == 0)
            {
                MessageBox.Show("No valid config found. Use the calibration tool to create one.", "Error", MessageBoxButtons.OK);
                return;
            }
            // Hook up the kinect
            _config = configManager.GetConfigById(configs[0]);
            _detector = new HumanDetectorPipeline(_config) {BlobFactory =  new BlobFactory() };
            _bmpCreator = new BitmapCreator();
            _hoGManager = new HoGManager();

            _detector.DetectionUpdate += DetectorOnDetectionUpdate;
            InitializeComponent();
        }

        private int id = 0;
        private int _rateCount = 0;
        private void DetectorOnDetectionUpdate(object sender, EventArgs eventArgs)
        {
            // Sampling rate (as specified by the user)
            if (++_rateCount <= sampleRate.Value)
                return;
            _rateCount = 0;

            if(_detector.RawDepth == null) // the first frame is sometimes null (kinect = glitchy)
                return;
            
            //var depthSnapShot = (short[])_detector.RawDepth.Clone();
            var snapshot = _detector.Blobs.ToArray(); // prevents changes by another thread (problematic when using foreach)

            //create bmp
            Bitmap bmpCrop = null; // for display
            _bmpCreator.CreateBitmapFromColorFrame(_detector.RawColor,_detector.ColorH, _detector.ColorW);
            double depthToRgbRatio = (double)_detector.ColorW/_detector.DepthW;

            // compute HoG
            _hoGManager.ComputeCells(_detector.RawColor, _detector.ColorW, _detector.ColorH);
            foreach(var obj in snapshot)
            {
                var cropArea = new Rectangle((int)(obj.MinX * depthToRgbRatio), (int)(obj.MinY * depthToRgbRatio), (int)((obj.MaxX - obj.MinX) * depthToRgbRatio), (int)((obj.MaxY - obj.MinY) * depthToRgbRatio));
                bmpCrop = _bmpCreator.ColorBitamp.Clone(cropArea, _bmpCreator.ColorBitamp.PixelFormat);
                bmpCrop.Save(_path + "\\" + id + ".bmp");
                var res = _hoGManager.GetHistogram(cropArea.Left, cropArea.Top, cropArea.Right, cropArea.Bottom);
                var bins = res.Aggregate("", (agg, next) => next + "," + agg);
                _writer.WriteLine(id + "," + obj.Width + "," + obj.Height + "," + obj.DistanceZ + "," + obj.Surface + "," + bins);
                id++;
            }

            if (bmpCrop!= null)
                OutputPictureBox.Image = bmpCrop;
        }

        private void ToggleStartStopButtonClick(object sender, EventArgs e)
        {
            if(toggleStartStopButton.Text == "Start")
            {
                //start
                _detector.Start();
                toggleStartStopButton.Text = "Stop";
            }
            else
            {
                //stop
                _detector.Stop();
                toggleStartStopButton.Text = "Start";
            }
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            _detector.Stop();
        }
    }
}
