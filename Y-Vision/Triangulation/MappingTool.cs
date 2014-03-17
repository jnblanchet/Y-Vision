using System;
using Y_Vision.Core;

namespace Y_Vision.Triangulation
{
    public class MappingTool : CoordinateSystemTool
    {

        private int _leftPadding, _rightPadding, _displayWidth, _displayHeight, _displayDistanceFromGround;

        public MappingTool(TriangulationTool tool) {
            //double sensorX, double sensorY, double sensorZ, double sensorAngle
            NbSensors = tool.NbSensors;
            SensorsPos = new Point3D[tool.NbSensors];
            SensorsAngle = new double[tool.NbSensors];

            for (int i = 0; i < tool.NbSensors; i++)
            {
                SensorsPos[i].X = tool.SensorsPos[i].X;
                SensorsPos[i].Y = tool.SensorsPos[i].Y;
                SensorsPos[i].Z = tool.SensorsPos[i].Z;
                SensorsAngle[i] = tool.SensorsAngle[i];
            }
        }

        private double _a, _b, _c, _mod, _mmToDisplayRatio, _displayRatio; // cached constants
        public void InitializeProjection(int leftPadding, int rightPadding, int displayWidth, int displayHeight, int displayDistanceFromGround)
        {
            _leftPadding = leftPadding;
            _rightPadding = rightPadding;
            _displayWidth = displayWidth;
            _displayHeight = displayHeight;
            _displayDistanceFromGround = displayDistanceFromGround;

            // Cache algebra stuff
            // Display Plane (line) ax + bz + c = 0
            double x1 = GetSensorPosX(0), x2 = GetSensorPosX(1);
            double z1 = GetSensorPosZ(0), z2 = GetSensorPosZ(1);

            _a = (x2 - x1);
            _b = -(z2 - z1);
            _c = (-z2 + z1) * x1 + z1 * (x2 - x1);
            _mod = _a*_a + _b*_b;

            _mmToDisplayRatio = (_displayWidth + _leftPadding + _rightPadding) / Math.Sqrt(_a * _a + _b * _b);

            _displayRatio = (double) _displayWidth/_displayHeight;
        }

        public override double GetSensorPosX(int idSensor)
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
		
            return SensorsPos[idSensor].X;
        }
	
        public override double GetSensorPosY(int idSensor)
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
		
            return SensorsPos[idSensor].Y;
        }
	
        public override double GetSensorPosZ(int idSensor)
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
		
            return SensorsPos[idSensor].Z;
        }
	
        public override double GetSensorAngle(int idSensor)
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
		
            return SensorsAngle[idSensor];
        }

        // use this to get a coordinate after the triangulation
        public override Point3D? GetNormalizedCoordinates(int idSensor)
        {
            if (idSensor < 0 || idSensor >= NbSensors)
            {
                if (ThrowExceptions)
                {
                    throw new ArgumentException("idSensor must between 0 and " + (NbSensors-1));
                }
                else
                {
                    return null;
                }
            }
		
            if (idSensor == 0)
            {
                // coordinates are already mapped for the first sensor
                return null;
            }
		
            return GetNormalizedCoordinates(idSensor, SensorsPos[idSensor].X, SensorsPos[idSensor].Y, SensorsPos[idSensor].Z);
        }

        public void SetSensorConfig(int idSensor, double sensorX, double sensorY, double sensorZ, double sensorAngle)
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

            SensorsPos[idSensor].X = sensorX;
            SensorsPos[idSensor].Y = sensorY;
            SensorsPos[idSensor].Z = sensorZ;
            SensorsAngle[idSensor] = sensorAngle;
        }

        /// <summary>
        /// Takes a point in the global system (in mm) and projects it onto the display which is defined by the properties.
        /// </summary>
        /// <param name="point">a point in the global system.</param>
        /// <returns> a point in the new screen projection </returns>
        public Point3D ProjectPointOnDisplay(Point3D point)
        {
            double projX = (_b * (_b * point.X - _a * point.Z) - _a * _c) / (_mod);
            double projZ = (_a * (-_b * point.X + _a * point.Z) - _b * _c) / (_mod);

            double alpha = Math.Atan2(projZ, projX);
            double offSetX = Math.Cos(alpha) * _leftPadding;
            double offSetZ = Math.Sin(alpha) * _leftPadding;
            
            return new Point3D(projX + offSetX, (point.Y-_displayDistanceFromGround)*(_displayRatio), projZ + offSetZ) * _mmToDisplayRatio;
        }
    }
}
