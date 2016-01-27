using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using ColossalFramework;




namespace RoadsUnited
{

    public class RoadsUnited : MonoBehaviour

    {
        public static Texture2D LoadTexture(string texturePath)
        {
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(File.ReadAllBytes(texturePath));
            texture2D.anisoLevel = 8;
            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.Apply();
            return texture2D;
        }


        public static void ReplaceNetTextures(string textureDir)
        {
            //Replace node textures
            NetCollection[] array = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            NetCollection[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                NetCollection netCollection = array2[i];
                NetInfo[] prefabs = netCollection.m_prefabs;
                for (int j = 0; j < prefabs.Length; j++)
                {
                    NetInfo netInfo = prefabs[j];
                    string text = netInfo.name.Replace(" ", "_").ToLowerInvariant().Trim();

                    #region.Node Textures

                    NetInfo.Node[] nodes = netInfo.m_nodes;
                    for (int k = 0; k < nodes.Length; k++)
                    {

                        NetInfo.Node node = nodes[k];
                        string text2 = Path.Combine(textureDir, text + "_n.png");
                        string text3 = Path.Combine(textureDir, node.m_mesh.name.ToLowerInvariant() + "_n.png");
                        string text4 = Path.Combine(textureDir, text + "_n_map.png");



                        // Begin replacing nodes
//                        bool flag22 = text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle") || text.Contains("oneway");
                        bool flag22 = text.Contains("oneway");

                        if (flag22)
                        {
                            bool flag220 = text.Contains("large");
                            bool flag221 = text.Contains("medium");
                            bool flag222 = text.Contains("basic") || text.Contains("small");

                            if (flag220)
                            {
                                text2 = Path.Combine(textureDir, "large_road_n.png");
                                text4 = Path.Combine(textureDir, "large_road_n_map.png");
                            }

                            if (flag221)
                            {
                                text2 = Path.Combine(textureDir, "medium_road_n.png");
                                text4 = Path.Combine(textureDir, "medium_road_n_map.png");
                            }
                            if (flag222)
                            {
                                text2 = Path.Combine(textureDir, "basic_road_n.png");
                                text4 = Path.Combine(textureDir, "basic_road_n_map.png");
                            }


                            Debug.Log("flag2 Roads United: Replaced decoration node: " + text2 + " and its n_map" + text4);

                        }

                        //Setting the node textures
                        bool flag3 = File.Exists(text3);
                        if (flag3)
                        {
                            node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text3));
                            node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text3));

                            Debug.Log("flag3 Roads United: Replaced mesh node: " + text3);
                        }

                        bool flag4 = File.Exists(text2);
                        if (flag4)
                        {
                            node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text2));
                            node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text2));

                            Debug.Log("flag4 Roads United: Replaced node: " + text2);
                        }
                        bool flag5 = File.Exists(text4);
                        if (flag5)
                        {
                            node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(text4));
                            node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(text4));

                            Debug.Log("flag5 Roads United: Replaced n_map" + text4);
                        }
                   //     node.m_combinedMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "RoadLOD.png")));

                    }


                    #endregion

                    #region.Segment Textures
                    // Look for segments



                    NetInfo.Segment[] segments = netInfo.m_segments;
                    for (int l = 0; l < segments.Length; l++)
                    {
                        NetInfo.Segment segment = segments[l];
                        string text5 = Path.Combine(textureDir, text + "_s.png");
                        string text6 = Path.Combine(textureDir, text + "_s_map.png");
                        string text55 = Path.Combine(textureDir, text + "_s_lod.png");
                        string text7 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + ".png");
                        string text77 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_map.png");
                        string text78 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_lod.png");
                        string text8 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco1_grass.png");
                        string text9 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco2_trees.png");



                        // Begin replacing segment textures

                        if ((!(text.Contains("tl") || text.Contains("3l") || text.Contains("4l") || text.Contains("avenue"))) && File.Exists(text7))
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text7));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text7));

                           if (File.Exists(text78))

                            {
                                segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text78));
                            }

                            if (File.Exists(text77))
                            {
                                segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(text77));
                                segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(text77));
                            }
                            Debug.Log("Roads United: flag5 Replaced segment material: " + text7);
                        }

                        bool flag6 = text.Contains("decoration_grass") & File.Exists(text8);
                        if (flag6)
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text8));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text8));


                        }
                        bool flag66 = text.Contains("decoration_trees") & File.Exists(text9);
                        if (flag66)
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text9));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text9));

                        }



                        //                       bool flag8 = (!(text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle") || text.Contains("oneway")) && !text.Contains("tunnel") && File.Exists(text6));
                        //						if (flag8)
                        //					{
                        if (File.Exists(text6))
                        {
                            segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(text6));
                            segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(text6));
                        }
                        //              }

                        /*							bool flag10 = (text == "medium_road_decoration_trees" || text == "medium_road_decoration_grass") & File.Exists(text8);
                                                    if (flag10)
                                                    {
                                                        segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text8));
                                                        segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text8));
                                                    }
                        */

                        bool flag200 = (text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle") || text.Contains("oneway")) && !text.Contains("tunnel") & File.Exists(text5);
                        if (flag200)
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text5));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text5));

                            if (File.Exists(text55))
                            {
                                segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, text55)));
                            }
                            

                        }
                        if (text.Equals("medium_road_bus"))
                        {

                            if (segment.m_mesh.name.Equals("RoadMediumSegmentBusSide"))

                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "roadmediumsegmentbusside-buslane.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "roadmediumsegmentbusside-buslane.png")));

                            }

                            if (segment.m_mesh.name.Equals("RoadMediumSegmentBusBoth"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "roadmediumsegmentbusboth-buslane.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "roadmediumsegmentbusboth-buslane.png")));
                            }
                        }

                        //Exception for Large Bus Roads - Bus Stops
                        if (text.Equals("large_road_bus") && text7.Contains("bus") && File.Exists(text7))
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text7));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text7));
                        }

                        //       large_oneway_s2_busside.png

                        if (text.Contains("large") && text.Contains("oneway"))
                        {

                            if (segment.m_mesh.name.Equals("LargeRoadSegmentBusSide"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "large_oneway_s_busside.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "large_oneway_s_busside.png")));

                            }

                            if (segment.m_mesh.name.Equals("LargeRoadSegment2BusSide"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "large_oneway_s2_busside.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "large_oneway_s2_busside.png")));

                            }

                            if (segment.m_mesh.name.Equals("LargeRoadSegment2") && text.Contains("decoration"))

                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "large_oneway_s2.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "large_oneway_s2.png")));

                            }

                            /*
                            if (segment.m_mesh.name.Equals("LargeRoadSegmentBusBothBuslane"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "largeroadsegmentbusboth-bus.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "largeroadsegmentbusboth-bus.png")));
                            }
                            */
                        }


                        if ((text.Contains("elevated") || text.Contains("bridge")) && text.Contains("bus"))
                        {
                            if (text.Contains("medium"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "mediumroadelevatedsegmentbus.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "mediumroadelevatedsegmentbus.png")));
                            }

                            if (text.Contains("large"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "largeroadelevatedsegmentbus.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "largeroadelevatedsegmentbus.png")));
                            }
                        }

                        //     segment.m_combinedMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "RoadLOD.png")));
                        segment.m_lodRenderDistance = 2500;
                    }
                    #endregion

                    #region.Node Exceptions elevated


                    bool flag11 = (text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle")) && !text.Contains("tunnel");
                    if (flag11)
                    {
                        NetInfo.Node[] nodes2 = netInfo.m_nodes;
                        for (int m = 0; m < nodes2.Length; m++)
                        {
                            NetInfo.Node node2 = nodes2[m];
                            string text9 = Path.Combine(textureDir, text.ToLowerInvariant() + ".png");
                            string text10 = Path.Combine(textureDir, text.ToLowerInvariant() + ".png");

                            bool flag12 = text.Contains("basic");
                            if (flag12)
                            {
                                bool flag13 = text.Contains("slope");
                                if (flag13)
                                {
                                    text9 = Path.Combine(textureDir, "basic_road_decoration_n.png");
                                    text10 = Path.Combine(textureDir, "basic_road_decoration_n_map.png");
                                }
                                else
                                {
                                    bool flag14 = text.Contains("elevated") || text.Contains("bridge");
                                    if (flag14)

                                    {
                                        if (text.Contains("bicycle"))
                                        {
                                            text9 = Path.Combine(textureDir, "smallroadelevatedbikenode.png");
                                        }
                                        else
                                        {
                                            text9 = Path.Combine(textureDir, "basic_road_bicycle_n.png");
                                        }

                                    }
                                }
                                bool flag15 = text.Contains("medium");
                                if (flag15)
                                {
                                    bool flag16 = text.Contains("slope");
                                    if (flag16)
                                    {
                                        text9 = Path.Combine(textureDir, "medium_road_n.png");
                                        text10 = Path.Combine(textureDir, "medium_road_n_map.png");
                                    }
                                    else
                                    {
                                        bool flag17 = text.Contains("elevated") || text.Contains("bridge");
                                        if (flag17)
                                        {
                                            text9 = Path.Combine(textureDir, "roadmediumelevatednode.png");
                                            text10 = Path.Combine(textureDir, "nan_map.png");
                                        }
                                        else
                                        {
                                            text9 = Path.Combine(textureDir, "medium_road_n.png");
                                            text10 = Path.Combine(textureDir, "medium_road_n_map.png");
                                        }
                                    }
                                }
                                bool flag18 = text.Contains("large");
                                if (flag18)
                                {
                                    bool flag19 = text.Contains("slope");
                                    if (flag19)
                                    {
                                        text9 = Path.Combine(textureDir, "large_road_decoration_n.png");
                                        text10 = Path.Combine(textureDir, "large_road_decoration_n_map.png");
                                    }
                                    else
                                    {
                                        bool flag20 = text.Contains("elevated") || text.Contains("bridge");
                                        if (flag20)
                                        {
                                            text9 = Path.Combine(textureDir, "largeroadelevatednode.png");
                                            text10 = Path.Combine(textureDir, "large_road_elevated_n_map.png");
                                        }
                                        else
                                        {
                                            text9 = Path.Combine(textureDir, "large_road_n.png");
                                            text10 = Path.Combine(textureDir, "large_road_n_map.png");
                                        }
                                    }
                                }
                                bool flag21 = File.Exists(text9);
                                if (flag21)
                                {
                                    node2.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text9));
                                    node2.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text9));
                                }
                                bool flag22 = File.Exists(text10);
                                if (flag22)
                                {
                                    node2.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(text10));
                                    node2.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(text10));
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            



        }

    }
}





