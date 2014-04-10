using System;
using System.Globalization;

namespace Y_API.DetectionAPI.MessageObjects
{
    public class Person : IStringEncodable
    {
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the X axis.
        /// The value represents a 1d float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public virtual float VelocityX { get; private set; }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Y axis.
        /// The value represents a 1 dimension float vector where 0 is no movement and 1 is equal to the entire scene width in a single frame.
        /// </summary>
        public virtual float VelocityY { get; private set; }
        /// <summary>
        /// The velocity expressing the the speed at which the person moves on the Z axis.
        /// The value represents a 1 dimension float vector of mm per frame.
        /// </summary>
        public virtual float VelocityZ { get; private set; }
        /// <summary>
        /// The number of frames elasped since the object was first detected.
        /// </summary>
        public virtual int Age { get; private set; }
        /// <summary>
        /// The number of frames elasped since the object was last detected. Any value above 0 means it is not currently being tracked sucessfully.
        /// Objects will naturally expire (see events for more info) after a certain period.
        /// </summary>
        public virtual int LastSeen { get; private set; }
        /// <summary>
        /// A unique long value identifying the object
        /// </summary>
        public virtual ulong UniqueId { get; private set; }

        public virtual float X { get; private set; }
        public virtual float Y { get; private set; }
        public virtual float Z { get; private set; }

        private const char Split = '/';
        private NumberFormatInfo provider;

        // A constructor used for decoding
        public Person(string code)
        {
            string[] val = code.Split(Split);
            if (val.Length != 9)
            {
                throw new ArgumentException("Cannot decode the string '" + code + "' as a Person.");
            }

            VelocityX = float.Parse(val[0]);
            VelocityY = float.Parse(val[1]);
            VelocityZ = float.Parse(val[2]);
            Age = int.Parse(val[3]);
            LastSeen = int.Parse(val[4]);
            UniqueId = ulong.Parse(val[5]);
            X = float.Parse(val[6]);
            Y = float.Parse(val[7]);
            Z = float.Parse(val[8]);
        }

        // An empty constructor for subclasses
        public Person() { }

        public override string ToString()
        {
            return String.Format("Person at ({0}%,{1}%,{2}%) age={3}", (int)(X * 100), (int)(Y * 100), (int)(Z * 100), Age);
        }

        public string Encode()
        {
            // Using '/' since ',' is culture specific (float.toString() = x,xx in some cases)
            // We need this for french settings
            provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            return VelocityX.ToString(provider) + "/" + VelocityY.ToString(provider) + "/" + VelocityZ.ToString(provider) +
                "/" + Age + "/" + LastSeen + "/" + UniqueId + "/" + X.ToString(provider) + "/" + Y.ToString(provider) +
                "/" + Z.ToString(provider);
        }

        // Updates the current object using another already existing object
        public void UpdateFrom(Person other)
        {
            VelocityX = other.VelocityX;
            VelocityY = other.VelocityY;
            VelocityZ = other.VelocityZ;
            Age = other.Age;
            LastSeen = other.LastSeen;
            UniqueId = other.UniqueId;
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }
    }
}
