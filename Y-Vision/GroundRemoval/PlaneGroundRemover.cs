using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Y_Vision.Configuration;
using Y_Vision.Core;
using Y_Vision.SensorStreams;

namespace Y_Vision.GroundRemoval
{
    [Serializable()]
    public class PlaneGroundRemover
    {
        private short[,] _groundMask;
        private readonly Mutex _arrayLock;

        private readonly List<Point3D> _points;

        private double _distanceThreshold;
        private readonly short _minValue;
        private int _w;
        private int _h;
        private readonly int _maxSamples;
        private readonly CoordinateSystemConverter _distanceConverter;
        private Point3D _normalVector, _p0;

        // TODO: Allow maxSamples configuration
        public PlaneGroundRemover(KinectSensorContext context, SensorConfig config, int maxSamples = 4)
        {            
            _points = config.Ground;
            config.GroundPointsChanged += (sender, args) => AddPoint(args.Point);

            _distanceThreshold = config.GroundRemovalDistanceThreshold;
            config.GroundRemovalDistanceThresholdChanged += (sender, args) => {_distanceThreshold = config.GroundRemovalDistanceThreshold; };
            
            _minValue = KinectSensorContext.MinDepthValue;

            _maxSamples = maxSamples;

            _distanceConverter = new CoordinateSystemConverter(context);

            _arrayLock = new Mutex();
        }

        public void AddPoint(double x, double y, double z)
        {
            if (_points.Count == _maxSamples)
                _points.RemoveAt(0);

            var point = _distanceConverter.ToXyz(x, y, z, _h, _w);
            _points.Add(point);

            if(_points.Count >= 3)
                ComputeMask();
        }
        public void AddPoint(Point3D p)
        {
            AddPoint(p.X, p.Y, p.Z);
        }

        private void ComputeMask()
        {
            _arrayLock.WaitOne();
            if (_points.Count < 3) // recheck needed, state might have changed since mutex release
            {
                _arrayLock.ReleaseMutex();
                return;
            }
            _groundMask = new short[_h,_w];

            var n = new Point3D(0, 0, 0);
            // Calculate a bunch of normal vectors, average them
            for (int i = 1; i < _points.Count-1; i++)
            {
                //compute vectors
                var a = _points.ElementAt(i+1) - _points.ElementAt(i);
                var b = _points.ElementAt(i-1) - _points.ElementAt(i);
                //cross product
                var r = new Point3D(a.Y*b.Z - a.Z*b.Y, a.Z*b.X - a.X*b.Z, a.X*b.Y - a.Y*b.X);
                if (r.Y < 0) { r.X *= -1; r.Y *= -1; r.Z *= -1; }
                n = (n + r);
            }

            n.X /= (_points.Count - 2);
            n.Y /= (_points.Count - 2);
            n.Z /= (_points.Count - 2);

            _normalVector = n;
            _p0 = _points.ElementAt(0); // center

            for (int j = 0; j < _h; j++)
            {
                for (int i = 0; i < _w; i++)
                {
                    //// new version uses exacte x,y,z coordinates
                    var p = _distanceConverter.ToXyz(i, j, 1,_h, _w); // the distance is unknown, we'll solve it.
                    var t = (n.X*_p0.X+n.Y*_p0.Y+n.Z*_p0.Z)/(n.X*p.X+n.Y*p.Y+n.Z*p.Z);
                    _groundMask[j,i] = (short)Math.Max(Math.Min(p.Z * t, short.MaxValue), 0);
                    //Classic version with onscreen coordinates
                    //_groundMask[j][i] = (short)Math.Max(Math.Min((((n.X * (i - c.X) + n.Y * (j - c.Y)) / (-1 * n.Z)) + c.Z), short.MaxValue), 0);
                }
            }
            _arrayLock.ReleaseMutex();
        }

        // fixed 3 point version
        //private void ComputeMask(Point3D p1, Point3D p2, Point3D p3)
        //{
        //    _groundMask = new short[_h][];
        //    for (int i = 0; i < _groundMask.Length; i++)
        //    {
        //        _groundMask[i] = new short[_w];
        //    }

        //    //compute vectors
        //    var a = p2 - p1;
        //    var b = p3 - p1;
        //    //cross product
        //    var n = new Point3D(a.Y*b.Z - a.Z*b.Y, a.Z*b.X - a.X*b.Z, a.X*b.Y - a.Y*b.X);

        //    // Equation is n.X(X-P1.X) + n.Y(Y-P1.Y) + n.Z(Z-P1.Z) = 0
        //    // OR ((n.X(X-P1.X) + n.Y(Y-P1.Y)) / -n.Z) + P1.Z = Z

        //    for (int j = 0; j < _h; j++)
        //    {
        //        for (int i = 0; i < _w; i++)
        //        {
        //            _groundMask[j][i] = (short)Math.Max(Math.Min((((n.X * (i - p1.X) + n.Y * (j - p1.Y)) / (-1 * n.Z)) + p1.Z), short.MaxValue),0);
        //            // Average Center version
        //            // _groundMask[j, i] = (short)Math.Max(Math.Min((((n.X * (i - (p1.X + p2.X + p3.X) / 3) + n.Y * (j - (p1.Y + p2.Y + p3.Y) / 3)) / (-1 * n.Z)) + (p1.Z + p2.Z + p3.Z) / 3), short.MaxValue), 0);

        //        }
        //    }
        //}

        public void ApplyMask(short[,] depth)
        {
            _arrayLock.WaitOne();
            var w = depth.GetLength(1);
            var h = depth.GetLength(0);

            if(w != _w) // dirty index changed: masks needs to be updated
            {
                _w = w;
                _h = h;

                // If the mask is null, it means the object has just been instanciated, the points exist, but the mask hasn't been generated yet.
                // Otherwise, the frame has been rotated: reset everything.
                if (_groundMask == null)
                {
                    ComputeMask();  
                }
                else
                {
                    _points.RemoveAll(r => true);
                    _groundMask = null; 
                }
            }

            if (_groundMask != null)
                for (int j = 0; j < _h; j++)
                {
                    for (int i = 0; i < _w; i++)
                    {
                        if (depth[j, i] > _minValue && Math.Abs(_groundMask[j,i] - depth[j, i]) <= _distanceThreshold)
                        {
                           depth[j, i] = 0;
                        }
                    }
                }
            _arrayLock.ReleaseMutex();
        }

        public void SaveConfig(string filename = "PlaneGroundRemover.cfg")
        {
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Create))
                {
                    var bin = new BinaryFormatter();
                    bin.Serialize(stream, this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File Serialization fail: " + e.Message);
            }
        }

        public bool IsCloseToGround(int onScreenX, int onScreenY, int distance, double threshold)
        {
            var point = _distanceConverter.ToXyz(onScreenX, onScreenY, distance, _h, _w);

            return Math.Abs(_normalVector.X*point.X + _normalVector.Y*point.Y + _normalVector.Z*point.Z)/
                   Math.Sqrt(_normalVector.X*_normalVector.X + _normalVector.Y*_normalVector.Y +
                             _normalVector.Z*_normalVector.Z) < threshold;
        }
    }
}
