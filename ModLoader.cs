using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Packaging;
using ColossalFramework.Steamworks;
using ICities;
using System;
using System.IO;

using System.Collections.Generic;
using UnityEngine;
using ColossalFramework.UI;
using System.Xml.Serialization;
using RoadsUnited.Hook;



namespace RoadsUnited
{


    public class RoadsUnitedModLoader : LoadingExtensionBase
    {
        public static Configuration config;

        public static readonly string configPath = "RoadsUnitedConfig.xml";

        private GameObject hookGo;

        private NetInfoHook hook;



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
            hookGo = new GameObject("RoadsUnited hook");
            hook = hookGo.AddComponent<NetInfoHook>();

            string modPath = getModPath();
            RoadsUnited.ReplaceNetTextures(modPath);

            #region.RoadColorChanger

            RoadColorChanger.ChangeColor(config.large_road_red, config.large_road_green, config.large_road_blue, "Large Road", modPath);
            RoadColorChanger.ChangeColor(config.medium_road_red, config.medium_road_green, config.medium_road_blue, "Medium Road", modPath);
            RoadColorChanger.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "Small Road", modPath);
            RoadColorChanger.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "Electricity Dam", modPath);
            RoadColorChanger.ChangeColor(config.small_road_red, config.small_road_green, config.small_road_blue, "Train Track", modPath);
            RoadColorChanger.ReplaceLodAprAtlas(modPath);
            RoadColorChanger.ChangeColor(config.highway_red, config.highway_green, config.highway_blue, "Highway", modPath);
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
            if (this.hook != null)
            {
                this.hook.DisableHook();
            }
            if (this.hookGo != null)
            {
                UnityEngine.Object.Destroy(this.hookGo);
            }
            this.hook = null;
            base.OnLevelUnloading();
        }


        public static void SaveConfig()
        {
            Configuration.Serialize(RoadsUnitedModLoader.configPath, RoadsUnitedModLoader.config);
        }




#if Debug
        public void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            RoadsUnited.ReplaceNetTextures(modPath);
        } 
#endif
    }


}


