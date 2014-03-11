using System;
using System.Collections.Generic;
using System.Linq;
using Y_API.DetectionAPI.MessageObjects;

namespace Y_API.DetectionAPI
{
    public class StringProtocol
    {
        private readonly ObjectFactory _objectFactory;

        private const char ObjectSeparator = ';';
        public StringProtocol()
        {
            _objectFactory = new ObjectFactory();
        }

        public string Encode(IEnumerable<IStringEncodable> objects)
        {
            return objects.Aggregate("", (agg, next) => _objectFactory.AppendHeader(next.GetType(), next.Encode()) + ObjectSeparator + agg);
        }

        public IEnumerable<IStringEncodable> Decode(string code)
        {
            var strings = code.Split(ObjectSeparator);
            var objects = strings.Where(s => !String.IsNullOrEmpty(s)).Select(s => _objectFactory.CreateObject(s));

            // This can be used to get only objects of a single type:
            // objects.Where(p => p is Person);

            return objects;
        }
    }
}
