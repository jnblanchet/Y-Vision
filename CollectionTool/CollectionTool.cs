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
using Y_Visualization;
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
        private readonly HoGManager _colorHoGManager, _depthHoGManager;

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
            _writer.WriteLine("sampleId,Width,Height,Distance,Surface" + Enumerable.Range(1, HoGManager.BinCount).Aggregate("", (agg, next) => agg + ",RgbBin" + next) + Enumerable.Range(1, HoGManager.BinCount).Aggregate("", (agg, next) => agg + ",DepthBin" + next));

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
            _colorHoGManager = new HoGManager();
            _depthHoGManager = new HoGManager();

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
            _colorHoGManager.ComputeCells(_detector.RawColor, _detector.ColorW, _detector.ColorH);
            _depthHoGManager.ComputeCells(_detector.Depth2D, _detector.DepthW, _detector.DepthH);
            foreach(var obj in snapshot)
            {
                var hogArea = new Rectangle((int)(obj.MinX * depthToRgbRatio), (int)(obj.MinY * depthToRgbRatio), (int)((obj.MaxX - obj.MinX) * depthToRgbRatio), (int)((obj.MaxY - obj.MinY) * depthToRgbRatio));
                // 1 cell size px padding (as the HOG Manager will use)
                var x0 = (int) (Math.Max(obj.MinX * depthToRgbRatio - HoGManager.PixelsPerCell, 0));
                var y0 = (int) (Math.Max(obj.MinY * depthToRgbRatio - HoGManager.PixelsPerCell, 0));
                var cropArea = new Rectangle(x0, y0,
                    Math.Min((int)(obj.MaxX * depthToRgbRatio + HoGManager.PixelsPerCell * 2), _detector.ColorW) - x0,
                    Math.Min((int)(obj.MaxY * depthToRgbRatio + HoGManager.PixelsPerCell * 2), _detector.ColorH) - y0
                    );
                // Save Bitmap
                bmpCrop = _bmpCreator.ColorBitamp.Clone(cropArea, _bmpCreator.ColorBitamp.PixelFormat);
                bmpCrop.Save(_path + "\\" + id + ".bmp");
                // Compute RGB HOG
                var res = _colorHoGManager.GetHistogram(hogArea.Left, hogArea.Top, hogArea.Right, hogArea.Bottom);
                var rgbBins = res.Aggregate("", (agg, next) => next.ToString("r") + "," + agg); // r for "round-trip", meaning the output can be used as an input
                rgbBins = rgbBins.Substring(0, rgbBins.Length - 1);
                // Compute Depth HOG
                res = _depthHoGManager.GetHistogram(obj.MinX, obj.MinY, obj.MaxX, obj.MaxY);
                var depthBins = res.Aggregate("", (agg, next) => next.ToString("r") + "," + agg); // r for "round-trip", meaning the output can be used as an input
                depthBins = depthBins.Substring(0, depthBins.Length - 1);
                _writer.WriteLine(id + "," + obj.Width + "," + obj.Height + "," + obj.DistanceZ + "," + obj.Surface + "," + rgbBins + "," + depthBins);
                id++;

                //write debug
                //var r = new ArrayWriter(true);
                //r.ToTextFile(_hoGManager.CellMagnitude, id + ".log");
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
