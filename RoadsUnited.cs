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
                    string prefab_road_name = netInfo.name.Replace(" ", "_").ToLowerInvariant().Trim();

                    #region.Node Textures

                    NetInfo.Node[] nodes = netInfo.m_nodes;
                    for (int k = 0; k < nodes.Length; k++)
                    {

                        NetInfo.Node node = nodes[k];
                        string prefab_node_name_n = Path.Combine(textureDir, prefab_road_name + "_n.png");
                        string prefab_node_name_n_map = Path.Combine(textureDir, prefab_road_name + "_n_map.png");
                        string node_mesh_name_n = Path.Combine(textureDir, node.m_mesh.name.ToLowerInvariant() + "_n.png");
                        string node_mesh_name_n_map = Path.Combine(textureDir, node.m_mesh.name.ToLowerInvariant() + "_n_map.png");



                        // Begin replacing nodes
                        //                        bool flag22 = text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle") || text.Contains("oneway");
                        bool flag22 = prefab_road_name.Contains("oneway");

                        if (flag22)
                        {
                            bool flag220 = prefab_road_name.Contains("large");
                            bool flag221 = prefab_road_name.Contains("medium");
                            bool flag222 = prefab_road_name.Contains("basic") || prefab_road_name.Contains("small");

                            if (flag220)
                            {
                                prefab_node_name_n = Path.Combine(textureDir, "large_road_n.png");
                                prefab_node_name_n_map = Path.Combine(textureDir, "large_road_n_map.png");
                            }

                            if (flag221)
                            {
                                prefab_node_name_n = Path.Combine(textureDir, "medium_road_n.png");
                                prefab_node_name_n_map = Path.Combine(textureDir, "medium_road_n_map.png");
                            }
                            if (flag222)
                            {
                                prefab_node_name_n = Path.Combine(textureDir, "basic_road_n.png");
                                prefab_node_name_n_map = Path.Combine(textureDir, "basic_road_n_map.png");
                            }


                            Debug.Log("flag2 Roads United: Replaced decoration node: " + prefab_node_name_n + " and its n_map" + prefab_node_name_n_map);

                        }



                        //Setting the node textures
                        bool flag3 = File.Exists(node_mesh_name_n);
                        if (flag3)
                        {
                            node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(node_mesh_name_n));
                            node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(node_mesh_name_n));
                            if (File.Exists(node_mesh_name_n_map))
                            {
                                node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(node_mesh_name_n_map));
                                node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(node_mesh_name_n_map));
                            }
                            Debug.Log("flag3 Roads United: Replaced mesh node: " + node_mesh_name_n);
                        }




                        bool flag4 = File.Exists(prefab_node_name_n);
                        if (flag4)
                        {
                            node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(prefab_node_name_n));
                            node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(prefab_node_name_n));

                            Debug.Log("flag4 Roads United: Replaced node: " + prefab_node_name_n);
                        }
                        bool flag5 = File.Exists(prefab_node_name_n_map);
                        if (flag5)
                        {
                            node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(prefab_node_name_n_map));
                            node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(prefab_node_name_n_map));

                            Debug.Log("flag5 Roads United: Replaced n_map" + prefab_node_name_n_map);
                        }
                        //     node.m_combinedMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "RoadLOD.png")));





                        if (prefab_road_name.Contains("highway"))
                        {

                            if (netInfo.name.Equals("HighwayRamp") || netInfo.name.Equals("HighwayRampElevated"))
                            {
                                node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n.png")));
                                node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n.png")));
                                node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n_map.png")));
                                node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n_map.png")));
                            }

                            else
                            {
                                Texture2D texture2D = new Texture2D(1, 1);
                                texture2D = RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode_n_map.png"));
                                node.m_nodeMaterial.SetTexture("_APRMap", texture2D);
                                node.m_material.SetTexture("_APRMap", texture2D);

                            }

                            if ((node.m_mesh.name.Equals("HighwayBasePavement") || node.m_mesh.name.Equals("HighwayBridgeNode1") || (node.m_mesh.name.Equals("HighwayBridgeNode2"))))
                            {
                                node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode_n.png")));
                                node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode_n.png")));

                                if (node.m_mesh.name.Equals("HighwayBridgeNode1"))
                                {
                                    node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode_n_map.png")));
                                    node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode_n_map.png")));
                                }
                                else
                                {
                                    node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode2_n_map.png")));
                                    node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasenode2_n_map.png")));
                                }
                            }

                            if ((node.m_mesh.name.Equals("HighwayRampNode1") || (node.m_mesh.name.Equals("HighwayRampNode2"))))
                            {
                                node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n.png")));
                                node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n.png")));
                                node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n_map.png")));
                                node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampnode_n_map.png")));
                            }





                            node.m_lodRenderDistance = 6500;
                            node.m_lodMesh = null;

                        }
                        else
                        {
                            node.m_lodRenderDistance = 2500;
                        }
                    }


                    #endregion

                    #region.Segment Textures
                    // Look for segments



                    NetInfo.Segment[] segments = netInfo.m_segments;
                    for (int l = 0; l < segments.Length; l++)
                    {
                        NetInfo.Segment segment = segments[l];
                        string roadname_segment_default = Path.Combine(textureDir, prefab_road_name + "_s.png");
                        string roadname_segment_map = Path.Combine(textureDir, prefab_road_name + "_s_map.png");
                        string roadname_segment_lod = Path.Combine(textureDir, prefab_road_name + "_s_lod.png");
                        string meshname_segment_default = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + ".png");
                        string meshname_segment_map = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_map.png");
                        string meshname_segment_lod = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_lod.png");
                        string meshname_segment_deco1 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco1_grass.png");
                        string meshname_segment_deco2 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco2_trees.png");



                        // Begin replacing segment textures

                        if ((!(prefab_road_name.Contains("tl") || prefab_road_name.Contains("3l") || prefab_road_name.Contains("4l") || prefab_road_name.Contains("highway4l") ||  prefab_road_name.Contains("avenue"))) && File.Exists(meshname_segment_default))
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_default));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_default));

                            if (File.Exists(meshname_segment_lod))

                            {
                                segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_lod));
                            }

                            if (File.Exists(meshname_segment_map))
                            {
                                segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(meshname_segment_map));
                                segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(meshname_segment_map));
                            }

                            if (prefab_road_name.Contains("highway") && !segment.m_material.name.ToLower().Contains("cable"))
                            {
                                if (prefab_road_name.Equals("highwayramp") || prefab_road_name.Equals("highwayrampelevated"))
                                {
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampsegment_map.png")));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampsegment_map.png")));
                                }

                                else
                                {
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasesegment_map.png")));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasesegment_map.png")));

                                }

                                segment.m_lodMesh = null;
                            }




                            Debug.Log("Roads United: flag5 Replaced segment material: " + meshname_segment_default);


                        }
                        if ((prefab_road_name.Equals("highway_barrier") || prefab_road_name.Equals("highway_bridge") || prefab_road_name.Equals("highway_elevated")))
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasesegment.png")));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasesegment.png")));
                            segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasesegment_map.png")));
                            segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwaybasesegment_map.png")));

                        }

                        bool flag6 = prefab_road_name.Contains("decoration_grass") & File.Exists(meshname_segment_deco1);
                        if (flag6)
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_deco1));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_deco1));


                        }
                        bool flag66 = prefab_road_name.Contains("decoration_trees") & File.Exists(meshname_segment_deco2);
                        if (flag66)
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_deco2));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_deco2));

                        }



                        //                       bool flag8 = (!(text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle") || text.Contains("oneway")) && !text.Contains("tunnel") && File.Exists(roadname_segment_map));
                        //						if (flag8)
                        //					{
                        if (File.Exists(roadname_segment_map))
                        {
                            segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(roadname_segment_map));
                            segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(roadname_segment_map));
                        }
                        //              }

                        /*							bool flag10 = (text == "medium_road_decoration_trees" || text == "medium_road_decoration_grass") & File.Exists(meshname_segment_deco1);
                                                    if (flag10)
                                                    {
                                                        segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_deco1));
                                                        segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_deco1));
                                                    }
                        */

                        bool flag200 = (prefab_road_name.Contains("bus") || prefab_road_name.Contains("bike") || prefab_road_name.Contains("bicycle") || prefab_road_name.Contains("oneway")) && !prefab_road_name.Contains("tunnel") & File.Exists(roadname_segment_default);
                        if (flag200)
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(roadname_segment_default));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(roadname_segment_default));

                            if (File.Exists(roadname_segment_lod))
                            {
                                segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, roadname_segment_lod)));
                            }


                        }
                        if (prefab_road_name.Equals("medium_road_bus"))
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
                        if (prefab_road_name.Equals("large_road_bus") && meshname_segment_default.Contains("bus") && File.Exists(meshname_segment_default))
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_default));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(meshname_segment_default));
                        }

                        //       large_oneway_s2_busside.png

                        if (prefab_road_name.Contains("large") && prefab_road_name.Contains("oneway"))
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

                            if (segment.m_mesh.name.Equals("LargeRoadSegment2") && prefab_road_name.Contains("decoration"))

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


                        if ((prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge")) && prefab_road_name.Contains("bus"))
                        {
                            if (prefab_road_name.Contains("medium"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "mediumroadelevatedsegmentbus.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "mediumroadelevatedsegmentbus.png")));
                            }

                            if (prefab_road_name.Contains("large"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "largeroadelevatedsegmentbus.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "largeroadelevatedsegmentbus.png")));
                            }
                        }

                        //     segment.m_combinedMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "RoadLOD.png")));







                        if (prefab_road_name.Contains("highway"))
                        {
                            if (prefab_road_name.Contains("ramp"))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampsegment.png")));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampsegment.png")));
                                segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampsegment_map.png")));
                                segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTexture(Path.Combine(textureDir, "highwayrampsegment_map.png")));

                            }
                            segment.m_lodRenderDistance = 6500;
                            segment.m_lodMesh = null;

                        }
                        else
                        {
                            segment.m_lodRenderDistance = 2500;
                        }
                    }
                    #endregion

                    #region.Node Exceptions elevated


                    bool flag11 = (prefab_road_name.Contains("bus") || prefab_road_name.Contains("bike") || prefab_road_name.Contains("bicycle")) && !prefab_road_name.Contains("tunnel");
                    if (flag11)
                    {
                        NetInfo.Node[] nodes2 = netInfo.m_nodes;
                        for (int m = 0; m < nodes2.Length; m++)
                        {
                            NetInfo.Node node2 = nodes2[m];
                            string text9 = Path.Combine(textureDir, prefab_road_name.ToLowerInvariant() + ".png");
                            string text10 = Path.Combine(textureDir, prefab_road_name.ToLowerInvariant() + ".png");

                            bool flag12 = prefab_road_name.Contains("basic");
                            if (flag12)
                            {
                                bool flag13 = prefab_road_name.Contains("slope");
                                if (flag13)
                                {
                                    text9 = Path.Combine(textureDir, "basic_road_decoration_n.png");
                                    text10 = Path.Combine(textureDir, "basic_road_decoration_n_map.png");
                                }
                                else
                                {
                                    bool flag14 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                    if (flag14)

                                    {
                                        if (prefab_road_name.Contains("bicycle"))
                                        {
                                            text9 = Path.Combine(textureDir, "smallroadelevatedbikenode.png");
                                        }
                                        else
                                        {
                                            text9 = Path.Combine(textureDir, "basic_road_bicycle_n.png");
                                        }

                                    }
                                }
                                bool flag15 = prefab_road_name.Contains("medium");
                                if (flag15)
                                {
                                    bool flag16 = prefab_road_name.Contains("slope");
                                    if (flag16)
                                    {
                                        text9 = Path.Combine(textureDir, "medium_road_n.png");
                                        text10 = Path.Combine(textureDir, "medium_road_n_map.png");
                                    }
                                    else
                                    {
                                        bool flag17 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
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
                                bool flag18 = prefab_road_name.Contains("large");
                                if (flag18)
                                {
                                    bool flag19 = prefab_road_name.Contains("slope");
                                    if (flag19)
                                    {
                                        text9 = Path.Combine(textureDir, "large_road_decoration_n.png");
                                        text10 = Path.Combine(textureDir, "large_road_decoration_n_map.png");
                                    }
                                    else
                                    {
                                        bool flag20 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
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





