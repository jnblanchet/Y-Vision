using System;
using Y_Vision.Core;

namespace Y_Vision.Triangulation
{
    public abstract class CoordinateSystemTool
    {
		// if we calculate a standard deviation greater than
		// this for the sensor angle, reject the points
		protected const double maxAngleStdDev = 8.0d;
		
        internal int NbSensors;
        protected bool ThrowExceptions = true;
        internal Point3D[] SensorsPos;
        internal double[] SensorsAngle;

        public abstract double GetSensorPosX(int idSensor);
        public abstract double GetSensorPosY(int idSensor);
        public abstract double GetSensorPosZ(int idSensor);
        public abstract double GetSensorAngle(int idSensor);

        public Point3D? GetNormalizedCoordinates(int idSensor, double x, double y, double z)
        {
            double angle = Math.PI * SensorsAngle[idSensor] / 180.0;    

            /*newX = Math.Cos(angle) * x - Math.Sin(angle) * z + sensorX * (1 - Math.Cos(angle)) + sensorZ * Math.Sin(angle);
            newZ = Math.Sin(angle) * x + Math.Cos(angle) * z - sensorX * Math.Sin(angle) + sensorZ * (1 - Math.Cos(angle));*/

            double newX = Math.Cos(angle) * x - Math.Sin(angle) * z + GetSensorPosX(idSensor);
            double newZ = Math.Sin(angle) * x + Math.Cos(angle) * z + GetSensorPosZ(idSensor);

            return new Point3D(newX, y, newZ);
        }

        public Point3D? GetNormalizedCoordinates(int idSensor, Point3D p)
        {
            return GetNormalizedCoordinates(idSensor, p.X, p.Y, p.Z);
        }


        public abstract Point3D? GetNormalizedCoordinates(int idSensor);
    }
}