using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.Core
{
    [Serializable()]
    public class Point
    {
        public double X, Y, Z;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point c1, Point c2)
        {
            return new Point(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static Point operator -(Point c1, Point c2)
        {
            return new Point(c1.X - c2.X, c1.Y - c2.Y);
        }

        public static Point operator *(Point c1, int s)
        {
            return new Point(c1.X * s, c1.Y * s);
        }
    }
}
