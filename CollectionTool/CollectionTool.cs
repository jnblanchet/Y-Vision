using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
        public MainForm()
        {
            var configs = new ConfigurationManager();
            _config = configs.GetConfigById(null);
            _detector = new HumanDetectorPipeline(_config);
            _bmpCreator = new BitmapCreator();

            _detector.DetectionUpdate += DetectorOnDetectionUpdate;
            InitializeComponent();
        }

        private void DetectorOnDetectionUpdate(object sender, EventArgs eventArgs)
        {
            var depthSnapShot = (short[])_detector.RawDepth.Clone();
            var snapshot = _detector.DepthTrackedObjects.ToArray(); // prevents changes by another thread (problematic when using foreach)

            //crate bmp
            _bmpCreator.CreateBitmapFromColorFrame(_detector.RawColor,_detector.ColorH, _detector.ColorW);
            foreach(var obj in snapshot)
            {
                //obj.
                //Bitmap bmpCrop = _bmpCreator.ColorBitamp.Clone(cropArea, bmpImage.PixelFormat);
                //return (Image)(bmpCrop);
            }
            
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
    }
}
