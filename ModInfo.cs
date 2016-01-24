using ICities;
using System;
using UnityEngine;
using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.Steamworks;
using ICities;
using System.IO;
using UnityEngine;

namespace RoadsUnited
{
    public class RoadsUnitedMod : IUserMod
    {
        public const UInt64 workshop_id = 598151121;

        public static OptionsManager.ModOptions Options = OptionsManager.ModOptions.None;
        static OptionsManager sm_optionsManager;

        GameObject m_initializer;

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

        public void OnSettingsUI(UIHelperBase helper)
        {
            if (sm_optionsManager == null)
                sm_optionsManager = new GameObject("OptionsManager").AddComponent<OptionsManager>();

            sm_optionsManager.CreateSettings(helper);
        }



    }

}

