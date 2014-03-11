using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.Tracking;

namespace Y_Vision.BlobDescriptor
{
    public class BlobObject : TrackableObject
    {
        public BlobObject(ConnectedComponentLabling.Blob blob)
        {
            X = blob.X;
            Y = blob.Y;
            Z = blob.Z;

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
            return new TrackedObject(X, Y, Z, MaxX, MinX, MaxY, MinY,Surface);
        }

      

        public override string ToString()
        {
            return String.Format("Blob at ({0},{1}) with size ({2},{3}) and total surface = {4}", X, Y,
                                 MaxX - MinX, MaxY - MinY, Surface);
        }
    }
}
