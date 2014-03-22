using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_API.DetectionAPI.MessageObjects;

namespace Y_API.DetectionAPI
{
    static class ObjectFactory
    {
        public const string ClassDelim = ":";
        private static readonly Builder[] _builders = new Builder[] { new PersonBuilder(), new EmptyFrameBuilder(),  };

        /// <summary>
        /// Returns the newly created object or throws an exception if invalid
        /// </summary>
        public static IStringEncodable CreateObject(String code)
        {
            // Get encoded class id
            int typeHeader = code.IndexOf(ClassDelim);

            if (typeHeader < 1)
            {
                throw new ArgumentException("Code '" + code + "' does not contain an id.");
            }
            // Get encoded class info
            int classId = int.Parse(code.Substring(0, typeHeader));
            string endCode = code.Substring(typeHeader + 1);

            // Select the builder, build the object and return the new object
            return _builders.First(b => b.ClassId == classId).Build(endCode);
        }

        public static string AppendHeader(Type type, string body)
        {
            return _builders.First(b => b.BuildingType.IsAssignableFrom(type)).AppendHeader(body);
        }
       
        private abstract class Builder
        {
            // To allow instanciation through a factory.
            public int ClassId { get; private set; }
            public Type BuildingType { get; private set; }

            private static int _idGen = 0;

            protected Builder(Type buildingType)
            {
                BuildingType = buildingType;
                ClassId = _idGen++;
            }

            public abstract IStringEncodable Build(string code);
            public string AppendHeader(string body) { return ClassId + ClassDelim + body; }
        }

        private class PersonBuilder : Builder
        {
            public PersonBuilder() : base(typeof(Person)) {}

            public override IStringEncodable Build(string code)
            {
                return new Person(code);
            }
        }

        private class EmptyFrameBuilder : Builder
        {
            public EmptyFrameBuilder() : base(typeof(EmptyFrameMessage)) { }

            public override IStringEncodable Build(string code)
            {
                return new EmptyFrameMessage();
            }
        }
    }


}
