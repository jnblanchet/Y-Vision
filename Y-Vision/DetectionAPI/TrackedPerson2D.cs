using System;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.Tracking;

namespace Y_Vision.DetectionAPI
{
    public class TrackedPerson2D : Person
    {
        protected readonly TrackedObject Source = null;

        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the X axis.
        /// The value represents a 1d float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public override float VelocityX { get { return (float) (Source.VelocityX / _w); } }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Y axis.
        /// The value represents a 1 dimension float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public override float VelocityY { get { return (float)(Source.VelocityY / _h); } }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Z axis.
        /// The value represents a 1 dimension float vector of mm per frame.
        /// </summary>
        public override float VelocityZ { get { return (float)Source.VelocityZ; } }
        /// <summary>
        /// The number of frames elasped since the object was first detected.
        /// </summary>
        public override int Age { get { return Source.Age; } }
        /// <summary>
        /// The number of frames elasped since the object was last detected. Any value above 0 means it is not currently being tracked sucessfully.
        /// Objects will naturally expire (see events for more info) after a certain period.
        /// </summary>
        public override int LastSeen { get { return Source.LastSeen; } }
        /// <summary>
        /// A unique long value identifying the object
        /// </summary>
        public override ulong UniqueId { get { return Source.UniqueId; } }

        public override float X { get { return (float)Source.X / _w; } }
        public override float Y { get { return (float)Source.Y / _h; } }
        public override float Z { get { return (float)Source.Z; } }

        private readonly int _w, _h;

        /// <summary>
        /// This version of Person will hook itself to a source that is expected to be updated over time.
        /// The parameters w and h will be used to normalize the width and height of the position (assuming 2d)
        /// </summary>
        public TrackedPerson2D(TrackedObject source, int w, int h)
        {
            if (source == null)
                throw new NullReferenceException("The source must be defined!");
            _w = w;
            _h = h;
            Source = source;
        }
    }
}
