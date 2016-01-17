using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using ColossalFramework;


namespace RoadsUnited
{
    public class Configuration
    {
        public bool disable_optional_arrows = true;

        public bool use_alternate_pavement_texture = false;

        public bool use_cracked_roads = false;

        public float crackIntensity = 1f;

        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }

        public static void Serialize(string filename, Configuration config)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                config.OnPreSerialize();
                xmlSerializer.Serialize(streamWriter, config);
            }
        }

        public static Configuration Deserialize(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
            Configuration result;
            try
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    Configuration configuration = (Configuration)xmlSerializer.Deserialize(streamReader);
                    configuration.OnPostDeserialize();
                    result = configuration;
                    return result;
                }
            }
            catch
            {
            }
            result = null;
            return result;
        }
    }


    public class RoadsUnited : MonoBehaviour
    
	{


    private static Texture2D cracksTex;

        public static Configuration config;

        public static readonly string configPath = (Path.Combine(RoadsUnitedModLoader.getModPath(), "RoadsUnitedConfig.xml"));

        private static PropInfo sl15 = new PropInfo();

        private static PropInfo sl25 = new PropInfo();

        private static PropInfo sl30 = new PropInfo();

        private static PropInfo sl45 = new PropInfo();

        private static PropInfo sl65 = new PropInfo();

        private static PropInfo left_turn = new PropInfo();

        private static PropInfo right_turn = new PropInfo();

        private static PropInfo motorwaysign = new PropInfo();

        private static bool motorwaypropfound = false;

        private static int turnsignpropsfound = 0;

        private static PropInfo parkingsign = new PropInfo();

        private static bool parkingsignpropfound = false;

        private static int slpropsfound = 0;

        public static void SaveConfig()
        {
            Configuration.Serialize(RoadsUnited.configPath, RoadsUnited.config);
        }


        // American_Roads.RoadsUnited
    
    

        public static Texture2D LoadTexture(string texturePath)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.LoadImage(File.ReadAllBytes(texturePath));
            texture2D.anisoLevel = 8;
            return texture2D;
		}

        public static Texture2D LoadTextureDDS(string texturePath)
        {
            byte[] array = File.ReadAllBytes(texturePath);
            int num = BitConverter.ToInt32(array, 12);
            int num2 = BitConverter.ToInt32(array, 16);
            Texture2D texture2D = new Texture2D(num2, num);
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
            texture2D.anisoLevel = 8;
            return texture2D;
        }


        // American_Roads.RoadsUnited
        public static void ChangeProps(string textureDir, bool remove_arrows)
        {
            var prop_collections = UnityEngine.Object.FindObjectsOfType<PropCollection>();
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    if (remove_arrows && (prefab.name.Equals("Road Arrow LFR") || prefab.name.Equals("Road Arrow LR")))
                    {
                        prefab.m_maxRenderDistance = 0f;
                        prefab.m_maxScale = 0f;
                        prefab.m_minScale = 0f;
                    }

                    if (prefab.name.Equals("Motorway Overroad Signs"))
                    {
                        //var tex = new Texture2D (1, 1);
                        //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs.png")));
                        prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs.dds")));
                        //var tex2 = new Texture2D (1, 1);
                        //tex2.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs-motorway-overroad-signs-aci.png")));
                        prefab.m_material.SetTexture("_ACIMap", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs-motorway-overroad-signs-aci.dds")));
                        //var tex3 = new Texture2D (1, 1);
                        //tex3.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs-motorway-overroad-signs-xys.png")));
                        prefab.m_material.SetTexture("_XYSMap", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs-motorway-overroad-signs-xys.dds")));
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        //prefab.m_maxRenderDistance = 12000;
                        prefab.RefreshLevelOfDetail();
                    }
                    else if (prefab.name.Equals("Street Name Sign"))
                    {
                        //var tex = new Texture2D (1, 1);
                        //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "street-name-sign.png")));
                        prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "street-name-sign.dds")));
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;

                        prefab.RefreshLevelOfDetail();
                    }
                }
            }


            var net_collections = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            foreach (var nc in net_collections)
            {
                foreach (var prefab in nc.m_prefabs)
                {
                    if (prefab.m_class.name.Equals("Highway"))
                    {
                        foreach (var lane in prefab.m_lanes)
                        {
                            var list = new FastList<NetLaneProps.Prop>();
                            foreach (var prop in lane.m_laneProps.m_props)
                            {
                                if (remove_arrows)
                                {
                                    if (!prop.m_prop.name.Equals("Road Arrow F") && !prop.m_prop.name.Equals("Manhole"))
                                    {
                                        list.Add(prop);
                                    }
                                }
                                else if (!prop.m_prop.name.Equals("Manhole"))
                                {
                                    list.Add(prop);
                                }
                            }
                            lane.m_laneProps.m_props = list.ToArray();
                        }

                    }
                    if (prefab.m_class.name.Contains("Elevated") || prefab.m_class.name.Contains("Bridge"))
                    {
                        foreach (var lane in prefab.m_lanes)
                        {
                            var list = new FastList<NetLaneProps.Prop>();
                            foreach (var prop in lane.m_laneProps.m_props)
                            {
                                if (!prop.m_prop.name.Equals("Manhole"))
                                {
                                    list.Add(prop);
                                }
                            }
                            lane.m_laneProps.m_props = list.ToArray();
                        }
                    }
                }
            }
        }

        public static void ReplaceNetTextures(string textureDir)
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
					string text = netInfo.name.Replace(" ", "_").ToLowerInvariant().Trim();
//					bool flag = !text.Contains("bus") && !text.Contains("bike") && !text.Contains("bicycle");
//					if (flag)
//					{
						NetInfo.Node[] nodes = netInfo.m_nodes;
						for (int k = 0; k < nodes.Length; k++)
						{

							NetInfo.Node node = nodes[k];
							string text2 = Path.Combine(textureDir, text + "_n.png");
							string text3 = Path.Combine(textureDir, node.m_mesh.name.ToLowerInvariant() + ".png");
							string text4 = Path.Combine(textureDir, text + "_n_map.png");
						
							bool flag2 = text.Contains("decoration");
                        if (flag2)
                        {
                            text2 = Path.Combine(textureDir, text.Remove(text.Length - 6) + "_n.png");
                            text4 = Path.Combine(textureDir, text.Remove(text.Length - 6) + "_n_map.png");
                        }
                            bool flag22 = text.Contains("bus") || text.Contains("bike") || text.Contains("bicycle") || text.Contains("oneway");
                            if (flag22)
                            {
                                bool flag220 = text.Contains("large");
                                bool flag221 = text.Contains("medium");
                                //                                bool flag222 = text.Contains

                                if  (flag220)
                                    {
                                    text2 = Path.Combine(textureDir, "large_road_n.png");
                                    text4 = Path.Combine(textureDir, "large_road_n_map.png");
                                    }

                                if (flag221)
                                    {
                                    text2 = Path.Combine(textureDir, "medium_road_n.png");
                                    text4 = Path.Combine(textureDir, "medium_road_n_map.png");
                                }

                                
                                
                                    text2 = Path.Combine(textureDir, "basic_road_n.png");
                                    text4 = Path.Combine(textureDir, "basic_road_n_map.png");
                                


                                Debug.Log("flag2 Roads United: Replaced decoration node: " + text2 + " and its n_map" + text4);

							}
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
						}
						NetInfo.Segment[] segments = netInfo.m_segments;
                    for (int l = 0; l < segments.Length; l++)
                    {
                        NetInfo.Segment segment = segments[l];
                        string text5 = Path.Combine(textureDir, text + "_s.png");
                        string text6 = Path.Combine(textureDir, text + "_s_map.png");

                        string text7 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + ".png");
                        string text77 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_map.png");

                        string text8 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco1_grass.png");
                        string text9 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco2_trees.png");
                        //                       string text800 = Path.Combine(Texture, segment.m_mesh.name.ToLowerInvariant() + )

                        //						string text101 = Path.Combine(textureDir, segment.m_material.mainTexture.name.ToLowerInvariant() + ".png");
                        //						bool flag5 =  ; 
                        //					if (flag5)
                        //						{
                        if ((!(text.Contains("tl") || text.Contains("3l") || text.Contains("4l") || text.Contains("avenue"))) && File.Exists(text7))
                        {
                            segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTexture(text7));
                            segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTexture(text7));


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

                        if (text.Contains("large") && text.Contains("oneway") && text.Contains("decoration"))
                                              {

                                                  if (segment.m_mesh.name.Equals("LargeRoadSegment2"))

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
                    }


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
										text9 = Path.Combine(textureDir, "smallroadelevatednode.png");
										text10 = Path.Combine(textureDir, "basic_road_elevated_n_map.png");
									}
									else
									{
										text9 = Path.Combine(textureDir, "basic_road_n.png");
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
			}
		}





    }


}


