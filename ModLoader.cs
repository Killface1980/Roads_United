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
        public static OptionsManager.ModOptions Options = OptionsManager.ModOptions.None;
        static OptionsManager sm_optionsManager;

        GameObject m_initializer;


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


        public class EnableAchievementsLoad : LoadingExtensionBase
        {
            public override void OnLevelLoaded(LoadMode mode)
            {
                Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
            }
        }


        /*        public override void OnLevelLoaded(LoadMode mode)
                {

                    string modPath = RoadsUnitedModLoader.getModPath();
                    RoadsUnited.ReplaceNetTextures(modPath);

                }
                */

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);

            if (sm_optionsManager != null)
            {
                sm_optionsManager.LoadOptions();
            }

            if (m_initializer == null)
            {
                m_initializer = new GameObject("CSL-Traffic Custom Prefabs");
                m_initializer.AddComponent<Initializer>();
            }
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            if (m_initializer != null)
                m_initializer.GetComponent<Initializer>().OnLevelUnloading();
        }

        public override void OnReleased()
        {
            base.OnReleased();

            GameObject.Destroy(m_initializer);
        }




    }
}

