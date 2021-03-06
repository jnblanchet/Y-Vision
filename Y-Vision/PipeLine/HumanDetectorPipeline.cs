﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Y_Vision.BlobDescriptor;
using Y_Vision.Configuration;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.Core;
using Y_Vision.GroundRemoval;
using Y_Vision.SensorStreams;
using Y_Vision.Tracking;
using Y_Vision.Triangulation;

namespace Y_Vision.PipeLine
{
    /// <summary>
    /// This class includes all the detectection pipeline for a signle sensor.
    /// </summary>
    public class HumanDetectorPipeline : IDisposable
    {
        // Pipeline
        private readonly KinectStreamMicrosoftApi _kinect;
        private readonly ConnectedComponentLabling _ccl;
        private BlobFilter _blobFilter;
        private readonly BranchAndBoundTracker _tracker;
        private readonly PlaneGroundRemover _groundRemover;
        private BlobFactory _blobFactory;
        public BlobFactory BlobFactory
        {
           private get { return _blobFactory; }
           set
            {
                _blobFactory = value;
                _blobFilter = new BlobFilter(_blobFactory, _kinect.Context);
            }
        }

        private HoGManager _hoGManager;

        // Pipieline Artifact
        public List<TrackedObject> DepthTrackedObjects { private set; get; }
        public List<TrackableObject> Blobs { private set; get; }
        public short[,] BlobIndex { private set; get; }
        public short[] RawDepth { private set; get; }
        public short[,] Depth2D { private set; get; }
        public int DepthW { private set; get; }
        public int DepthH { private set; get; }
        public byte[] RawColor { private set; get; }
        public int ColorW { private set; get; }
        public int ColorH { private set; get; }

        private readonly Stopwatch _sw;
        public long LastProcessTime { get; private set; }
        public long Fps { get { return 1000/Math.Max(LastProcessTime,1); } }

        private BackgroundWorker _depthProcessor;
        private BackgroundWorker _colorProcessor;

        public KinectSensorContext Context { get { return _kinect.Context;  } }
        public SensorConfig Config { private set; get; }

        /// <summary>
        /// Creates a pipeline that will use a sensor to trigger detection events
        /// </summary>
        /// <param name="config">The sensor configuration.</param>
        public HumanDetectorPipeline(SensorConfig config)
        {
            const int maxTrackingDistancePixelSystem = 20000;
            _sw = new Stopwatch();
            _ccl = new ConnectedComponentLabling();
            _tracker = new BranchAndBoundTracker(maxTrackingDistancePixelSystem);

            _kinect = new KinectStreamMicrosoftApi(config);
            _groundRemover = new PlaneGroundRemover(_kinect.Context, config);
            Config = config;

            _hoGManager = new HoGManager();
        }
        //_blobFactory = new BlobFactory(new CoordinateSystemConverter(_kinect.Context), _kinect.Context);

        public void Start()
        {
            if(BlobFactory == null)
                throw new ArgumentNullException("A blob factory must be specified before starting the detector.");

            // Multithread stream processing
            _depthProcessor = new BackgroundWorker();
            _colorProcessor = new BackgroundWorker();
            _depthProcessor.DoWork += DepthProcessorOnDoWork;
            _colorProcessor.DoWork += ColorProcessorOnDoWork;

            // Stream Hook
            _kinect.FramesReady += KinectOnFramesReady;

            _kinect.Start();
        }

        public void Stop()
        {
            _kinect.FramesReady -= KinectOnFramesReady;

            if (_depthProcessor != null)
                _depthProcessor.DoWork -= DepthProcessorOnDoWork;
            if (_colorProcessor != null)
                _colorProcessor.DoWork -= ColorProcessorOnDoWork;

            _kinect.Stop();
        }


        private void ColorProcessorOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var frames = (FramesReadyEventArgs) doWorkEventArgs.Argument;
            //_hoGManager.ComputeCells(frames.ColorArray, frames.ColorWidth, frames.ColorHeight); // TODO: use this (after proxy optimization)
        }

        private void DepthProcessorOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var frames = (FramesReadyEventArgs) doWorkEventArgs.Argument;

            if (frames.ColorArray == null || frames.DepthArray == null)
                return;

            RawDepth = frames.DepthArray; DepthW = frames.DepthWidth; DepthH = frames.DepthHeight;
            RawColor = frames.ColorArray; ColorW = frames.ColorWidth; ColorH = frames.ColorHeight;

            Depth2D = frames.DepthTo2DArray();

            //Remove ground
            _groundRemover.ApplyMask(Depth2D);

            // Region segmentation
            BlobIndex = _ccl.FindConnectedComponents(Depth2D);

            // Blob detection
            Blobs = _blobFilter.ToBlobObjects(_ccl.BlobInfo);

            // Blob tracking
            DepthTrackedObjects = _tracker.TrackObjects(Blobs);
        }

        private void KinectOnFramesReady(object sender, EventArgs args)
        {
            var frames = (FramesReadyEventArgs) args;

            if (!_colorProcessor.IsBusy && !_depthProcessor.IsBusy)
            {
                // FPS counter
                _sw.Stop();
                LastProcessTime = _sw.ElapsedMilliseconds;
                _sw.Reset();
                _sw.Start();

                // Last frame is completed by now: invoke event!
                DetectionUpdate.Invoke(this,new EventArgs());

                // Process new frame now!
                _depthProcessor.RunWorkerAsync(frames);
                _colorProcessor.RunWorkerAsync(frames);
            }
        }
        public event EventHandler<EventArgs> DetectionUpdate;

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// This method checks if the specified coordinates are in the sensor's range. If they are, an image depth point is returned. If not, null is returned.
        /// </summary>
        public Point3D? RatioPointInRange(double x, double y)
        {
            var sensorXCoords = (int) (x * DepthW + 0.5 - 1);
            var sensorYCoords = (int) (y * DepthH + 0.5 - 1);

            var value = RawDepth[sensorYCoords * DepthW + sensorXCoords];
            if (value > KinectSensorContext.MinDepthValue && value <= KinectSensorContext.MaxDepthValue)
                return new Point3D(sensorXCoords, sensorYCoords, value);

            return null;
        }
    }
}
