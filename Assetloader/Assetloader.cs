using System;
using System.Collections.Generic;
using System.IO;
using ColossalFramework;
//using ObjUnity3D;
using UnityEngine;

namespace RoadsUnited.Framework
{
    public class AssetManager : Singleton<AssetManager>
    {
        private readonly IDictionary<string, Texture2D> _allTextures = new Dictionary<string, Texture2D>();

        public IEnumerable<Action> CreateLoadingSequence(string textures_path)
        {
            var modDirectory = new DirectoryInfo(textures_path);

            var files = new List<FileInfo>();
            files.AddRange(modDirectory.GetFiles("*.dds", SearchOption.AllDirectories));


            foreach (var textureFile in files)
            {
                var assetFullPath = textureFile.FullName;
                var assetRelativePath = textureFile.FullName.Replace(textures_path, "").TrimStart(new[] { '\\', '/' });

                if (_allTextures.ContainsKey(assetRelativePath))
                {
                    //                    continue;

                    yield return () =>
                    {
                        _allTextures[assetRelativePath] = RoadsUnited.LoadTextureDDS(assetFullPath);
                    };
                    break;
                }


            }
        }



    }



}

