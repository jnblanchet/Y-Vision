using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Y_Vision.SensorStreams
{
    public class KinectSensorContext : SensorContext
    {
        // TODO: use some sort of enum config object to this can be changed easily (ConstDepthHeight, ConstDepthWidth) are dependant at the moment
        internal DepthImageFormat DepthConfig { get; private set; }

        public KinectSensorContext(bool isDefaultSetup) : base(isDefaultSetup)
        {
        }
        
        // This is called whenever is a rotation occurs (when calibrating the setup).
        public override void RotateFieldOfView(bool isDefaultSetup)
        {
            switch(isDefaultSetup)
            {
                case true:
                    VerticalFieldOfViewRad = 43.0d / 180.0d * Math.PI;
                    HorizontalFieldOfViewRad = 57.0d / 180.0d * Math.PI;
                    VerticalFieldOfViewDeg = 43.0d;
                    HorizontalFieldOfViewDeg = 57.0d;
                    DepthConfig = DepthImageFormat.Resolution320x240Fps30;
                    DepthWidth = ConstDepthWidth;
                    DepthHeight = ConstDepthHeight;
                    break;
                case false:
                    VerticalFieldOfViewRad = 57.0d / 180.0d * Math.PI;
                    HorizontalFieldOfViewRad = 43.0d / 180.0d * Math.PI;
                    VerticalFieldOfViewDeg = 57.0d;
                    HorizontalFieldOfViewDeg = 43.0d;
                    DepthConfig = DepthImageFormat.Resolution320x240Fps30;
                    DepthWidth = ConstDepthHeight;
                    DepthHeight = ConstDepthWidth;
                    break;
            }
        }
    }
}
