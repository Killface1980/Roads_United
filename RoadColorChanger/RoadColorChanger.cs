using ColossalFramework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RoadsUnited
{
    public class RoadColourChanger : MonoBehaviour
    {
        public static Configuration config;

        public static void ReplaceLodAprAtlas(string dir)
        {
            Texture2D texture2D = new Texture2D(Singleton<NetManager>.instance.m_lodAprAtlas.width, Singleton<NetManager>.instance.m_lodAprAtlas.height);
            texture2D.anisoLevel = 8;
            for (int i = 0; i < texture2D.height; i++)
            {
                for (int j = 0; j < texture2D.width; j++)
                {
                    if (Singleton<NetManager>.instance.m_lodAprAtlas.GetPixel(j, i).b > 0f)
                    {
                        texture2D.SetPixel(j, i, new Color(Singleton<NetManager>.instance.m_lodAprAtlas.GetPixel(j, i).r, Singleton<NetManager>.instance.m_lodAprAtlas.GetPixel(j, i).g, 1f));
                    }
                    else
                    {
                        texture2D.SetPixel(j, i, Singleton<NetManager>.instance.m_lodAprAtlas.GetPixel(j, i));
                    }
                }
            }
            texture2D.Apply();
            Singleton<NetManager>.instance.m_lodAprAtlas = texture2D;
        }

        public static void ReplaceLodRgbAtlas(string dir)
        {
            Texture2D texture2D = new Texture2D(Singleton<NetManager>.instance.m_lodRgbAtlas.width, Singleton<NetManager>.instance.m_lodRgbAtlas.height);
            texture2D.anisoLevel = 8;
            for (int i = 0; i < texture2D.height; i++)
            {
                for (int j = 0; j < texture2D.width; j++)
                {
                    if (Singleton<NetManager>.instance.m_lodRgbAtlas.GetPixel(j, i).b > 0f)
                    {
                        texture2D.SetPixel(j, i, new Color(Singleton<NetManager>.instance.m_lodRgbAtlas.GetPixel(j, i).r, Singleton<NetManager>.instance.m_lodRgbAtlas.GetPixel(j, i).g, 1f));
                    }
                    else
                    {
                        texture2D.SetPixel(j, i, Singleton<NetManager>.instance.m_lodRgbAtlas.GetPixel(j, i));
                    }
                }
            }
            texture2D.Apply();
            Singleton<NetManager>.instance.m_lodRgbAtlas = texture2D;
        }


        //        public static void ChangeColor(float brightnees, float red, float green, float blue, string prefab_road_name, string TextureDir)

        public static void ChangeColour(float brightness, string prefab_road_name, string TextureDir)
        {
            NetCollection[] array = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            NetCollection[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                NetCollection netCollection = array2[i];
                NetInfo[] prefabs = netCollection.m_prefabs;
                for (int j = 0; j < prefabs.Length; j++)
                {
                    NetInfo netInfo = prefabs[j];

                    //if (netInfo.m_class.name.Equals(prefab_road_name))
                    if (netInfo.name.Equals(prefab_road_name))
                    {
                        #region.train
                        if (netInfo.m_class.name.Equals("Train Track"))
                        {
                            if (netInfo.name.Equals("Train Track"))
                            {
                                netInfo.m_color = new Color(brightness, brightness, brightness);
                                //                                netInfo.m_color = new Color(red, green, blue);
                            }
                        }
                        #endregion
                        else
                        {
                            if (netInfo.name.Equals(prefab_road_name))
                            {
                                if (netInfo.m_color != null)
                                netInfo.m_color = new Color(brightness, brightness, brightness);
                            }
                            //                            netInfo.m_color = new Color(red, green, blue);

                        }
                        if (prefab_road_name.Equals("Highway"))
                        {

                            NetInfo.Segment[] segments = netInfo.m_segments;
                            for (int l = 0; l < segments.Length; l++)
                            {

                                NetInfo.Segment segment = segments[l]; //das hier wieder zu color changer mit ausnahmen
                                if (!segment.m_material.name.ToLower().Contains("cable"))
                                {
                                    Texture2D texture2D = new Texture2D(1, 1);
                                    if (ModLoader.config.use_custom_textures == false)
                                    {

                                        texture2D = RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasesegment_vanilla_map.dds"));

                                        segment.m_material.SetTexture("_APRMap", texture2D);
                                        segment.m_segmentMaterial.SetTexture("_APRMap", texture2D);
                                        segment.m_lodMesh = null;
                                        segment.m_lodRenderDistance = 2500;

                                    }
                                }
                            }

                            NetInfo.Node[] nodes = netInfo.m_nodes;
                            for (int k = 0; k < nodes.Length; k++)
                            {

                                NetInfo.Node node = nodes[k];
                                node.m_lodMesh = null;

                                Texture2D texture2D = new Texture2D(1, 1);
                                if (netInfo.name.Equals("HighwayRamp") || netInfo.name.Equals("HighwayRampElevated"))
                                {
                                    texture2D = RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampnode_n_map.dds"));
                                }
                                else
                                {
                                    texture2D = RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasenode_n_map.dds"));
                                }
                                texture2D.anisoLevel = 8;
                                node.m_nodeMaterial.SetTexture("_APRMap", texture2D);
                                node.m_lodMesh = null;
                                node.m_lodRenderDistance = 2500;


                            }
                            netInfo.RefreshLevelOfDetail();
                        }
                    }
                }
            }
            NetNode[] buffer = Singleton<NetManager>.instance.m_nodes.m_buffer;
            for (int i = 0; i < buffer.Length; i++)
            {
                NetNode netNode = buffer[i];
                if (prefab_road_name.Equals("Train Track"))
                {
                    if (netNode.Info.name.Equals("Train Track"))
                    {
                        netNode.Info.m_color = new Color(brightness, brightness, brightness);
                    }
                }
                else if (netNode.Info.name.Equals(prefab_road_name))
                {
                    netNode.Info.m_color = new Color(brightness, brightness, brightness);
                }
            }
            NetSegment[] buffer2 = Singleton<NetManager>.instance.m_segments.m_buffer;
            for (int i = 0; i < buffer2.Length; i++)
            {
                NetSegment netSegment = buffer2[i];
                if (prefab_road_name.Equals("Train Track"))
                {
                    if (netSegment.Info.name.Equals("Train Track"))
                    {
                        netSegment.Info.m_color = new Color(brightness, brightness, brightness);
                    }
                }
                else if (netSegment.Info.name.Equals(prefab_road_name))
                {
                    netSegment.Info.m_color = new Color(brightness, brightness, brightness);
                }
            }
        }

        // RoadsUnited.RoadColourChanger
        public static void ChangeColourNetExt(float brightness, string Prefab_Class_Name, string TextureDir)
        {
            NetCollection[] array = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            for (int i = 0; i < array.Length; i++)
            {
                NetInfo[] prefabs = array[i].m_prefabs;
                for (int j = 0; j < prefabs.Length; j++)
                {
                    NetInfo netInfo = prefabs[j];
                    if (netInfo.m_class.name.Equals(Prefab_Class_Name))
                    {
                        netInfo.m_color = new Color(brightness, brightness, brightness);
                    }
                    if (netInfo.name.Contains(Prefab_Class_Name))
                    {
                        netInfo.m_color = new Color(brightness, brightness, brightness);
                    }
                }
            }
            NetNode[] buffer = Singleton<NetManager>.instance.m_nodes.m_buffer;
            for (int k = 0; k < buffer.Length; k++)
            {
                NetNode netNode = buffer[k];
                if (netNode.Info.m_class.name.Equals(Prefab_Class_Name))
                {
                    netNode.Info.m_color = new Color(brightness, brightness, brightness);
                }
                if (netNode.Info.name.Contains(Prefab_Class_Name))
                {
                    netNode.Info.m_color = new Color(brightness, brightness, brightness);
                }
            }
            NetSegment[] buffer2 = Singleton<NetManager>.instance.m_segments.m_buffer;
            for (int l = 0; l < buffer2.Length; l++)
            {
                NetSegment netSegment = buffer2[l];
                if (netSegment.Info.m_class.name.Equals(Prefab_Class_Name))
                {
                    netSegment.Info.m_color = new Color(brightness, brightness, brightness);
                }
                if (netSegment.Info.name.Contains(Prefab_Class_Name))
                {
                    netSegment.Info.m_color = new Color(brightness, brightness, brightness);
                }
            }
        }



    }
}
