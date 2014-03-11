using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.GroundRemoval;
using Y_Vision.SensorStreams;

namespace Y_Vision.BlobDescriptor
{
    public static class BlobFilter
    {
        private const float CloseToGroundThreshold = 0.65f;
        private const float MinScreenSurface = 0.05f;

        public static List<BlobObject> ToBlobObjects(IEnumerable<ConnectedComponentLabling.Blob> blobs, KinectSensorContext context, int w, int h)
        {
            int minsurface = (int) (w*h*MinScreenSurface);
            int closeToGround = (int) (h*CloseToGroundThreshold);
            var newBlobs = new List<BlobObject>();
            foreach(var b in blobs)
            {
                if (b.Count > minsurface) // Large enough surface
                    if ((double)(b.MaxY - b.MinY) / (b.MaxX - b.MinX) > 2.0d) // Human like proportion
                        if (FilterByHumanHeight(b.Y,b.MinY,b.MaxY,h,b.Z,context.VerticalFieldOfViewRad)) // Human height range
                            if (b.MaxY > closeToGround) // Close enough from the ground approximation
                                newBlobs.Add(new BlobObject(b));
            }
            return newBlobs;
        }

        private static bool FilterByHumanHeight(int yCenter, int yMin, int yMax, int height, int distance, double verticalFoV)
        {
            const int minDistance = 1500;
            // Filter not applicable if the person is not completely visible
            if (distance < minDistance)
                return true;


            const int minHeight = 800;
            const int maxHeight = 2100;

            var personHeight = Math.Tan(((double)(yCenter - yMin) / height) * verticalFoV) * distance
                + Math.Tan(((double)(yMax - yCenter) / height) * verticalFoV) * distance;

            return (personHeight >= minHeight && personHeight <= maxHeight);
        }
    }
}
