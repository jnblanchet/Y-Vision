using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.Tracking
{
    public class TrackedObject : TrackableObject
    {
        public double VelocityX, VelocityY, VelocityZ;
        // In frame count
        public int Age { get; private set; }
        public int LastSeen { get; private set; }
        private static ulong _idGenerator = 0;
        public ulong UniqueId { get; private set; }

        public TrackedObject(double x, double y, double z, int maxX, int minX, int maxY, int minY, int count)
        {
            X = x;
            Y = y;
            Z = z;
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;

            Surface = count;

            VelocityX = 0;
            VelocityY = 0;
            VelocityZ = 0;

            Age = 0;
            LastSeen = 0;

            UniqueId = _idGenerator++;
        }

        internal void PrepareForNewTrackingFrame()
        {
            Age++;
            LastSeen++;
        }


        internal void UpdateUntrackedFrame()
        {
            X += VelocityX;
            VelocityX *= 0.95;

            //Y += VelocityY;
            //VelocityY *= 0.35;

            Z += VelocityZ;
            VelocityZ *= 0.75;
        }

        internal void UpdateTrackedFrame(TrackableObject obj)
        {
            Age++;
            LastSeen = 0;

            VelocityX = VelocityX * 0.3 + (obj.X - X) * 0.7;
            VelocityY = VelocityY * 0.5 + (obj.Y - Y) * 0.5;
            VelocityZ = VelocityZ * 0.3 + (obj.Z - Z) * 0.7;

            X = (X * 0.5 + obj.X * 0.5);
            Y = (Y * 0.5 + obj.Y * 0.5);
            Z = (Z * 0.5 + obj.Z * 0.5);

            MaxX = (int) (MaxX * 0.5 + obj.MaxX * 0.5);
            MinX = (int)(MinX * 0.5 + obj.MinX * 0.5);
            MaxY = (int) (MaxY * 0.5 + obj.MaxY * 0.5);
            MinY = (int)(MinY * 0.5 + obj.MinY * 0.5);

            Surface = (int)(Surface * 0.7 + obj.Surface * 0.3);
        }

        public override int ComputeDistanceWith(TrackableObject other)
        {
            // random divisions for weights TODO: FIX WEIGTHS
            return (int)Math.Pow(other.X - (X + VelocityX), 2)
                   + (int)Math.Pow(other.Y - (Y + VelocityY), 2)
                   + (int)Math.Pow((other.Z - (Z + VelocityZ)) / 10, 2)
                   + Math.Abs(Surface - other.Surface) / 20;
        }

        public override TrackedObject ToTrackedObject()
        {
            return this;
        }
    }
}
