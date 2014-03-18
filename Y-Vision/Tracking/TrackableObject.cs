using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_Vision.Core;

namespace Y_Vision.Tracking
{
    /// <summary>
    /// This class represents and object that could potentially be tracked.
    /// </summary>
    public abstract class TrackableObject
    {
        // Even though input is discrete (int), double allows more interpolation, extrapolation and smoothing precision
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public double Z { get; protected set; }
        public double OnscreenX { get; protected set; }
        public double OnscreenY { get; protected set; }
        public double DistanceZ { get; protected set; }

        public int Surface { get; protected set; }

        public int MinX { get; protected set; }
        public int MaxX { get; protected set; }
        public int MinY { get; protected set; }
        public int MaxY { get; protected set; }

        public int Width
        {
            get { return (MaxX - MinX); }
        }
        public int Height
        {
            get { return (MaxY - MinY); }
        }

        public abstract int ComputeDistanceWith(TrackableObject other);

        public abstract TrackedObject ToTrackedObject();

        public void ChangeCoordiateSystem(Point3D newPoint)
        {
            X = newPoint.X;
            Y = newPoint.Y;
            Z = newPoint.Z;
        }

        public override string ToString()
        {
            return String.Format("{0} at ({1};{2};{3}) and total surface = {4}",
                GetType(),OnscreenX, OnscreenY, DistanceZ, Surface);
        }
    }
}
