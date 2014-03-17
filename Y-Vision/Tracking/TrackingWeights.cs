using System;
using Y_Vision.BlobDescriptor;

namespace Y_Vision.Tracking
{
    // This static class returns weights for the tracking. It only exists to maintain support of the old 2d system.
    // With the old system, the onscreen coordinates (X,Y) use a different scale than the distance in mm (Z). This is why values
    // need to be weighted.
    internal static class TrackingWeights
    {
        public static int GetWeightZ(TrackableObject t)
        {
            if (t.GetType() == typeof(BlobObject))
                return 10;
            if (t.GetType() == typeof(FlatBlobObject))
                return 1;
            if (t.GetType() == typeof(TrackedObject))
                return 1;
            throw new Exception("The new blob type is not associated with any weights!");
        }

        public static int GetWeightSurface(TrackableObject t)
        {
            if (t.GetType() == typeof(BlobObject))
                return 2;
            if (t.GetType() == typeof(FlatBlobObject))
                return 20;
            if (t.GetType() == typeof(TrackedObject))
                return 2;
            throw new Exception("The new blob type is not associated with any weights!");
        }
    }
}
