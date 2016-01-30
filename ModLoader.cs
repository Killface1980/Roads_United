using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Steamworks;
using ICities;
using System.IO;

using UnityEngine;





namespace RoadsUnited
{


    public class RoadsUnitedModLoader : LoadingExtensionBase
    {
        public static Configuration config;

        public static readonly string configPath = "RoadsUnitedConfig.xml";

        public static string getModPath()
        {
            string text = ".";
            PublishedFileId[] subscribedItems = Steam.workshop.GetSubscribedItems();
            for (int i = 0; i < subscribedItems.Length; i++)
            {
                PublishedFileId id = subscribedItems[i];
                if (id.AsUInt64 == 598151121)
                {
                    text = Steam.workshop.GetSubscribedItemPath(id);
                    Debug.Log("Roads United: Workshop path: " + text);
                    break;
                }
            }
            string text2 = DataLocation.modsPath + "/RoadsUnited";
            Debug.Log("Roads United: " + text2);
            string result;
            if (Directory.Exists(text2))
            {
                Debug.Log("Roads United: Local path exists, looking for assets here: " + text2);
                result = text2;
            }
            else
            {
                result = text;
            }
            return result;
        }

        public static string modPath = getModPath();



        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);


            #region.Config
            config = Configuration.Deserialize(configPath);
            if (config == null)
            {
                config = new Configuration();
            }
            SaveConfig();
            #endregion


            // deploy (after event handler registration!)
        }




        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);


            string modPath = getModPath();
            RoadsUnited.ReplaceNetTextures(modPath);

            #region.RoadColorChanger


            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_brightness, "Basic Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_brightness, "Basic Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_bicycle_brightness, "Basic Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_grass_brightness, "Basic Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.basic_road_decoration_trees_brightness, "Basic Road Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_brightness, "Oneway Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_brightness, "Oneway Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_decoration_grass_brightness, "Oneway Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.oneway_road_decoration_trees_brightness, "Oneway Road Decoration Trees", RoadsUnitedModLoader.modPath);

            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_brightness, "Medium Road", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_brightness, "Medium Road Elevated", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_brightness, "Medium Road Bicycle", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_bicycle_brightness, "Medium Road Elevated Bike", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_decoration_grass_brightness, "Medium Road Decoration Grass", RoadsUnitedModLoader.modPath);
            RoadColorChanger.ChangeColor(RoadsUnitedModLoader.config.medium_road_decoration_trees_brightness, "Medium Road Decoration Trees", RoadsUnitedModLoader.modPath);
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


            #endregion




            //           Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;


#if Debug
            var uiView = UIView.GetAView();

            // Add a new button to the view.
            var button = (UIButton)uiView.AddUIComponent(typeof(UIButton));

            // Set the text to show on the button.
            button.text = "Reload textures";

            // Set the button dimensions.
            button.width = 250;
            button.height = 30;

            // Style the button to look like a menu button.
            button.normalBgSprite = "ButtonMenu";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.focusedBgSprite = "ButtonMenuFocused";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.textColor = new Color32(255, 255, 255, 255);
            button.disabledTextColor = new Color32(7, 7, 7, 255);
            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);
            button.pressedTextColor = new Color32(30, 30, 44, 255);

            // Enable button sounds.
            button.playAudioEvents = true;

            // Place the button.
            button.transformPosition = new Vector3(-1.0f, 0.97f);

            // Respond to button click.
            // NOT GETTING CALLED
            button.eventClick += ButtonClick; 
#endif
        }



        public override void OnLevelUnloading()
        {

            base.OnLevelUnloading();


        }


        public static void SaveConfig()
        {
            Configuration.Serialize(RoadsUnitedModLoader.configPath, RoadsUnitedModLoader.config);
        }

        public override void OnReleased()
        {
            base.OnReleased();

        }


#if Debug
        public void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            RoadsUnited.ReplaceNetTextures(modPath);
        } 
#endif
    }


}


