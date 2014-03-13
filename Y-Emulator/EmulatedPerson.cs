using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_API.DetectionAPI.MessageObjects;

namespace Y_Emulator
{
    class EmulatedPerson : Person
    {

        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the X axis.
        /// The value represents a 1d float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public override float VelocityX { get { return em_VelocityX; } }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Y axis.
        /// The value represents a 1 dimension float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public override float VelocityY { get { return em_VelocityY; } }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Z axis.
        /// The value represents a 1 dimension float vector of mm per frame.
        /// </summary>
        public override float VelocityZ { get { return em_VelocityZ; } }
        /// <summary>
        /// The number of frames elasped since the object was first detected.
        /// </summary>
        public override int Age { get { return em_Age; } }
        /// <summary>
        /// The number of frames elasped since the object was last detected. Any value above 0 means it is not currently being tracked sucessfully.
        /// Objects will naturally expire (see events for more info) after a certain period.
        /// </summary>
        public override int LastSeen { get { return em_LastSeen; } }
        /// <summary>
        /// A unique long value identifying the object
        /// </summary>
        public override ulong UniqueId { get { return em_UniqueId; } }

        public override float X { get { return em_X; } }
        public override float Y { get { return em_Y; } }
        public override float Z { get { return em_Z; } }

        // storing the emulated data
        private float em_VelocityX;
        private float em_VelocityY;
        private float em_VelocityZ;
        private int em_Age;
        private int em_LastSeen;
        private ulong em_UniqueId;
        private float em_X;
        private float em_Y;
        private float em_Z;

        // A constructor used for decoding
        public EmulatedPerson(float VelocityX, float VelocityY, float VelocityZ, 
            int Age, int LastSeen, ulong UniqueId, float X, float Y, float Z)
        {
            em_VelocityX = VelocityX;
            em_VelocityY = VelocityY;
            em_VelocityZ = VelocityZ;
            em_Age = Age;
            em_LastSeen = LastSeen;
            em_UniqueId = UniqueId;
            em_X = X;
            em_Y = Y;
            em_Z = Z;
        }

        // An empty constructor for subclasses
        public EmulatedPerson() { }
    }
}
