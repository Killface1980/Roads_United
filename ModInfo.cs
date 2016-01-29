using ICities;
using System;


namespace RoadsUnited
{
    public class RoadsUnitedMod : IUserMod
    {
        public const UInt64 workshop_id = 598151121;

        public string Name
        {
            get
            {
                return "Roads United";
            }
        }

        public string Description
        {
            get
            {
                return "Replaces road textures and other road feature with more European ones.";
            }
        }


        private void EventSmallRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventSmallRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_bicycle_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventSmallRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventSmallRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }


        private void EventMediumRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventReloadColor()
        {
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_brightness, "Basic Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_brightness, "Basic Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, "Basic Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, "Basic Road Decoration Trees", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_brightness, "Medium Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_brightness, "Large Road", RoadsUnitedModLoader.modPath);
            
        }

        private void EventResetColor()
        {
            RoadsUnitedModLoader.config.basic_road_brightness = 0.25f;
            RoadsUnitedModLoader.config.basic_road_bicycle_brightness = 0.25f;
            RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness = 0.25f;
            RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness = 0.25f;
            RoadsUnitedModLoader.config.medium_road_brightness = 0.25f;
            RoadsUnitedModLoader.config.large_road_brightness = 0.25f;
            RoadsUnitedModLoader.SaveConfig();
        }


        public void OnSettingsUI(UIHelperBase helper)
        {
            RoadsUnitedModLoader.config = Configuration.Deserialize(RoadsUnitedModLoader.configPath);
            if (RoadsUnitedModLoader.config == null)
            {
                RoadsUnitedModLoader.config = new Configuration();
            }
            RoadsUnitedModLoader.SaveConfig();
            UIHelperBase uIHelperSmallRoads = helper.AddGroup("Small Roads Settings");
            uIHelperSmallRoads.AddSlider("Normal Brightness", 0, 1f, 0.05f, RoadsUnitedModLoader.config.basic_road_brightness, new OnValueChanged(EventSmallRoadBrightness));
            uIHelperSmallRoads.AddSlider("Bike Brightness", 0, 1f, 0.05f, RoadsUnitedModLoader.config.basic_road_bicycle_brightness, new OnValueChanged(EventSmallRoadBicycleBrightness));
            uIHelperSmallRoads.AddSlider("Deco Grass Brightness", 0, 1f, 0.05f, RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, new OnValueChanged(EventSmallRoadDecorationGrassBrightness));
            uIHelperSmallRoads.AddSlider("Deco Trees Brightness", 0, 1f, 0.05f, RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, new OnValueChanged(EventSmallRoadDecorationTreesBrightness));
            UIHelperBase uIHelperMediumRoads = helper.AddGroup("Medium Roads Settings");
            uIHelperMediumRoads.AddSlider("Medium Road Brightness", 0, 1f, 0.05f, RoadsUnitedModLoader.config.medium_road_brightness, new OnValueChanged(EventMediumRoadBrightness));
            UIHelperBase uIHelperLargeRoads = helper.AddGroup("Large Roads Settings");
            uIHelperLargeRoads.AddSlider("Large Road Brightness", 0, 1f, 0.05f, RoadsUnitedModLoader.config.large_road_brightness, new OnValueChanged(EventLargeRoadBrightness));
            helper.AddButton("Reload roads", EventReloadColor);
            helper.AddButton("Revert to default on next level loading (sliders will not move)", EventResetColor);


        }


    }

}

