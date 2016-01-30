using System;
using System.IO;
using System.Xml.Serialization;

namespace RoadsUnited
{
    public class Configuration
    {

        public bool use_custom_textures = true;
        public bool use_custom_colours = true;


        public float basic_road_brightness = 0.3f;
        public float basic_road_elevated_brightness = 0.3f;
        public float basic_road_bridge_brightness = 0.3f;

        public float basic_road_decoration_grass_brightness = 0.3f;
        public float basic_road_decoration_trees_brightness = 0.3f;
        public float basic_road_bicycle_brightness = 0.3f;
        public float basic_road_bicycle_elevated_brightness = 0.3f;
        public float basic_road_bicycle_bridge_brightness = 0.3f;


        public float oneway_road_brightness = 0.3f;
        public float oneway_road_elevated_brightness = 0.3f;
        public float oneway_road_bridge_brightness = 0.3f;
        public float oneway_road_decoration_grass_brightness = 0.3f;
        public float oneway_road_decoration_trees_brightness = 0.3f;


        public float medium_road_brightness = 0.3f;
        public float medium_road_elevated_brightness = 0.3f;
        public float medium_road_bridge_brightness = 0.3f;
        public float medium_road_decoration_grass_brightness = 0.3f;
        public float medium_road_decoration_trees_brightness = 0.3f;
        public float medium_road_bicycle_brightness = 0.3f;
        public float medium_road_bicycle_elevated_brightness = 0.3f;
        public float medium_road_bicycle_bridge_brightness = 0.3f;
        public float medium_road_bus_brightness = 0.3f;



        public float large_road_brightness = 0.3f;
        public float large_road_decoration_grass_brightness = 0.3f;
        public float large_road_decoration_trees_brightness = 0.3f;
        public float large_road_bicycle_brightness = 0.3f;
        public float large_road_bus_brightness = 0.3f;

        public float large_oneway_brightness = 0.3f;
        public float large_oneway_decoration_grass_brightness = 0.3f;
        public float large_oneway_decoration_trees_brightness = 0.3f;

        public float highway_brightness = 0.3f;


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
