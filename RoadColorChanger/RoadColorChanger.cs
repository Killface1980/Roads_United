using ColossalFramework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RoadsUnited
{
    public class RoadColorChanger : MonoBehaviour
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
        /*
                public static Texture2D LoadTextureDDS(string texturePath)
                {
                    byte[] array = File.ReadAllBytes(texturePath);
                    int num = BitConverter.ToInt32(array, 12);
                    int num2 = BitConverter.ToInt32(array, 16);
                    Texture2D texture2D = new Texture2D(num2, num, 12, true);
                    List<byte> list = new List<byte>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (i > 127)
                        {
                            list.Add(array[i]);
                        }
                    }
                    texture2D.LoadRawTextureData(list.ToArray());
                    texture2D.Apply();
                    texture2D.set_anisoLevel(8);
                    return texture2D;
                }
        */

//        public static void ChangeColor(float brightnees, float red, float green, float blue, string roadtype, string dir)

        public static void ChangeColor(float brightness, string roadtype, string dir)
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

                    //if (netInfo.m_class.name.Equals(roadtype))
                    if (netInfo.name.Contains(roadtype))
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
                            if (netInfo.name.Equals(roadtype))
                                {
                                netInfo.m_color = new Color(brightness, brightness, brightness);
                                }
                            //                            netInfo.m_color = new Color(red, green, blue);

                        }
                        if (roadtype.Equals("Highway"))
                        {

                            NetInfo.Segment[] segments = netInfo.m_segments;
                            for (int k = 0; k < segments.Length; k++)
                            {
                                NetInfo.Segment segment = segments[k];

                                /*
                                if (!segment.m_material.name.ToLower().Contains("cable"))
                                {
                                    Texture2D texture2D = new Texture2D(1, 1);
                                    if (netInfo.name.Equals("HighwayRamp") || netInfo.name.Equals("HighwayRampElevated"))
                                    {
                                        texture2D = RoadColorChanger2.LoadTextureDDS(Path.Combine(dir, "highway_ramp_segment_apr.dds"));
                                    }

                                    else
                                    {
                                        texture2D = RoadColorChanger2.LoadTextureDDS(Path.Combine(dir, "highway_segment_apr.dds"));
                                    }
                                    texture2D.set_anisoLevel(8);
                                    segment.m_segmentMaterial.SetTexture("_APRMap", texture2D);
                                    segment.m_lodMesh = null;
                                }
                                */
                            }
                            
                            NetInfo.Node[] nodes = netInfo.m_nodes;
                            for (int k = 0; k < nodes.Length; k++)
                            {

                                NetInfo.Node node = nodes[k];
                                node.m_lodMesh = null;

                                /*                                Texture2D texture2D = new Texture2D(1, 1);
                                                                if (netInfo.name.Equals("HighwayRamp") || netInfo.name.Equals("HighwayRampElevated"))
                                                                {
                                                                    texture2D = RoadColorChanger2.LoadTextureDDS(Path.Combine(dir, "highway_ramp_node_apr.dds"));
                                                                }
                                                                else
                                                                {
                                                                    texture2D = RoadColorChanger2.LoadTextureDDS(Path.Combine(dir, "highway_node_apr.dds"));
                                                                }
                                                                texture2D.anisoLevel = 8;
                                                                node.m_nodeMaterial.SetTexture("_APRMap", texture2D);
                                                                node.m_lodMesh = null;
                                                                */
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
                if (roadtype.Equals("Train Track"))
                {
                    if (netNode.Info.name.Equals("Train Track"))
                    {
                        netNode.Info.m_color = new Color(brightness, brightness, brightness);
                    }
                }
                else if (netNode.Info.m_class.name.Equals(roadtype))
                {
                    netNode.Info.m_color = new Color(brightness, brightness, brightness);
                }
            }
            NetSegment[] buffer2 = Singleton<NetManager>.instance.m_segments.m_buffer;
            for (int i = 0; i < buffer2.Length; i++)
            {
                NetSegment netSegment = buffer2[i];
                if (roadtype.Equals("Train Track"))
                {
                    if (netSegment.Info.name.Equals("Train Track"))
                    {
                        netSegment.Info.m_color = new Color(brightness, brightness, brightness);
                    }
                }
                else if (netSegment.Info.m_class.name.Equals(roadtype))
                {
                    netSegment.Info.m_color = new Color(brightness, brightness, brightness);
                }
            }
        }
    }
}
