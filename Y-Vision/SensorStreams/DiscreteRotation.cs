using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.SensorStreams
{
    public class DiscreteRotation
    {
        public int Angle { get; private set; }
        public bool InvertHeightWidth { get; private set; }
        private readonly bool _translationX;
        private readonly bool _translationY;
        private readonly int Xx, Xy;
        private readonly int Yx, Yy;

        private DiscreteRotation(int angle, bool invertHeightWidth, bool translationX, bool translationY)
        {
            InvertHeightWidth = invertHeightWidth;
            Angle = angle;
            Xx = (int)Math.Cos((double)Angle / 180 * Math.PI);
            Xy = (int)(-1 * Math.Sin((double)Angle / 180 * Math.PI));
            Yx = (int)Math.Sin((double)Angle / 180 * Math.PI);
            Yy = (int)Math.Cos((double)Angle/180 * Math.PI);

            _translationX = translationX;
            _translationY = translationY;
        }

        public struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X, Y;
        }
        public Point Rotate(int x, int y, int w, int h)
        {
            return new Point(x * Xx + y * Xy + (_translationX ? w : 0), x * Yx + y * Yy + (_translationY? h : 0));
        }

        public static readonly DiscreteRotation NoRotation = new DiscreteRotation(0, false, false, false);
        public static readonly DiscreteRotation Cw90 = new DiscreteRotation(90, true, true, false);
        public static readonly DiscreteRotation Cw180 = new DiscreteRotation(180, false, true, true);
        public static readonly DiscreteRotation Cw270 = new DiscreteRotation(270, true,false, true);

        public static readonly DiscreteRotation[] All = {NoRotation, Cw90, Cw180, Cw270};

        public static DiscreteRotation GetRotatedNext(DiscreteRotation r)
        {
            return All.First(o => o.Angle == (r.Angle + 90)%360);
        }
    };
}
