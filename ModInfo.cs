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

        private void EventMediumRoadBusElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bus_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventMediumRoadBusBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.medium_road_bus_bridge_brightness = c;
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

        private void EventLargeRoadElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBicycleBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bicycle_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBicycleElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bicycle_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBicycleBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bicycle_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBusBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bus_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBusElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bus_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeRoadBusBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_road_bus_bridge_brightness = c;
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
            RoadsUnitedModLoader.config.large_oneway_ground_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeOnewayElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventLargeOnewayBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.large_oneway_bridge_brightness = c;
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

        private void EventHighwayRampGroundBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_ramp_ground_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }
        private void EventHighwayRampElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_ramp_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventHighwayGroundBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_ground_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventHighwayElevatedBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_elevated_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventHighwayBridgeBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_bridge_brightness = c;
            RoadsUnitedModLoader.SaveConfig();
        }
        private void EventHighwayBarrierBrightness(float c)
        {
            RoadsUnitedModLoader.config.highway_barrier_brightness = c;
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
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_elevated_brightness, "Basic Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bridge_brightness, "Basic Road Bridge", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_elevated_brightness, "Basic Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_bridge_brightness, "Basic Road Bridge Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, "Basic Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, "Basic Road Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_brightness, "Oneway Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_elevated_brightness, "Oneway Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_bridge_brightness, "Oneway Road Bridge", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness, "Oneway Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness, "Oneway Road Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_brightness, "Medium Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_elevated_brightness, "Medium Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bridge_brightness, "Medium Road Bridge", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_brightness, "Medium Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_elevated_brightness, "Medium Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_bridge_brightness, "Medium Road Bridge Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness, "Medium Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness, "Medium Road Decoration Trees", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bus_brightness, "Medium Road Bus", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bus_elevated_brightness, "Medium Road Elevated Bus", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bus_bridge_brightness, "Medium Road Bridge Bus", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_brightness, "Large Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_elevated_brightness, "Large Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bridge_brightness, "Large Road Bridge", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_decoration_grass_brightness, "Large Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_decoration_trees_brightness, "Large Road Decoration Trees", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bicycle_brightness, "Large Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bicycle_elevated_brightness, "Large Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bicycle_bridge_brightness, "Large Road Bridge Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bus_brightness, "Large Road Bus", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bus_elevated_brightness, "Large Road Elevated Bus", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_road_bus_bridge_brightness, "Large Road Bridge Bus", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_ground_brightness, "Large Oneway", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_elevated_brightness, "Large Oneway Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_bridge_brightness, "Large Oneway Bridge", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_decoration_grass_brightness, "Large Oneway Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.large_oneway_decoration_trees_brightness, "Large Oneway Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_ramp_ground_brightness, "HighwayRamp", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_ramp_elevated_brightness, "HighwayRampElevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_ground_brightness, "Highway", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_elevated_brightness, "Highway Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_bridge_brightness, "Highway Bridge", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.highway_barrier_brightness, "Highway Barrier", RoadsUnitedModLoader.modPath);
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

        private void EventResetConfig()
        {
            RoadsUnitedModLoader.config = new Configuration();
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
//            uIHelperGeneralSettings.AddCheckbox("Use mods Vanilla roads texture replacements", RoadsUnitedModLoader.config.use_custom_textures, EventCheckUseCustomTextures);
            uIHelperGeneralSettings.AddCheckbox("Activate the brightness sliders below. Slider to the right for a lighter colour.", RoadsUnitedModLoader.config.use_custom_colours, EventCheckUseCustomColours);
            //            helper.AddButton("Reload roads", EventReloadColor);
            uIHelperGeneralSettings.AddButton("Reset all sliders on next level load. ", EventResetConfig);


            UIHelperBase uIHelperSmallRoads = helper.AddGroup("Small Roads");
            uIHelperSmallRoads.AddSlider("Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_brightness, new OnValueChanged(EventSmallRoadBrightness));
            uIHelperSmallRoads.AddSlider("Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_elevated_brightness, new OnValueChanged(EventSmallRoadElevatedBrightness));
            uIHelperSmallRoads.AddSlider("Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_bridge_brightness, new OnValueChanged(EventSmallRoadBridgeBrightness));
            uIHelperSmallRoads.AddSpace(25);
            uIHelperSmallRoads.AddSlider("Decoration Grass", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, new OnValueChanged(EventSmallRoadDecorationGrassBrightness));
            uIHelperSmallRoads.AddSlider("Decoration Trees", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, new OnValueChanged(EventSmallRoadDecorationTreesBrightness));

            UIHelperBase uIHelperSmallBikeRoads = helper.AddGroup("Small Road with Bikelane");
            uIHelperSmallBikeRoads.AddSlider("Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_bicycle_brightness, new OnValueChanged(EventSmallRoadBicycleBrightness));
            uIHelperSmallBikeRoads.AddSlider("Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_bicycle_elevated_brightness, new OnValueChanged(EventSmallRoadBicycleElevatedBrightness));
            uIHelperSmallBikeRoads.AddSlider("Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.basic_road_bicycle_bridge_brightness, new OnValueChanged(EventSmallRoadBicycleBridgeBrightness));


            UIHelperBase uIHelperOnewayRoads = helper.AddGroup("Small Oneway");
            uIHelperOnewayRoads.AddSlider("Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.oneway_road_brightness, new OnValueChanged(EventOnewayRoadBrightness));
            uIHelperOnewayRoads.AddSlider("Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.oneway_road_elevated_brightness, new OnValueChanged(EventOnewayRoadElevatedBrightness));
            uIHelperOnewayRoads.AddSlider("Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.oneway_road_bridge_brightness, new OnValueChanged(EventOnewayRoadBridgeBrightness));
            uIHelperOnewayRoads.AddSpace(25);
            uIHelperOnewayRoads.AddSlider("Decoration Grass", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness, new OnValueChanged(EventOnewayRoadDecorationGrassBrightness));
            uIHelperOnewayRoads.AddSlider("Decoration Trees ", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness, new OnValueChanged(EventOnewayRoadDecorationTreesBrightness));

            UIHelperBase uIHelperMediumRoads = helper.AddGroup("Medium Roads");
            uIHelperMediumRoads.AddSlider("Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_brightness, new OnValueChanged(EventMediumRoadBrightness));
            uIHelperMediumRoads.AddSlider("Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_elevated_brightness, new OnValueChanged(EventMediumRoadElevatedBrightness));
            uIHelperMediumRoads.AddSlider("Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bridge_brightness, new OnValueChanged(EventMediumRoadBridgeBrightness));
            uIHelperMediumRoads.AddSpace(25);
            uIHelperMediumRoads.AddSlider("Decoration Grass", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness, new OnValueChanged(EventMediumRoadDecorationGrassBrightness));
            uIHelperMediumRoads.AddSlider("Decoration Trees", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness, new OnValueChanged(EventMediumRoadDecorationTreesBrightness));

            UIHelperBase uIHelperMediumBusBikeRoads = helper.AddGroup("Medium Roads with Bicycle or Bus Lane");
            uIHelperMediumBusBikeRoads.AddSlider("Bike Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bicycle_brightness, new OnValueChanged(EventMediumRoadBicycleBrightness));
            uIHelperMediumBusBikeRoads.AddSlider("Bike Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bicycle_elevated_brightness, new OnValueChanged(EventMediumRoadBicycleElevatedBrightness));
            uIHelperMediumBusBikeRoads.AddSlider("Bike Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bicycle_bridge_brightness, new OnValueChanged(EventMediumRoadBicycleBridgeBrightness));
            uIHelperMediumBusBikeRoads.AddSpace(25);
            uIHelperMediumBusBikeRoads.AddSlider("Bus Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bus_brightness, new OnValueChanged(EventMediumRoadBusBrightness));
            uIHelperMediumBusBikeRoads.AddSlider("Bus Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bus_elevated_brightness, new OnValueChanged(EventMediumRoadBusElevatedBrightness));
            uIHelperMediumBusBikeRoads.AddSlider("Bus Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.medium_road_bus_bridge_brightness, new OnValueChanged(EventMediumRoadBusBridgeBrightness));

            UIHelperBase uIHelperLargeRoads = helper.AddGroup("Large Roads");
            uIHelperLargeRoads.AddSlider("Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_brightness, new OnValueChanged(EventLargeRoadBrightness));
            uIHelperLargeRoads.AddSlider("Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_elevated_brightness, new OnValueChanged(EventLargeRoadElevatedBrightness));
            uIHelperLargeRoads.AddSlider("Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bridge_brightness, new OnValueChanged(EventLargeRoadBridgeBrightness));
            uIHelperLargeRoads.AddSpace(25);
            uIHelperLargeRoads.AddSlider("Decoration Grass", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_decoration_grass_brightness, new OnValueChanged(EventLargeRoadDecorationGrassBrightness));
            uIHelperLargeRoads.AddSlider("Decoration Trees", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_decoration_trees_brightness, new OnValueChanged(EventLargeRoadDecorationTreesBrightness));

            UIHelperBase uIHelperLargeBusBikeRoads = helper.AddGroup("Large Roads with Bicycle or Bus Lane");
            uIHelperLargeBusBikeRoads.AddSlider("Bike Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bicycle_brightness, new OnValueChanged(EventLargeRoadBicycleBrightness));
            uIHelperLargeBusBikeRoads.AddSlider("Bike Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bicycle_elevated_brightness, new OnValueChanged(EventLargeRoadBicycleElevatedBrightness));
            uIHelperLargeBusBikeRoads.AddSlider("Bike Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bicycle_bridge_brightness, new OnValueChanged(EventLargeRoadBicycleBridgeBrightness));
            uIHelperLargeBusBikeRoads.AddSpace(25);
            uIHelperLargeBusBikeRoads.AddSlider("Bus Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bus_brightness, new OnValueChanged(EventLargeRoadBusBrightness));
            uIHelperLargeBusBikeRoads.AddSlider("Bus Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bus_elevated_brightness, new OnValueChanged(EventLargeRoadBusElevatedBrightness));
            uIHelperLargeBusBikeRoads.AddSlider("Bus Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_road_bus_bridge_brightness, new OnValueChanged(EventLargeRoadBusBridgeBrightness));

            UIHelperBase uIHelperLargeOneway = helper.AddGroup("Large Oneway");
            uIHelperLargeOneway.AddSlider("Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_oneway_ground_brightness, new OnValueChanged(EventLargeOnewayBrightness));
            uIHelperLargeOneway.AddSlider("Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_oneway_elevated_brightness, new OnValueChanged(EventLargeOnewayElevatedBrightness));
            uIHelperLargeOneway.AddSlider("Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_oneway_bridge_brightness, new OnValueChanged(EventLargeOnewayBridgeBrightness));
            uIHelperLargeOneway.AddSpace(25);
            uIHelperLargeOneway.AddSlider("Decoration Grass", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_oneway_decoration_grass_brightness, new OnValueChanged(EventLargeOnewayDecorationGrassBrightness));
            uIHelperLargeOneway.AddSlider("Decoration Trees ", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.large_oneway_decoration_trees_brightness, new OnValueChanged(EventLargeOnewayDecorationTreesBrightness));

            UIHelperBase uIHelperHighways = helper.AddGroup("Highways");
            uIHelperHighways.AddSlider("Highway Ramp Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.highway_ramp_ground_brightness, new OnValueChanged(EventHighwayRampGroundBrightness));
            uIHelperHighways.AddSlider("Highway Ramp Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.highway_ramp_elevated_brightness, new OnValueChanged(EventHighwayRampElevatedBrightness));
            uIHelperHighways.AddSpace(25);
            uIHelperHighways.AddSlider("Highway Ground", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.highway_ground_brightness, new OnValueChanged(EventHighwayGroundBrightness));
            uIHelperHighways.AddSlider("Highway Elevated", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.highway_elevated_brightness, new OnValueChanged(EventHighwayElevatedBrightness));
            uIHelperHighways.AddSlider("Highway Bridge", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.highway_bridge_brightness, new OnValueChanged(EventHighwayBridgeBrightness));
            uIHelperHighways.AddSpace(25);
            uIHelperHighways.AddSlider("Highway Barrier", 0, 1f, 0.0625f, RoadsUnitedModLoader.config.highway_barrier_brightness, new OnValueChanged(EventHighwayBarrierBrightness));






            //           helper.AddButton("Revert to default on next level loading (sliders will not move)", EventResetColor);


        }


    }

}

