using Y_Vision.ConnectedComponentLabeling;
using Y_Vision.GroundRemoval;
using Y_Vision.SensorStreams;
using Y_Vision.Tracking;

namespace Y_Vision.BlobDescriptor
{
    /// <summary>
    /// This class will build a type of blob depending on what's avaible in the constructor. If a system converter is provided, the blob will use
    /// 3d coordinates. Otherwise it will be flat. IMPORTANT NOTE: width and height may NOT change at runtime! (e.g. when calibrating and rotating)
    /// The goal of this object is to provide retro-compatibility with the old (flat) blob system.
    /// </summary>
    public class BlobFactory
    {
        private readonly CoordinateSystemConverter _conv;
        private readonly SensorContext _context;

        // TODO: a builder pattern will be needed for this object to be extensible
        public BlobFactory(CoordinateSystemConverter conv = null, SensorContext context = null)
        {
            _conv = conv;
            _context = context;
        }

        public TrackableObject CreateBlob(ConnectedComponentLabling.Blob b)
        {
            if (_conv != null || _context != null)
                return new BlobObject(b, _conv, _context.DepthHeight, _context.DepthWidth);

            return new FlatBlobObject(b);
        }
    }
}
