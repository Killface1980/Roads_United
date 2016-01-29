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

        private void EventCheckArrows(bool c)
        {
            RoadsUnitedModLoader.config.disable_optional_arrows = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventCheckPavement(bool c)
        {
            RoadsUnitedModLoader.config.use_alternate_pavement_texture = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventCheckCrack(bool c)
        {
            RoadsUnitedModLoader.config.use_cracked_roads = c;
            RoadsUnitedModLoader.SaveConfig();
        }

        private void EventSlideCrack(float c)
        {
            RoadsUnitedModLoader.config.crackIntensity = c;
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
            UIHelperBase uIHelperBase = helper.AddGroup("Roads United Settings");
            uIHelperBase.AddCheckbox("Remove optional lane arrows", RoadsUnitedModLoader.config.disable_optional_arrows, new OnCheckChanged(this.EventCheckArrows));
            uIHelperBase.AddCheckbox("Use alternate pavement texture", RoadsUnitedModLoader.config.use_alternate_pavement_texture, new OnCheckChanged(this.EventCheckPavement));
            uIHelperBase.AddCheckbox("Cracked roads?", RoadsUnitedModLoader.config.use_cracked_roads, new OnCheckChanged(this.EventCheckCrack));
            uIHelperBase.AddSlider("Crack intensity", 0.5f, 2.5f, 0.1f, RoadsUnitedModLoader.config.crackIntensity, new OnValueChanged(this.EventSlideCrack));
        }


    }

}

