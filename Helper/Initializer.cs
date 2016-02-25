using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using RoadsUnited.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;

namespace RoadsUnited
{
    public class Initializer : MonoBehaviour
    {
        [Flags]
        enum RoadType
        {
            Normal = 0,

            Grass = 1,
            Trees = 2,

            Elevated = 4,
            Bridge = 8,
            Slope = 16,
            Tunnel = 32,

            Pavement = 64,
            Gravel = 128,

            OneWay = 256
        }

        static Queue<IEnumerator> sm_actionQueue = new Queue<IEnumerator>();
        static System.Object sm_queueLock = new System.Object();

        public static Dictionary<string, TextureInfo> sm_fileIndex = new Dictionary<string, TextureInfo>();


        Dictionary<string, PrefabInfo> m_customPrefabs;
        Dictionary<string, Texture2D> m_customTextures;
        //Queue<Action> m_postLoadingActions;
        bool m_initialized;
        float m_gameStartedTime;
        int m_level;

        void Awake()
        {
            DontDestroyOnLoad(this);

            m_customPrefabs = new Dictionary<string, PrefabInfo>();
            m_customTextures = new Dictionary<string, Texture2D>();

        }


        void OnLevelWasLoaded(int level)
        {
            this.m_level = level;

            if (level == 6)
            {

                m_initialized = false;

                while (!Monitor.TryEnter(sm_queueLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
                try
                {
                    sm_actionQueue.Clear();
                }
                finally
                {
                    Monitor.Exit(sm_queueLock);
                }

                m_customPrefabs.Clear();
                
                //m_postLoadingActions.Clear();
            }
        }



        void Update()
        {




            if (!Singleton<LoadingManager>.instance.m_loadingComplete)
                return;
            else if (m_gameStartedTime == 0f)
                m_gameStartedTime = Time.realtimeSinceStartup;

            //while (m_postLoadingActions.Count > 0)
            //	m_postLoadingActions.Dequeue().Invoke();

            // contributed by Japa

        }





        #region Textures
        [Flags]
        enum TextureType
        {
            Normal = 0,
            Bus = 1,
            BusBoth = 2,
            Node = 4,
            LOD = 8,
            BusLOD = 9,
            BusBothLOD = 10,
            NodeLOD = 12
        }

        static string[] sm_mapNames = new string[] { "_MainTex", "_XYSMap", "_ACIMap", "_APRMap" };

        void OnGUI()
        {
            if (Singleton<LoadingManager>.instance.m_loadingComplete)
            {
                if (GUI.Button(new Rect(10, 900, 150, 30), "Update Textures"))
                {
                    m_customTextures.Clear();
                    LoadTextureIndex();
                    foreach (var item in m_customPrefabs.Values)
                    {
                        NetInfo netInfo = item as NetInfo;
                        if (netInfo.m_segments.Length == 0)
                            continue;

                        TextureInfo textureInfo;
                        if (!sm_fileIndex.TryGetValue(netInfo.name, out textureInfo))
                            continue;

                        FileManager.Folder folder;
                        if (netInfo.name.Contains("Large"))
                            folder = FileManager.Folder.LargeRoad;
                        else if (netInfo.name.Contains("Small"))
                            folder = FileManager.Folder.SmallRoad;
                        else
                            folder = FileManager.Folder.PedestrianRoad;

                        for (int i = 0; i < netInfo.m_segments.Length; i++)
                        {
                            TextureType textureType = TextureType.Normal;
                            if (!netInfo.name.Contains("Bridge") && !netInfo.name.Contains("Elevated") && !netInfo.name.Contains("Slope") && !netInfo.name.Contains("Tunnel"))
                            {
                                if (i == 1) textureType = TextureType.Bus;
                                if (i == 2) textureType = TextureType.BusBoth;
                            }

                            ReplaceTextures(textureInfo, textureType, folder, netInfo.m_segments[i].m_segmentMaterial);
                        }
                    }

                    FileManager.ClearCache();
                }

                //if (GUI.Button(new Rect(10, 850, 150, 30), "Road Customizer"))
                //{
                //    //ToolsModifierControl.SetTool<RoadCustomizerTool>();
                //    //RoadCustomizerTool.InitializeUI();
                //}
                //if (GUI.Button(new Rect(10, 800, 150, 30), "Add Button"))
                //{
                //    RoadCustomizerTool.SetUIButton();
                //}
            }
        }

        void LoadTextureIndex()
        {
            TextureInfo[] textureIndex = FileManager.GetTextureIndex();
            if (textureIndex == null)
                return;

            sm_fileIndex.Clear();
            foreach (TextureInfo item in textureIndex)
                sm_fileIndex.Add(item.name, item);
        }



        bool ReplaceTextures(TextureInfo textureInfo, TextureType textureType, FileManager.Folder textureFolder, Material mat, int anisoLevel = 8, FilterMode filterMode = FilterMode.Trilinear, bool skipCache = false)
        {
            bool success = false;
            byte[] textureBytes;
            Texture2D tex = null;

            for (int i = 0; i < sm_mapNames.Length; i++)
            {
                if (mat.HasProperty(sm_mapNames[i]) && mat.GetTexture(sm_mapNames[i]) != null)
                {
                    string fileName = GetTextureName(sm_mapNames[i], textureInfo, textureType);
                    if (!String.IsNullOrEmpty(fileName) && !m_customTextures.TryGetValue(fileName, out tex))
                    {
                        if (FileManager.GetTextureBytes(fileName + ".png", textureFolder, skipCache, out textureBytes))
                        {
                            tex = new Texture2D(1, 1);
                            tex.LoadImage(textureBytes);
                        }
                        else if (fileName.Contains("-LOD"))
                        {
                            Texture2D original = mat.GetTexture(sm_mapNames[i]) as Texture2D;
                            if (original != null)
                            {
                                tex = new Texture2D(original.width, original.height);
                                tex.SetPixels(original.GetPixels());
                                tex.Apply();
                            }
                        }
                    }

                    if (tex != null)
                    {
                        tex.name = fileName;
                        tex.anisoLevel = anisoLevel;
                        tex.filterMode = filterMode;
                        mat.SetTexture(sm_mapNames[i], tex);
                        m_customTextures[tex.name] = tex;
                        success = true;
                        tex = null;
                    }
                }
            }

            return success;
        }

        string GetTextureName(string map, TextureInfo info, TextureType type)
        {
            switch (type)
            {
                case TextureType.Normal:
                    switch (map)
                    {
                        case "_MainTex": return info.mainTex;
                        case "_XYSMap": return info.xysTex;
                        case "_ACIMap": return info.aciTex;
                        case "_APRMap": return info.aprTex;
                    }
                    break;
                case TextureType.Bus:
                    switch (map)
                    {
                        case "_MainTex": return info.mainTexBus;
                        case "_XYSMap": return info.xysTexBus;
                        case "_ACIMap": return info.aciTexBus;
                        case "_APRMap": return info.aprTexBus;
                    }
                    break;
                case TextureType.BusBoth:
                    switch (map)
                    {
                        case "_MainTex": return info.mainTexBusBoth;
                        case "_XYSMap": return info.xysTexBusBoth;
                        case "_ACIMap": return info.aciTexBusBoth;
                        case "_APRMap": return info.aprTexBusBoth;
                    }
                    break;
                case TextureType.Node:
                    switch (map)
                    {
                        case "_MainTex": return info.mainTexNode;
                        case "_XYSMap": return info.xysTexNode;
                        case "_ACIMap": return info.aciTexNode;
                        case "_APRMap": return info.aprTexNode;
                    }
                    break;
                case TextureType.LOD:
                    switch (map)
                    {
                        case "_MainTex": return info.lodMainTex;
                        case "_XYSMap": return info.lodXysTex;
                        case "_ACIMap": return info.lodAciTex;
                        case "_APRMap": return info.lodAprTex;
                    }
                    break;
                case TextureType.BusLOD:
                    switch (map)
                    {
                        case "_MainTex": return info.lodMainTexBus;
                        case "_XYSMap": return info.lodXysTexBus;
                        case "_ACIMap": return info.lodAciTexBus;
                        case "_APRMap": return info.lodAprTexBus;
                    }
                    break;
                case TextureType.BusBothLOD:
                    switch (map)
                    {
                        case "_MainTex": return info.lodMainTexBusBoth;
                        case "_XYSMap": return info.lodXysTexBusBoth;
                        case "_ACIMap": return info.lodAciTexBusBoth;
                        case "_APRMap": return info.lodAprTexBusBoth;
                    }
                    break;
                case TextureType.NodeLOD:
                    switch (map)
                    {
                        case "_MainTex": return info.lodMainTexNode;
                        case "_XYSMap": return info.lodXysTexNode;
                        case "_ACIMap": return info.lodAciTexNode;
                        case "_APRMap": return info.lodAprTexNode;
                    }
                    break;
                default:
                    break;
            }

            return null;
        }



#if DEBUG
        public static void DumpRenderTexture(RenderTexture rt, string pngOutPath)
        {
            var oldRT = RenderTexture.active;

            var tex = new Texture2D(rt.width, rt.height);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
            RenderTexture.active = oldRT;
        }

        public static void DumpTextureToPNG(Texture previewTexture, string filename = null)
        {
            if (filename == null)
            {
                filename = "";
                var filenamePrefix = String.Format("rt_dump_{0}", previewTexture.name);
                if (!File.Exists(filenamePrefix + ".png"))
                {
                    filename = filenamePrefix + ".png";
                }
                else
                {
                    int i = 1;
                    while (File.Exists(String.Format("{0}_{1}.png", filenamePrefix, i)))
                    {
                        i++;
                    }

                    filename = String.Format("{0}_{1}.png", filenamePrefix, i);
                }
            }

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            if (previewTexture is RenderTexture)
            {
                DumpRenderTexture((RenderTexture)previewTexture, filename);
                //Log.Warning(String.Format("Texture dumped to \"{0}\"", filename));
            }
            else if (previewTexture is Texture2D)
            {
                var texture = previewTexture as Texture2D;
                byte[] bytes = null;

                try
                {
                    bytes = texture.EncodeToPNG();
                }
                catch (UnityException)
                {
                    //Log.Warning(String.Format("Texture \"{0}\" is marked as read-only, running workaround..", texture.name));
                }

                if (bytes == null)
                {
                    try
                    {
                        var rt = RenderTexture.GetTemporary(texture.width, texture.height, 0);
                        Graphics.Blit(texture, rt);
                        DumpRenderTexture(rt, filename);
                        RenderTexture.ReleaseTemporary(rt);
                        //Log.Warning(String.Format("Texture dumped to \"{0}\"", filename));
                    }
                    catch (Exception ex)
                    {
                        //Log.Error("There was an error while dumping the texture - " + ex.Message);
                    }

                    return;
                }

                File.WriteAllBytes(filename, bytes);
                //Log.Warning(String.Format("Texture dumped to \"{0}\"", filename));
            }
            else
            {
                //Log.Error(String.Format("Don't know how to dump type \"{0}\"", previewTexture.GetType()));
            }
        }
#endif
        #endregion

        // TODO: Put this in its own class



        public class TextureInfo
        {
            [XmlAttribute]
            public string name;

            // normal
            public string mainTex = "";
            public string aprTex = "";
            public string xysTex = "";
            public string aciTex = "";
            public string lodMainTex = "";
            public string lodAprTex = "";
            public string lodXysTex = "";
            public string lodAciTex = "";

            // bus
            public string mainTexBus = "";
            public string aprTexBus = "";
            public string xysTexBus = "";
            public string aciTexBus = "";
            public string lodMainTexBus = "";
            public string lodAprTexBus = "";
            public string lodXysTexBus = "";
            public string lodAciTexBus = "";

            // busBoth
            public string mainTexBusBoth = "";
            public string aprTexBusBoth = "";
            public string xysTexBusBoth = "";
            public string aciTexBusBoth = "";
            public string lodMainTexBusBoth = "";
            public string lodAprTexBusBoth = "";
            public string lodXysTexBusBoth = "";
            public string lodAciTexBusBoth = "";

            // node
            public string mainTexNode = "";
            public string aprTexNode = "";
            public string xysTexNode = "";
            public string aciTexNode = "";
            public string lodMainTexNode = "";
            public string lodAprTexNode = "";
            public string lodXysTexNode = "";
            public string lodAciTexNode = "";
        }


    }

}