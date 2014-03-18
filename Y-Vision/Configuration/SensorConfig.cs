using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y_Vision.Core;
using Y_Vision.SensorStreams;

namespace Y_Vision.Configuration
{
    /// <summary>
    /// TODO: translate
    /// Un ConfigurationManager sérialisé contient toutes les SensorConfig.
    /// Le calibration tool charge ce fichier au Lancement (ou l'instancie si inexistant).
    /// Un bouton sauvegarde permet de sérialiser le fichier.
    /// Chaque fois qu'un sensor est sélectionné la config appropriée est chargée (ou créée) et passée en paramètre au HumanDetectorPipeline.
    /// Le constructeur du HumanDetectorPipeline propage la config. Tous peuvent l'utiliser.
    /// Pour chaque paramètre, la config offre des événements permettant d'écouter les changements.
    /// Les classes concernées écoutent les événements et se mettent à jour en conséquence.
    /// </summary>
    [Serializable()]
    public class SensorConfig
    {
        public readonly string SensorId;
        internal List<Point3D> Ground;
        private int _rotationAngle;
        private double _groundRemovalDistanceThreshold;

        public SensorConfig(string uniqueId)
        {
            SensorId = uniqueId;
            Ground = new List<Point3D>();
            _rotationAngle = 0;
            _groundRemovalDistanceThreshold = 40;
        }

        public void RotateSensor()
        {
            _rotationAngle = DiscreteRotation.GetRotatedNext(Rotation).Angle;
        }

        // This method does not really add a point, it only triggers the event. The Point should be added to the list by the handler.
        // This is needed since there are required operations before the point can be added. (e.g. must be converted to another axis system).
        public void AddGroundPoint(Point3D p)
        {
            if (GroundPointsChanged != null)
                GroundPointsChanged.Invoke(this,new PointEventArgs(p));
        }

        public class PointEventArgs : EventArgs
        {
            public Point3D Point;
            public PointEventArgs(Point3D point)
            {
                Point = point;
            }
        }

        [field: NonSerialized]
        public event EventHandler<PointEventArgs> GroundPointsChanged;

        public DiscreteRotation Rotation
        {
            get { return DiscreteRotation.All.First(r => r.Angle == _rotationAngle); }
            internal set { _rotationAngle = value.Angle; }
        }

        [field: NonSerialized]
        public event EventHandler<EventArgs> GroundRemovalDistanceThresholdChanged;


        public double GroundRemovalDistanceThreshold
        {
            get { return _groundRemovalDistanceThreshold; }
            set {
                _groundRemovalDistanceThreshold = value;
                GroundRemovalDistanceThresholdChanged.Invoke(this,new EventArgs());
            }
        }
    }
}
