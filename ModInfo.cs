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

        #region Small road config

        private void EventSmallRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }
        private void EventSmallRoadElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventSmallRoadBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }


        private void EventSmallRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_bicycle_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }
        private void EventSmallRoadBicycleElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_bicycle_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventSmallRoadBicycleBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.basic_road_bicycle_bridge_brightness = c;
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
        #endregion

        #region Oneway config
        private void EventOnewayRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventOnewayRoadElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventOnewayRoadBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventOnewayRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventOnewayRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        #endregion

        #region Medium roads config
        private void EventMediumRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bicycle_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadBicycleElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bicycle_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadBicycleBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bicycle_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadBusBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bus_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }


        private void EventMediumRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventMediumRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        #endregion

        #region Large road config

        private void EventLargeRoadBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventLargeRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bicycle_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventLargeRoadBusBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bus_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }


        private void EventLargeRoadDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_decoration_grass_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventLargeRoadDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_decoration_trees_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        #endregion

        #region Large oneway

        private void EventLargeOnewayBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventLargeOnewayDecorationGrassBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_decoration_grass_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventLargeOnewayDecorationTreesBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_decoration_trees_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        #endregion

        #region Highway config

        private void EventHighwayBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_brightness = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        #endregion

        #region Config stuff

        private void EventCheckUseCustomTextures(bool c)
        {
            RoadsUnitedModLoader.config.use_custom_textures = c;
            RoadsUnitedModLoader.SaveConfig();

        }

        private void EventCheckUseCustomColours(bool c)
        {
            RoadsUnitedModLoader.config.use_custom_colours = c;
            RoadsUnitedModLoader.SaveConfig();

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

        #endregion

        public void OnSettingsUI(UIHelperBase helper)
        {
            RoadsUnitedModLoader.config = Configuration.Deserialize(RoadsUnitedModLoader.configPath);
            if (RoadsUnitedModLoader.config == null)
            {
                RoadsUnitedModLoader.config = new Configuration();
            }
            RoadsUnitedModLoader.SaveConfig();

            UIHelperBase uIHelperGeneralSettings = helper.AddGroup("General Settings");
            uIHelperGeneralSettings.AddCheckbox("Use mods Vanilla roads texture replacements", RoadsUnitedModLoader.config.use_custom_textures, EventCheckUseCustomTextures);
            uIHelperGeneralSettings.AddCheckbox("Use mods road brightness sliders", RoadsUnitedModLoader.config.use_custom_colours, EventCheckUseCustomColours);
            helper.AddButton("Reload roads", EventReloadColor);

            UIHelperBase uIHelperSmallRoads = helper.AddGroup("Small Roads Brightness Settings");
            uIHelperSmallRoads.AddSlider("Basic Road", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_brightness, new OnValueChanged(EventSmallRoadBrightness));
            uIHelperSmallRoads.AddSlider("Basic Elevated", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_elevated_brightness, new OnValueChanged(EventSmallRoadElevatedBrightness));
            uIHelperSmallRoads.AddSlider("Basic Bridge", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_bridge_brightness, new OnValueChanged(EventSmallRoadBridgeBrightness));
            uIHelperSmallRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, new OnValueChanged(EventSmallRoadDecorationGrassBrightness));
            uIHelperSmallRoads.AddSlider("Decoration Trees", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, new OnValueChanged(EventSmallRoadDecorationTreesBrightness));
            uIHelperSmallRoads.AddSlider("Basic Road Bikelane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_bicycle_brightness, new OnValueChanged(EventSmallRoadBicycleBrightness));
            uIHelperSmallRoads.AddSlider("Bikelane Elevated", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_bicycle_elevated_brightness, new OnValueChanged(EventSmallRoadBicycleElevatedBrightness));
            uIHelperSmallRoads.AddSlider("Bikelane Bridge", 0, 1f, 0.1f, RoadsUnitedModLoader.config.basic_road_bicycle_bridge_brightness, new OnValueChanged(EventSmallRoadBicycleBridgeBrightness));


            UIHelperBase uIHelperOnewayRoads = helper.AddGroup("Oneway Brightness Settings");
            uIHelperOnewayRoads.AddSlider("Oneway Road", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_brightness, new OnValueChanged(EventOnewayRoadBrightness));
            uIHelperOnewayRoads.AddSlider("Oneway Elevated", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_elevated_brightness, new OnValueChanged(EventOnewayRoadElevatedBrightness));
            uIHelperOnewayRoads.AddSlider("Oneway Bridge", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_bridge_brightness, new OnValueChanged(EventOnewayRoadBridgeBrightness));
            uIHelperOnewayRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness, new OnValueChanged(EventOnewayRoadDecorationGrassBrightness));
            uIHelperOnewayRoads.AddSlider("Decoration Trees ", 0, 1f, 0.1f, RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness, new OnValueChanged(EventOnewayRoadDecorationTreesBrightness));

            UIHelperBase uIHelperMediumRoads = helper.AddGroup("Medium Roads Brightness Settings");
            uIHelperMediumRoads.AddSlider("Medium Road", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_brightness, new OnValueChanged(EventMediumRoadBrightness));
            uIHelperMediumRoads.AddSlider("Medium Elevated", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_elevated_brightness, new OnValueChanged(EventMediumRoadElevatedBrightness));
            uIHelperMediumRoads.AddSlider("Medium Bridge", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_bridge_brightness, new OnValueChanged(EventMediumRoadBridgeBrightness));
            uIHelperMediumRoads.AddSlider("Decoration Grass", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness, new OnValueChanged(EventMediumRoadDecorationGrassBrightness));
            uIHelperMediumRoads.AddSlider("Decoration Trees", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness, new OnValueChanged(EventMediumRoadDecorationTreesBrightness));
            uIHelperMediumRoads.AddSlider("Medium Road Bikelane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_bicycle_brightness, new OnValueChanged(EventMediumRoadBicycleBrightness));
            uIHelperMediumRoads.AddSlider("Medium Bike Elevated", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_bicycle_elevated_brightness, new OnValueChanged(EventMediumRoadBicycleElevatedBrightness));
            uIHelperMediumRoads.AddSlider("Medium Road Bikelane", 0, 1f, 0.1f, RoadsUnitedModLoader.config.medium_road_bicycle_bridge_brightness, new OnValueChanged(EventMediumRoadBicycleBridgeBrightness));
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






            //           helper.AddButton("Revert to default on next level loading (sliders will not move)", EventResetColor);


        }


    }

}

