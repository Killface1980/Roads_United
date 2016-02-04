﻿using ColossalFramework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace RoadsUnited
{

    public class RoadsUnited : MonoBehaviour

    {
        public static Configuration config;

        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();

        private static Dictionary<string, Texture2D> prefabSegProperties = new Dictionary<string, Texture2D>();

        private static Dictionary<string, Texture2D> prefabSegMatProperties = new Dictionary<string, Texture2D>();

        private static Dictionary<string, Texture2D> prefabNodeProperties = new Dictionary<string, Texture2D>();

        private static Dictionary<string, Texture2D> prefabNodeMatProperties = new Dictionary<string, Texture2D>();


        private static Texture2D defaultmap;
        private static Texture2D xysmap;
        private static Texture2D aprmap;

        public static Texture2D LoadTextureDDS(string fullPath)
        {
            // Testen ob Textur bereits geladen, in dem Fall geladene Textur zurückgeben
            Texture2D texture;
            if (textureCache.TryGetValue(fullPath, out texture)) return texture;

            // Nein? Textur laden
            var numArray = File.ReadAllBytes(fullPath);
            var width = BitConverter.ToInt32(numArray, 16);
            var height = BitConverter.ToInt32(numArray, 12);

            texture = new Texture2D(width, height, TextureFormat.DXT5, true);
            var list = new List<byte>();

            for (int index = 0; index < numArray.Length; ++index)
            {
                if (index > (int)sbyte.MaxValue)
                    list.Add(numArray[index]);
            }
            texture.LoadRawTextureData(list.ToArray());
            texture.name = Path.GetFileName(fullPath);
            texture.anisoLevel = 8;
            texture.Apply();

            textureCache.Add(fullPath, texture); // Neu geladene Textur in den Cache packen

            return texture;
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

                    if (!(prefab_road_name.Contains("tl") || prefab_road_name.Contains("3l") || prefab_road_name.Contains("4l") || prefab_road_name.Contains("rural") || prefab_road_name.Contains("four-lane") || prefab_road_name.Contains("five-lane") || prefab_road_name.Contains("avenue") || prefab_road_name.Contains("large_highway")))
                    {


                        #region.Node Textures

                        NetInfo.Node[] nodes = netInfo.m_nodes;
                        for (int k = 0; k < nodes.Length; k++)
                        {
                            NetInfo.Node node = nodes[k];
                            string prefab_node_name_n = Path.Combine(ModLoader.currentTexturesPath_default, prefab_road_name + "_n.dds");
                            string prefab_node_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, prefab_road_name + "_n_map.dds");
                            string node_mesh_name_n = Path.Combine(ModLoader.currentTexturesPath_default, node.m_mesh.name.ToLowerInvariant() + "_n.dds");
                            string node_mesh_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, node.m_mesh.name.ToLowerInvariant() + "_n_map.dds");

                            prefabNodeProperties.Clear();
                            prefabNodeMatProperties.Clear();

                            prefabNodeProperties.Add("_MainTex", node.m_material.GetTexture("_MainTex") as Texture2D);
                            prefabNodeProperties.Add("_XYSMap", node.m_material.GetTexture("_XYSMap") as Texture2D);
                            prefabNodeProperties.Add("_APRMap", node.m_material.GetTexture("_APRMap") as Texture2D);

                            prefabNodeMatProperties.Add("_MainTex", node.m_nodeMaterial.GetTexture("_MainTex") as Texture2D);
                            prefabNodeMatProperties.Add("_XYSMap", node.m_nodeMaterial.GetTexture("_XYSMap") as Texture2D);
                            prefabNodeMatProperties.Add("_APRMap", node.m_nodeMaterial.GetTexture("_APRMap") as Texture2D);



                            // Begin replacing nodes

                            if (prefab_road_name.Contains("oneway"))
                            {
                                bool prefab_road_name_LARGE = prefab_road_name.Contains("large");
                                bool prefab_road_name_MEDIUM = prefab_road_name.Contains("medium");
                                bool prefab_road_name_SMALL = prefab_road_name.Contains("basic") || prefab_road_name.Contains("small");

                                if (prefab_road_name_LARGE)
                                {
                                    if (prefabNodeProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeProperties.Remove("_MainTex");
                                        prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_road_n.dds")));
                                    }

                                    if (prefabNodeProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeProperties.Remove("_APRMap");
                                        prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "large_road_n_map.dds")));
                                    }

                                    if (prefabNodeMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeMatProperties.Remove("_MainTex");
                                        prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_road_n.dds")));
                                    }

                                    if (prefabNodeMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeMatProperties.Remove("_APRMap");
                                        prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "large_road_n_map.dds")));
                                    }
                                }

                                if (prefab_road_name_MEDIUM)
                                {
                                    if (prefabNodeProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeProperties.Remove("_MainTex");
                                        prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "medium_road_n.dds")));
                                    }

                                    if (prefabNodeProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeProperties.Remove("_APRMap");
                                        prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "medium_road_n_map.dds")));
                                    }

                                    if (prefabNodeMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeMatProperties.Remove("_MainTex");
                                        prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "medium_road_n.dds")));
                                    }

                                    if (prefabNodeMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeMatProperties.Remove("_APRMap");
                                        prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "medium_road_n_map.dds")));
                                    }
                                }

                                if (prefab_road_name_SMALL)
                                {
                                    if (prefabNodeProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeProperties.Remove("_MainTex");
                                        prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "basic_road_n.dds")));
                                    }

                                    if (prefabNodeProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeProperties.Remove("_APRMap");
                                        prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "basic_road_n_map.dds")));
                                    }

                                    if (prefabNodeMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeMatProperties.Remove("_MainTex");
                                        prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "basic_road_n.dds")));
                                    }

                                    if (prefabNodeMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeMatProperties.Remove("_APRMap");
                                        prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "basic_road_n_map.dds")));
                                    }
                                }
                            }


                            if (prefab_road_name.Contains("highway"))
                            {

                                if ((node.m_mesh.name.Equals("HighwayRampNode1") || node.m_mesh.name.Equals("HighwayRampNode2") || netInfo.name.Equals("HighwayRamp") || netInfo.name.Equals("HighwayRampElevated")))
                                {
                                    if (prefabNodeProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeProperties.Remove("_MainTex");
                                        prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwayrampnode_n.dds")));
                                    }
                                    if (prefabNodeProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeProperties.Remove("_APRMap");
                                        prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampnode_n_map.dds")));
                                    }
                                    if (prefabNodeMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeMatProperties.Remove("_MainTex");
                                        prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwayrampnode_n.dds")));
                                    }
                                    if (prefabNodeMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeMatProperties.Remove("_APRMap");
                                        prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampnode_n_map.dds")));
                                    }
                                }

                                else // ((node.m_mesh.name.Equals("HighwayBarrierNode") || node.m_mesh.name.Equals("HighwayBarrierPavement") || node.m_mesh.name.Equals("HighwayBasePavement") || node.m_mesh.name.Equals("HighwayBridgeNode1") || (node.m_mesh.name.Equals("HighwayBridgeNode2"))))
                                {
                                    if (prefabNodeProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeProperties.Remove("_MainTex");
                                        prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwaybasenode_n.dds")));
                                    }
                                    if (prefabNodeProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeProperties.Remove("_APRMap");
                                        prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasenode_n_map.dds")));
                                    }
                                    if (prefabNodeMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabNodeMatProperties.Remove("_MainTex");
                                        prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwaybasenode_n.dds")));
                                    }
                                    if (prefabNodeMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabNodeMatProperties.Remove("_APRMap");
                                        prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasenode_n_map.dds")));
                                    }
                                }
                                node.m_lodMesh = null;
                            }

                            if (File.Exists(prefab_node_name_n))
                            {
                                if ((prefabNodeProperties.ContainsKey("_MainTex")) && File.Exists(prefab_node_name_n))
                                {
                                    prefabNodeProperties.Remove("_MainTex");
                                    prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, prefab_node_name_n)));
                                }

                                if ((prefabNodeProperties.ContainsKey("_APRMap")) && File.Exists(prefab_node_name_n_map))
                                {
                                    prefabNodeProperties.Remove("_APRMap");
                                    prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, prefab_node_name_n_map)));
                                }

                                if ((prefabNodeMatProperties.ContainsKey("_MainTex")) && File.Exists(prefab_node_name_n))
                                {
                                    prefabNodeMatProperties.Remove("_MainTex");
                                    prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, prefab_node_name_n)));
                                }

                                if ((prefabNodeMatProperties.ContainsKey("_APRMap")) && File.Exists(prefab_node_name_n_map))
                                {
                                    prefabNodeMatProperties.Remove("_APRMap");
                                    prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, prefab_node_name_n_map)));
                                }
                            }

                            else
                            if (File.Exists(node_mesh_name_n))
                            {
                                if ((prefabNodeProperties.ContainsKey("_MainTex")) && File.Exists(node_mesh_name_n))
                                {
                                    prefabNodeProperties.Remove("_MainTex");
                                    prefabNodeProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, node_mesh_name_n)));
                                }

                                if ((prefabNodeProperties.ContainsKey("_APRMap")) && File.Exists(node_mesh_name_n_map))
                                {
                                    prefabNodeProperties.Remove("_APRMap");
                                    prefabNodeProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, node_mesh_name_n_map)));
                                }

                                if ((prefabNodeMatProperties.ContainsKey("_MainTex")) && File.Exists(node_mesh_name_n))
                                {
                                    prefabNodeMatProperties.Remove("_MainTex");
                                    prefabNodeMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, node_mesh_name_n)));
                                }

                                if ((prefabNodeMatProperties.ContainsKey("_APRMap")) && File.Exists(node_mesh_name_n_map))
                                {
                                    prefabNodeMatProperties.Remove("_APRMap");
                                    prefabNodeMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, node_mesh_name_n_map)));
                                }
                            }

                            node.m_lodRenderDistance = 2500;


                            if (prefabNodeProperties.TryGetValue("_MainTex", out defaultmap)) node.m_material.SetTexture("_MainTex", defaultmap);
                            if (prefabNodeProperties.TryGetValue("_XYSMap", out xysmap)) node.m_material.SetTexture("_XYSMap", xysmap);
                            if (prefabNodeProperties.TryGetValue("_APRMap", out aprmap)) node.m_material.SetTexture("_APRMap", aprmap);

                            if (prefabNodeMatProperties.TryGetValue("_MainTex", out defaultmap)) node.m_nodeMaterial.SetTexture("_MainTex", defaultmap);
                            if (prefabNodeMatProperties.TryGetValue("_XYSMap", out xysmap)) node.m_nodeMaterial.SetTexture("_XYSMap", xysmap);
                            if (prefabNodeMatProperties.TryGetValue("_APRMap", out aprmap)) node.m_nodeMaterial.SetTexture("_APRMap", aprmap);
                        }



                        #endregion
                    }
                    #region.Segment Textures
                    // Look for segments


                    NetInfo.Segment[] segments = netInfo.m_segments;
                    for (int l = 0; l < segments.Length; l++)
                    {
                        NetInfo.Segment segment = segments[l];
                        string roadname_segment_default = Path.Combine(ModLoader.currentTexturesPath_default, prefab_road_name + "_s.dds");
                        string roadname_segment_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, prefab_road_name + "_s_map.dds");
                        string roadname_segment_lod = Path.Combine(ModLoader.currentTexturesPath_lod_rgb, prefab_road_name + "_s_lod.dds");
                        string meshname_segment_default = Path.Combine(ModLoader.currentTexturesPath_default, segment.m_mesh.name.ToLowerInvariant() + ".dds");
                        string meshname_segment_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, segment.m_mesh.name.ToLowerInvariant() + "_map.dds");
                        string meshname_segment_lod = Path.Combine(ModLoader.currentTexturesPath_lod_rgb, segment.m_mesh.name.ToLowerInvariant() + "_lod.dds");
                        string meshname_segment_deco1 = Path.Combine(ModLoader.currentTexturesPath_default, segment.m_mesh.name.ToLowerInvariant() + "_deco1_grass.dds");
                        string meshname_segment_deco2 = Path.Combine(ModLoader.currentTexturesPath_default, segment.m_mesh.name.ToLowerInvariant() + "_deco2_trees.dds");

                        //exceptions for config no parking small
                        if (ModLoader.config.basic_road_parking == 0)
                        {
                            if (meshname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, "smallroadsegment.dds")))
                            {
                                meshname_segment_default = Path.Combine(ModLoader.currentTexturesPath_noParking, "smallroadsegment_noParking.dds");
                            }

                            if (meshname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("smallroadsegmentbusside.dds"))))
                            {
                                meshname_segment_default = Path.Combine(ModLoader.currentTexturesPath_noParking, "smallroadsegmentbusside_noParking.dds");
                            }
                        }

                        if (ModLoader.config.medium_road_parking == 0)
                        {
                            if (meshname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegment.dds")))
                            {
                                meshname_segment_default = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_noParking.dds");
                            }

                            if (meshname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("roadmediumsegmentbusside.dds"))))
                            {
                                meshname_segment_default = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_noParking.dds");
                            }

                            if (meshname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("roadmediumsegmentbusboth.dds"))))
                            {
                                meshname_segment_default = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_noParking.dds");
                            }
                        }

                        if (ModLoader.config.medium_road_grass_parking == 0)
                        {
                            if (meshname_segment_deco1.Equals(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegment_deco1_grass.dds")))
                            {
                                meshname_segment_deco1 = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_noParking.dds");
                            }

                            if (meshname_segment_deco1.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("roadmediumsegmentbusside_deco1_grass.dds"))))
                            {
                                meshname_segment_deco1 = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_noParking.dds");
                            }

                            if (meshname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("roadmediumsegmentbusboth_deco1_grass.dds"))))
                            {
                                meshname_segment_deco1 = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_noParking.dds");
                            }
                        }

                        if (ModLoader.config.medium_road_trees_parking == 0)
                        {
                            if (meshname_segment_deco2.Equals(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegment_deco2_trees.dds")))
                            {
                                meshname_segment_deco2 = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_deco2_trees_noParking.dds");
                            }

                            if (meshname_segment_deco2.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("roadmediumsegmentbusside_deco2_trees.dds"))))
                            {
                                meshname_segment_deco2 = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_deco2_trees_noParking.dds");
                            }

                            if (meshname_segment_deco2.Equals(Path.Combine(ModLoader.currentTexturesPath_default, ("roadmediumsegmentbusboth_deco2_trees.dds"))))
                            {
                                meshname_segment_deco2 = Path.Combine(ModLoader.currentTexturesPath_noParking, "roadmediumsegment_deco2_trees_noParking.dds");
                            }
                        }

                        if (ModLoader.config.medium_road_bus_parking == 0)
                        {
                            if (roadname_segment_default.Equals(Path.Combine(ModLoader.currentTexturesPath_default, "medium_road_bus_s.dds")))
                            {
                                roadname_segment_default = Path.Combine(ModLoader.currentTexturesPath_noParking, "medium_road_bus_s_noParking.dds");
                            }

                        }





                        prefabSegProperties.Clear();
                        prefabSegMatProperties.Clear();

                        prefabSegProperties.Add("_MainTex", segment.m_material.GetTexture("_MainTex") as Texture2D);
                        prefabSegProperties.Add("_XYSMap", segment.m_material.GetTexture("_XYSMap") as Texture2D);
                        prefabSegProperties.Add("_APRMap", segment.m_material.GetTexture("_APRMap") as Texture2D);

                        prefabSegMatProperties.Add("_MainTex", segment.m_segmentMaterial.GetTexture("_MainTex") as Texture2D);
                        prefabSegMatProperties.Add("_XYSMap", segment.m_segmentMaterial.GetTexture("_XYSMap") as Texture2D);
                        prefabSegMatProperties.Add("_APRMap", segment.m_segmentMaterial.GetTexture("_APRMap") as Texture2D);


                        // Begin replacing segment textures
                        if (!(prefab_road_name.Contains("tl") || prefab_road_name.Contains("3l") || prefab_road_name.Contains("4l") || prefab_road_name.Contains("rural") || prefab_road_name.Contains("four-lane") || prefab_road_name.Contains("five-lane") || prefab_road_name.Contains("avenue") || prefab_road_name.Contains("large_highway")))
                        {

                            if ((prefab_road_name.Contains("highway") && !segment.m_material.name.ToLower().Contains("cable")))
                            {
                                if (prefab_road_name.Equals("highwayramp") || prefab_road_name.Equals("highwayrampelevated"))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwayrampsegment.dds")));
                                    }

                                    if (prefabSegProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegProperties.Remove("_APRMap");
                                        prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampsegment_map.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwayrampsegment.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegMatProperties.Remove("_APRMap");
                                        prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampsegment_map.dds")));
                                    }

                                    goto SetSegmentMaterial;

                                }

                                else if (prefab_road_name.Equals("highwayramp_slope"))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwayramp_tunnel.dds")));
                                    }

                                    if (prefabSegProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegProperties.Remove("_APRMap");
                                        prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayramp_tunnel_slope_map.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwayramp_tunnel.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegMatProperties.Remove("_APRMap");
                                        prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayramp_tunnel_slope_map.dds")));
                                    }

                                    goto SetSegmentMaterial;

                                }

                                else if ((prefab_road_name.Equals("highway_barrier") || prefab_road_name.Equals("highway_bridge") || prefab_road_name.Equals("highway_elevated")))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwaybasesegment.dds")));
                                    }

                                    if (prefabSegProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegProperties.Remove("_APRMap");
                                        prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasesegment_map.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highwaybasesegment.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegMatProperties.Remove("_APRMap");
                                        prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasesegment_map.dds")));
                                    }

                                    segment.m_lodMesh = null;

                                    goto SetSegmentMaterial;
                                }

                                else if (prefab_road_name.Equals("highway_slope"))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highway_tunnel.dds")));
                                    }

                                    if (prefabSegProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegProperties.Remove("_APRMap");
                                        prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highway_tunnel_slope_map.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "highway_tunnel.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_APRMap"))
                                    {
                                        prefabSegMatProperties.Remove("_APRMap");
                                        prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highway_tunnel_slope_map.dds")));
                                    }

                                    goto SetSegmentMaterial;

                                }

                                else
                                    segment.m_lodMesh = null;
                            }




                            //                           goto finished;

                            if (prefab_road_name.Contains("medium"))
                            {
                                if (prefab_road_name.Contains("decoration_grass") || prefab_road_name.Contains("decoration_trees"))
                                {
                                    if ((prefab_road_name.Contains("decoration_grass") && File.Exists(meshname_segment_deco1)))
                                    {
                                        if (prefabSegProperties.ContainsKey("_MainTex"))
                                        {
                                            prefabSegProperties.Remove("_MainTex");
                                            prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_deco1)));
                                        }

                                        if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                        {
                                            prefabSegMatProperties.Remove("_MainTex");
                                            prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_deco1)));
                                        }

                                        goto SetSegmentMaterial;
                                    }

                                    if ((prefab_road_name.Contains("decoration_trees") && File.Exists(meshname_segment_deco2)))
                                    {
                                        if (prefabSegProperties.ContainsKey("_MainTex"))
                                        {
                                            prefabSegProperties.Remove("_MainTex");
                                            prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_deco2)));
                                        }

                                        if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                        {
                                            prefabSegMatProperties.Remove("_MainTex");
                                            prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_deco2)));
                                        }

                                        goto SetSegmentMaterial;
                                    }
                                }

                                if (prefab_road_name.Equals("medium_road_bus"))
                                {
                                    if (segment.m_mesh.name.Equals("RoadMediumSegmentBusSide"))
                                    {
                                        if (ModLoader.config.medium_road_bus_parking == 0)
                                        {
                                            if (prefabSegProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegProperties.Remove("_MainTex");
                                                prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_noParking, "medium_road_bus_s_noParking.dds")));
                                            }

                                            if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegMatProperties.Remove("_MainTex");
                                                prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_noParking, "medium_road_bus_s_noParking.dds")));
                                            }
                                            goto SetSegmentMaterial;
                                        }

                                        if (ModLoader.config.medium_road_bus_parking == 1)
                                        {
                                            if (prefabSegProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegProperties.Remove("_MainTex");
                                                prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegmentbusside-buslane.dds")));
                                            }

                                            if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegMatProperties.Remove("_MainTex");
                                                prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegmentbusside-buslane.dds")));
                                            }

                                            goto SetSegmentMaterial;

                                        }
                                    }

                                    if (segment.m_mesh.name.Equals("RoadMediumSegmentBusBoth"))
                                    {
                                        if (ModLoader.config.medium_road_bus_parking == 0)
                                        {
                                            if (prefabSegProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegProperties.Remove("_MainTex");
                                                prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_noParking, "medium_road_bus_s_noParking.dds")));
                                            }

                                            if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegMatProperties.Remove("_MainTex");
                                                prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_noParking, "medium_road_bus_s_noParking.dds")));
                                            }
                                            goto SetSegmentMaterial;
                                        }

                                        if (ModLoader.config.medium_road_bus_parking == 1)
                                        {
                                            if (prefabSegProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegProperties.Remove("_MainTex");
                                                prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegmentbusboth-buslane.dds")));
                                            }

                                            if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                            {
                                                prefabSegMatProperties.Remove("_MainTex");
                                                prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "roadmediumsegmentbusboth-buslane.dds")));
                                            }

                                            goto SetSegmentMaterial;

                                        }
                                    }


                                }
                            }

                            //Exception for Large Bus Roads - Bus Stops

                            if (prefab_road_name.Equals("large_road_bus") && meshname_segment_default.Contains("bus") && File.Exists(meshname_segment_default))
                            {
                                if (prefabSegProperties.ContainsKey("_MainTex"))
                                {
                                    prefabSegProperties.Remove("_MainTex");
                                    prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_default)));
                                }

                                if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                {
                                    prefabSegMatProperties.Remove("_MainTex");
                                    prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_default)));
                                }

                                goto SetSegmentMaterial;
                            }


                            if (prefab_road_name.Contains("large") && prefab_road_name.Contains("oneway"))
                            {
                                if (segment.m_mesh.name.Equals("LargeRoadSegmentBusSide"))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_oneway_s_busside.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_oneway_s_busside.dds")));
                                    }

                                    goto SetSegmentMaterial;
                                }

                                if (segment.m_mesh.name.Equals("LargeRoadSegment2BusSide"))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_oneway_s2_busside.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_oneway_s2_busside.dds")));
                                    }

                                    goto SetSegmentMaterial;
                                }

                                if (segment.m_mesh.name.Equals("LargeRoadSegment2") && prefab_road_name.Contains("decoration"))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_oneway_s2.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "large_oneway_s2.dds")));
                                    }

                                    goto SetSegmentMaterial;
                                }

                            }

                            if ((prefab_road_name.Contains("bus") || prefab_road_name.Contains("bike") || prefab_road_name.Contains("bicycle") || prefab_road_name.Contains("oneway")) && !prefab_road_name.Contains("tunnel") & File.Exists(roadname_segment_default))
                            {
                                if ((prefabSegProperties.ContainsKey("_MainTex")) && File.Exists(roadname_segment_default))
                                {
                                    prefabSegProperties.Remove("_MainTex");
                                    prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, roadname_segment_default)));
                                }

                                if ((prefabSegProperties.ContainsKey("_APRMap")) && File.Exists(roadname_segment_map))
                                {
                                    prefabSegProperties.Remove("_APRMap");
                                    prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, roadname_segment_map)));
                                }

                                if ((prefabSegMatProperties.ContainsKey("_MainTex")) && File.Exists(roadname_segment_default))
                                {
                                    prefabSegMatProperties.Remove("_MainTex");
                                    prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, roadname_segment_default)));
                                }

                                if ((prefabSegMatProperties.ContainsKey("_APRMap")) && File.Exists(roadname_segment_map))
                                {
                                    prefabSegMatProperties.Remove("_APRMap");
                                    prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, roadname_segment_map)));
                                }
                                goto SetSegmentMaterial;
                            }

                            if ((prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge")) && prefab_road_name.Contains("bus"))
                            {
                                if ((prefab_road_name.Contains("medium") && !meshname_segment_default.Contains("suspension")))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "mediumroadelevatedsegmentbus.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "mediumroadelevatedsegmentbus.dds")));
                                    }

                                    goto SetSegmentMaterial;
                                }

                                else if ((prefab_road_name.Contains("large") && !meshname_segment_default.Contains("suspension")))
                                {
                                    if (prefabSegProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegProperties.Remove("_MainTex");
                                        prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "largeroadelevatedsegmentbus.dds")));
                                    }

                                    if (prefabSegMatProperties.ContainsKey("_MainTex"))
                                    {
                                        prefabSegMatProperties.Remove("_MainTex");
                                        prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, "largeroadelevatedsegmentbus.dds")));
                                    }
                                    goto SetSegmentMaterial;

                                }
                            }


                            if (File.Exists(roadname_segment_default))
                            {
                                if ((prefabSegProperties.ContainsKey("_MainTex")) && File.Exists(roadname_segment_default))
                                {
                                    prefabSegProperties.Remove("_MainTex");
                                    prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, roadname_segment_default)));
                                }

                                if ((prefabSegProperties.ContainsKey("_APRMap")) && File.Exists(roadname_segment_map))
                                {
                                    prefabSegProperties.Remove("_APRMap");
                                    prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, roadname_segment_map)));
                                }

                                if ((prefabSegMatProperties.ContainsKey("_MainTex")) && File.Exists(roadname_segment_default))
                                {
                                    prefabSegMatProperties.Remove("_MainTex");
                                    prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, roadname_segment_default)));
                                }

                                if ((prefabSegMatProperties.ContainsKey("_APRMap")) && File.Exists(roadname_segment_map))
                                {
                                    prefabSegMatProperties.Remove("_APRMap");
                                    prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, roadname_segment_map)));
                                }

                                goto SetSegmentMaterial;
                            }

                            if (File.Exists(meshname_segment_default))
                            {
                                if (File.Exists(meshname_segment_lod))
                                {
                                    segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_lod));
                                }

                                if ((prefabSegProperties.ContainsKey("_MainTex")) && File.Exists(meshname_segment_default))
                                {
                                    prefabSegProperties.Remove("_MainTex");
                                    prefabSegProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_default)));
                                }

                                if ((prefabSegProperties.ContainsKey("_APRMap")) && File.Exists(meshname_segment_map))
                                {
                                    prefabSegProperties.Remove("_APRMap");
                                    prefabSegProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, meshname_segment_map)));
                                }

                                if ((prefabSegMatProperties.ContainsKey("_MainTex")) && File.Exists(meshname_segment_default))
                                {
                                    prefabSegMatProperties.Remove("_MainTex");
                                    prefabSegMatProperties.Add("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_default, meshname_segment_default)));
                                }

                                if ((prefabSegMatProperties.ContainsKey("_APRMap")) && File.Exists(meshname_segment_map))
                                {
                                    prefabSegMatProperties.Remove("_APRMap");
                                    prefabSegMatProperties.Add("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, meshname_segment_map)));
                                }

                            }


                        SetSegmentMaterial:;

                            if (prefabSegProperties.TryGetValue("_MainTex", out defaultmap)) segment.m_material.SetTexture("_MainTex", defaultmap);
                            if (prefabSegProperties.TryGetValue("_XYSMap", out xysmap)) segment.m_material.SetTexture("_XYSMap", xysmap);
                            if (prefabSegProperties.TryGetValue("_APRMap", out aprmap)) segment.m_material.SetTexture("_APRMap", aprmap);

                            if (prefabSegMatProperties.TryGetValue("_MainTex", out defaultmap)) segment.m_segmentMaterial.SetTexture("_MainTex", defaultmap);
                            if (prefabSegMatProperties.TryGetValue("_XYSMap", out xysmap)) segment.m_segmentMaterial.SetTexture("_XYSMap", xysmap);
                            if (prefabSegMatProperties.TryGetValue("_APRMap", out aprmap)) segment.m_segmentMaterial.SetTexture("_APRMap", aprmap);




                            //     segment.m_combinedMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "RoadLOD.dds")));




                            segment.m_lodRenderDistance = 2500;


                            #endregion

                            #region.Node Exceptions elevated


                            bool flag11 = (prefab_road_name.Contains("bus") || prefab_road_name.Contains("bike") || prefab_road_name.Contains("bicycle")) && !prefab_road_name.Contains("tunnel");
                            if (flag11)
                            {
                                NetInfo.Node[] nodes2 = netInfo.m_nodes;
                                for (int m = 0; m < nodes2.Length; m++)
                                {
                                    NetInfo.Node node2 = nodes2[m];
                                    string text9 = Path.Combine(textureDir, prefab_road_name.ToLowerInvariant() + ".dds");
                                    string text10 = Path.Combine(textureDir, prefab_road_name.ToLowerInvariant() + ".dds");

                                    bool flag12 = prefab_road_name.Contains("basic");
                                    if (flag12)
                                    {
                                        bool flag13 = prefab_road_name.Contains("slope");
                                        if (flag13)
                                        {
                                            text9 = Path.Combine(textureDir, "basic_road_decoration_n.dds");
                                            text10 = Path.Combine(textureDir, "basic_road_decoration_n_map.dds");
                                        }
                                        else
                                        {
                                            bool flag14 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                            if (flag14)

                                            {
                                                if (prefab_road_name.Contains("bicycle"))
                                                {
                                                    text9 = Path.Combine(textureDir, "smallroadelevatedbikenode.dds");
                                                }
                                                else
                                                {
                                                    text9 = Path.Combine(textureDir, "basic_road_bicycle_n.dds");
                                                }

                                            }
                                        }
                                        bool flag15 = prefab_road_name.Contains("medium");
                                        if (flag15)
                                        {
                                            bool flag16 = prefab_road_name.Contains("slope");
                                            if (flag16)
                                            {
                                                text9 = Path.Combine(textureDir, "medium_road_n.dds");
                                                text10 = Path.Combine(textureDir, "medium_road_n_map.dds");
                                            }
                                            else
                                            {
                                                bool flag17 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                                if (flag17)
                                                {
                                                    text9 = Path.Combine(textureDir, "roadmediumelevatednode.dds");
                                                    text10 = Path.Combine(textureDir, "nan_map.dds");
                                                }
                                                else
                                                {
                                                    text9 = Path.Combine(textureDir, "medium_road_n.dds");
                                                    text10 = Path.Combine(textureDir, "medium_road_n_map.dds");
                                                }
                                            }
                                        }
                                        bool flag18 = prefab_road_name.Contains("large");
                                        if (flag18)
                                        {
                                            bool flag19 = prefab_road_name.Contains("slope");
                                            if (flag19)
                                            {
                                                text9 = Path.Combine(textureDir, "large_road_decoration_n.dds");
                                                text10 = Path.Combine(textureDir, "large_road_decoration_n_map.dds");
                                            }
                                            else
                                            {
                                                bool flag20 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                                if (flag20)
                                                {
                                                    text9 = Path.Combine(textureDir, "largeroadelevatednode.dds");
                                                    text10 = Path.Combine(textureDir, "large_road_elevated_n_map.dds");
                                                }
                                                else
                                                {
                                                    text9 = Path.Combine(textureDir, "large_road_n.dds");
                                                    text10 = Path.Combine(textureDir, "large_road_n_map.dds");
                                                }
                                            }
                                        }
                                        bool flag21 = File.Exists(text9);
                                        if (flag21)
                                        {
                                            node2.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(text9));
                                            node2.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(text9));
                                        }
                                        bool flag22 = File.Exists(text10);
                                        if (flag22)
                                        {
                                            node2.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(text10));
                                            node2.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(text10));
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

        /*
        public static CreateMaterial (string test)
        {
        return NewMaterial}*/
    }

}






