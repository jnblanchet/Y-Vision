using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.Core;
using Y_Vision.GroundRemoval;

namespace Y_Vision.BlobDescriptor
{
    /// <summary>
    /// represents a set of pixels associated with a segmented object. Unlike the flatBlob, its coordinates represent a position in 3d space.
    /// </summary>
    class BlobObject : FlatBlobObject
    {
        public BlobObject(ConnectedComponentLabling.Blob blob, CoordinateSystemConverter conv, int h, int w) : base(blob)
        {
            var realCoords = conv.ToXyz(blob.X, blob.Y, blob.Z + 100, h, w);
            X = realCoords.X;
            Y = realCoords.Y;
            Z = realCoords.Z;
        }
    }
}
