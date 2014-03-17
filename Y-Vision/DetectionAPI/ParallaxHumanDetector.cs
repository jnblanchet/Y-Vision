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
        private readonly ConfigurationManager _configManager;
        private readonly SensorConfig _firstConfig, _secondConfig;
        private readonly HumanDetectorPipeline _firstDetector, _secondDetector;
        private readonly MappingTool _mappingTool;
        private volatile bool _frameReadyFirst = false, _frameReadySecond = false;
        private readonly BranchAndBoundMatcher _matcher; // Used to merge the two sensors (no notion of tracking/persistence)
        private readonly BranchAndBoundTracker _tracker; // Used to tracked the resulting merged objects (with persistence)

        public ParallaxHumanDetector(ConfigurationManager manager, string firstSensor, string secondSensor)
        {
            _configManager = manager;

            // Create MappingTool: a Triangulation tool must first be created since only the calibration points are available in the config file (TODO: FIX)
            var t = new TriangulationTool(2);
            var pointsFirst = _configManager.ParallaxConfig.Get3DPoints(firstSensor);
            var pointsSecond = _configManager.ParallaxConfig.Get3DPoints(secondSensor);
            foreach (var p in pointsFirst)
                t.AddTriangulationPoint(0, p.X, p.Y, p.Z);
            foreach (var p in pointsSecond)
                t.AddTriangulationPoint(1, p.X, p.Y, p.Z);

            _mappingTool = new MappingTool(t);
            _mappingTool.InitializeProjection(manager.ParallaxConfig.LeftPadding,
                                                manager.ParallaxConfig.DisplayWidth,
                                                manager.ParallaxConfig.RightPadding,
                                                manager.ParallaxConfig.DisplayHeight,
                                                manager.ParallaxConfig.DisplayDistanceFromGround);

            // Create pipelines
            _firstConfig = manager.GetConfigById(firstSensor);
            _secondConfig = manager.GetConfigById(secondSensor);

            _firstDetector = new HumanDetectorPipeline(_firstConfig);
            _firstDetector.BlobFactory = new BlobFactory { Context = _firstDetector.Context, Conv = new CoordinateSystemConverter(_firstDetector.Context), MappingTool = _mappingTool, SensorId = 0};
            _secondDetector = new HumanDetectorPipeline(_secondConfig);
            _firstDetector.BlobFactory = new BlobFactory { Context = _secondDetector.Context, Conv = new CoordinateSystemConverter(_secondDetector.Context), MappingTool = _mappingTool, SensorId = 1 };

            _matcher = new BranchAndBoundMatcher();
            _tracker = new BranchAndBoundTracker();

            // TODO: Map events to a banch and bound pipeline or a tracker or something so the items can be merged
            _firstDetector.DetectionUpdate += (sender, args) => { _frameReadyFirst = true; DetectorOnDetectionUpdate(); };
            _secondDetector.DetectionUpdate += (sender, args) => { _frameReadySecond = true; DetectorOnDetectionUpdate(); };


        }

        private void DetectorOnDetectionUpdate()
        {
            if (!_frameReadyFirst || !_frameReadySecond)
                return;

            var firstResults = _firstDetector.DepthTrackedObjects;
            var secondResults = _firstDetector.DepthTrackedObjects;
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
