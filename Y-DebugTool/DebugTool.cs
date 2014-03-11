using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Y_Vision.Configuration;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.GroundRemoval;
using Y_Vision.SensorStreams;
using Y_Vision.BlobDescriptor;
using Y_Vision.Tracking;
using Y_Visualization.Drawing;

namespace Y_DebugTool
{
    /// <summary>
    /// This sandbox class displays RGBD frames. It can be used for testing various logic.
    /// But most of the logic should be part of the Y-Vision framework.
    /// </summary>
    public partial class DebugTool : Form
    {
        private FrameDebugPipeline _top/*, _bottom*/;

        public DebugTool()
        {
            InitializeComponent();
        }

        private void DebugToolLoad(object sender, EventArgs e)
        {
            _top = new FrameDebugPipeline(rbgdViewerTop,1,0);
            //_bottom = new FrameDebugPipeline(rbgdViewerBottom,4,2);

            _top.FpsUpdated += (o, args) =>
                                   {
                                       Text = _applicationText + " - TOP:" + _top.FPS /*+ " fps, BOTTOM: " + _bottom.FPS +
                                              " fps"*/;
                                   };

            _top.StartKinect();
            //_bottom.StartKinect();

            rbgdViewerTop.PointSelected += RbgdViewerTopOnPointSelected;
        }

        private void RbgdViewerTopOnPointSelected(object sender, RgbdViewer.PointClickEventArgs pointClickEventArgs)
        {
            //Console.WriteLine("(x,y)=(" + pointClickEventArgs.X + "," + pointClickEventArgs.Y + ")");
            _top.SpecifyGroundPoint(pointClickEventArgs.X, pointClickEventArgs.Y);
        }

        private void DebugToolFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_top != null)
               _top.StopKinect();
            /*if(_bottom != null)
            _bottom.StopKinect();*/
        }

        private void DisplayPanelResize(object sender, EventArgs e)
        {
            horizontalSplit.SplitterDistance = (Height-40)/2;
        }

        private class FrameDebugPipeline
        {
            public int FPS { get; private set; }
            public int Time { get; private set; }

            private readonly KinectStreamMicrosoftApi _kinect;
            private readonly BitmapCreator _bmpCreator;
            private readonly Stopwatch _sw;
            private readonly RgbdViewer _displayControl;
            private readonly ConnectedComponentLabling _ccl;
            //private readonly GreedyTracker _tracker;
            //private readonly BranchAndBoundTracker _tracker;
            private PlaneGroundRemover _pgr /*= new PlaneGroundRemover()*/;
            private short[,] _depthArr2D;

            private readonly int _threshold;
            private int _count;

            internal FrameDebugPipeline(RgbdViewer displayControl, int threshold = 1, int startCount = 0)
            {
                _threshold = threshold;
                _count = startCount;
                _displayControl = displayControl;

                _sw = new Stopwatch();
                _bmpCreator = new BitmapCreator();
                _kinect = new KinectStreamMicrosoftApi();

                _ccl = new ConnectedComponentLabling();
                //_tracker = new GreedyTracker();
                //_tracker = new BranchAndBoundTracker();

                _kinect.FramesReady += NewFramesReady;
            }

            private void NewFramesReady(object sender, EventArgs args)
            {
                if (_count++ < _threshold)
                    return; // Skip frame
                _count = 0; // Else, reset Counter

                _sw.Stop();
                if (_sw.ElapsedMilliseconds > 0)
                {
                    FPS = (int) (1000/_sw.ElapsedMilliseconds);
                    Time = (int) _sw.ElapsedMilliseconds;
                }
                _sw.Reset();
                _sw.Start();
                var frames = (FramesReadyEventArgs)args;
                _depthArr2D = frames.DepthTo2DArray();

                if (frames.ColorArray == null || frames.DepthArray == null)
                    return;

                if (_pgr == null)
                {
                    _pgr = new PlaneGroundRemover(new KinectSensorContext(true),new SensorConfig(null)); // TODO: THIS MAY CAUSE A CRASH,
                }
                else
                {
                    _pgr.ApplyMask(_depthArr2D);
                }

                _bmpCreator.CreateBitmapFromColorFrame(frames.ColorArray, frames.ColorHeight, frames.ColorWidth);
                // Plane removal debug display
                //if (_pgr._groundMask != null)
                //{
                //    _bmpCreator.CreateBitmapFromIndex(_pgr._groundMask);
                //    ArrayWriter.ToTextFile(_pgr._groundMask, frames.DepthHeight, frames.DepthWidth);
                //}
                //else
                //    _bmpCreator.CreateBitmapFromDepthFrame(frames.DepthArray, frames.DepthHeight, frames.DepthWidth);
                

                var index = _ccl.FindConnectedComponents(_depthArr2D);
                _bmpCreator.CreateBitmapFromIndex(index);

                //// Blob detection
                //var blobs = BlobFilter.ToBlobObjects(_ccl.BlobInfo,);

                //_bmpCreator.DrawTrackedPeople(_bmpCreator.DepthBitamp, blobs/*, frames.ColorHeight / frames.DepthHeight*/);
                //// Tracking
                //var trackedObjects = _tracker.TrackObjects(blobs);

                ////Draw
                //_bmpCreator.DrawTrackedPeople(_bmpCreator.ColorBitamp, trackedObjects, frames.ColorHeight / frames.DepthHeight);

                _displayControl.DisplayFrames(_bmpCreator.DepthBitamp, _bmpCreator.ColorBitamp);

                if (FpsUpdated!=null)
                    FpsUpdated.Invoke(this, null);
            }

            public void StartKinect()
            {
                _kinect.Start();
            }

            public void StopKinect()
            {
                _kinect.Stop();
                //_pgr.SaveConfig();
            }

            public void SpecifyGroundPoint(double x, double y)
            {
                int h = _depthArr2D.GetLength(0), w = _depthArr2D.GetLength(1);
                var value = _depthArr2D[(int) (y*h + 0.5), (int) (x*w + 0.5)];
                if (value > 400 && value <= 4095)
                   _pgr.AddPoint((int)(x * w + 0.5), (int)(y * h + 0.5), value);
            }

            public event EventHandler<EventArgs> FpsUpdated;
        }
    }
}
