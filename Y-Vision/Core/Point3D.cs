using System;

namespace Y_Vision.Core
{
    // A simple primitive 3d point type in the form of a struct with basic operators
    [Serializable()]
    public struct Point3D
    {
        public double X, Y, Z;

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D c1, Point3D c2)
        {
            return new Point3D(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public static Point3D operator -(Point3D c1, Point3D c2)
        {
            return new Point3D(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public static Point3D operator *(Point3D c1, int s)
        {
            return new Point3D(c1.X * s, c1.Y * s, c1.Z * s);
        }
    }
}
