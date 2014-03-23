using System;

namespace Y_Vision.Tracking
{
    public class TrackedObject : TrackableObject
    {
        public double VelocityX, VelocityY, VelocityZ;
        public double OnscreenVelocityX, OnscreenVelocityZ;
        // In frame count
        public int Age { get; private set; }
        public int LastSeen { get; private set; }
        private static ulong _idGenerator = 0;
        public ulong UniqueId { get; private set; }

        public TrackedObject(double x, double y, double z, double onScreenX, double onScreenY, double distance, int maxX, int minX, int maxY, int minY, int count)
        {
            OnscreenX = onScreenX;
            OnscreenY = onScreenY;
            DistanceZ = distance;

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
            // Real coords
            X += VelocityX;
            VelocityX *= 0.95;

            //Y += VelocityY; // Not used anymore, as it is unreliable
            //VelocityY *= 0.35;

            Z += VelocityZ;
            VelocityZ *= 0.75;


            // On screen for display
            OnscreenX += OnscreenVelocityX;
            OnscreenVelocityX *= 0.95;
            DistanceZ += OnscreenVelocityZ;
            OnscreenVelocityZ *= 0.75;
            
            if(OnAttributesUpdate!= null)
                OnAttributesUpdate.Invoke(this,new EventArgs());
        }

        // For blobs and detection
        public void UpdateTrackedFrame(TrackableObject other)
        {
            LastSeen = 0;

            OnscreenVelocityX = OnscreenVelocityX * 0.5 + (other.OnscreenX - OnscreenX) * 0.5;
            OnscreenVelocityZ = OnscreenVelocityZ * 0.5 + (other.DistanceZ - DistanceZ) * 0.5;

            VelocityX = VelocityX * 0.5 + (other.X - X) * 0.5;
            VelocityY = VelocityY * 0.5 + (other.Y - Y) * 0.5;
            VelocityZ = VelocityZ * 0.5 + (other.Z - Z) * 0.5;

            // For display mostly
            OnscreenX = (int) (OnscreenX * 0.5 + other.OnscreenX * 0.5);
            OnscreenY = (int) (OnscreenY * 0.5 + other.OnscreenY * 0.5);
            DistanceZ = (int) (DistanceZ * 0.5 + other.DistanceZ * 0.5);

            X = (X * 0.5 + other.X * 0.5);
            Y = (Y * 0.5 + other.Y * 0.5);
            Z = (Z * 0.5 + other.Z * 0.5);

            MaxX = (int) (MaxX * 0.5 + other.MaxX * 0.5);
            MinX = (int)(MinX * 0.5 + other.MinX * 0.5);
            MaxY = (int) (MaxY * 0.5 + other.MaxY * 0.5);
            MinY = (int)(MinY * 0.5 + other.MinY * 0.5);

            Surface = (int)(Surface * 0.5 + other.Surface * 0.5);

            if (OnAttributesUpdate != null)
                OnAttributesUpdate.Invoke(this, new EventArgs());
        }

        public override int ComputeDistanceWith(TrackableObject other)
        {
            return (int)Math.Pow(other.X - (X + VelocityX), 2)
                   + (int)Math.Pow(other.Y - (Y), 2)
                   + (int)Math.Pow((other.Z - (Z + VelocityZ)) / TrackingWeights.GetWeightZ(other), 2)
                   + Math.Abs(Surface - other.Surface) / TrackingWeights.GetWeightSurface(other);
        }

        public override TrackedObject ToTrackedObject()
        {
            return new TrackedObject(X, Y, Z, OnscreenX, OnscreenY, DistanceZ, MaxX, MinX, MaxY, MinY, Surface);
        }

        public override string ToString()
        {
            return String.Format("{0} at ({1};{2};{3}) and speed ({5};0;{6}) total surface = {4}, age={7}, lastSeen={8}",
                GetType(), OnscreenX, OnscreenY, DistanceZ, Surface, OnscreenVelocityX, OnscreenVelocityZ, Age, LastSeen);
        }

        public event EventHandler<EventArgs> OnAttributesUpdate;
    }
}
