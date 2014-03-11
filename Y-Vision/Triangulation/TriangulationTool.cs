﻿using System;
using System.Collections.Generic;

namespace Y_Vision.Triangulation
{
    public class TriangulationTool
    {

        protected class Point3D
        {
            public double X;
            public double Y;
            public double Z;

            public Point3D(double x, double y, double z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
        }

        private class ClusteredPoint
        {
            public readonly Point3D Point;
            public readonly double ClusteredValue;
		
            public ClusteredPoint(Point3D point, double clusteredValue)
            {
                this.Point = point;
                this.ClusteredValue = clusteredValue;
            }
        }

        protected int NbSensors;
        protected bool ThrowExceptions = true;
        private readonly List<Point3D>[] _points;
        protected Point3D[] SensorsPos;
        protected double[] SensorsAngle;
        private bool _dirty = true;

        // Based on http://paulbourke.net/geometry/circlesphere/tvoght.c Tim Voght, 3/26/2005
        private List<Point3D> circle_circle_intersection(double x0, double z0, double r0, double x1, double z1, double r1)
        {
            double a, dx, dz, d, h, rx, rz;
            double x2, z2;

            // dx and dy are the vertical and horizontal distances between the circle centers.
            dx = x1 - x0;
            dz = z1 - z0;

            // Determine the straight-line distance between the centers.
            d = Math.Sqrt((dz * dz) + (dx * dx));

            /* Check for solvability. */
            if (d > (r0 + r1))
            {
                /* no solution. circles do not intersect. */
                return null;
            }
            if (d < Math.Abs(r0 - r1))
            {
                /* no solution. one circle is contained in the other */
                return null;
            }

            /* 'point 2' is the point where the line through the circle
		 * intersection points crosses the line between the circle
		 * centers.  
		 */

            /* Determine the distance from point 0 to point 2. */
            a = ((r0*r0) - (r1*r1) + (d*d)) / (2.0 * d) ;
		
            /* Determine the coordinates of point 2. */
            x2 = x0 + (dx * a/d);
            z2 = z0 + (dz * a/d);

            /* Determine the distance from point 2 to either of the
		 * intersection points.
		 */
            h = Math.Sqrt((r0*r0) - (a*a));

            /* Now determine the offsets of the intersection points from
		 * point 2.
		 */
            rx = -dz * (h/d);
            rz = dx * (h/d);

            /* Determine the absolute intersection points. */
            Point3D point1 = new Point3D(x2 + rx, 0, z2 + rz);
            Point3D point2 = new Point3D(x2 - rx, 0, z2 - rz);

            List<Point3D> intersections = new List<Point3D>();
            intersections.Add(point1);
            // only consider point2 a different intersection if it is different as point1, give or take
            if (!(point2.X > point1.X - 0.01d && point2.X < point1.X + 0.01d && point2.Z > point1.Z - 0.01d && point2.Z < point1.Z + 0.01d))
            {
                intersections.Add(point2);
            }

            return intersections;
        }

        // will return a list of the N most clustered points in lPoints
        private List<Point3D> cluster(List<Point3D> lPoints, int n)
        {
            int nbPoints = lPoints.Count;
            List<ClusteredPoint> clusteringPoints = new List<ClusteredPoint>();

            for (int i=0; i < nbPoints; i++)
            {
                List<double> lDist = new List<double>();
                for (int j=0; j < nbPoints; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    // get the distance between every point A and every different point B
                    double distX = lPoints[i].X - lPoints[j].X;
                    double distZ = lPoints[i].Z - lPoints[j].Z;
                    lDist.Add(Math.Sqrt((distX * distX) + (distZ * distZ)));
                }

                // get the N-1 lowest distances and add them to get the clustering value of point A
                lDist.Sort();
                double sumDist = 0.0d;
                for (int k=0; k < n; k++)
                {
                    sumDist += lDist[k];
                }
                clusteringPoints.Add(new ClusteredPoint(lPoints[i], sumDist));
            }

            // get the N lowest clustered values
            clusteringPoints.Sort((a, b) => a.ClusteredValue.CompareTo(b.ClusteredValue));
            List<Point3D> returnCluster = new List<Point3D> ();
            for (int i=0; i < n; i++)
            {
                returnCluster.Add(clusteringPoints[i].Point);
            }

            return returnCluster;
        }

        private void CalculateSensorPos(int idSensor)
        {
            int nbPoints = _points[idSensor].Count;

            List<Point3D> intersections = new List<Point3D>();
            double x0, z0, r0, x1, z1, r1;

            for (int i=0; i < nbPoints; i++)
            {
                for (int j=i+1; j < nbPoints; j++)
                {
                    // always use the coordinate from the first sensor since it is the reference
                    x0 = _points[0][i].X;
                    z0 = _points[0][i].Z;
                    x1 = _points[0][j].X;
                    z1 = _points[0][j].Z;
                    // only the radius changes
                    r0 = Math.Sqrt((_points[idSensor][i].X * _points[idSensor][i].X) + (_points[idSensor][i].Z * _points[idSensor][i].Z));
                    r1 = Math.Sqrt((_points[idSensor][j].X * _points[idSensor][j].X) + (_points[idSensor][j].Z * _points[idSensor][j].Z));
                    List<Point3D> inters = circle_circle_intersection(x0, z0, r0, x1, z1, r1);
                    if (inters != null)
                    {
                        // should always be the case
                        intersections.AddRange(inters);
                    }
                }
            }

            /*Debug.Log("Intersections for sensor " + idSensor);
		foreach(Point3D aPoint in intersections)
		{
			Debug.Log(aPoint.x + "/" + aPoint.y + "/" + aPoint.z);
		}*/

            if (nbPoints > intersections.Count)
            {
                nbPoints = intersections.Count;
            }

            List<Point3D> clusteredPoints = cluster(intersections, nbPoints);

            // Calculate the mean value of the cluster
            Point3D sumOfCluster = new Point3D(0.0d, 0.0d, 0.0d);
            for (int i=0; i < nbPoints; i++)
            {
                sumOfCluster.X += clusteredPoints[i].X;
                sumOfCluster.Y += clusteredPoints[i].Y;
                sumOfCluster.Z += clusteredPoints[i].Z;
            }
            SensorsPos[idSensor].X = sumOfCluster.X / (double)nbPoints;
            SensorsPos[idSensor].Y = sumOfCluster.Y / (double)nbPoints;
            SensorsPos[idSensor].Z = sumOfCluster.Z / (double)nbPoints;
        }

        private void CalculateTransformationMatrix(int idSensor)
        {
            if (idSensor == 0)
            {
                // no transformations needed
                return;
            }

            // angle between sensor 0 and it's first point
            //double angle1 = Math.Atan2(points[0][0].z - sensorsPos[0].z, points[0][0].x - sensorsPos[0].x) * (180/Math.PI);
            double angle1 = Math.Atan2(_points[0][0].Z - (SensorsPos[idSensor].Z - SensorsPos[0].Z), _points[0][0].X - (SensorsPos[idSensor].X - SensorsPos[0].X)) * (180/Math.PI);
            // angle between sensor idSensor and it's first point
            //double angle2 = Math.Atan2(points[idSensor][0].z - sensorsPos[idSensor].z, points[idSensor][0].x - sensorsPos[idSensor].x) * (180/Math.PI);
            double angle2 = Math.Atan2(_points[idSensor][0].Z, _points[idSensor][0].X) * (180/Math.PI);
            // difference
            SensorsAngle[idSensor] = angle1 - angle2;
        }

        // return true if still dirty
        private bool undirty()
        {
            // check if we have an equal number of points for each sensor
            int nbPoints = _points[0].Count;
            bool nbPointsOk = true;
            for (int i=1; i < NbSensors; i++)
            {
                if (nbPoints != _points[i].Count)
                {
                    nbPointsOk = false;
                    break;
                }
            }
            if (!nbPointsOk)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("There needs to be an equal number of points for each sensor");
                }
                else
                {
                    return true;
                }
            }
                // As of now, we need at least 3 points
            else if (nbPoints < 3)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("There needs to be at least 3 points for each sensor");
                }
                else
                {
                    return true;
                }
            }

            // calculate the position of each sensor
            // first we calculate the first one since it is the reference
            for (int i=0; i < NbSensors; i++)
            {
                CalculateSensorPos(i);
                CalculateTransformationMatrix(i);
            }

            return false;
        }





        // nbSensors is the number of cameras used
        public TriangulationTool(int nbSensors)
        {
            if (nbSensors < 1)
            {
                throw new ArgumentException("nbSensors must be at least 1");
            }

            this.NbSensors = nbSensors;
            _points = new List<Point3D>[nbSensors];
            SensorsPos = new Point3D[nbSensors];
            SensorsAngle = new double[nbSensors];
            for (int i=0; i < nbSensors; i++)
            {
                _points[i] = new List<Point3D>();
                SensorsPos[i] = new Point3D(0.0d, 0.0d, 0.0d);
            }
        }

        public void EnableExceptionThrowing()
        {
            ThrowExceptions = true;
        }

        public void DisableExceptionThrowing()
        {
            ThrowExceptions = false;
        }

        public virtual void Reset()
        {
            for (int i=0; i < NbSensors; i++)
            {
                _points[i].Clear();
            }

            _dirty = true;
        }

        // add a 3D point to help triangulate the sensors
        // idSensor is the sensor used to compute the 3D point, on base 0
        public virtual void AddTriangulationPoint(int idSensor, double x, double y, double z)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return;
                }
            }

            _points[idSensor].Add(new Point3D(x, y, z));

            _dirty = true;
        }

        public virtual double GetSensorPosX(int idSensor)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return 0.0d;
                }
            }

            if (_dirty)
            {
                if (_dirty = undirty())
                {
                    return 0.0d;
                }
            }

            return SensorsPos[idSensor].X;
        }

        public virtual double GetSensorPosY(int idSensor)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return 0.0d;
                }
            }
		
            if (_dirty)
            {
                if (_dirty = undirty())
                {
                    return 0.0d;
                }
            }
		
            return SensorsPos[idSensor].Y;
        }

        public virtual double GetSensorPosZ(int idSensor)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return 0.0d;
                }
            }
		
            if (_dirty)
            {
                if (_dirty = undirty())
                {
                    return 0.0d;
                }
            }
		
            return SensorsPos[idSensor].Z;
        }

        public virtual double GetSensorAngle(int idSensor)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return 0.0d;
                }
            }

            if (_dirty)
            {
                if (_dirty = undirty())
                {
                    return 0.0d;
                }
            }

            return SensorsAngle[idSensor];
        }

        // use this to get a coordinate based on the config
        public static void GetNormalizedCoordinates(double sensorX, double sensorY, double sensorZ, double sensorAngle, ref double x, ref double y, ref double z)
        {
            double newX, newZ;
            double angle = Math.PI * sensorAngle / 180.0;

            /*newX = Math.Cos(angle) * x - Math.Sin(angle) * z + sensorX * (1 - Math.Cos(angle)) + sensorZ * Math.Sin(angle);
		newZ = Math.Sin(angle) * x + Math.Cos(angle) * z - sensorX * Math.Sin(angle) + sensorZ * (1 - Math.Cos(angle));*/

            newX = Math.Cos(angle) * x - Math.Sin(angle) * z + sensorX;
            newZ = Math.Sin(angle) * x + Math.Cos(angle) * z + sensorZ;

            x = newX;
            z = newZ;
        }

        // use this to get a coordinate after the triangulation
        public virtual void GetNormalizedCoordinates(int idSensor, ref double x, ref double y, ref double z)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return;
                }
            }
		
            if (idSensor == 0)
            {
                // coordinates are already mapped for the first sensor
                return;
            }

            if (_dirty)
            {
                if (_dirty = undirty())
                {
                    return;
                }
            }

            GetNormalizedCoordinates(SensorsPos[idSensor].X, SensorsPos[idSensor].Y, SensorsPos[idSensor].Z, SensorsAngle[idSensor], ref x, ref y, ref z);
        }

    }
}
















