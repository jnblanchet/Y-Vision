using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.Configuration;
using Y_Vision.PipeLine;
using Y_Vision.Triangulation;

namespace Y_Vision.DetectionAPI
{
    public class ParallaxHumanDetector : HumanDetector
    {
        private readonly ConfigurationManager _configManager;
        private readonly SensorConfig _firstConfig, _secondConfig;
        private readonly HumanDetectorPipeline _firstDetector, _secondDetector;
        private readonly MappingTool _mappingTool;
        private bool _frameReadyFirst = false, _frameReadySecond = false;

        public ParallaxHumanDetector(ConfigurationManager manager, string firstSensor, string secondSensor)
        {
            _configManager = manager;

            // Create pipelines
            _firstConfig = manager.GetConfigById(firstSensor);
            _secondConfig = manager.GetConfigById(secondSensor);

            _firstDetector = new HumanDetectorPipeline(_firstConfig);
            _secondDetector = new HumanDetectorPipeline(_secondConfig);

            // Create MappingTool: a Triangulation tool must first be created since only the calibration points are available in the config file (TODO: FIX)
            var t = new TriangulationTool(2);
            var pointsFirst = _configManager.ParallaxConfig.Get3DPoints(firstSensor);
            var pointsSecond = _configManager.ParallaxConfig.Get3DPoints(secondSensor);
            foreach (var p in pointsFirst)
                t.AddTriangulationPoint(0, p.X, p.Y, p.Z);
            foreach (var p in pointsSecond)
                t.AddTriangulationPoint(1, p.X, p.Y, p.Z);

            _mappingTool = new MappingTool(t);

            // TODO: Map events to a banch and bound pipeline or a tracker or something so the items can be merged
            _firstDetector.DetectionUpdate += (sender, args) => { _frameReadyFirst = true; DetectorOnDetectionUpdate(); };
            _secondDetector.DetectionUpdate += (sender, args) => { _frameReadySecond = true; DetectorOnDetectionUpdate(); };


        }

        private void DetectorOnDetectionUpdate()
        {
            if (!_frameReadyFirst || !_frameReadySecond)
                return;

            // Merge both detector sources

            /*var alreadyHandled = new List<Person>();
            foreach (var person in _detector.DepthTrackedObjects)
            {
                var result = DetectedPeople.Find(p => p.UniqueId == person.UniqueId);
                if (result == null)
                {
                    var newPerson = new TrackedPerson(person);
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
                AllPeopleUpdated.Invoke(this, new EventArgs());*/

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
    }
}
