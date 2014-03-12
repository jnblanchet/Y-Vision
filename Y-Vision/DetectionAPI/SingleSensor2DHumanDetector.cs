using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.Configuration;
using Y_Vision.PipeLine;

namespace Y_Vision.DetectionAPI
{
    public class SingleSensor2DHumanDetector : HumanDetector
    {
        private readonly SensorConfig _config;
        private readonly HumanDetectorPipeline _detector;

        public SingleSensor2DHumanDetector(SensorConfig config = null)
        {
            try
            {
                if (config == null)
                {
                    var configs = new ConfigurationManager();
                    _config = configs.GetConfigById(null);
                }
                else
                {
                    _config = config;
                }
                _detector = new HumanDetectorPipeline(_config);
                _detector.DetectionUpdate += DetectorOnDetectionUpdate;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Default configuration file not found! Use the CalibrationBoard to generate one.");
                //_detector = new HumanDetectorPipeline(new SensorConfig(null));
                //_detector.DetectionUpdate += DetectorOnDetectionUpdate;
            }
            catch (Exception e)
            {
                throw new Exception("Error : " + e.Message);
            }

            DetectedPeople = new List<Person>();
        }

        public override void Start()
        {
            _detector.Start();
        }

        public override void Stop()
        {
            _detector.Stop();
        }

        // All the data is merged here
        private void DetectorOnDetectionUpdate(object sender, EventArgs eventArgs)
        {
            if (_detector.DepthTrackedObjects == null)
                return;

            var alreadyHandled = new List<Person>();
            foreach (var person in _detector.DepthTrackedObjects)
            {
                var result = DetectedPeople.Find(p => p.UniqueId == person.UniqueId);
                if(result == null)
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
                AllPeopleUpdated.Invoke(this, new EventArgs());
        }

        public override event EventHandler<PersonEventArgs> PersonEnter;
        public override event EventHandler<PersonEventArgs> PersonLeft;
        public override event EventHandler<PersonEventArgs> PersonUpdated;
        public override event EventHandler<EventArgs> AllPeopleUpdated;

    }
}
