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




    }

}

