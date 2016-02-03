using ColossalFramework;
using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RoadsUnited
{
    public class ResourceReplacer : Singleton<ResourceReplacer>
    {

        private TextureManager textureManager;

        // Resource packs
        public void OnCreated()
        {
            if (textureManager == null) textureManager = gameObject.AddComponent<TextureManager>();
        }

        public void OnLevelLoaded()
        {
            textureManager.UnloadUnusedTextures();
        }

        private bool ReplaceMainTexture(Material material, string fullTexturePath)
        {
            var originalTexture = material.GetTexture("_MainTex");
            var newTexture = RoadsUnited.LoadTextureDDS(fullTexturePath);

            if (newTexture != null)
            {
                // apply new texture
                material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(fullTexturePath));
                // mark old texture for unload
                if (originalTexture != null) textureManager.MarkTextureUnused(originalTexture);

                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
