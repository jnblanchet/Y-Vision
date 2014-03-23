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
                return 10;// Should be 1 when using mm tracking
            if (t.GetType() == typeof(TrackedObject))
                return 1;// Should be 1 when using mm tracking
            throw new Exception("The new blob type is not associated with any weights!");
        }

        public static int GetWeightSurface(TrackableObject t)
        {
            if (t.GetType() == typeof(BlobObject))
                return 20;
            if (t.GetType() == typeof(FlatBlobObject))
                return 20;
            if (t.GetType() == typeof(TrackedObject))
                return 20;
            throw new Exception("The new blob type is not associated with any weights!");
        }
    }
}
