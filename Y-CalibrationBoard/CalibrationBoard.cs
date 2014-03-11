using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect.Toolkit;
using Y_Vision.Configuration;
using Y_Vision.Core;
using Y_Vision.GroundRemoval;
using Y_Vision.PipeLine;
using Y_Vision.SensorStreams;
using Y_Vision.Triangulation;
using Y_Visualization.Drawing;
using Point = System.Drawing.Point;

namespace Y_CalibrationBoard
{
    public partial class CalibrationBoard : Form
    {
        private HumanDetectorPipeline _detector;
        private readonly BitmapCreator _bmpCreator;
        private KinectSensorChooser.Id[] _sensors;
        private readonly ConfigurationManager _configs;
        private SensorConfig _currentConfig;

        // UI related elements
        private DepthModes _depthMode = DepthModes.GrayScale;
        private enum DepthModes
        {
            GrayScale = 1, BlobIndex = 2
        }
        private enum DetectionDisplayModes
        {
            None = 0, Blobs = 1, TrackedObject = 2
        }

        public CalibrationBoard()
        {
            _bmpCreator = new BitmapCreator();

            _configs = new ConfigurationManager();

            InitializeComponent();

            setupRgbdViewer.PointSelected += delegate(object sender, RgbdViewer.PointClickEventArgs pointClickEventArgs)
                                                 {
                                                     var p = _detector.RatioPointInRange(pointClickEventArgs.X, pointClickEventArgs.Y);
                                                     if(p.HasValue)
                                                        _currentConfig.AddGroundPoint(p.Value);
                                                 };
            InitCalibrationTab();
        }

        private void CalibrationBoardLoad(object sender, EventArgs e)
        {
            LoadSensors();
        }

        private void LoadSensors()
        {
            _sensors = KinectStreamMicrosoftApi.SensorIdList;
            cbSensorId.Items.AddRange(_sensors.Select(p => (object)p.ConnectionId).ToArray());
        }

        private void CbSensorIdSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSensorId.SelectedIndex != -1)
            {
                if (_detector != null)
                    _detector.Stop();

                _currentConfig = _configs.GetConfigById(cbSensorId.Text);
                GroundThresholdTextBox.Text = _currentConfig.GroundRemovalDistanceThreshold.ToString(CultureInfo.InvariantCulture);
                GroundThresholdTextBox.Enabled = true;

                _detector = new HumanDetectorPipeline(_currentConfig);
                _detector.DetectionUpdate += (o, args) =>
                                                 {
                                                     toolStripFpsLabel.Text = String.Format("{0} FPS", _detector.Fps);
                                                     // Generate bitmaps and display!
                                                     switch (_depthMode)
                                                     {
                                                         case DepthModes.BlobIndex:
                                                             _bmpCreator.CreateBitmapFromIndex(_detector.BlobIndex);
                                                             break;
                                                         case DepthModes.GrayScale:
                                                             _bmpCreator.CreateBitmapFromDepthFrame(_detector.RawDepth, _detector.DepthH, _detector.DepthW);
                                                             break;
                                                     }
                                                     _bmpCreator.CreateBitmapFromColorFrame(_detector.RawColor, _detector.ColorH, _detector.ColorW);
                                                     // Add tracking information
                                                     switch ((DetectionDisplayModes)toolStripComboBoxTracking.SelectedIndex)
                                                     {
                                                        case DetectionDisplayModes.None:
                                                            break;
                                                        case DetectionDisplayModes.Blobs:
                                                            _bmpCreator.DrawTrackedPeople(_bmpCreator.DepthBitamp, _detector.Blobs);
                                                            _bmpCreator.DrawTrackedPeople(_bmpCreator.ColorBitamp, _detector.Blobs, (double)_detector.ColorW / _detector.DepthW);
                                                            break;
                                                        case DetectionDisplayModes.TrackedObject:
                                                            _bmpCreator.DrawTrackedPeople(_bmpCreator.DepthBitamp, _detector.DepthTrackedObjects);
                                                            _bmpCreator.DrawTrackedPeople(_bmpCreator.ColorBitamp, _detector.DepthTrackedObjects, (double)_detector.ColorW / _detector.DepthW);
                                                            break;
                                                     }
                                                     setupRgbdViewer.DisplayFrames(_bmpCreator.DepthBitamp, _bmpCreator.ColorBitamp);
                                                 };
                _detector.Start();
            }
        }


        private void CalibrationBoardFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_detector != null)
            {
                _detector.Stop();
            }
        }

        private void RotatetoolStripButtonClick(object sender, EventArgs e)
        {
            if (_currentConfig != null)
            {
                _currentConfig.RotateSensor();
            }
        }

        private void SavetoolStripButtonClick(object sender, EventArgs e)
        {
            try
            {
                _configs.SaveConfig();
                MessageBox.Show("Config file saved.", "Success", MessageBoxButtons.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error, failed to save config file: " + ex.GetBaseException() + "\n" + ex.Message, "Fail", MessageBoxButtons.OK);
            }                
        }

        private void SelectionModeClick(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in toolStripDropDownButtonMode.DropDownItems)
            {
                item.Checked = false;
            }
            
            var selected = (ToolStripMenuItem)sender;
            selected.Checked = true;

            _depthMode = (DepthModes) int.Parse(selected.Tag.ToString());
        }

        // textbox for customizing the ground threshold
        private void GroundThresholdTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            if(_configs != null)
            {
                if (String.IsNullOrEmpty(GroundThresholdTextBox.Text))
                    GroundThresholdTextBox.Text = "0";
                int threshold = int.Parse(GroundThresholdTextBox.Text);
                _currentConfig.GroundRemovalDistanceThreshold = threshold;
                GroundThresholdTextBox.Text = _currentConfig.GroundRemovalDistanceThreshold.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void TabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            // First tab not selected!
            if (tabControl.SelectedTab != SensorSetupTab)
            {
                // Stop streams on first tab
                if (_detector != null) _detector.Stop();
                // Start streams on second and third tab
                if (_parallaxLeft != null) _parallaxLeft.Start();
                if (_parallaxRight != null) _parallaxRight.Start();
            }
            else
            {
                // Stop streams on second and third tab
                if (_parallaxLeft != null) _parallaxLeft.Stop();
                if (_parallaxRight != null) _parallaxRight.Stop();
                // Start streams on first tab
                if (_detector != null) _detector.Start();
            }
        }

        # region 2nd and 3rd tab

        private HumanDetectorPipeline _parallaxLeft, _parallaxRight;
        private BitmapCreator _parallaxLeftBmp, _parallaxRightBmp;

        /// <summary>
        /// Initiated the 2d and 3d tab to handle point selection in the 2nd tab,
        /// to build the config, and to draw the scene in the 3d tab
        /// </summary>
        private void InitCalibrationTab()
        {            
            var sensorList = KinectStreamMicrosoftApi.SensorIdList.Select(p => (object)p.ConnectionId).ToArray();
            comboBoxCalibrationLeft.Items.AddRange(sensorList);
            comboBoxCalibrationRight.Items.AddRange(sensorList);

            _parallaxLeftBmp = new BitmapCreator();
            _parallaxRightBmp = new BitmapCreator();

            ParallaxContainer.PointSelected += (sender, args) =>
                                                   {
                                                       switch (args.SourceId)
                                                       {
                                                            case 1:
                                                               UpdateParallaxConfig(_parallaxLeft, args.X, args.Y, comboBoxCalibrationLeft.Text);
                                                                break;
                                                            case 2:
                                                                UpdateParallaxConfig(_parallaxRight, args.X, args.Y, comboBoxCalibrationRight.Text);
                                                                break;
                                                       }
                                                       DrawScene();
                                                   };
        }

        /// <summary>
        /// Uses onscreen point and the pipeline to build the parallax configuration for the given sensor.
        /// </summary>
        private void UpdateParallaxConfig(HumanDetectorPipeline pipeline, float x, float y, string sensorId)
        {
            var normalizedPoint = pipeline.RatioPointInRange(x, y);
            if (normalizedPoint.HasValue)
            {
                //var cfg = _configs.GetConfigById(sensor);
                //cfg.ReferencePoint = normalizedPoint.Value;
                _configs.ParallaxConfig.AddPoint(sensorId, normalizedPoint.Value, new Y_Vision.Core.Point((int)normalizedPoint.Value.X, (int)normalizedPoint.Value.Y));
            }
        }

        /// <summary>
        /// Starts the selected sensor and binds the events properly to allow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxCalibrationSelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(((ComboBox)sender).Text))
                return;

            if (sender == comboBoxCalibrationLeft)
            {
                if(_parallaxLeft != null)
                    _parallaxLeft.Stop();
                var conf = _configs.GetConfigById(comboBoxCalibrationLeft.Text);
                _parallaxLeft = new HumanDetectorPipeline(conf);
                _parallaxLeft.DetectionUpdate += (o, args) =>
                        {
                            _parallaxLeftBmp.CreateBitmapFromDepthFrame(_parallaxLeft.RawDepth,_parallaxLeft.DepthH,_parallaxLeft.DepthW);
                            _parallaxLeftBmp.DrawPointsWithUniqueColor(_parallaxLeftBmp.DepthBitamp, _configs.ParallaxConfig.Get2DPoints(conf.SensorId));
                            ParallaxContainer.DisplayFrames(_parallaxLeftBmp.DepthBitamp, null);
                        };
                _parallaxLeft.Start();
            }
            else if (sender == comboBoxCalibrationRight)
            {
                if (_parallaxRight != null)
                    _parallaxRight.Stop();
                var conf = _configs.GetConfigById(comboBoxCalibrationRight.Text);
                _parallaxRight = new HumanDetectorPipeline(conf);
                _parallaxRight.DetectionUpdate += (o, args) =>
                        {
                            _parallaxRightBmp.CreateBitmapFromDepthFrame(_parallaxRight.RawDepth,_parallaxRight.DepthH,_parallaxRight.DepthW);
                            _parallaxRightBmp.DrawPointsWithUniqueColor(_parallaxRightBmp.DepthBitamp, _configs.ParallaxConfig.Get2DPoints(conf.SensorId));
                            ParallaxContainer.DisplayFrames(null, _parallaxRightBmp.DepthBitamp);
                        };
                _parallaxRight.Start();
            }
        }
        #endregion

        #region temporary drawing code

        readonly SceneVisualizer _sceneDrawer = new SceneVisualizer();
        private void DrawScene()
        {
            var tool = new TriangulationTool(2);

            var pointsLeft = _configs.ParallaxConfig.Get3DPoints(comboBoxCalibrationLeft.Text).ToArray();
            var pointsRight = _configs.ParallaxConfig.Get3DPoints(comboBoxCalibrationRight.Text).ToArray();

            if (pointsLeft.Length < 3 || pointsRight.Length < 3)
                return;

            foreach (var p in pointsLeft)
                tool.AddTriangulationPoint(0, p.X, p.Y, p.Z);
            foreach (var p in pointsRight)
                tool.AddTriangulationPoint(1, p.X, p.Y, p.Z);

            tool.GetSensorAngle(0);
            tool.GetSensorAngle(1);

            SetupPictureBox.Image = _sceneDrawer.drawScene(SetupPictureBox.Width, SetupPictureBox.Height,
                                            new Point((int)tool.GetSensorPosX(0), (int)tool.GetSensorPosZ(0)),
                                            new Point((int)tool.GetSensorPosX(1), (int)tool.GetSensorPosZ(1)),
                                            tool.GetSensorAngle(0), tool.GetSensorAngle(1),
                                            4095);

            /*var conf1 = _configs.GetConfigById(comboBoxCalibrationLeft.Text);
            var conf2 = _configs.GetConfigById(comboBoxCalibrationRight.Text);
            var p1 = conf1.ReferencePoint;
            var p2 = conf2.ReferencePoint;

            if ((p1.X > 0 || p1.Y > 0) && (p2.X > 0 || p2.Y > 0))
            {

                var convL = new CoordinateSystemConverter(_parallaxLeft.Context);
                var convR = new CoordinateSystemConverter(_parallaxRight.Context);

                var pointNorm1 = convL.ToXyz(p1.X, p1.Y, p1.Z, _parallaxLeftBmp.DepthBitamp.Width,
                                             _parallaxLeftBmp.DepthBitamp.Height);
                var pointNorm2 = convR.ToXyz(p2.X, p2.Y, p2.Z, _parallaxRightBmp.DepthBitamp.Width,
                                             _parallaxRightBmp.DepthBitamp.Height);

                pointNorm1 *= -1;
                pointNorm2 *= -1;

                SetupPictureBox.Image = _sceneDrawer.drawScene(SetupPictureBox.Width, SetupPictureBox.Height,
                                                               new Point((int) pointNorm1.X, (int) pointNorm1.Z),
                                                               new Point((int)pointNorm2.X, (int)pointNorm2.Z), (int) conf1.ReferencePoint2D.X, (int) conf2.ReferencePoint2D.X,
                                                               4095);
            }*/
        }
        #endregion
    }
}
