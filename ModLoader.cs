using ColossalFramework;
using ColossalFramework.IO;
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
        //AR

 //           public static bool exor = false;

//            private GameObject hookGo;

 //           private Hook4 hook;


        //AR end
    

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

        // Ameriacn %Roads begin
        /*
        public override void OnCreated(ILoading loading)
        {
            RoadsUnited.config = Configuration.Deserialize(RoadsUnited.configPath);
            if (RoadsUnited.config == null)
            {
                RoadsUnited.config = new Configuration();
            }
            RoadsUnited.SaveConfig();
        
        			base.OnCreated(loading);
        }
        */
        //American Roads end

        public class EnableAchievementsLoad : LoadingExtensionBase
        {
            public override void OnLevelLoaded(LoadMode mode)
            {
                Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
		{

  //              this.hookGo = new GameObject("RoadsUnited hook");
 //           this.hook = this.hookGo.AddComponent<Hook4>();
            string modPath = RoadsUnitedModLoader.getModPath();
            RoadsUnited.ReplaceNetTextures(modPath);

                /*           if (RoadsUnited.config.use_alternate_pavement_texture)
                           {
                               RoadsUnited.ReplacePavement(modPath, RoadsUnited.config.use_cracked_roads, RoadsUnited.config.use_alternate_pavement_texture);
                           }
                           RoadsUnited.ReplaceProps();
               */


                //			RoadsUnited.ReplaceNetTextures(modPath);
                //			UKRoads.ReplacePropTextures(modPath);
  //              RoadsUnitedModLoader.exor = false;

    //        base.OnLevelLoaded(mode);
        }
        /*
        public static void SaveConfig()
        {
            Configuration.Serialize(RoadsUnited.configPath, RoadsUnited.config);
        }
        */
    }
}

