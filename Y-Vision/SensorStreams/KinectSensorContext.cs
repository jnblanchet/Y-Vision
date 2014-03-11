using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.SensorStreams
{
    public class KinectSensorContext //TODO: use abstraction for multiple sensor model support
    {
        public double VerticalFieldOfView;
        public double HorizontalFieldOfView;

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
                    VerticalFieldOfView = 43.0d / 180.0d * Math.PI;
                    HorizontalFieldOfView = 57.0d / 180.0d * Math.PI;
                    break;
                case false:
                    VerticalFieldOfView = 57.0d / 180.0d * Math.PI;
                    HorizontalFieldOfView = 43.0d / 180.0d * Math.PI;
                    break;
            }
        }
    }
}
