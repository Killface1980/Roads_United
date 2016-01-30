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
        }

        private void EventSmallRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_bicycle_brightness = c;
        }

        private void EventSmallRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness = c;
        }

        private void EventSmallRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness = c;
        }


        private void EventOnewayRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_brightness = c;
        }

        private void EventOnewayRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness = c;
        }

        private void EventOnewayRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness = c;
        }



        private void EventMediumRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_brightness = c;
        }

        private void EventMediumRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bicycle_brightness = c;
        }

        private void EventMediumRoadBusBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bus_brightness = c;
        }


        private void EventMediumRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness = c;
        }

        private void EventMediumRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness = c;
        }

        private void EventLargeRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_brightness = c;
        }

        private void EventLargeRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bicycle_brightness = c;
        }

        private void EventLargeRoadBusBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bus_brightness = c;
        }


        private void EventLargeRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_decoration_grass_brightness = c;
        }

        private void EventLargeRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_decoration_trees_brightness = c;
        }

        private void EventLargeOnewayBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_brightness = c;
        }

        private void EventLargeOnewayDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_decoration_grass_brightness = c;
        }

        private void EventLargeOnewayDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_decoration_trees_brightness = c;
        }

        private void EventHighwayBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_brightness = c;
        }

        private void EventReloadColor()
        {
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_brightness, "Basic Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_brightness, "Basic Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, "Basic Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, "Basic Road Decoration Trees", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Elevated Bike", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_brightness, "Oneway Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_brightness, "Oneway Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness, "Oneway Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness, "Oneway Road Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_brightness, "Medium Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_brightness, "Medium Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness, "Medium Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness, "Medium Road Decoration Trees", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_brightness, "Medium Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_brightness, "Medium Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bus_brightness, "Medium Road Bus", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bus_brightness, "Medium Road Elevated Bus", RoadsUnitedModLoader.modPath);


            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_brightness, "Large Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_brightness, "Large Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_decoration_grass_brightness, "Large Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_decoration_trees_brightness, "Large Road Decoration Trees", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bicycle_brightness, "Large Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bicycle_brightness, "Large Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bus_brightness, "Large Road Bus", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bus_brightness, "Large Road Elevated Bus", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_brightness, "Large Oneway", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_brightness, "Large Oneway Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_decoration_grass_brightness, "Large Oneway Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_decoration_trees_brightness, "Large Oneway Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_brightness, "Highway", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_brightness, "Highway Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_brightness, "HighwayRamp", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_brightness, "Highway Barrier", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_brightness, "HighwayRampElevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_brightness, "Highway Bridge", RoadsUnitedModLoader.modPath);
        }

        private void EventResetColor()
        {
            RoadsUnitedModLoader.config.basic_road_brightness = 0.3f;
            RoadsUnitedModLoader.config.basic_road_bicycle_brightness = 0.3f;
            RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness = 0.3f;
            RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness = 0.3f;

            RoadsUnitedModLoader.config.medium_road_brightness = 0.3f;

            RoadsUnitedModLoader.config.large_road_brightness = 0.3f;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventSaveSettings()
        {
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
            UIHelperBase uIHelperSmallRoads = helper.AddGroup("Small Roads Brightness Settings");
            uIHelperSmallRoads.AddSlider("Basic Road", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_brightness, new OnValueChanged(EventSmallRoadBrightness));
            uIHelperSmallRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, new OnValueChanged(EventSmallRoadDecorationGrassBrightness));
            uIHelperSmallRoads.AddSlider("Decoration Trees", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, new OnValueChanged(EventSmallRoadDecorationTreesBrightness));
            uIHelperSmallRoads.AddSlider("Basic Road Bikelane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_bicycle_brightness, new OnValueChanged(EventSmallRoadBicycleBrightness));

            UIHelperBase uIHelperOnewayRoads = helper.AddGroup("Oneway Brightness Settings");
            uIHelperOnewayRoads.AddSlider("Oneway Road", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_brightness, new OnValueChanged(EventOnewayRoadBrightness));
            uIHelperOnewayRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness, new OnValueChanged(EventOnewayRoadDecorationGrassBrightness));
            uIHelperOnewayRoads.AddSlider("Decoration Trees ", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness, new OnValueChanged(EventOnewayRoadDecorationTreesBrightness));

            UIHelperBase uIHelperMediumRoads = helper.AddGroup("Medium Roads Brightness Settings");
            uIHelperMediumRoads.AddSlider("Medium Road", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_brightness, new OnValueChanged(EventMediumRoadBrightness));
            uIHelperMediumRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness, new OnValueChanged(EventMediumRoadDecorationGrassBrightness));
            uIHelperMediumRoads.AddSlider("Decoration Trees", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness, new OnValueChanged(EventMediumRoadDecorationTreesBrightness));
            uIHelperMediumRoads.AddSlider("Medium Road Bikelane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_bicycle_brightness, new OnValueChanged(EventMediumRoadBicycleBrightness));
            uIHelperMediumRoads.AddSlider("Medium Road Buslane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_bus_brightness, new OnValueChanged(EventMediumRoadBusBrightness));

            UIHelperBase uIHelperLargeRoads = helper.AddGroup("Large Roads Settings");
            uIHelperLargeRoads.AddSlider("Large Road Brightness", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_road_brightness, new OnValueChanged(EventLargeRoadBrightness));
            uIHelperLargeRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_road_decoration_grass_brightness, new OnValueChanged(EventLargeRoadDecorationGrassBrightness));
            uIHelperLargeRoads.AddSlider("Decoration Trees", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_road_decoration_trees_brightness, new OnValueChanged(EventLargeRoadDecorationTreesBrightness));
            uIHelperLargeRoads.AddSlider("Large Road Bikelane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_road_bicycle_brightness, new OnValueChanged(EventLargeRoadBicycleBrightness));
            uIHelperLargeRoads.AddSlider("Large Road Buslane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_road_bus_brightness, new OnValueChanged(EventLargeRoadBusBrightness));

            UIHelperBase uIHelperLargeOneway = helper.AddGroup("Large Oneway Brightness Settings");
            uIHelperLargeOneway.AddSlider("Large Oneway", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_oneway_brightness, new OnValueChanged(EventLargeOnewayBrightness));
            uIHelperLargeOneway.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_oneway_decoration_grass_brightness, new OnValueChanged(EventLargeOnewayDecorationGrassBrightness));
            uIHelperLargeOneway.AddSlider("Decoration Trees ", 0, 1f, 0.1f, RoadsUnitedModLoader.config.large_oneway_decoration_trees_brightness, new OnValueChanged(EventLargeOnewayDecorationTreesBrightness));

            UIHelperBase uIHelperHighways = helper.AddGroup("Highways Brightness Settings");
            uIHelperHighways.AddSlider("Highway", 0, 1f, 0.1f, RoadsUnitedModLoader.config.highway_brightness, new OnValueChanged(EventHighwayBrightness));




            helper.AddButton("Reload roads", EventReloadColor);
            helper.AddButton("Save Settings", EventSaveSettings);

            //           helper.AddButton("Revert to default on next level loading (sliders will not move)", EventResetColor);


        }


    }

}

