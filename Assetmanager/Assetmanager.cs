using System;
using System.Collections.Generic;
using System.IO;
using ColossalFramework;
using UnityEngine;

namespace RoadsUnited
{
    public class AssetManager : Singleton<AssetManager>
    {


        private readonly IDictionary<string, Texture2D> _allTextures = new Dictionary<string, Texture2D>();

        public IEnumerable<Action> CreateLoadingSequence(string modPath)
        {
            var modDirectory = new DirectoryInfo(modPath);
            Debug.Log("Modpath loaded");

            var files = new List<FileInfo>();
            Debug.Log("var files declared");
            files.AddRange(modDirectory.GetFiles("*.dds", SearchOption.AllDirectories));
            Debug.Log("add dds");

            foreach (var assetFile in files)
            {
                var assetFullPath = assetFile.FullName;
                Debug.Log(assetFullPath);
                var assetRelativePath = assetFile.FullName.Replace(modPath, "").TrimStart(new[] { '\\', '/' });
                var assetName = assetFile.Name;

                if (_allTextures.ContainsKey(assetRelativePath))
                {
                    continue;
                }

                switch (assetFile.Extension.ToLower())
                {
                    case ".dds":
                        yield return () =>
                        {
                            _allTextures[assetRelativePath] = LoadTextureDDS(assetFullPath, assetName);
                        };
                        break;
                }
            }
        }

        private static Texture2D LoadTextureDDS(string fullPath, string textureName)
        {
            var numArray = File.ReadAllBytes(fullPath);
            var width = BitConverter.ToInt32(numArray, 16);
            var height = BitConverter.ToInt32(numArray, 12);

            var texture = new Texture2D(width, height, TextureFormat.DXT5, true);
            var list = new List<byte>();

            for (int index = 0; index < numArray.Length; ++index)
            {
                if (index > (int)sbyte.MaxValue)
                    list.Add(numArray[index]);
            }

            texture.LoadRawTextureData(list.ToArray());
            texture.anisoLevel = 8;
            texture.name = Path.GetFileNameWithoutExtension(textureName);
            texture.Apply();
            return texture;
        }
    }
}