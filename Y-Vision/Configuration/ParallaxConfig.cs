﻿using System;
using System.Collections.Generic;
using Y_Vision.Core;

namespace Y_Vision.Configuration
{
    [Serializable()]
    public class ParallaxConfig
    {
        private const int MaxSampleCount = 6;
        private readonly Dictionary<string, List<Point3D>> _parallaxPoint3DSets;
        private readonly Dictionary<string, List<Point>> _referencePoint2DSets;

        public ParallaxConfig()
        {
            _parallaxPoint3DSets = new Dictionary<string, List<Point3D>>(2);
            _referencePoint2DSets = new Dictionary<string, List<Point>>(2);
        }

        /// <summary>
        /// Creates a list for the sensor if it doesn't exist. Inserts the point in the appropriate list.
        /// </summary>
        public void AddPoint(string sensorId, Point3D p3D, Point p2D)
        {
            // Add the 3d point
            if (!_parallaxPoint3DSets.ContainsKey(sensorId))
                _parallaxPoint3DSets.Add(sensorId, new List<Point3D>());

            var list3D = _parallaxPoint3DSets[sensorId];

            if (list3D.Count >= MaxSampleCount)
                list3D.RemoveAt(0);

            list3D.Add(p3D);

            // add the 2d reference (for visualization
            if (!_referencePoint2DSets.ContainsKey(sensorId))
                _referencePoint2DSets.Add(sensorId, new List<Point>());

            var list2D = _referencePoint2DSets[sensorId];

            if (list2D.Count >= MaxSampleCount)
                list2D.RemoveAt(0);

            list2D.Add(p2D);

        }

        public IEnumerable<Point3D> Get3DPoints(string sensorId)
        {
            if(String.IsNullOrEmpty(sensorId))
                return null;

            if (!_parallaxPoint3DSets.ContainsKey(sensorId))
            {
                _parallaxPoint3DSets.Add(sensorId, new List<Point3D>());
                _referencePoint2DSets.Remove(sensorId); // Attempt to create for 2d aswell
                _referencePoint2DSets.Add(sensorId, new List<Point>());
            }

            return _parallaxPoint3DSets[sensorId];
        }

        public IEnumerable<Point> Get2DPoints(string sensorId)
        {
            if (!_referencePoint2DSets.ContainsKey(sensorId))
            {
                _referencePoint2DSets.Add(sensorId, new List<Point>());
                _parallaxPoint3DSets.Remove(sensorId); // Attempt to create for 3d aswell
                _parallaxPoint3DSets.Add(sensorId, new List<Point3D>());
            }

            return _referencePoint2DSets[sensorId];
        }

        public override string ToString()
        {
            string s = "\nin 3D:\n";
            foreach (var k in _parallaxPoint3DSets.Keys)
            {
                s += "sensor " + k + ": \n";
                foreach (var p in _parallaxPoint3DSets[k])
                {
                    s += "(" + p.X + "," + p.Y + "," + p.Z + "); \n";
                }
            }
            s += "\nin 2D:\n";
            foreach (var k in _referencePoint2DSets.Keys)
            {
                s += "sensor " + k + ": \n";
                foreach (var p in _referencePoint2DSets[k])
                {
                    s += "(" + p.X + "," + p.Y + "," + p.Z + "); \n";
                }
            }
            return s;
        }
    }
}
