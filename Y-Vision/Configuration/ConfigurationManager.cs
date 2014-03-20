using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Y_Vision.Core;

namespace Y_Vision.Configuration
{
    [Serializable()]
    public class ConfigurationManager
    {
        public const String DefaultName = "Y-Vision.cfg";
        public List<SensorConfig> SensorConfigurations { private set; get; }
        public ParallaxConfig ParallaxConfig { private set; get; }

        public SensorConfig GetConfigById(string uniqueSensorId)
        {
            var r = SensorConfigurations.Find(s => s.SensorId == uniqueSensorId);
            if(r == null)
            {
                r = new SensorConfig(uniqueSensorId);
                SensorConfigurations.Add(r);
            }
            return r;
        }

        public ConfigurationManager(string savedConfigFile = DefaultName)
        {
            try
            {
                using (Stream stream = File.Open(savedConfigFile, FileMode.Open))
                {
                    var bin = new BinaryFormatter();

                    var loadedConfigManager = (ConfigurationManager)bin.Deserialize(stream);

                    SensorConfigurations = loadedConfigManager.SensorConfigurations;
                    ParallaxConfig = loadedConfigManager.ParallaxConfig ?? new ParallaxConfig();
                }
            }
            catch (Exception e)
            {
                SensorConfigurations = new List<SensorConfig>();
                ParallaxConfig = new ParallaxConfig();
                Console.WriteLine("Unable to load planeGroundRemover config: " + e.Message);
            }
        }

        /// <summary>
        /// This method will attempt to serialize config. It may throw an exception!
        /// </summary>
        /// <param name="filename"></param>
        public void SaveConfig(string filename = DefaultName)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                var bin = new BinaryFormatter();
                bin.Serialize(stream, this);
            }
        }

        public string[] GetSensorId()
        {
            return SensorConfigurations.Select(c => c.SensorId).ToArray();
        }
    }
}
