using System;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.Core;
using Y_Vision.Tracking;
using Y_Vision.Triangulation;

namespace Y_Vision.DetectionAPI
{
    public class ParallaxTrackedPerson : Person
    {
        protected readonly TrackedObject Source = null;
        private readonly MappingTool _projectionTool;

        public override float VelocityX { get { return (float) Source.VelocityX; } }

        public override float VelocityY { get { return (float) Source.VelocityY; } }

        public override float VelocityZ { get { return (float) Source.VelocityZ; } }

        private readonly DateTime _creationTs;
        /// <summary>
        /// The Total number of seconds since the object was created.
        /// </summary>
        public override int Age { get { return (int) (DateTime.Now - _creationTs).TotalSeconds; } }

        /// <summary>
        /// The number of frames since it was last detected. If greater than 0 it means it is not currently detected.
        /// </summary>
        public override int LastSeen { get { return Source.LastSeen; } }

        public override ulong UniqueId { get { return Source.UniqueId; } }

        private float _x, _y, _z;
        public override float X { get { return _x; } }
        public override float Y { get { return _y; } }
        public override float Z { get { return _z; } }

        public ParallaxTrackedPerson(TrackedObject source, MappingTool projectionTool)
        {
            if (source == null)
                throw new NullReferenceException("The source must be defined!");

            _creationTs = DateTime.Now;

            Source = source;

            _projectionTool = projectionTool;

            // Project points on screen whenever the fields are updated.
            Source.OnAttributesUpdate += (sender, args) => 
            {
                var p = new Point3D(source.X, source.Y, source.Z);
                var newP = _projectionTool.ProjectPointOnDisplay(p);
                _x = (float) newP.X;
                _y = (float) newP.Y;
                _z = (float) newP.Z;
            };
        }
    }
}
