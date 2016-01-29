using System;
using System.IO;
using System.Xml.Serialization;

namespace RoadsUnited
{
    public class Configuration
    {
        public float highway_red = 0.25f;

        public float highway_green = 0.25f;

        public float highway_blue = 0.25f;

        public float large_road_red = 0.25f;

        public float large_road_green = 0.25f;

        public float large_road_blue = 0.25f;

        public float medium_road_red = 0.25f;

        public float medium_road_green = 0.25f;

        public float medium_road_blue = 0.25f;

        public float small_road_red = 0.25f;

        public float small_road_green = 0.25f;

        public float small_road_blue = 0.25f;

        public bool disable_optional_arrows = true;

        public bool use_alternate_pavement_texture = false;

        public bool use_cracked_roads = false;

        public float crackIntensity = 1f;

        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }

        public static void Serialize(string filename, Configuration config)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                config.OnPreSerialize();
                xmlSerializer.Serialize(streamWriter, config);
            }
        }

        public static Configuration Deserialize(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
            Configuration result;
            try
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    Configuration configuration = (Configuration)xmlSerializer.Deserialize(streamReader);
                    configuration.OnPostDeserialize();
                    result = configuration;
                    return result;
                }
            }
            catch
            {
            }
            result = null;
            return result;
        }
    }

}
