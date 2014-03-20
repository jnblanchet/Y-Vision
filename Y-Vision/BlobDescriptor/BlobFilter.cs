using System;
using System.Collections.Generic;
using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.SensorStreams;
using Y_Vision.Tracking;

namespace Y_Vision.BlobDescriptor
{
    internal class BlobFilter
    {
        private const float CloseToGroundThreshold = 0.65f;
        private const float MinScreenSurface = 0.05f;

        private readonly BlobFactory _factory;
        private readonly KinectSensorContext _context;

        internal BlobFilter(BlobFactory factory, KinectSensorContext context)
        {
            _factory = factory;
            _context = context;
        }


        public List<TrackableObject> ToBlobObjects(IEnumerable<ConnectedComponentLabling.Blob> blobs)
        {
            var minsurface = (int)(_context.DepthWidth * _context.DepthHeight * MinScreenSurface);
            var closeToGround = (int) (_context.DepthHeight * CloseToGroundThreshold);
            var newBlobs = new List<TrackableObject>();

            foreach(var b in blobs)
            {
                if (b.Count > minsurface) // Large enough surface
                    if ((double)(b.MaxY - b.MinY) / (b.MaxX - b.MinX) > 1.2d) // Human like proportion
                        if (FilterByHumanHeight(b.Y, b.MinY, b.MaxY, _context.DepthHeight, b.Z, _context.VerticalFieldOfViewRad)) // Human height range
                            if (b.MaxY > closeToGround) // Close enough from the ground approximation
                                newBlobs.Add(_factory.CreateBlob(b));
            }
            return newBlobs;
        }

        private bool FilterByHumanHeight(int yCenter, int yMin, int yMax, int height, int distance, double verticalFoV)
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
