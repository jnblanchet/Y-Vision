using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.SensorStreams
{
    public class KinectSensorContext //TODO: use abstraction for multiple sensor model support
    {
        public double VerticalFieldOfViewRad;
        public double HorizontalFieldOfViewRad;
        public double VerticalFieldOfViewDeg;
        public double HorizontalFieldOfViewDeg;

        public const int MinDepthValue = 400;
        public const int MaxDepthValue = 4050;

        

        public KinectSensorContext(bool isDefaultSetup)
        {
            RotateFieldOfView(isDefaultSetup);
        }

        public void RotateFieldOfView(bool isDefaultSetup)
        {
            switch(isDefaultSetup)
            {
                case true:
                    VerticalFieldOfViewRad = 43.0d / 180.0d * Math.PI;
                    HorizontalFieldOfViewRad = 57.0d / 180.0d * Math.PI;
                    VerticalFieldOfViewDeg = 43.0d;
                    HorizontalFieldOfViewDeg = 57.0d;
                    break;
                case false:
                    VerticalFieldOfViewRad = 57.0d / 180.0d * Math.PI;
                    HorizontalFieldOfViewRad = 43.0d / 180.0d * Math.PI;
                    VerticalFieldOfViewDeg = 57.0d;
                    HorizontalFieldOfViewDeg = 43.0d;
                    break;
            }
        }
    }
}
