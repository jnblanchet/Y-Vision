using System;
using System.ComponentModel;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Y_Vision.Configuration;

namespace Y_Vision.SensorStreams
{
    /// <summary>
    /// This class extracts the various streams from a Kinect stream.
    /// </summary>
    public class KinectStreamMicrosoftApi : IKinectStream
    {
        private readonly KinectSensorChooser _chooser;

        // The frames are stored as primitive types
        private short[] _originalDepth, _depth;
        private int _depthWidth, _depthHeight;
        private byte[] _originalColor, _color;
        private int _colorWidth, _colorHeight;

        private readonly BackgroundWorker _depthProcessor;
        private readonly BackgroundWorker _colorProcessor;
        private AllFramesReadyEventArgs _bgThreadArg;

        private readonly SensorConfig _config;

        public const int ColorStreamBytesPerPixel = 4;

        public KinectSensorContext Context { get; private set; }

        // Old constructor, still used by the debug tool
        /*public KinectStreamMicrosoftApi(string uniqueId  = null, DiscreteRotation rotation = null)
        {
            _colorProcessor = new BackgroundWorker();
            _depthProcessor = new BackgroundWorker();
            _depthProcessor.DoWork += (o, args) => SensorDepthFrameReady(_bgThreadArg);
            _colorProcessor.DoWork += (o, args) => SensorColorFrameReady(_bgThreadArg);

            _config = new SensorConfig(uniqueId);
            if (rotation != null)
                _config.Rotation = rotation;
            _chooser = new KinectSensorChooser();
            _chooser.KinectChanged += ChooserSensorChanged;

            Context = rotation == null ? new KinectSensorContext(false) : new KinectSensorContext(!rotation.InvertHeightWidth);
        }*/

        public KinectStreamMicrosoftApi(SensorConfig config)
        {
            _config = config;

            _colorProcessor = new BackgroundWorker();
            _depthProcessor = new BackgroundWorker();
            _depthProcessor.DoWork += (o, args) => SensorDepthFrameReady(_bgThreadArg);
            _colorProcessor.DoWork += (o, args) => SensorColorFrameReady(_bgThreadArg);

            _chooser = new KinectSensorChooser();
            _chooser.KinectChanged += ChooserSensorChanged;

            Context = new KinectSensorContext(config.Rotation.InvertHeightWidth);
        }

        public static KinectSensorChooser.Id[] SensorIdList
        {
            get { return KinectSensorChooser.SensorIdList; }
        }

        private void ChooserSensorChanged(object sender, KinectChangedEventArgs e)
        {
            Stop(e.OldSensor);
            var newsensor = e.NewSensor;
            if (newsensor == null)
            {
                return;
            }


            newsensor.SkeletonStream.Disable();
            newsensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30); // TODO: this should be a config preset obtained in the constructor (like depth)
            newsensor.DepthStream.Enable(Context.DepthConfig);
            newsensor.AllFramesReady += SensorAllFramesReady;
            try
            {
                _chooser.Start();
                //newsensor.Start();
            }
            catch (System.IO.IOException)
            {
                // maybe another app is using Kinect
                _chooser.TryResolveConflict();
            }
        }

        private void SensorAllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (!_colorProcessor.IsBusy && !_depthProcessor.IsBusy)
            {
                // Last frame is completed by now: invoke event!
                if (_depth != null && _color != null && FramesReady != null) // The first frame may be null
                    FramesReady.Invoke(this, new FramesReadyEventArgs(_depth, _depthHeight, _depthWidth, _color, _colorHeight, _colorWidth));

                // Process new frame now!
                _bgThreadArg = e;
                _depthProcessor.RunWorkerAsync();
                _colorProcessor.RunWorkerAsync();
            }
        }

        private DiscreteRotation _currentDepthRotation;
        private void SensorDepthFrameReady(AllFramesReadyEventArgs e)
        {
            using (var frame = e.OpenDepthImageFrame())
            {
                // The frame may be null when the kinect is shutting down
                if (frame != null)
                {
                    // Create array for the first frame or when the resolution changes
                    if (_depth == null || _depth.Length != frame.PixelDataLength || _config.Rotation != _currentDepthRotation)
                    {
                        Context.RotateFieldOfView(!_config.Rotation.InvertHeightWidth);
                        _currentDepthRotation = _config.Rotation;
                        if (_currentDepthRotation.InvertHeightWidth)
                        {
                            _depthWidth = frame.Height;
                            _depthHeight = frame.Width;
                        }
                        else
                        {
                            _depthWidth = frame.Width;
                            _depthHeight = frame.Height;
                        }

                        _originalDepth = new short[frame.PixelDataLength];
                        _depth = new short[frame.PixelDataLength];
                    }

                    //Copy the depth frame data onto the bitmap 
                    frame.CopyPixelDataTo(_originalDepth);
                    // Format data (shift + rotation)

                    int k = 0;
                    for (int j = 0; j < frame.Height; j++)
                        for (int i = 0; i < frame.Width; i++,k++)
                        {
                            var rotatedPoint = _currentDepthRotation.Rotate(i, j, _depthWidth - 1, _depthHeight - 1);
                            _depth[(rotatedPoint.X) + (rotatedPoint.Y) * _depthWidth] = _originalDepth[k] >>= 3;
                        }
                }
            }
        }

        private DiscreteRotation _currentColorRotation;

        private void SensorColorFrameReady(AllFramesReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
                // The frame may be null when the kinect is shutting down
                if (frame != null)
                {
                    // Create array for the first frame or when the resolution changes
                    if (_color == null || _color.Length != frame.PixelDataLength || _config.Rotation != _currentColorRotation)
                    {
                        _currentColorRotation = _config.Rotation;
                        if (_currentColorRotation.InvertHeightWidth)
                        {
                            _colorWidth = frame.Height;
                            _colorHeight = frame.Width;
                        }
                        else
                        {
                            _colorWidth = frame.Width;
                            _colorHeight = frame.Height;
                        }

                        _originalColor = new byte[frame.PixelDataLength];
                        _color = new byte[frame.PixelDataLength];
                    }
                    
                    //Copy the depth frame data onto the bitmap 
                    frame.CopyPixelDataTo(_originalColor);

                    int k = 0;
                    for (int j = 0; j < frame.Height; j++)
                        for (int i = 0; i < frame.Width; i++, k++)
                        {
                            var rotatedPoint = _currentColorRotation.Rotate(i, j, _colorWidth - 1, _colorHeight - 1);
                            var indexByteDest = ((rotatedPoint.X) + (rotatedPoint.Y) * _colorWidth) * ColorStreamBytesPerPixel;
                            var indexByteSource = k * ColorStreamBytesPerPixel;
                            _color[indexByteDest] = _originalColor[indexByteSource];
                            _color[indexByteDest + 1] = _originalColor[indexByteSource + 1];
                            _color[indexByteDest + 2] = _originalColor[indexByteSource + 2];
                            _color[indexByteDest + 3] = _originalColor[indexByteSource + 3];
                        }
                }
            }
        }

        public void Start()
        {
            if (!string.IsNullOrEmpty(_config.SensorId))
                _chooser.TryStartKinectWithId(_config.SensorId);
            else
            _chooser.Start();
        }

        public void Stop()
        {
            Stop(_chooser.Kinect);
        }

        private void Stop(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    sensor.AllFramesReady -= SensorAllFramesReady;
                    _chooser.Stop();
                    // not necessary, the sensor chooser whill handle this
                    //sensor.Stop();
                    //sensor.AudioSource.Stop();
                }
            }
        }

        public event EventHandler<EventArgs> FramesReady;
    }
}
