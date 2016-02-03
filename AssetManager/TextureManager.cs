using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RoadsUnited
{
    public class TextureManager : MonoBehaviour
    {

        private readonly HashSet<Texture> unusedTextures = new HashSet<Texture>();


        public void MarkTextureUnused(Texture texture)
        {
            unusedTextures.Add(texture);
        }

        public void UnloadUnusedTextures()
        {
            foreach (var texture in unusedTextures)
            {
                UnityEngine.Object.Destroy(texture);
            }
            unusedTextures.Clear();


        }
    }
}
