using System;
using Y_Vision.Core;
using Y_Vision.SensorStreams;

namespace Y_Vision.GroundRemoval
{
    // TODO: make Internal class
    public class CoordinateSystemConverter
    {
        private readonly KinectSensorContext _context;

        public CoordinateSystemConverter(KinectSensorContext context)
        {
            _context = context;
        }

        public Point3D ToXyz(double onScreenX, double onScreenY, double distance, double h, double w)
        {
            var angleX = -1 * ((onScreenX/w) - 0.5d)*(_context.HorizontalFieldOfViewRad);
            var angleY = ((onScreenY/h) - 0.5d)*(_context.VerticalFieldOfViewRad);

            // The horizontal angle (along the X axis) is rotated using the Y axis. The same applies to the vertical angle.
            var cartesianPoint = RotateXAxis(RotateYAxis(new Point3D(0, 0, 1), angleX), angleY);

            cartesianPoint.X *= distance;
            cartesianPoint.Y *= distance;
            cartesianPoint.Z *= distance;

            return cartesianPoint;
        }

        private Point3D RotateXAxis(Point3D v, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            double ty = v.Y;
            double tz = v.Z;

            v.Y = (cos * ty) + (sin * tz);
            v.Z = (cos * tz) - (sin * ty);

            return v;
        }

        private Point3D RotateYAxis(Point3D v, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            double tx = v.X;
            double tz = v.Z;

            v.X = (cos * tx) - (sin * tz);
            v.Z = (cos * tz) + (sin * tx);

            return v;
        }

    }
}
