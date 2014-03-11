using System;
using Y_Vision.Core;

namespace Y_Vision.Triangulation
{
    public class MappingTool : TriangulationTool
    {

        public MappingTool(int nbSensors) : base(nbSensors)
        {

        }

        public override void Reset()
        {
                    
        }

        public override void AddTriangulationPoint(int idSensor, double x, double y, double z)
        {

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
		
            return GetNormalizedCoordinates(SensorsPos[idSensor].X, SensorsPos[idSensor].Y, SensorsPos[idSensor].Z, SensorsAngle[idSensor]);
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

    }
}
