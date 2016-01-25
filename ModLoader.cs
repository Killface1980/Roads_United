using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Packaging;
using ColossalFramework.Steamworks;
using ICities;
using System;
using System.IO;

using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;


namespace RoadsUnited
{


    public class RoadsUnitedModLoader : LoadingExtensionBase
    {
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





        public override void OnLevelLoaded(LoadMode mode)
        {

            string modPath = RoadsUnitedModLoader.getModPath();
            RoadsUnited.ReplaceNetTextures(modPath);
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;


        }






    }
}

