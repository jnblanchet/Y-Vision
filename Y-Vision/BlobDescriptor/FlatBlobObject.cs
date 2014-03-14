using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.Tracking;

namespace Y_Vision.BlobDescriptor
{
    /// <summary>
    /// This class reprensents a set of pixels on screen. It is "flat" as X and Y represent positions of the captured frame.
    /// </summary>
    public class FlatBlobObject : TrackableObject
    {
        public FlatBlobObject(ConnectedComponentLabling.Blob blob)
        {
            X = OnscreenX = blob.X;
            Y = OnscreenY = blob.Y;
            Z = DistanceZ = blob.Z;

            MaxX = blob.MaxX;
            MinX = blob.MinX;
            MaxY = blob.MaxY;
            MinY = blob.MinY;

            Surface = blob.Count;
        }


        public override int ComputeDistanceWith(TrackableObject other)
        {
            throw new NotImplementedException();
        }

        public override TrackedObject ToTrackedObject()
        {
            return new TrackedObject(X, Y, Z, OnscreenX, OnscreenY, DistanceZ, MaxX, MinX, MaxY, MinY,Surface);
        }

      

        public override string ToString()
        {
            return String.Format("Blob at ({0},{1}) with size ({2},{3}) and total surface = {4}", X, Y,
                                 MaxX - MinX, MaxY - MinY, Surface);
        }
    }
}
