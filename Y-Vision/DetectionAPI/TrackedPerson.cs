using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.Tracking;

namespace Y_Vision.DetectionAPI
{
    public class TrackedPerson : Person
    {
        private readonly TrackedObject _source = null;

        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the X axis.
        /// The value represents a 1d float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public override float VelocityX { get { return (float) (_source.VelocityX / 320.0f); } }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Y axis.
        /// The value represents a 1 dimension float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public override float VelocityY { get { return (float)(_source.VelocityY / 240.0f); } }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Z axis.
        /// The value represents a 1 dimension float vector of mm per frame.
        /// </summary>
        public override float VelocityZ { get { return (float)_source.VelocityZ; } }
        /// <summary>
        /// The number of frames elasped since the object was first detected.
        /// </summary>
        public override int Age { get { return _source.Age; } }
        /// <summary>
        /// The number of frames elasped since the object was last detected. Any value above 0 means it is not currently being tracked sucessfully.
        /// Objects will naturally expire (see events for more info) after a certain period.
        /// </summary>
        public override int LastSeen { get { return _source.LastSeen; } }
        /// <summary>
        /// A unique long value identifying the object
        /// </summary>
        public override ulong UniqueId { get { return _source.UniqueId; } }

        public override float X { get { return (float)_source.X / 320.0f; } }
        public override float Y { get { return (float)_source.Y / 240.0f; } }
        public override float Z { get { return (float)_source.Z; } }

        public TrackedPerson(TrackedObject source)
        {
            if (source == null)
                throw new NullReferenceException("The source must be defined!");
            _source = source;
        }
    }
}
