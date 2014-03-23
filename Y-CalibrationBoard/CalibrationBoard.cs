﻿using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Kinect.Toolkit;
using Y_Vision.BlobDescriptor;
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

        private double _smoothedProcessingTime = 0;
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
                _detector.BlobFactory = new BlobFactory { Context = _detector.Context, Conv = new CoordinateSystemConverter(_detector.Context) };

                _detector.DetectionUpdate += (o, args) =>
                                                 {
                                                     FpsLabel.Text = String.Format("{0} FPS", _detector.Fps);
                                                     _smoothedProcessingTime = (_smoothedProcessingTime*5 + _detector.LastProcessTime)/6;
                                                     MsLabel.Text = String.Format("{0} ms", (int)_smoothedProcessingTime);
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
        private CoordinateSystemConverter _convL;
        private CoordinateSystemConverter _convR;

        /// <summary>
        /// Initiated the 2d and 3d tab to handle point selection in the 2nd tab,
        /// to build the config, and to draw the scene in the 3d tab
        /// </summary>
        private void InitCalibrationTab()
        {
            userSceneScale.SelectedIndex = 0;
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
                                                               UpdateParallaxConfig(_parallaxLeft, args.X, args.Y, comboBoxCalibrationLeft.Text, _convL, _parallaxLeftBmp.DepthBitamp.Width, _parallaxLeftBmp.DepthBitamp.Height);
                                                                break;
                                                            case 2:
                                                                UpdateParallaxConfig(_parallaxRight, args.X, args.Y, comboBoxCalibrationRight.Text, _convR, _parallaxRightBmp.DepthBitamp.Width, _parallaxRightBmp.DepthBitamp.Height);
                                                                break;
                                                       }
                                                       DrawScene();
                                                   };

            LoadNumericparams();
        }

        /// <summary>
        /// Uses onscreen point and the pipeline to build the parallax configuration for the given sensor.
        /// </summary>
        private void UpdateParallaxConfig(HumanDetectorPipeline pipeline, float x, float y, string sensorId, CoordinateSystemConverter c,int h, int w)
        {
            var normalizedPoint = pipeline.RatioPointInRange(x, y);
            if (normalizedPoint.HasValue)
            {
                var pointMm = c.ToXyz(normalizedPoint.Value.X, normalizedPoint.Value.Y, normalizedPoint.Value.Z, h, w);
                _configs.ParallaxConfig.AddPoint(sensorId, pointMm, new Y_Vision.Core.Point((int)normalizedPoint.Value.X, (int)normalizedPoint.Value.Y));
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
                _parallaxLeft.BlobFactory = new BlobFactory { Context = _parallaxLeft.Context, Conv = new CoordinateSystemConverter(_parallaxLeft.Context) };
                _parallaxLeft.DetectionUpdate += (o, args) =>
                        {
                            _parallaxLeftBmp.CreateBitmapFromDepthFrame(_parallaxLeft.RawDepth,_parallaxLeft.DepthH,_parallaxLeft.DepthW);
                            _parallaxLeftBmp.DrawPointsWithUniqueColor(_parallaxLeftBmp.DepthBitamp, _configs.ParallaxConfig.Get2DPoints(conf.SensorId));
                            ParallaxContainer.DisplayFrames(_parallaxLeftBmp.DepthBitamp, null);
                            if(_averageModeLeft > 0)
                            {
                                _configs.ParallaxConfig.SmoothAllPoints(conf.SensorId, _parallaxLeft.Depth2D, FrameCountAverageMode - _averageModeLeft, 1, _convL);
                                _averageModeLeft--;
                            }
                                
                        };
                _convL = new CoordinateSystemConverter(_parallaxLeft.Context);
                _parallaxLeft.Start();
            }
            else if (sender == comboBoxCalibrationRight)
            {
                if (_parallaxRight != null)
                    _parallaxRight.Stop();
                var conf = _configs.GetConfigById(comboBoxCalibrationRight.Text);
                _parallaxRight = new HumanDetectorPipeline(conf);
                _parallaxRight.BlobFactory = new BlobFactory { Context = _parallaxRight.Context, Conv = new CoordinateSystemConverter(_parallaxRight.Context) };
                _parallaxRight.DetectionUpdate += (o, args) =>
                        {
                            _parallaxRightBmp.CreateBitmapFromDepthFrame(_parallaxRight.RawDepth,_parallaxRight.DepthH,_parallaxRight.DepthW);
                            _parallaxRightBmp.DrawPointsWithUniqueColor(_parallaxRightBmp.DepthBitamp, _configs.ParallaxConfig.Get2DPoints(conf.SensorId));
                            ParallaxContainer.DisplayFrames(null, _parallaxRightBmp.DepthBitamp);
                            DrawTrackedPeople();
                            if (_averageModeRight > 0)
                            {
                                _configs.ParallaxConfig.SmoothAllPoints(conf.SensorId, _parallaxRight.Depth2D, FrameCountAverageMode - _averageModeRight, 1, _convR);
                                _averageModeRight--;
                                if (_averageModeRight == 0)
                                    DrawScene();
                            }
                        };
                _convR = new CoordinateSystemConverter(_parallaxLeft.Context);
                _parallaxRight.Start();
            }
            DrawScene(); // Will not work if enough points aren't available
        }
        #endregion

        #region temporary drawing code

        private Bitmap _baseBitmap;
        private readonly SceneVisualizer _sceneDrawer = new SceneVisualizer();
        private MappingTool _mappingTool;
        private void DrawScene()
        {
            var tool = new TriangulationTool(2);

            // Retrieve the points and validate there are enough
            var pointsLeft = _configs.ParallaxConfig.Get3DPoints(comboBoxCalibrationLeft.Text);
            var pointsRight = _configs.ParallaxConfig.Get3DPoints(comboBoxCalibrationRight.Text);

            if (pointsLeft == null || pointsRight == null)
                return;

            var arrPointsLeft = pointsLeft.ToArray();
            var arrPointsRight = pointsRight.ToArray();

            if (arrPointsLeft.Length < 3 || arrPointsRight.Length < 3 || arrPointsLeft.Length != arrPointsRight.Length)
                return;

            // Add the points for triangulation (TODO: try catch and explain to pick other points)
            foreach (var p in arrPointsLeft)
                tool.AddTriangulationPoint(0, p.X, p.Y, p.Z);
            foreach (var p in arrPointsRight)
                tool.AddTriangulationPoint(1, p.X, p.Y, p.Z);

            //TODO: add try catch for invalid points
            try
            {
                // Retrieve the angle for drawing
                tool.GetSensorAngle(0);
                tool.GetSensorAngle(1);

                _baseBitmap = _sceneDrawer.DrawScene(SetupPictureBox.Width, SetupPictureBox.Height,
                                                    new Point((int) tool.GetSensorPosX(0), (int) -tool.GetSensorPosZ(0)),
                                                    new Point((int) tool.GetSensorPosX(1), (int) -tool.GetSensorPosZ(1)),
                                                    -90 - _parallaxLeft.Context.HorizontalFieldOfViewDeg / 2 + tool.GetSensorAngle(0),
                                                    -90 - _parallaxRight.Context.HorizontalFieldOfViewDeg / 2 - tool.GetSensorAngle(1),
                                                    4095,
                                                    (float) _parallaxLeft.Context.HorizontalFieldOfViewDeg,
                                                    (float) _parallaxRight.Context.HorizontalFieldOfViewDeg,
                                                    (float)(1 / Math.Pow(2,int.Parse(userSceneScale.Text)-1)));

                _mappingTool = new MappingTool(tool);

                DrawTrackedPeople();
            }
            catch (Exception)
            {
                StopAverage();
                MessageBox.Show("The selected points are considered bad, try again with better points.", "Bad Points", MessageBoxButtons.OK);
                _configs.ParallaxConfig.ClearAll();
            }
        }

        private void DrawTrackedPeople()
        {
            // Also draw tracked people
            if (_baseBitmap != null && _parallaxRight.DepthTrackedObjects != null)
            {
                var bmp = (Bitmap) _baseBitmap.Clone();

                // Draw Left sensor tracked objects
                foreach (var obj in _parallaxLeft.DepthTrackedObjects)
                {
                    //var p = _convL.ToXyz(obj.X, obj.Y, obj.Z, _parallaxLeftBmp.DepthBitamp.Width, _parallaxLeftBmp.DepthBitamp.Height);
                    var p = new Point3D(obj.X, obj.Y, obj.Z);
                    var normalizedP = _mappingTool.GetNormalizedCoordinates(0,p);
                    if (normalizedP.HasValue)
                    {
                        var drawableP = _sceneDrawer.ConvertToLastKnownScale(new Point((int)normalizedP.Value.X, -(int)normalizedP.Value.Z));
                        _bmpCreator.DrawPoint(bmp, new Y_Vision.Core.Point(drawableP.X, drawableP.Y), 1.0, 1, 6);                        
                    }
                }

                var cL = _configs.ParallaxConfig.Get3DPoints(comboBoxCalibrationLeft.Text);
                if (cL == null) return;
                // Draw Left sensor tracking points
                foreach (var p in cL)
                {
                    var normalizedP = _mappingTool.GetNormalizedCoordinates(0, p);
                    if (normalizedP.HasValue)
                    {
                        var drawableP = _sceneDrawer.ConvertToLastKnownScale(new Point((int)normalizedP.Value.X, -(int)normalizedP.Value.Z));
                        _bmpCreator.DrawPoint(bmp, new Y_Vision.Core.Point(drawableP.X, drawableP.Y), 1.0, 1, 2);
                    }
                }

                // Draw Right sensor tracked objects
                foreach (var obj in _parallaxRight.DepthTrackedObjects)
                {
                    //var p = _convR.ToXyz(obj.X, obj.Y, obj.Z, _parallaxRightBmp.DepthBitamp.Width, _parallaxRightBmp.DepthBitamp.Height);
                    var p = new Point3D(obj.X,obj.Y,obj.Z);
                    var normalizedP = _mappingTool.GetNormalizedCoordinates(1, p);
                    if (normalizedP.HasValue)
                    {
                        var drawableP = _sceneDrawer.ConvertToLastKnownScale(new Point((int)normalizedP.Value.X, -(int)normalizedP.Value.Z));
                        _bmpCreator.DrawPoint(bmp, new Y_Vision.Core.Point(drawableP.X, drawableP.Y), 1.0, 2, 6);
                    }
                }

                var cR = _configs.ParallaxConfig.Get3DPoints(comboBoxCalibrationRight.Text);
                if (cR == null) return;
                // Draw right sensor tracking points
                foreach (var p in cR)
                {
                    var normalizedP = _mappingTool.GetNormalizedCoordinates(1, p);
                    if (normalizedP.HasValue)
                    {
                        var drawableP = _sceneDrawer.ConvertToLastKnownScale(new Point((int)normalizedP.Value.X, -(int)normalizedP.Value.Z));
                        _bmpCreator.DrawPoint(bmp, new Y_Vision.Core.Point(drawableP.X, drawableP.Y), 1.0, 2, 2);
                    }
                }


                SetupPictureBox.Image = bmp;
            }
        }
        #endregion

        private void UserSceneScaleSelectedIndexChanged(object sender, EventArgs e)
        {
            DrawScene();
        }

        private int _averageModeLeft, _averageModeRight;
        private const int FrameCountAverageMode = 30;
        private void AverageButtonClick(object sender, EventArgs e)
        {
            _averageModeLeft = FrameCountAverageMode;
            _averageModeRight = FrameCountAverageMode;
        }
        private void StopAverage()
        {
            _averageModeLeft = 0;
            _averageModeRight = 0;
        }

        private void PrintPointsButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(_configs.ParallaxConfig.ToString(), "Parallax configuration", MessageBoxButtons.OK);
        }

        private void NumericUpDownValueChanged(object sender, EventArgs e)
        {
            switch (((NumericUpDown)sender).Name.Last())
            {
                case 'A': _configs.ParallaxConfig.LeftPadding = (int)numericUpDownA.Value; break;
                case 'B': _configs.ParallaxConfig.DisplayWidth = (int) numericUpDownB.Value; break;
                case 'C': _configs.ParallaxConfig.RightPadding = (int) numericUpDownC.Value; break;
                case 'D': _configs.ParallaxConfig.DisplayHeight = (int) numericUpDownD.Value; break;
                case 'E': _configs.ParallaxConfig.DisplayDistanceFromGround = (int) numericUpDownE.Value; break;
            }
        }

        private void LoadNumericparams()
        {
            numericUpDownA.Value = _configs.ParallaxConfig.LeftPadding;
            numericUpDownB.Value = _configs.ParallaxConfig.DisplayWidth;
            numericUpDownC.Value = _configs.ParallaxConfig.RightPadding;
            numericUpDownD.Value = _configs.ParallaxConfig.DisplayHeight;
            numericUpDownE.Value = _configs.ParallaxConfig.DisplayDistanceFromGround;
        }
    }
}
