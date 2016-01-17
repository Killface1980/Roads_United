using ICities;
using System;

namespace RoadsUnited
{
	public class RoadsUnitedModInfo : IUserMod
	{
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

        /*
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
        }
        */


    }

}
