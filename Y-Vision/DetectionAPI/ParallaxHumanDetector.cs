using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Y_API.DetectionAPI;
using Y_Vision.Configuration;
using Y_Vision.PipeLine;

namespace Y_Vision.DetectionAPI
{
    public class ParallaxHumanDetector : HumanDetector
    {
        private readonly SensorConfig _config;
        private readonly HumanDetectorPipeline _detector;

        public ParallaxHumanDetector(SensorConfig FirstConfig, SensorConfig SecondConfig)
        {

        }

        public override void Start()
        {
            _detector.Start();
        }

        public override void Stop()
        {
            _detector.Stop();
        }

        public override event EventHandler<PersonEventArgs> PersonEnter;
        public override event EventHandler<PersonEventArgs> PersonLeft;
        public override event EventHandler<PersonEventArgs> PersonUpdated;
    }
}
