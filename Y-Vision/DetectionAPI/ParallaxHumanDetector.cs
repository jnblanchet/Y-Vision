using System;
using System.Collections.Generic;
using System.Linq;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.BlobDescriptor;
using Y_Vision.Configuration;
using Y_Vision.GroundRemoval;
using Y_Vision.PipeLine;
using Y_Vision.Tracking;
using Y_Vision.Triangulation;

namespace Y_Vision.DetectionAPI
{
    public class ParallaxHumanDetector : HumanDetector
    {
        private readonly ConfigurationManager _manager;
        private readonly SensorConfig _firstConfig, _secondConfig;
        private readonly HumanDetectorPipeline _firstDetector, _secondDetector;
        private readonly MappingTool _mappingTool;
        private volatile bool _frameReadyFirst = false, _frameReadySecond = false;
        private readonly BranchAndBoundMatcher _matcher; // Used to merge the two sensors (no notion of tracking/persistence)
        private readonly BranchAndBoundTracker _tracker; // Used to tracked the resulting merged objects (with persistence)

        //TODO: remove the optionnal values and include a ui to select options
        public ParallaxHumanDetector(ConfigurationManager config, string firstSensor = null, string secondSensor = null)
        {
            //load config
            if (config == null)
            {
                _manager = new ConfigurationManager();
            }
            else
            {
                _manager = config;
            }
            // Load parallax config
            if(firstSensor == null || secondSensor == null)
            {
                try
                {
                    firstSensor = _manager.ParallaxConfig.GetSensorId()[0];
                    secondSensor = _manager.ParallaxConfig.GetSensorId()[1];
                }catch(Exception)
                {
                    throw new NullReferenceException("No valid config file was found!");
                }
            }

            // Create MappingTool: a Triangulation tool must first be created since only the calibration points are available in the config file (TODO: FIX)
            var t = new TriangulationTool(2);
            var pointsFirst = _manager.ParallaxConfig.Get3DPoints(firstSensor);
            var pointsSecond = _manager.ParallaxConfig.Get3DPoints(secondSensor);
            foreach (var p in pointsFirst)
                t.AddTriangulationPoint(0, p.X, p.Y, p.Z);
            foreach (var p in pointsSecond)
                t.AddTriangulationPoint(1, p.X, p.Y, p.Z);

            _mappingTool = new MappingTool(t);
            _mappingTool.InitializeProjection(_manager.ParallaxConfig.LeftPadding,
                                                _manager.ParallaxConfig.DisplayWidth,
                                                _manager.ParallaxConfig.RightPadding,
                                                _manager.ParallaxConfig.DisplayHeight,
                                                _manager.ParallaxConfig.DisplayDistanceFromGround);

            // Create pipelines
            _firstConfig = _manager.GetConfigById(firstSensor);
            _secondConfig = _manager.GetConfigById(secondSensor);

            _firstDetector = new HumanDetectorPipeline(_firstConfig);
            _firstDetector.BlobFactory = new BlobFactory { Context = _firstDetector.Context, Conv = new CoordinateSystemConverter(_firstDetector.Context), MappingTool = _mappingTool, SensorId = 0};
            _secondDetector = new HumanDetectorPipeline(_secondConfig);
            _secondDetector.BlobFactory = new BlobFactory { Context = _secondDetector.Context, Conv = new CoordinateSystemConverter(_secondDetector.Context), MappingTool = _mappingTool, SensorId = 1 };

            //const int maxTrackingDistancePixelSystem = 100000; // For now, the human tracking pipeline system still uses pixels for tracking as it is more precise
            const int maxTrackingDistanceMmSystem = 1000000; // Roughly equivalent to 1m

            _matcher = new BranchAndBoundMatcher(maxTrackingDistanceMmSystem);
            _tracker = new BranchAndBoundTracker(maxTrackingDistanceMmSystem);

            _firstDetector.DetectionUpdate += (sender, args) => { _frameReadyFirst = true; DetectorOnDetectionUpdate(); };
            _secondDetector.DetectionUpdate += (sender, args) => { _frameReadySecond = true; DetectorOnDetectionUpdate(); };


        }

        private void DetectorOnDetectionUpdate()
        {
            if (!_frameReadyFirst || !_frameReadySecond)
                return;

            var firstResults = _firstDetector.DepthTrackedObjects;
            var secondResults = _secondDetector.DepthTrackedObjects;

            if (firstResults == null || secondResults == null)
                return;

            var combinedObjects = _matcher.GenerateMatches(firstResults, secondResults);

            var trackedCombinedObject = _tracker.TrackObjects(combinedObjects);

            // Merge both detector sources
            var alreadyHandled = new List<Person>();
            foreach (var person in trackedCombinedObject)
            {
                var result = DetectedPeople.Find(p => p.UniqueId == person.UniqueId);
                if (result == null)
                {
                    var newPerson = new ParallaxTrackedPerson(person,_mappingTool);
                    alreadyHandled.Add(newPerson);
                    DetectedPeople.Add(newPerson);
                    if (PersonEnter != null)
                        PersonEnter.Invoke(this, new PersonEventArgs(newPerson));
                }
                else
                {
                    alreadyHandled.Add(result);
                    if (PersonUpdated != null)
                        PersonUpdated.Invoke(this, new PersonEventArgs(result));
                }

            }
            var deleted = DetectedPeople.Except(alreadyHandled).ToList();
            foreach (var old in deleted)
            {
                DetectedPeople.Remove(old);
                if (PersonLeft != null)
                    PersonLeft.Invoke(this, new PersonEventArgs(old));
            }
            if (AllPeopleUpdated != null)
                AllPeopleUpdated.Invoke(this, new EventArgs());

            // Flag next frame available
            _frameReadyFirst = false;
            _frameReadySecond = false;
        }

        public override void Start()
        {
            _firstDetector.Start();
            _secondDetector.Start();
        }

        public override void Stop()
        {
            _firstDetector.Stop();
            _secondDetector.Stop();
        }

        public override event EventHandler<PersonEventArgs> PersonEnter;
        public override event EventHandler<PersonEventArgs> PersonLeft;
        public override event EventHandler<PersonEventArgs> PersonUpdated;
        public override event EventHandler<EventArgs> AllPeopleUpdated;
    }
}
