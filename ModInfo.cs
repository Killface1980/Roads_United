using ICities;
using System;
using System.IO;
using System.Xml.Serialization;

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
				return "Replaces road textures and other road feature with better ones.";
			}
		}

        private void EventCheckArrows(bool c)
        {
            RoadsUnited.config.disable_optional_arrows = c;
            RoadsUnited.SaveConfig();
        }

        private void EventCheckPavement(bool c)
        {
            RoadsUnited.config.use_alternate_pavement_texture = c;
            RoadsUnited.SaveConfig();
        }

        private void EventCheckCrack(bool c)
        {
            RoadsUnited.config.use_cracked_roads = c;
            RoadsUnited.SaveConfig();
        }

        private void EventSlideCrack(float c)
        {
            RoadsUnited.config.crackIntensity = c;
            RoadsUnited.SaveConfig();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {

            RoadsUnited.config = Configuration.Deserialize(RoadsUnited.configPath);
            if (RoadsUnited.config == null)
            {
                RoadsUnited.config = new Configuration();
            }
            RoadsUnited.SaveConfig();
            UIHelperBase uIHelperBase = helper.AddGroup("Roads United Settings");
            uIHelperBase.AddCheckbox("Remove optional lane arrows", RoadsUnited.config.disable_optional_arrows, new OnCheckChanged(this.EventCheckArrows));
            uIHelperBase.AddCheckbox("Use alternate pavement texture", RoadsUnited.config.use_alternate_pavement_texture, new OnCheckChanged(this.EventCheckPavement));
            uIHelperBase.AddCheckbox("Cracked roads?", RoadsUnited.config.use_cracked_roads, new OnCheckChanged(this.EventCheckCrack));
            uIHelperBase.AddSlider("Crack intensity", 0.5f, 2.5f, 0.1f, RoadsUnited.config.crackIntensity, new OnValueChanged(this.EventSlideCrack));
        }
    }

}

