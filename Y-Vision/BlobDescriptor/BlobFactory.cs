using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.GroundRemoval;
using Y_Vision.SensorStreams;
using Y_Vision.Tracking;
using Y_Vision.Triangulation;

namespace Y_Vision.BlobDescriptor
{
    /// <summary>
    /// This class will build a type of blob depending on what's avaible in the constructor. If a system converter is provided, the blob will use
    /// 3d coordinates. Otherwise it will be flat. IMPORTANT NOTE: width and height may NOT change at runtime! (e.g. when calibrating and rotating)
    /// The goal of this object is to provide retro-compatibility with the old (flat) blob system.
    /// </summary>
    public class BlobFactory
    {
        // Builder pattern.
        public CoordinateSystemConverter Conv; // turns the 2d into 3d
        public SensorContext Context;
        public MappingTool MappingTool;
        public int SensorId;

        public TrackableObject CreateBlob(ConnectedComponentLabling.Blob b)
        {
            TrackableObject trackableObject;
            
            if (Conv != null || Context != null)
                trackableObject = new BlobObject(b, Conv, Context.DepthHeight, Context.DepthWidth);
            else
                trackableObject = new FlatBlobObject(b);

            if (MappingTool != null)
            {
                var newPoint = MappingTool.GetNormalizedCoordinates(SensorId, trackableObject.X, trackableObject.Y, trackableObject.Z);
                trackableObject.ChangeCoordiateSystem(newPoint.Value);
            }

            return trackableObject;
        }
    }
}
