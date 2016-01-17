using ICities;
using ColossalFramework;
using ColossalFramework.Steamworks;
using ColossalFramework.IO;
using ColossalFramework.Math;
using System;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using ColossalFramework.Packaging;
//using ColossalFramework.Packaging.Package;


namespace American_Roads
{
    public class Mod : IUserMod
    {
        public const UInt64 workshop_id = 418637762;

        public string Name
        {
            get { return "American Roads"; }
        }

        public string Description
        {
            get { return "Americanizes Roads"; }
        }
    }

    public class Configuration
    {

        public bool disable_optional_arrows = true;
        public bool use_alternate_pavement_texture = true;
        //public bool use_new_signs = true;


        //public bool highway_ramps_enabled = true;
        //public bool highways_enabled = false;
        //public string NOTE = "IF HIGHWAYS/HGHWY RAMPS ENABLED, THEIR TEXTURES MAY BE FLIPPED AND NEED TO BE REBUILT TO FLIP THE CORRECT WAY! BUILDING IN SHORT SEGMENTS AVOIDS THIS PROBLEM!";

        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }

        public static void Serialize(string filename, Configuration config)
        {
            var serializer = new XmlSerializer(typeof(Configuration));

            using (var writer = new StreamWriter(filename))
            {
                config.OnPreSerialize();
                serializer.Serialize(writer, config);
            }
        }

        public static Configuration Deserialize(string filename)
        {
            var serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                using (var reader = new StreamReader(filename))
                {
                    var config = (Configuration)serializer.Deserialize(reader);
                    config.OnPostDeserialize();
                    return config;
                }
            }
            catch { }

            return null;
        }
    }

    public struct RedirectCallsState
    {
        public byte a, b, c, d, e;
        public ulong f;
    }

    public static class RedirectionHelper
    {
        /// <summary>
        /// Redirects all calls from method 'from' to method 'to'.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static RedirectCallsState RedirectCalls(MethodInfo from, MethodInfo to)
        {
            // GetFunctionPointer enforces compilation of the method.
            var fptr1 = from.MethodHandle.GetFunctionPointer();
            var fptr2 = to.MethodHandle.GetFunctionPointer();
            return PatchJumpTo(fptr1, fptr2);
        }

        public static void RevertRedirect(MethodInfo from, RedirectCallsState state)
        {
            var fptr1 = from.MethodHandle.GetFunctionPointer();
            RevertJumpTo(fptr1, state);
        }

        /// <summary>
        /// Primitive patching. Inserts a jump to 'target' at 'site'. Works even if both methods'
        /// callers have already been compiled.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="target"></param>
        private static RedirectCallsState PatchJumpTo(IntPtr site, IntPtr target)
        {
            RedirectCallsState state = new RedirectCallsState();

            // R11 is volatile.
            unsafe
            {
                byte* sitePtr = (byte*)site.ToPointer();
                state.a = *sitePtr;
                state.b = *(sitePtr + 1);
                state.c = *(sitePtr + 10);
                state.d = *(sitePtr + 11);
                state.e = *(sitePtr + 12);
                state.f = *((ulong*)(sitePtr + 2));

                *sitePtr = 0x49; // mov r11, target
                *(sitePtr + 1) = 0xBB;
                *((ulong*)(sitePtr + 2)) = (ulong)target.ToInt64();
                *(sitePtr + 10) = 0x41; // jmp r11
                *(sitePtr + 11) = 0xFF;
                *(sitePtr + 12) = 0xE3;
            }

            return state;
        }

        private static void RevertJumpTo(IntPtr site, RedirectCallsState state)
        {
            unsafe
            {
                byte* sitePtr = (byte*)site.ToPointer();
                *sitePtr = state.a; // mov r11, target
                *(sitePtr + 1) = state.b;
                *((ulong*)(sitePtr + 2)) = state.f;
                *(sitePtr + 10) = state.c; // jmp r11
                *(sitePtr + 11) = state.d;
                *(sitePtr + 12) = state.e;
            }
        }

    }

    public class ModLoader : LoadingExtensionBase
    {

        public Configuration config;
        public static readonly string configPath = "AmericanRoadsConfig.xml";



        GameObject hookGo;
        Hook4 hook;


        public static bool colorChangerActive()
        {
            foreach (var plugin in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo())
            {
                if (plugin.isEnabled)
                {
                    if (plugin.name.Equals("417585852"))
                    {
                        //Debug.Log ("AR ENABLED");
                        return true;
                    }
                }
            }
            return false;
        }


        public static string getModPath()
        {
            string workshopPath = ".";
            foreach (PublishedFileId mod in Steam.workshop.GetSubscribedItems())
            {
                if (mod.AsUInt64 == Mod.workshop_id)
                {
                    workshopPath = Steam.workshop.GetSubscribedItemPath(mod);
                    Debug.Log("American Roads: Workshop path: " + workshopPath);
                    break;
                }
            }
            string localPath = DataLocation.modsPath + "/AmericanRoads";
            Debug.Log("American Roads: " + localPath);
            if (System.IO.Directory.Exists(localPath))
            {
                Debug.Log("American Roads: Local path exists, looking for assets here: " + localPath);
                return localPath;
            }
            return workshopPath;

        }

        public override void OnCreated(ILoading loading)
        {
            foreach (PublishedFileId mod in Steam.workshop.GetSubscribedItems())
            {
                if (mod.AsUInt64 == Mod.workshop_id)
                {
                    Singleton<PackageManager>.instance.LoadPackages(mod);
                }
            }

            base.OnCreated(loading);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            config = Configuration.Deserialize(configPath);
            if (config == null)
            {
                config = new Configuration();
            }
            SaveConfig();

            hookGo = new GameObject("AmericanRoads hook");
            hook = hookGo.AddComponent<Hook4>();


            string path = getModPath();
            AmericanRoads.ReplaceTextures(path, colorChangerActive());
            AmericanRoads.ChangeProps(path, config.disable_optional_arrows);
            if (config.use_alternate_pavement_texture)
            {
                AmericanRoads.ReplacePavement(path);
            }

            //if (config.replace_speedlimit_signs) {
            AmericanRoads.ReplaceSpeedLimitSigns();
            //}

            //if (config.replace_motorway_sign) {
            AmericanRoads.ReplaceMotorwaySign();
            //}

            //if (config.replace_turn_signs) {
            AmericanRoads.ReplaceTurnSigns();
            //}

            //if (config.replace_noparking_sign){
            AmericanRoads.ReplaceParkingSigns();
            //}
            //hook.SetBridgeMat ();
            //AmericanRoads.ReplaceSigns (path);

            base.OnLevelLoaded(mode);
        }

        public override void OnLevelUnloading()
        {
            hook.DisableHook();

            GameObject.Destroy(hookGo);
            hook = null;
            base.OnLevelUnloading();
        }

        void SaveConfig()
        {
            Configuration.Serialize(configPath, config);
        }

    }

    public class Hook4 : MonoBehaviour
    {
        public bool hookEnabled = false;
        private Dictionary<MethodInfo, RedirectCallsState> redirects = new Dictionary<MethodInfo, RedirectCallsState>();
        public static Material invertedBridgeMat;

        //public Color[] nodeColors = new Color[32768];
        //public Color[] segmentColors = new Color[32768];

        //
        //private MethodInfo refreshBendData;



        public void Update()
        {
            if (!hookEnabled)
            {
                EnableHook();
            }

            /*for(int i = 0; i < Singleton<NetManager>.instance.m_segments.m_buffer.Length; i++)
			{
				if(segmentColors[i] != null){
					segmentColors [i] = Color.Lerp (segmentColors [i], new Color (0.5f, 0.5f, 0.5f), Time.deltaTime);
				}
			}*/

        }

        public void EnableHook()
        {
            var allFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            var method = typeof(NetSegment).GetMethods(allFlags).Single(c => c.Name == "RenderInstance" && c.GetParameters().Length == 3);
            redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethod("RenderInstanceSegment", allFlags)));

            method = typeof(NetSegment).GetMethods(allFlags).Single(c => c.Name == "RenderLod");
            redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethod("RenderInstanceSegment", allFlags)));

            method = typeof(NetNode).GetMethods(allFlags).Single(c => c.Name == "RenderInstance" && c.GetParameters().Length == 3);
            redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethods(allFlags).Single(c => c.Name == "RenderInstanceNode" && c.GetParameters().Length == 3)));

            method = typeof(NetNode).GetMethods(allFlags).Single(c => c.Name == "RenderLod");
            redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Hook4).GetMethods(allFlags).Single(c => c.Name == "RenderInstanceNode" && c.GetParameters().Length == 3)));

            //refreshJunctionData = GetMethod ("RefreshJunctionData", 3);
            //refreshBendData = GetMethod ("RefreshBendData", 4);



            hookEnabled = true;
        }

        private MethodInfo GetMethod(string name, uint argCount)
        {
            MethodInfo[] methods = typeof(NetNode).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (MethodInfo m in methods)
            {
                if (m.Name == name && m.GetParameters().Length == argCount)
                    return m;
            }
            return null;
        }

        public void DisableHook()
        {
            if (!hookEnabled)
            {
                return;
            }
            foreach (var kvp in redirects)
            {

                RedirectionHelper.RevertRedirect(kvp.Key, kvp.Value);
            }
            redirects.Clear();
            hookEnabled = false;
        }

        private void RefreshJunctionData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex)
        {
            MethodInfo refreshJunctionData = GetMethod("RefreshJunctionData", 3);
            object[] p = new object[] { nodeID, info, instanceIndex };
            refreshJunctionData.Invoke(netnode, p);
            /*NetManager instance = Singleton<NetManager>.instance;
			Vector3 centerPos = netnode.m_position;
			for (int index1 = 0; index1 < 8; ++index1)
			{
				ushort segment1 = netnode.GetSegment(index1);
				if ((int) segment1 != 0)
				{
					NetInfo info1 = instance.m_segments.m_buffer[(int) segment1].Info;
					ItemClass connectionClass1 = info1.GetConnectionClass();
					Vector3 vector3_1 = (int) nodeID != (int) instance.m_segments.m_buffer[(int) segment1].m_startNode ? instance.m_segments.m_buffer[(int) segment1].m_endDirection : instance.m_segments.m_buffer[(int) segment1].m_startDirection;
					float a = -1f;
					for (int index2 = 0; index2 < 8; ++index2)
					{
						ushort segment2 = netnode.GetSegment(index2);
						if ((int) segment2 != 0 && (int) segment2 != (int) segment1)
						{
							NetInfo info2 = instance.m_segments.m_buffer[(int) segment2].Info;
							ItemClass connectionClass2 = info2.GetConnectionClass();
							if (connectionClass1.m_service == connectionClass2.m_service)
							{
								Vector3 vector3_2 = (int) nodeID != (int) instance.m_segments.m_buffer[(int) segment2].m_startNode ? instance.m_segments.m_buffer[(int) segment2].m_endDirection : instance.m_segments.m_buffer[(int) segment2].m_startDirection;
								float b = (float) ((double) vector3_1.x * (double) vector3_2.x + (double) vector3_1.z * (double) vector3_2.z);
								a = Mathf.Max(a, b);
								if (index2 > index1 && info.m_requireDirectRenderers)
								{
									float num = 0.01f - Mathf.Min(info1.m_maxTurnAngleCos, info2.m_maxTurnAngleCos);
									if ((double) b < (double) num && (int) instanceIndex != (int) ushort.MaxValue)
									{
										if ((double) info1.m_netAI.GetNodeInfoPriority(segment1, ref instance.m_segments.m_buffer[(int) segment1]) >= (double) info2.m_netAI.GetNodeInfoPriority(segment2, ref instance.m_segments.m_buffer[(int) segment2]))
											RefreshJunctionData(netnode, nodeID, index1, info1, segment1, segment2, ref instanceIndex, ref Singleton<RenderManager>.instance.m_instances[instanceIndex]);
										else
											RefreshJunctionData(netnode, nodeID, index2, info2, segment2, segment1, ref instanceIndex, ref Singleton<RenderManager>.instance.m_instances[instanceIndex]);
									}
								}
							}
						}
					}
					centerPos += vector3_1 * (float) (2.0 + (double) a * 2.0);
				}
			}
			centerPos.y = netnode.m_position.y + (float) netnode.m_heightOffset * (float) (1.0 / 64.0);
			if (!info.m_requireSegmentRenderers)
				return;
			for (int index = 0; index < 8; ++index)
			{
				ushort segment = netnode.GetSegment(index);
				if ((int) segment != 0 && (int) instanceIndex != (int) ushort.MaxValue)
					RefreshJunctionData(netnode, nodeID, index, segment, centerPos, ref instanceIndex, ref Singleton<RenderManager>.instance.m_instances[instanceIndex]);
			}*/
        }

        private void RefreshBendData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex, ref RenderManager.Instance data)
        {
            MethodInfo refreshBendData = GetMethod("RefreshBendData", 4);
            object[] p = new object[] { nodeID, info, instanceIndex, data };
            refreshBendData.Invoke(netnode, p);
            data = (RenderManager.Instance)p[3];


            //data.m_position = netnode.m_position;
            /*data.m_rotation = Quaternion.identity;
			data.m_initialized = true;
			float vScale = 0.05f;
			Vector3 cornerPos1 = Vector3.zero;
			Vector3 cornerPos2 = Vector3.zero;
			Vector3 cornerPos3 = Vector3.zero;
			Vector3 cornerPos4 = Vector3.zero;
			Vector3 cornerDirection1 = Vector3.zero;
			Vector3 cornerDirection2 = Vector3.zero;
			Vector3 cornerDirection3 = Vector3.zero;
			Vector3 cornerDirection4 = Vector3.zero;
			bool flag1 = false;
			int num = 0;
			for (int index = 0; index < 8; ++index)
			{
				ushort segment = netnode.GetSegment(index);
				if ((int) segment != 0)
				{
					NetSegment netSegment = Singleton<NetManager>.instance.m_segments.m_buffer[(int) segment];
					bool flag2 = ++num == 1;
					bool start = (int) netSegment.m_startNode == (int) nodeID;
					bool smooth;
					if (!flag2 && !flag1 || flag2 && !start)
					{
						netSegment.CalculateCorner(segment, true, start, false, out cornerPos1, out cornerDirection1, out smooth);
						netSegment.CalculateCorner(segment, true, start, true, out cornerPos2, out cornerDirection2, out smooth);
						flag1 = true;
					}
					else
					{
						netSegment.CalculateCorner(segment, true, start, true, out cornerPos3, out cornerDirection3, out smooth);
						netSegment.CalculateCorner(segment, true, start, false, out cornerPos4, out cornerDirection4, out smooth);
					}
				}
			}
			Vector3 middlePos1_1;
			Vector3 middlePos2_1;
			NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos3, -cornerDirection3, true, true, out middlePos1_1, out middlePos2_1);
			Vector3 middlePos1_2;
			Vector3 middlePos2_2;
			NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos4, -cornerDirection4, true, true, out middlePos1_2, out middlePos2_2);
			data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, netnode.m_position, vScale);
			data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, netnode.m_position, vScale);
			data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
			data.m_dataVector3 = RenderManager.GetColorLocation(65536U + (uint) nodeID);
			data.m_dataColor0 = info.m_color;
			data.m_dataColor0.a = 0.0f;
			if (!info.m_requireSurfaceMaps)
				return;
			Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);*/
        }

        private void RefreshJunctionData(NetNode netnode, ushort nodeID, int segmentIndex, ushort nodeSegment, Vector3 centerPos, ref uint instanceIndex, ref RenderManager.Instance data)
        {
            NetManager instance = Singleton<NetManager>.instance;
            data.m_position = netnode.m_position;
            data.m_rotation = Quaternion.identity;
            data.m_initialized = true;
            float vScale = 0.05f;
            Vector3 cornerPos1 = Vector3.zero;
            Vector3 cornerPos2 = Vector3.zero;
            Vector3 cornerDirection1 = Vector3.zero;
            Vector3 cornerDirection2 = Vector3.zero;
            Vector3 cornerPos3 = Vector3.zero;
            Vector3 cornerPos4 = Vector3.zero;
            Vector3 cornerDirection3 = Vector3.zero;
            Vector3 cornerDirection4 = Vector3.zero;
            Vector3 cornerPos5 = Vector3.zero;
            Vector3 cornerPos6 = Vector3.zero;
            Vector3 cornerDirection5 = Vector3.zero;
            Vector3 cornerDirection6 = Vector3.zero;
            NetSegment netSegment1 = instance.m_segments.m_buffer[(int)nodeSegment];
            NetInfo info1 = netSegment1.Info;
            ItemClass connectionClass1 = info1.GetConnectionClass();
            Vector3 vector3_1 = (int)nodeID != (int)netSegment1.m_startNode ? netSegment1.m_endDirection : netSegment1.m_startDirection;
            float num1 = -4f;
            float num2 = -4f;
            ushort segmentID1 = (ushort)0;
            ushort segmentID2 = (ushort)0;
            for (int index = 0; index < 8; ++index)
            {
                ushort segment = netnode.GetSegment(index);
                if ((int)segment != 0 && (int)segment != (int)nodeSegment)
                {
                    ItemClass connectionClass2 = instance.m_segments.m_buffer[(int)segment].Info.GetConnectionClass();
                    if (connectionClass1.m_service == connectionClass2.m_service)
                    {
                        NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segment];
                        Vector3 vector3_2 = (int)nodeID != (int)netSegment2.m_startNode ? netSegment2.m_endDirection : netSegment2.m_startDirection;
                        float num3 = (float)((double)vector3_1.x * (double)vector3_2.x + (double)vector3_1.z * (double)vector3_2.z);
                        if ((double)vector3_2.z * (double)vector3_1.x - (double)vector3_2.x * (double)vector3_1.z < 0.0)
                        {
                            if ((double)num3 > (double)num1)
                            {
                                num1 = num3;
                                segmentID1 = segment;
                            }
                            float num4 = -2f - num3;
                            if ((double)num4 > (double)num2)
                            {
                                num2 = num4;
                                segmentID2 = segment;
                            }
                        }
                        else
                        {
                            if ((double)num3 > (double)num2)
                            {
                                num2 = num3;
                                segmentID2 = segment;
                            }
                            float num4 = -2f - num3;
                            if ((double)num4 > (double)num1)
                            {
                                num1 = num4;
                                segmentID1 = segment;
                            }
                        }
                    }
                }
            }
            bool start1 = (int)netSegment1.m_startNode == (int)nodeID;
            bool smooth;
            netSegment1.CalculateCorner(nodeSegment, true, start1, false, out cornerPos1, out cornerDirection1, out smooth);
            netSegment1.CalculateCorner(nodeSegment, true, start1, true, out cornerPos2, out cornerDirection2, out smooth);
            if ((int)segmentID1 != 0 && (int)segmentID2 != 0)
            {
                float x = (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5);
                float y = 1f;
                if ((int)segmentID1 != 0)
                {
                    NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segmentID1];
                    NetInfo info2 = netSegment2.Info;
                    bool start2 = (int)netSegment2.m_startNode == (int)nodeID;
                    netSegment2.CalculateCorner(segmentID1, true, start2, true, out cornerPos3, out cornerDirection3, out smooth);
                    netSegment2.CalculateCorner(segmentID1, true, start2, false, out cornerPos4, out cornerDirection4, out smooth);
                    float num3 = (float)((double)info2.m_pavementWidth / (double)info2.m_halfWidth * 0.5);
                    x = (float)(((double)x + (double)num3) * 0.5);
                    y = (float)(2.0 * (double)info1.m_halfWidth / ((double)info1.m_halfWidth + (double)info2.m_halfWidth));
                }
                float z = (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5);
                float w = 1f;
                if ((int)segmentID2 != 0)
                {
                    NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segmentID2];
                    NetInfo info2 = netSegment2.Info;
                    bool start2 = (int)netSegment2.m_startNode == (int)nodeID;
                    netSegment2.CalculateCorner(segmentID2, true, start2, true, out cornerPos5, out cornerDirection5, out smooth);
                    netSegment2.CalculateCorner(segmentID2, true, start2, false, out cornerPos6, out cornerDirection6, out smooth);
                    float num3 = (float)((double)info2.m_pavementWidth / (double)info2.m_halfWidth * 0.5);
                    z = (float)(((double)z + (double)num3) * 0.5);
                    w = (float)(2.0 * (double)info1.m_halfWidth / ((double)info1.m_halfWidth + (double)info2.m_halfWidth));
                }
                Vector3 middlePos1_1;
                Vector3 middlePos2_1;
                NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos3, -cornerDirection3, true, true, out middlePos1_1, out middlePos2_1);
                Vector3 middlePos1_2;
                Vector3 middlePos2_2;
                NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos4, -cornerDirection4, true, true, out middlePos1_2, out middlePos2_2);
                Vector3 middlePos1_3;
                Vector3 middlePos2_3;
                NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos5, -cornerDirection5, true, true, out middlePos1_3, out middlePos2_3);
                Vector3 middlePos1_4;
                Vector3 middlePos2_4;
                NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos6, -cornerDirection6, true, true, out middlePos1_4, out middlePos2_4);
                data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_3, middlePos2_3, cornerPos5, cornerPos1, middlePos1_3, middlePos2_3, cornerPos5, netnode.m_position, vScale);
                data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_4, middlePos2_4, cornerPos6, cornerPos2, middlePos1_4, middlePos2_4, cornerPos6, netnode.m_position, vScale);
                data.m_dataVector0 = new Vector4(0.5f / info1.m_halfWidth, 1f / info1.m_segmentLength, (float)(0.5 - (double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5));
                data.m_dataVector1 = (Vector4)(centerPos - data.m_position);
                data.m_dataVector1.w = (float)(((double)data.m_dataMatrix0.m33 + (double)data.m_extraData.m_dataMatrix2.m33 + (double)data.m_extraData.m_dataMatrix3.m33 + (double)data.m_dataMatrix1.m33) * 0.25);
                data.m_dataVector2 = new Vector4(x, y, z, w);
                data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(65536U + (uint)nodeID);
            }
            else
            {
                centerPos.x = (float)(((double)cornerPos1.x + (double)cornerPos2.x) * 0.5);
                centerPos.z = (float)(((double)cornerPos1.z + (double)cornerPos2.z) * 0.5);
                Vector3 vector3_2 = cornerPos2;
                Vector3 vector3_3 = cornerPos1;
                Vector3 vector3_4 = cornerDirection2;
                Vector3 vector3_5 = cornerDirection1;
                float num3 = Mathf.Min(info1.m_halfWidth * 1.333333f, 16f);
                Vector3 vector3_6 = cornerPos1 - cornerDirection1 * num3;
                Vector3 vector3_7 = vector3_2 - vector3_4 * num3;
                Vector3 vector3_8 = cornerPos2 - cornerDirection2 * num3;
                Vector3 vector3_9 = vector3_3 - vector3_5 * num3;
                Vector3 vector3_10 = cornerPos1 + cornerDirection1 * num3;
                Vector3 vector3_11 = vector3_2 + vector3_4 * num3;
                Vector3 vector3_12 = cornerPos2 + cornerDirection2 * num3;
                Vector3 vector3_13 = vector3_3 + vector3_5 * num3;
                data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, vector3_6, vector3_7, vector3_2, cornerPos1, vector3_6, vector3_7, vector3_2, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, vector3_12, vector3_13, vector3_3, cornerPos2, vector3_12, vector3_13, vector3_3, netnode.m_position, vScale);
                data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(cornerPos1, vector3_10, vector3_11, vector3_2, cornerPos1, vector3_10, vector3_11, vector3_2, netnode.m_position, vScale);
                data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos2, vector3_8, vector3_9, vector3_3, cornerPos2, vector3_8, vector3_9, vector3_3, netnode.m_position, vScale);
                data.m_dataMatrix0.SetRow(3, data.m_dataMatrix0.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_extraData.m_dataMatrix2.SetRow(3, data.m_extraData.m_dataMatrix2.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_extraData.m_dataMatrix3.SetRow(3, data.m_extraData.m_dataMatrix3.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_dataMatrix1.SetRow(3, data.m_dataMatrix1.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
                data.m_dataVector0 = new Vector4(0.5f / info1.m_halfWidth, 1f / info1.m_segmentLength, (float)(0.5 - (double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5));
                data.m_dataVector1 = (Vector4)(centerPos - data.m_position);
                data.m_dataVector1.w = (float)(((double)data.m_dataMatrix0.m33 + (double)data.m_extraData.m_dataMatrix2.m33 + (double)data.m_extraData.m_dataMatrix3.m33 + (double)data.m_dataMatrix1.m33) * 0.25);
                data.m_dataVector2 = new Vector4((float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), 1f, (float)((double)info1.m_pavementWidth / (double)info1.m_halfWidth * 0.5), 1f);
                data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(65536U + (uint)nodeID);
            }
            data.m_dataInt0 = segmentIndex;
            data.m_dataColor0 = info1.m_color;
            data.m_dataColor0.a = 0.0f;
            if (info1.m_requireSurfaceMaps)
                Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector3);
            instanceIndex = (uint)data.m_nextInstance;
        }

        private void RefreshJunctionData(NetNode netnode, ushort nodeID, int segmentIndex, NetInfo info, ushort nodeSegment, ushort nodeSegment2, ref uint instanceIndex, ref RenderManager.Instance data)
        {
            data.m_position = netnode.m_position;
            data.m_rotation = Quaternion.identity;
            data.m_initialized = true;
            float vScale = 0.05f;
            Vector3 cornerPos1 = Vector3.zero;
            Vector3 cornerPos2 = Vector3.zero;
            Vector3 cornerPos3 = Vector3.zero;
            Vector3 cornerPos4 = Vector3.zero;
            Vector3 cornerDirection1 = Vector3.zero;
            Vector3 cornerDirection2 = Vector3.zero;
            Vector3 cornerDirection3 = Vector3.zero;
            Vector3 cornerDirection4 = Vector3.zero;
            bool start1 = (int)Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].m_startNode == (int)nodeID;
            bool smooth;
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].CalculateCorner(nodeSegment, true, start1, false, out cornerPos1, out cornerDirection1, out smooth);
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].CalculateCorner(nodeSegment, true, start1, true, out cornerPos2, out cornerDirection2, out smooth);
            bool start2 = (int)Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].m_startNode == (int)nodeID;
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].CalculateCorner(nodeSegment2, true, start2, true, out cornerPos3, out cornerDirection3, out smooth);
            Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].CalculateCorner(nodeSegment2, true, start2, false, out cornerPos4, out cornerDirection4, out smooth);
            Vector3 middlePos1_1;
            Vector3 middlePos2_1;
            NetSegment.CalculateMiddlePoints(cornerPos1, -cornerDirection1, cornerPos3, -cornerDirection3, true, true, out middlePos1_1, out middlePos2_1);
            Vector3 middlePos1_2;
            Vector3 middlePos2_2;
            NetSegment.CalculateMiddlePoints(cornerPos2, -cornerDirection2, cornerPos4, -cornerDirection4, true, true, out middlePos1_2, out middlePos2_2);
            data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, netnode.m_position, vScale);
            data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos1_2, middlePos2_2, cornerPos4, cornerPos1, middlePos1_1, middlePos2_1, cornerPos3, netnode.m_position, vScale);
            data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
            data.m_dataVector3 = RenderManager.GetColorLocation(65536U + (uint)nodeID);
            data.m_dataInt0 = 8 | segmentIndex;
            data.m_dataColor0 = info.m_color;
            data.m_dataColor0.a = 0.0f;
            if (info.m_requireSurfaceMaps)
                Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);
            instanceIndex = (uint)data.m_nextInstance;
        }

        private int CalculateRendererCount(NetNode netnode, NetInfo info)
        {
            if ((netnode.m_flags & NetNode.Flags.Junction) == NetNode.Flags.None)
                return 1;
            int num = 0;
            if (info.m_requireSegmentRenderers)
                num += netnode.CountSegments();
            if (info.m_requireDirectRenderers)
                num += (int)netnode.m_connectCount;
            return num;
        }

        public void RenderInstanceNode(RenderManager.CameraInfo cameraInfo, ushort nodeID, int layerMask)
        {
            NetManager nm = Singleton<NetManager>.instance;
            NetNode nn = nm.m_nodes.m_buffer[(int)nodeID];

            if (nn.m_flags == NetNode.Flags.None)
                return;
            NetInfo info = nn.Info;
            if (!cameraInfo.Intersect(nn.m_bounds))
                return;
            if (nn.m_problems != Notification.Problem.None && (layerMask & 1 << Singleton<NotificationManager>.instance.m_notificationLayer) != 0)
            {
                Vector3 position = nn.m_position;
                position.y += Mathf.Max(5f, info.m_maxHeight);
                Notification.RenderInstance(cameraInfo, nn.m_problems, position, 1f);
            }
            if ((layerMask & info.m_netLayers) == 0 || (nn.m_flags & (NetNode.Flags.End | NetNode.Flags.Bend | NetNode.Flags.Junction)) == NetNode.Flags.None)
                return;
            if ((nn.m_flags & NetNode.Flags.Bend) != NetNode.Flags.None)
            {
                if (info.m_segments == null || info.m_segments.Length == 0)
                    return;
            }
            else if (info.m_nodes == null || info.m_nodes.Length == 0)
                return;
            uint count = (uint)CalculateRendererCount(nn, info);
            RenderManager instance = Singleton<RenderManager>.instance;
            uint instanceIndex;
            if (!instance.RequireInstance(65536U + (uint)nodeID, count, out instanceIndex))
                return;
            int iter = 0;
            while ((int)instanceIndex != (int)ushort.MaxValue)
            {
                RenderInstanceNode(cameraInfo, nodeID, info, iter, nn.m_flags, ref instanceIndex, ref instance.m_instances[instanceIndex]);
                if (++iter > 36)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + System.Environment.StackTrace);
                    break;
                }
            }
        }

        public void RenderInstanceNode(RenderManager.CameraInfo cameraInfo, ushort nodeID, NetInfo info, int iter, NetNode.Flags flags, ref uint instanceIndex, ref RenderManager.Instance data)
        {
            //Debug.Log ("Rendering node " + nodeID);
            NetManager nm = Singleton<NetManager>.instance;
            NetNode nn = nm.m_nodes.m_buffer[(int)nodeID];

            if (data.m_dirty)
            {
                data.m_dirty = false;
                if (iter == 0)
                {
                    if ((flags & NetNode.Flags.Junction) != NetNode.Flags.None)
                        RefreshJunctionData(nn, nodeID, info, instanceIndex);
                    else if ((flags & NetNode.Flags.Bend) != NetNode.Flags.None)
                        RefreshBendData(nn, nodeID, info, instanceIndex, ref data);
                    else if ((flags & NetNode.Flags.End) != NetNode.Flags.None)
                        RefreshEndData(nn, nodeID, info, instanceIndex, ref data);
                }
            }
            //if (info.m_class.name.Contains ("Road") || info.m_class.name.Contains ("Highway") || info.m_class.name.Contains ("Bridge") || info.m_class.name.Contains ("Elevated") || info.m_class.name.Contains ("Dam")) {
            //	data.m_dataColor0 = Color.Lerp (data.m_dataColor0, new Color (0.5f, 0.5f, 0.5f), (Time.deltaTime/300f) * Singleton<SimulationManager>.instance.m_simulationTimeSpeed);
            //}
            if (data.m_initialized)
            {
                if ((flags & NetNode.Flags.Junction) != NetNode.Flags.None)
                {
                    if ((data.m_dataInt0 & 8) != 0)
                    {
                        ushort segment = nn.GetSegment(data.m_dataInt0 & 7);
                        if ((int)segment != 0)
                        {
                            NetManager instance = Singleton<NetManager>.instance;
                            info = instance.m_segments.m_buffer[(int)segment].Info;
                            for (int index = 0; index < info.m_nodes.Length; ++index)
                            {
                                NetInfo.Node nodeData = info.m_nodes[index];
                                if (nodeData.CheckFlags(flags) && nodeData.m_directConnect)
                                {
                                    //if (cameraInfo.CheckRenderDistance(data.m_position, nodeData.m_lodRenderDistance))
                                    //{
                                    instance.m_materialBlock.Clear();
                                    instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                                    instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                                    instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                                    instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
                                    instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                                    if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                                    {
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                        instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
                                    }
                                    ++instance.m_drawCallData.m_defaultCalls;
                                    Graphics.DrawMesh(nodeData.m_nodeMesh, data.m_position, data.m_rotation, nodeData.m_nodeMaterial, nodeData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                                    //}
                                    /*else
									{
										if (info.m_requireSurfaceMaps && (UnityEngine.Object) data.m_dataTexture0 != (UnityEngine.Object) nodeData.m_surfaceTexA)
										{
											if (nodeData.m_combinedCount != 0)
												NetNode.RenderLod(cameraInfo, info, nodeData);
											nodeData.m_surfaceTexA = data.m_dataTexture0;
											nodeData.m_surfaceTexB = data.m_dataTexture1;
											nodeData.m_surfaceMapping = data.m_dataVector1;
										}
										nodeData.m_leftMatrices[nodeData.m_combinedCount] = data.m_dataMatrix0;
										nodeData.m_rightMatrices[nodeData.m_combinedCount] = data.m_extraData.m_dataMatrix2;
										nodeData.m_meshScales[nodeData.m_combinedCount] = data.m_dataVector0;
										nodeData.m_objectIndices[nodeData.m_combinedCount] = data.m_dataVector3;
										nodeData.m_meshLocations[nodeData.m_combinedCount] = (Vector4) data.m_position;
										nodeData.m_lodMin = Vector3.Min(nodeData.m_lodMin, data.m_position);
										nodeData.m_lodMax = Vector3.Max(nodeData.m_lodMax, data.m_position);
										if (++nodeData.m_combinedCount == nodeData.m_leftMatrices.Length)
											NetNode.RenderLod(cameraInfo, info, nodeData);
									}*/
                                }
                            }
                        }
                    }
                    else
                    {
                        ushort segment = nn.GetSegment(data.m_dataInt0 & 7);
                        if ((int)segment != 0)
                        {
                            NetManager instance = Singleton<NetManager>.instance;
                            info = instance.m_segments.m_buffer[(int)segment].Info;
                            for (int index = 0; index < info.m_nodes.Length; ++index)
                            {
                                NetInfo.Node nodeData = info.m_nodes[index];
                                if (nodeData.CheckFlags(flags) && !nodeData.m_directConnect)
                                {
                                    //if (cameraInfo.CheckRenderDistance(data.m_position, nodeData.m_lodRenderDistance))
                                    //{
                                    instance.m_materialBlock.Clear();
                                    instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                                    instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                                    instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrixB, data.m_extraData.m_dataMatrix3);
                                    instance.m_materialBlock.AddMatrix(instance.ID_RightMatrixB, data.m_dataMatrix1);
                                    instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                                    instance.m_materialBlock.AddVector(instance.ID_CenterPos, data.m_dataVector1);
                                    instance.m_materialBlock.AddVector(instance.ID_SideScale, data.m_dataVector2);
                                    instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_extraData.m_dataVector4);
                                    instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                                    if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                                    {
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                        instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                        instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector3);
                                    }
                                    ++instance.m_drawCallData.m_defaultCalls;
                                    Graphics.DrawMesh(nodeData.m_nodeMesh, data.m_position, data.m_rotation, nodeData.m_nodeMaterial, nodeData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                                    //}
                                    /*else
									{
										if (info.m_requireSurfaceMaps && (UnityEngine.Object) data.m_dataTexture0 != (UnityEngine.Object) nodeData.m_surfaceTexA)
										{
											if (nodeData.m_combinedCount != 0)
												NetNode.RenderLod(cameraInfo, info, nodeData);
											nodeData.m_surfaceTexA = data.m_dataTexture0;
											nodeData.m_surfaceTexB = data.m_dataTexture1;
											nodeData.m_surfaceMapping = data.m_dataVector3;
										}
										nodeData.m_leftMatrices[nodeData.m_combinedCount] = data.m_dataMatrix0;
										nodeData.m_leftMatricesB[nodeData.m_combinedCount] = data.m_extraData.m_dataMatrix3;
										nodeData.m_rightMatrices[nodeData.m_combinedCount] = data.m_extraData.m_dataMatrix2;
										nodeData.m_rightMatricesB[nodeData.m_combinedCount] = data.m_dataMatrix1;
										nodeData.m_meshScales[nodeData.m_combinedCount] = data.m_dataVector0;
										nodeData.m_centerPositions[nodeData.m_combinedCount] = data.m_dataVector1;
										nodeData.m_sideScales[nodeData.m_combinedCount] = data.m_dataVector2;
										nodeData.m_objectIndices[nodeData.m_combinedCount] = data.m_extraData.m_dataVector4;
										nodeData.m_meshLocations[nodeData.m_combinedCount] = (Vector4) data.m_position;
										nodeData.m_lodMin = Vector3.Min(nodeData.m_lodMin, data.m_position);
										nodeData.m_lodMax = Vector3.Max(nodeData.m_lodMax, data.m_position);
										if (++nodeData.m_combinedCount == nodeData.m_leftMatrices.Length)
											NetNode.RenderLod(cameraInfo, info, nodeData);
									}*/
                                }
                            }
                        }
                    }
                }
                else if ((flags & NetNode.Flags.End) != NetNode.Flags.None)
                {
                    NetManager instance = Singleton<NetManager>.instance;
                    for (int index = 0; index < info.m_nodes.Length; ++index)
                    {
                        NetInfo.Node nodeData = info.m_nodes[index];
                        if (nodeData.CheckFlags(flags) && !nodeData.m_directConnect)
                        {
                            //if (cameraInfo.CheckRenderDistance(data.m_position, nodeData.m_lodRenderDistance))
                            //{
                            instance.m_materialBlock.Clear();
                            instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                            instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                            instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrixB, data.m_extraData.m_dataMatrix3);
                            instance.m_materialBlock.AddMatrix(instance.ID_RightMatrixB, data.m_dataMatrix1);
                            instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                            instance.m_materialBlock.AddVector(instance.ID_CenterPos, data.m_dataVector1);
                            instance.m_materialBlock.AddVector(instance.ID_SideScale, data.m_dataVector2);
                            instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_extraData.m_dataVector4);
                            instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                            if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                            {
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector3);
                            }
                            ++instance.m_drawCallData.m_defaultCalls;
                            Graphics.DrawMesh(nodeData.m_nodeMesh, data.m_position, data.m_rotation, nodeData.m_nodeMaterial, nodeData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                            //}
                            /*else
							{
								if (info.m_requireSurfaceMaps && (UnityEngine.Object) data.m_dataTexture0 != (UnityEngine.Object) nodeData.m_surfaceTexA)
								{
									if (nodeData.m_combinedCount != 0)
										NetNode.RenderLod(cameraInfo, info, nodeData);
									nodeData.m_surfaceTexA = data.m_dataTexture0;
									nodeData.m_surfaceTexB = data.m_dataTexture1;
									nodeData.m_surfaceMapping = data.m_dataVector3;
								}
								nodeData.m_leftMatrices[nodeData.m_combinedCount] = data.m_dataMatrix0;
								nodeData.m_leftMatricesB[nodeData.m_combinedCount] = data.m_extraData.m_dataMatrix3;
								nodeData.m_rightMatrices[nodeData.m_combinedCount] = data.m_extraData.m_dataMatrix2;
								nodeData.m_rightMatricesB[nodeData.m_combinedCount] = data.m_dataMatrix1;
								nodeData.m_meshScales[nodeData.m_combinedCount] = data.m_dataVector0;
								nodeData.m_centerPositions[nodeData.m_combinedCount] = data.m_dataVector1;
								nodeData.m_sideScales[nodeData.m_combinedCount] = data.m_dataVector2;
								nodeData.m_objectIndices[nodeData.m_combinedCount] = data.m_extraData.m_dataVector4;
								nodeData.m_meshLocations[nodeData.m_combinedCount] = (Vector4) data.m_position;
								nodeData.m_lodMin = Vector3.Min(nodeData.m_lodMin, data.m_position);
								nodeData.m_lodMax = Vector3.Max(nodeData.m_lodMax, data.m_position);
								if (++nodeData.m_combinedCount == nodeData.m_leftMatrices.Length)
									NetNode.RenderLod(cameraInfo, info, nodeData);
							}*/
                        }
                    }
                }
                else if ((flags & NetNode.Flags.Bend) != NetNode.Flags.None)
                {
                    NetManager instance = Singleton<NetManager>.instance;
                    for (int index = 0; index < info.m_segments.Length; ++index)
                    {
                        NetInfo.Segment segmentData = info.m_segments[index];
                        bool turnAround;
                        if (segmentData.CheckFlags(NetSegment.Flags.None, out turnAround))
                        {
                            //if (cameraInfo.CheckRenderDistance(data.m_position, segmentData.m_lodRenderDistance))
                            //{
                            instance.m_materialBlock.Clear();
                            instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                            instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
                            instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                            instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
                            instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
                            if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture1 != (UnityEngine.Object)null)
                            {
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                                instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                                instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
                            }
                            ++instance.m_drawCallData.m_defaultCalls;
                            Graphics.DrawMesh(segmentData.m_segmentMesh, data.m_position, data.m_rotation, segmentData.m_segmentMaterial, segmentData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                            //}
                            /*else
							{
								if (info.m_requireSurfaceMaps && (UnityEngine.Object) data.m_dataTexture0 != (UnityEngine.Object) segmentData.m_surfaceTexA)
								{
									if (segmentData.m_combinedCount != 0)
										NetSegment.RenderLod(cameraInfo, info, segmentData);
									segmentData.m_surfaceTexA = data.m_dataTexture0;
									segmentData.m_surfaceTexB = data.m_dataTexture1;
									segmentData.m_surfaceMapping = data.m_dataVector1;
								}
								segmentData.m_leftMatrices[segmentData.m_combinedCount] = data.m_dataMatrix0;
								segmentData.m_rightMatrices[segmentData.m_combinedCount] = data.m_extraData.m_dataMatrix2;
								segmentData.m_meshScales[segmentData.m_combinedCount] = data.m_dataVector0;
								segmentData.m_objectIndices[segmentData.m_combinedCount] = data.m_dataVector3;
								segmentData.m_meshLocations[segmentData.m_combinedCount] = (Vector4) data.m_position;
								segmentData.m_lodMin = Vector3.Min(segmentData.m_lodMin, data.m_position);
								segmentData.m_lodMax = Vector3.Max(segmentData.m_lodMax, data.m_position);
								if (++segmentData.m_combinedCount == segmentData.m_leftMatrices.Length)
									NetSegment.RenderLod(cameraInfo, info, segmentData);
							}*/
                        }
                    }
                }
            }
            instanceIndex = (uint)data.m_nextInstance;
        }

        private void RefreshEndData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex, ref RenderManager.Instance data)
        {
            data.m_position = netnode.m_position;
            data.m_rotation = Quaternion.identity;
            data.m_initialized = true;
            float vScale = 0.05f;
            Vector3 zero = Vector3.zero;
            Vector3 zero2 = Vector3.zero;
            Vector3 vector = Vector3.zero;
            Vector3 vector2 = Vector3.zero;
            Vector3 zero3 = Vector3.zero;
            Vector3 zero4 = Vector3.zero;
            Vector3 a = Vector3.zero;
            Vector3 a2 = Vector3.zero;
            for (int i = 0; i < 8; i++)
            {
                ushort segment = netnode.GetSegment(i);
                if (segment != 0)
                {
                    NetSegment netSegment = Singleton<NetManager>.instance.m_segments.m_buffer[(int)segment];
                    bool start = netSegment.m_startNode == nodeID;
                    bool flag;
                    netSegment.CalculateCorner(segment, true, start, false, out zero, out zero3, out flag);
                    netSegment.CalculateCorner(segment, true, start, true, out zero2, out zero4, out flag);
                    vector = zero2;
                    vector2 = zero;
                    a = zero4;
                    a2 = zero3;
                }
            }
            float d = Mathf.Min(info.m_halfWidth * 1.33333337f, 16f);
            Vector3 vector3 = zero - zero3 * d;
            Vector3 vector4 = vector - a * d;
            Vector3 vector5 = zero2 - zero4 * d;
            Vector3 vector6 = vector2 - a2 * d;
            Vector3 vector7 = zero + zero3 * d;
            Vector3 vector8 = vector + a * d;
            Vector3 vector9 = zero2 + zero4 * d;
            Vector3 vector10 = vector2 + a2 * d;
            data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(zero, vector3, vector4, vector, zero, vector3, vector4, vector, netnode.m_position, vScale);
            data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(zero2, vector9, vector10, vector2, zero2, vector9, vector10, vector2, netnode.m_position, vScale);
            data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(zero, vector7, vector8, vector, zero, vector7, vector8, vector, netnode.m_position, vScale);
            data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(zero2, vector5, vector6, vector2, zero2, vector5, vector6, vector2, netnode.m_position, vScale);
            data.m_dataMatrix0.SetRow(3, data.m_dataMatrix0.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_extraData.m_dataMatrix2.SetRow(3, data.m_extraData.m_dataMatrix2.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_extraData.m_dataMatrix3.SetRow(3, data.m_extraData.m_dataMatrix3.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_dataMatrix1.SetRow(3, data.m_dataMatrix1.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 0.5f - info.m_pavementWidth / info.m_halfWidth * 0.5f, info.m_pavementWidth / info.m_halfWidth * 0.5f);
            data.m_dataVector1 = new Vector4(0f, (float)netnode.m_heightOffset * 0.015625f, 0f, 0f);
            data.m_dataVector1.w = (data.m_dataMatrix0.m33 + data.m_extraData.m_dataMatrix2.m33 + data.m_extraData.m_dataMatrix3.m33 + data.m_dataMatrix1.m33) * 0.25f;
            data.m_dataVector2 = new Vector4(info.m_pavementWidth / info.m_halfWidth * 0.5f, 1f, info.m_pavementWidth / info.m_halfWidth * 0.5f, 1f);
            data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(65536u + (uint)nodeID);
            data.m_dataColor0 = info.m_color;
            data.m_dataColor0.a = 0f;
            if (info.m_requireSurfaceMaps)
            {
                Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector3);
            }
        }



        public void RenderInstanceSegment(RenderManager.CameraInfo cameraInfo, ushort segmentID, int layerMask)
        {
            NetManager nm = Singleton<NetManager>.instance;
            NetSegment ns = nm.m_segments.m_buffer[(int)segmentID];

            if (ns.m_flags == NetSegment.Flags.None)
                return;
            NetInfo info = ns.Info;
            if ((layerMask & info.m_netLayers) == 0 || !cameraInfo.Intersect(ns.m_bounds))
                return;
            RenderManager instance = Singleton<RenderManager>.instance;
            uint instanceIndex;
            if (!instance.RequireInstance(32768U + (uint)segmentID, 1U, out instanceIndex))
                return;

            RenderInstanceSegmentNew(cameraInfo, segmentID, layerMask, info, ref instance.m_instances[instanceIndex]);
        }

        private void RenderInstanceSegmentNew(RenderManager.CameraInfo cameraInfo, ushort segmentID, int layerMask, NetInfo info, ref RenderManager.Instance data)
        {
            NetManager instance = Singleton<NetManager>.instance;
            //NetSegment instance.m_segments.m_buffer[(int) segmentID] = instance.m_segments.m_buffer[(int) segmentID];

            bool invert2 = false;


            if (((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None) && info.name.Contains("Highway") && !info.name.ToLower().Contains("tunnel"))
            {
                invert2 = true;

                var temp = instance.m_segments.m_buffer[(int)segmentID].m_endNode;
                instance.m_segments.m_buffer[(int)segmentID].m_endNode = instance.m_segments.m_buffer[(int)segmentID].m_startNode;
                instance.m_segments.m_buffer[(int)segmentID].m_startNode = temp;
                instance.m_segments.m_buffer[(int)segmentID].m_flags = instance.m_segments.m_buffer[(int)segmentID].m_flags & ~NetSegment.Flags.Invert;

                var tempDir = instance.m_segments.m_buffer[(int)segmentID].m_endDirection;
                instance.m_segments.m_buffer[(int)segmentID].m_endDirection = instance.m_segments.m_buffer[(int)segmentID].m_startDirection;
                instance.m_segments.m_buffer[(int)segmentID].m_startDirection = tempDir;
                //}
            }

            /*if (segmentColors [(int)segmentID] == null) {
				segmentColors [(int)segmentID] = info.m_color;
			}*/

            if (data.m_dirty)
            {
                data.m_dirty = false;



                Vector3 vector3_1 = instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].m_position;
                Vector3 vector3_2 = instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].m_position;

                data.m_position = (vector3_1 + vector3_2) * 0.5f;
                data.m_rotation = Quaternion.identity;
                data.m_dataColor0 = info.m_color;

                data.m_dataColor0.a = 0.0f;
                data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
                data.m_dataVector3 = RenderManager.GetColorLocation(32768U + (uint)segmentID);
                data.m_dataVector3.w = Singleton<WindManager>.instance.GetWindSpeed(data.m_position);

                if (info.m_segments == null || info.m_segments.Length == 0)
                {
                    if (info.m_lanes != null)
                    {
                        bool invert;
                        NetNode.Flags flags1;
                        Color color1;
                        NetNode.Flags flags2;
                        Color color2;

                        if ((invert2 && !info.name.Contains("Highway")) || (!invert2 && info.name.Contains("Highway")))
                        {
                            invert = true;
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags1, out color1);
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);

                        }
                        else
                        {
                            invert = false;
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags1, out color1);
                            instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);
                        }
                        int propIndex = 0;
                        uint laneID = instance.m_segments.m_buffer[(int)segmentID].m_lanes;
                        for (int index = 0; index < info.m_lanes.Length && (int)laneID != 0; ++index)
                        {
                            instance.m_lanes.m_buffer[laneID].RefreshInstance(laneID, info.m_lanes[index], invert, ref data, ref propIndex);
                            laneID = instance.m_lanes.m_buffer[laneID].m_nextLane;
                        }
                    }
                }
                else
                {
                    bool turnAround = false;
                    int index = 0;
                    while (index < info.m_segments.Length && !info.m_segments[index].CheckFlags(instance.m_segments.m_buffer[(int)segmentID].m_flags, out turnAround))
                        ++index;
                    float vScale = 0.05f;
                    Vector3 cornerPos1;
                    Vector3 cornerDirection1;
                    bool smooth1;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, true, true, out cornerPos1, out cornerDirection1, out smooth1);
                    Vector3 cornerPos2;
                    Vector3 cornerDirection2;
                    bool smooth2;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, false, true, out cornerPos2, out cornerDirection2, out smooth2);
                    Vector3 cornerPos3;
                    Vector3 cornerDirection3;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, true, false, out cornerPos3, out cornerDirection3, out smooth1);
                    Vector3 cornerPos4;
                    Vector3 cornerDirection4;
                    instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, false, false, out cornerPos4, out cornerDirection4, out smooth2);
                    Vector3 middlePos1_1;
                    Vector3 middlePos2_1;
                    NetSegment.CalculateMiddlePoints(cornerPos1, cornerDirection1, cornerPos4, cornerDirection4, smooth1, smooth2, out middlePos1_1, out middlePos2_1);
                    Vector3 middlePos1_2;
                    Vector3 middlePos2_2;
                    NetSegment.CalculateMiddlePoints(cornerPos3, cornerDirection3, cornerPos2, cornerDirection2, smooth1, smooth2, out middlePos1_2, out middlePos2_2);
                    if (turnAround)
                    {
                        data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos2, middlePos2_2, middlePos1_2, cornerPos3, cornerPos4, middlePos2_1, middlePos1_1, cornerPos1, data.m_position, vScale);
                        data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos4, middlePos2_1, middlePos1_1, cornerPos1, cornerPos2, middlePos2_2, middlePos1_2, cornerPos3, data.m_position, vScale);
                    }
                    else
                    {
                        data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(cornerPos1, middlePos1_1, middlePos2_1, cornerPos4, cornerPos3, middlePos1_2, middlePos2_2, cornerPos2, data.m_position, vScale);
                        data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(cornerPos3, middlePos1_2, middlePos2_2, cornerPos2, cornerPos1, middlePos1_1, middlePos2_1, cornerPos4, data.m_position, vScale);
                    }
                }
                if (info.m_requireSurfaceMaps)
                    Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);
            }
            //data.m_dataColor0 = segmentColors[(int)segmentID];

            //if (info.m_class.name.Contains ("Road") || info.m_class.name.Contains ("Highway") || info.m_class.name.Contains ("Bridge") || info.m_class.name.Contains ("Elevated") || info.m_class.name.Contains ("Dam")) {
            //	data.m_dataColor0 = Color.Lerp (data.m_dataColor0, new Color (0.5f, 0.5f, 0.5f),(Time.deltaTime/300f) * Singleton<SimulationManager>.instance.m_simulationTimeSpeed);
            //}
            if (info.m_segments != null)
            {
                for (int index = 0; index < info.m_segments.Length; ++index)
                {
                    NetInfo.Segment segmentData = info.m_segments[index];
                    bool turnAround;

                    if (segmentData.CheckFlags(instance.m_segments.m_buffer[(int)segmentID].m_flags, out turnAround))
                    {
                        //if (cameraInfo.CheckRenderDistance(data.m_position, segmentData.m_lodRenderDistance))
                        //{
                        instance.m_materialBlock.Clear();
                        instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
                        instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_dataMatrix1);


                        instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
                        instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
                        instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0 /*segmentColors [(int)segmentID]*/);
                        if (info.m_requireSurfaceMaps && (UnityEngine.Object)data.m_dataTexture0 != (UnityEngine.Object)null)
                        {
                            instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
                            instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
                            instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
                        }
                        ++instance.m_drawCallData.m_defaultCalls;

                        if ((info.name.Contains("Bridge") && index == 1) && invert2)
                        {
                            Graphics.DrawMesh(segmentData.m_segmentMesh, data.m_position, Quaternion.Euler(0, 180, 0), segmentData.m_segmentMaterial, segmentData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                        }
                        else {
                            Graphics.DrawMesh(segmentData.m_segmentMesh, data.m_position, data.m_rotation, segmentData.m_segmentMaterial, segmentData.m_layer, (Camera)null, 0, instance.m_materialBlock);
                        }


                        /*else
                        {
                            if (info.m_requireSurfaceMaps && (UnityEngine.Object) data.m_dataTexture0 != (UnityEngine.Object) segmentData.m_surfaceTexA)
                            {
                                if (segmentData.m_combinedCount != 0)
                                    NetSegment.RenderLod(cameraInfo, info, segmentData);
                                segmentData.m_surfaceTexA = data.m_dataTexture0;
                                segmentData.m_surfaceTexB = data.m_dataTexture1;
                                segmentData.m_surfaceMapping = data.m_dataVector1;
                            }
                            segmentData.m_leftMatrices[segmentData.m_combinedCount] = data.m_dataMatrix0;
                            segmentData.m_rightMatrices[segmentData.m_combinedCount] = data.m_dataMatrix1;
                            segmentData.m_meshScales[segmentData.m_combinedCount] = data.m_dataVector0;
                            segmentData.m_objectIndices[segmentData.m_combinedCount] = data.m_dataVector3;
                            segmentData.m_meshLocations[segmentData.m_combinedCount] = (Vector4) data.m_position;
                            segmentData.m_lodMin = Vector3.Min(segmentData.m_lodMin, data.m_position);
                            segmentData.m_lodMax = Vector3.Max(segmentData.m_lodMax, data.m_position);
                            if (++segmentData.m_combinedCount == segmentData.m_leftMatrices.Length)
                                NetSegment.RenderLod(cameraInfo, info, segmentData);
                        }*/
                    }
                    //} /*else {
                    //Debug.Log ("About to render highway top, invert2: " + invert2);
                    //if(invert2 == true){
                    //	invert2 = false;
                    //if (!info.name.Equals ("Highway Bridge")) {
                    /*	var temp = instance.m_segments.m_buffer [(int)segmentID].m_endNode;
                        instance.m_segments.m_buffer [(int)segmentID].m_endNode = instance.m_segments.m_buffer [(int)segmentID].m_startNode;
                        instance.m_segments.m_buffer [(int)segmentID].m_startNode = temp;
                        instance.m_segments.m_buffer [(int)segmentID].m_flags |= NetSegment.Flags.Invert;
                        var tempDir = instance.m_segments.m_buffer [(int)segmentID].m_endDirection;
                        instance.m_segments.m_buffer [(int)segmentID].m_endDirection = instance.m_segments.m_buffer [(int)segmentID].m_startDirection;
                        instance.m_segments.m_buffer [(int)segmentID].m_startDirection = tempDir;
                    //}
                //}
                data.m_dirty = true;
                RenderInstanceSegmentNewHighwayBridgeTop (cameraInfo, segmentID, layerMask, info, ref data);*/

                    //}
                }
            }

            //bool invert3 = false;

            if (invert2 == true)
            {
                invert2 = false;
                //invert3 = true;
                var temp = instance.m_segments.m_buffer[(int)segmentID].m_endNode;
                instance.m_segments.m_buffer[(int)segmentID].m_endNode = instance.m_segments.m_buffer[(int)segmentID].m_startNode;
                instance.m_segments.m_buffer[(int)segmentID].m_startNode = temp;
                instance.m_segments.m_buffer[(int)segmentID].m_flags |= NetSegment.Flags.Invert;
                var tempDir = instance.m_segments.m_buffer[(int)segmentID].m_endDirection;
                instance.m_segments.m_buffer[(int)segmentID].m_endDirection = instance.m_segments.m_buffer[(int)segmentID].m_startDirection;
                instance.m_segments.m_buffer[(int)segmentID].m_startDirection = tempDir;

            }

            if (info.m_lanes == null || (layerMask & info.m_treeLayers) == 0 && !cameraInfo.CheckRenderDistance(data.m_position, info.m_maxPropDistance + 128f))
                return;
            bool invert1;
            NetNode.Flags flags3;
            Color color3;
            NetNode.Flags flags4;
            Color color4;
            if ((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
            {
                invert1 = true;
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags3, out color3);
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags4, out color4);
            }
            else
            {
                invert1 = false;
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags3, out color3);
                instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags4, out color4);
            }
            float startAngle = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleStart * 0.02454369f;
            float endAngle = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleEnd * 0.02454369f;
            Vector4 objectIndex = data.m_dataVector3;
            InfoManager.InfoMode currentMode = Singleton<InfoManager>.instance.CurrentMode;
            if (currentMode != InfoManager.InfoMode.None && !info.m_netAI.ColorizeProps(currentMode))
                objectIndex.z = 0.0f;
            int propIndex1 = info.m_segments == null || info.m_segments.Length == 0 ? 0 : -1;
            uint laneID1 = instance.m_segments.m_buffer[(int)segmentID].m_lanes;
            for (int index = 0; index < info.m_lanes.Length && (int)laneID1 != 0; ++index)
            {
                instance.m_lanes.m_buffer[laneID1].RenderInstance(cameraInfo, laneID1, info.m_lanes[index], flags3, flags4, color3, color4, startAngle, endAngle, invert1, layerMask, objectIndex, ref data, ref propIndex1);
                laneID1 = instance.m_lanes.m_buffer[laneID1].m_nextLane;
            }

        }

    }


    public class AmericanRoads : MonoBehaviour
    {

        private static Texture2D nodeTex;
        private static Texture2D segmentTex;

        public static Texture2D LoadTexture(string texturePath)
        {
            var tex = new Texture2D(1, 1);
            tex.LoadImage(System.IO.File.ReadAllBytes(texturePath));
            tex.anisoLevel = 8;
            return tex;
        }

        public static Texture2D LoadTextureDDS(string texturePath)
        {


            var ddsBytes = File.ReadAllBytes(texturePath);
            var height = BitConverter.ToInt32(ddsBytes, 12);
            var width = BitConverter.ToInt32(ddsBytes, 16);
            var texture = new Texture2D(width, height, TextureFormat.DXT5, true);
            List<byte> byteList = new List<byte>();

            for (int i = 0; i < ddsBytes.Length; i++)
            {
                if (i > 127)
                {
                    byteList.Add(ddsBytes[i]);
                }
            }


            //texture.LoadRawTextureData(ddsBytes);
            texture.LoadRawTextureData(byteList.ToArray());

            texture.Apply();
            texture.anisoLevel = 8;
            //Debug.Log ("Path: " + texturePath + ", width: " + width + ", height: " + height);
            return texture;

        }

        public static void ReplacePavement(string textureDir)
        {
            /*var pavement_tex = new Texture2D (1,1);
			pavement_tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "pavement_texture.png")));
			pavement_tex.anisoLevel = 8;*/
            Shader.SetGlobalTexture("_TerrainPavementDiffuse", LoadTextureDDS(Path.Combine(textureDir, "pavement_texture.dds")));
        }


        public static void ReplaceSpeedLimitSigns()
        {
            PropInfo sl15 = new PropInfo();
            PropInfo sl25 = new PropInfo();
            PropInfo sl30 = new PropInfo();
            PropInfo sl45 = new PropInfo();
            PropInfo sl65 = new PropInfo();

            int propsfound = 0;

            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("speed limit 65"))
                {
                    sl65 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("speed limit 65 found");
                    propsfound++;
                }
                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("speed limit 45"))
                {
                    sl45 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("speed limit 45 found");
                    propsfound++;
                }
                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("speed limit 30"))
                {
                    sl30 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("speed limit 30 found");
                    propsfound++;
                }
                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("speed limit 25"))
                {
                    sl25 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("speed limit 25 found");
                    propsfound++;
                }
                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("speed limit 15"))
                {
                    sl15 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("speed limit 15 found");
                    propsfound++;
                }
            }


            if (propsfound >= 5)
            {
                /*var prop_collections = UnityEngine.Object.FindObjectsOfType<PropCollection>();
foreach (var pc in prop_collections)
{
	for(int i = 0; i < pc.m_prefabs.Length; i++)
	{
		if(pc.m_prefabs[i].name.ToLower().Contains("speed limit"))
		{
			if(pc.m_prefabs[i].nam.ToLower().Contains("30")
			{
					pc.m_prefabs[if
			}
					pc.m_prefabs[i] = testprop;
			Debug.Log("prop prefab replaced");
		}
	}
}*/



                var net_collections = UnityEngine.Object.FindObjectsOfType<NetCollection>();
                foreach (var nc in net_collections)
                {
                    foreach (var prefab in nc.m_prefabs)
                    {
                        if (prefab.m_lanes != null)
                        {
                            foreach (var lane in prefab.m_lanes)
                            {
                                if (lane.m_laneProps != null)
                                {
                                    for (int i = 0; i < lane.m_laneProps.m_props.Length; i++)
                                    {
                                        if (lane.m_laneProps.m_props[i].m_prop != null)
                                        {
                                            var pr = lane.m_laneProps.m_props[i].m_prop;
                                            var pr2 = lane.m_laneProps.m_props[i].m_finalProp;
                                            if (pr.name.ToLower().Contains("speed limit"))
                                            {
                                                PropInfo testprop = new PropInfo();
                                                if (pr.name.ToLower().Contains("30"))
                                                {
                                                    testprop = sl15;
                                                }
                                                else if (pr.name.ToLower().Contains("40"))
                                                {
                                                    testprop = sl25;
                                                }
                                                else if (pr.name.ToLower().Contains("50"))
                                                {
                                                    testprop = sl30;
                                                }
                                                else if (pr.name.ToLower().Contains("60"))
                                                {
                                                    testprop = sl45;
                                                }
                                                else if (pr.name.ToLower().Contains("100"))
                                                {
                                                    testprop = sl65;
                                                }

                                                //pr = UnityEngine.Object.Instantiate(testprop);
                                                //pr2 = UnityEngine.Object.Instantiate(testprop);
                                                pr.m_mesh = testprop.m_mesh;
                                                pr2.m_mesh = testprop.m_mesh;

                                                pr.m_material = testprop.m_material;
                                                pr2.m_material = testprop.m_material;

                                                pr.m_lodMesh = testprop.m_lodMesh;
                                                pr2.m_lodMesh = testprop.m_lodMesh;

                                                pr.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;
                                                pr2.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;

                                                pr.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;
                                                pr2.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;

                                                pr.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;
                                                pr2.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;

                                                pr.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;
                                                pr2.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;

                                                pr.m_lodMaterialCombined = testprop.m_lodMaterialCombined;
                                                pr2.m_lodMaterialCombined = testprop.m_lodMaterialCombined;

                                                pr.m_lodLocations = testprop.m_lodLocations;
                                                pr2.m_lodLocations = testprop.m_lodLocations;

                                                pr.m_lodObjectIndices = testprop.m_lodObjectIndices;
                                                pr2.m_lodObjectIndices = testprop.m_lodObjectIndices;

                                                pr.m_lodColors = testprop.m_lodColors;
                                                pr2.m_lodColors = testprop.m_lodColors;

                                                pr.m_lodCount = testprop.m_lodCount;
                                                pr2.m_lodCount = testprop.m_lodCount;

                                                pr.m_lodMin = testprop.m_lodMin;
                                                pr2.m_lodMin = testprop.m_lodMin;

                                                pr.m_lodMax = testprop.m_lodMax;
                                                pr2.m_lodMax = testprop.m_lodMax;

                                                pr.m_lodHeightMap = testprop.m_lodHeightMap;
                                                pr2.m_lodHeightMap = testprop.m_lodHeightMap;

                                                pr.m_lodHeightMapping = testprop.m_lodHeightMapping;
                                                pr2.m_lodHeightMapping = testprop.m_lodHeightMapping;

                                                pr.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;
                                                pr2.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;

                                                pr.m_lodRenderDistance = 2000;
                                                pr2.m_lodRenderDistance = 2000;

                                                Debug.Log(prefab.name + " sign replaced");
                                                //Debug.Log(prefab.name);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public static void ChangeProps(string textureDir, bool remove_arrows)
        {
            var prop_collections = UnityEngine.Object.FindObjectsOfType<PropCollection>();
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    if (remove_arrows && (prefab.name.Equals("Road Arrow LFR") || prefab.name.Equals("Road Arrow LR")))
                    {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
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

        public static void ReplaceTextures(string textureDir, bool color_change_active)
        {
            var net_collections = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            foreach (var nc in net_collections)
            {
                foreach (var prefab in nc.m_prefabs)
                {
                    if (prefab.m_class.name.Equals("Highway"))
                    {
                        //prefab.m_createGravel = false;
                        if (!color_change_active && !prefab.name.Contains("Ramp"))
                        {
                            prefab.m_color = new Color(0.45f, 0.45f, 0.45f);
                        }
                    }


                    if (System.IO.File.Exists(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_node.dds")))
                    {
                        nodeTex = LoadTextureDDS(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_node.dds"));
                    }


                    if (System.IO.File.Exists(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_segment.dds")))
                    {
                        segmentTex = LoadTextureDDS(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_segment.dds"));
                    }

                    foreach (var node in prefab.m_nodes)
                    {

                        if (System.IO.File.Exists(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_node.dds")))
                        {
                            //node.m_material.mainTexture = LoadTexture(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_node.png"));

                            node.m_material.SetTexture("_MainTex", nodeTex);
                            node.m_nodeMaterial.SetTexture("_MainTex", nodeTex);

                            Debug.Log("Replaced " + prefab.name + " node");
                            //node.m_lodMesh = null;
                        }
                    }

                    foreach (var segment in prefab.m_segments)
                    {
                        if (!segment.m_material.name.ToLower().Contains("cable"))
                        {
                            if (System.IO.File.Exists(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_segment.dds")))
                            {
                                //segment.m_material.mainTexture = LoadTexture(Path.Combine(textureDir, prefab.name.Replace(" ", "_").ToLowerInvariant().Trim() + "_segment.png"));

                                segment.m_material.SetTexture("_MainTex", segmentTex);
                                segment.m_segmentMaterial.SetTexture("_MainTex", segmentTex);

                                Debug.Log("Replaced " + prefab.name + " segment");
                                //segment.m_lodMesh = null;
                            }
                        }
                    }
                    //prefab.RefreshLevelOfDetail ();

                }
            }



        }

        public static void ReplaceMotorwaySign()
        {
            PropInfo testprop = new PropInfo();

            bool propfound = false;

            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("us interstate sign"))
                {
                    testprop = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("Interstate sign found.");
                    propfound = true;
                }
            }

            if (propfound)
            {

                /*var prop_collections = UnityEngine.Object.FindObjectsOfType<PropCollection>();
foreach (var pc in prop_collections)
{
	for(int i = 0; i < pc.m_prefabs.Length; i++)
	{
		if(pc.m_prefabs[i].name.ToLower().Contains("speed limit"))
		{
			if(pc.m_prefabs[i].nam.ToLower().Contains("30")
			{
					pc.m_prefabs[if
			}
					pc.m_prefabs[i] = testprop;
			Debug.Log("prop prefab replaced");
		}
	}
}*/



                var net_collections = UnityEngine.Object.FindObjectsOfType<NetCollection>();
                foreach (var nc in net_collections)
                {
                    foreach (var prefab in nc.m_prefabs)
                    {
                        if (prefab.m_lanes != null)
                        {
                            foreach (var lane in prefab.m_lanes)
                            {
                                if (lane.m_laneProps != null)
                                {
                                    for (int i = 0; i < lane.m_laneProps.m_props.Length; i++)
                                    {
                                        if (lane.m_laneProps.m_props[i].m_prop != null)
                                        {
                                            var pr = lane.m_laneProps.m_props[i].m_prop;
                                            var pr2 = lane.m_laneProps.m_props[i].m_finalProp;
                                            if (pr.name.ToLower().Contains("motorway sign"))
                                            {
                                                //var testprop = 
                                                /*if (pr.name.ToLower ().Contains ("30")) {
												testprop = sl15;
											} else if (pr.name.ToLower ().Contains ("40")) {
												testprop = sl25;
											} else if (pr.name.ToLower ().Contains ("50")) {
												testprop = sl30;
											} else if (pr.name.ToLower ().Contains ("60")) {
												testprop = sl45;
											} else if (pr.name.ToLower ().Contains ("100")) {
												testprop = sl65;
											}*/

                                                //pr = UnityEngine.Object.Instantiate(testprop);
                                                //pr2 = UnityEngine.Object.Instantiate(testprop);
                                                pr.m_mesh = testprop.m_mesh;
                                                pr2.m_mesh = testprop.m_mesh;

                                                pr.m_material = testprop.m_material;
                                                pr2.m_material = testprop.m_material;

                                                pr.m_lodMesh = testprop.m_lodMesh;
                                                pr2.m_lodMesh = testprop.m_lodMesh;

                                                pr.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;
                                                pr2.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;

                                                pr.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;
                                                pr2.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;

                                                pr.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;
                                                pr2.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;

                                                pr.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;
                                                pr2.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;

                                                pr.m_lodMaterialCombined = testprop.m_lodMaterialCombined;
                                                pr2.m_lodMaterialCombined = testprop.m_lodMaterialCombined;

                                                pr.m_lodLocations = testprop.m_lodLocations;
                                                pr2.m_lodLocations = testprop.m_lodLocations;

                                                pr.m_lodObjectIndices = testprop.m_lodObjectIndices;
                                                pr2.m_lodObjectIndices = testprop.m_lodObjectIndices;

                                                pr.m_lodColors = testprop.m_lodColors;
                                                pr2.m_lodColors = testprop.m_lodColors;

                                                pr.m_lodCount = testprop.m_lodCount;
                                                pr2.m_lodCount = testprop.m_lodCount;

                                                pr.m_lodMin = testprop.m_lodMin;
                                                pr2.m_lodMin = testprop.m_lodMin;

                                                pr.m_lodMax = testprop.m_lodMax;
                                                pr2.m_lodMax = testprop.m_lodMax;

                                                pr.m_lodHeightMap = testprop.m_lodHeightMap;
                                                pr2.m_lodHeightMap = testprop.m_lodHeightMap;

                                                pr.m_lodHeightMapping = testprop.m_lodHeightMapping;
                                                pr2.m_lodHeightMapping = testprop.m_lodHeightMapping;

                                                pr.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;
                                                pr2.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;

                                                pr.m_lodRenderDistance = 2000;
                                                pr2.m_lodRenderDistance = 2000;

                                                Debug.Log(prefab.name + " sign replaced");
                                                //Debug.Log(prefab.name);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public static void ReplaceTurnSigns()
        {
            PropInfo left_turn = new PropInfo();
            PropInfo right_turn = new PropInfo();

            int propsfound = 0;

            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("us no left turn"))
                {
                    left_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("left turn sign found.");
                    propsfound++;
                }
                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("us no right turn"))
                {
                    right_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("right turn sign found.");
                    propsfound++;
                }
            }


            if (propsfound >= 2)
            {
                /*var prop_collections = UnityEngine.Object.FindObjectsOfType<PropCollection>();
foreach (var pc in prop_collections)
{
	for(int i = 0; i < pc.m_prefabs.Length; i++)
	{
		if(pc.m_prefabs[i].name.ToLower().Contains("speed limit"))
		{
			if(pc.m_prefabs[i].nam.ToLower().Contains("30")
			{
					pc.m_prefabs[if
			}
					pc.m_prefabs[i] = testprop;
			Debug.Log("prop prefab replaced");
		}
	}
}*/



                var net_collections = UnityEngine.Object.FindObjectsOfType<NetCollection>();
                foreach (var nc in net_collections)
                {
                    foreach (var prefab in nc.m_prefabs)
                    {
                        if (prefab.m_lanes != null)
                        {
                            foreach (var lane in prefab.m_lanes)
                            {
                                if (lane.m_laneProps != null)
                                {
                                    for (int i = 0; i < lane.m_laneProps.m_props.Length; i++)
                                    {
                                        if (lane.m_laneProps.m_props[i].m_prop != null)
                                        {
                                            var pr = lane.m_laneProps.m_props[i].m_prop;
                                            var pr2 = lane.m_laneProps.m_props[i].m_finalProp;
                                            if (pr.name.ToLower().Contains("no right turn") || pr.name.ToLower().Contains("no left turn"))
                                            {
                                                //var testprop = 
                                                /*if (pr.name.ToLower ().Contains ("30")) {
												testprop = sl15;
											} else if (pr.name.ToLower ().Contains ("40")) {
												testprop = sl25;
											} else if (pr.name.ToLower ().Contains ("50")) {
												testprop = sl30;
											} else if (pr.name.ToLower ().Contains ("60")) {
												testprop = sl45;
											} else if (pr.name.ToLower ().Contains ("100")) {
												testprop = sl65;
											}*/

                                                PropInfo testprop = new PropInfo();

                                                if (pr.name.ToLower().Contains("no left turn sign"))
                                                {
                                                    testprop = left_turn;
                                                }
                                                else if (pr.name.ToLower().Contains("no right turn sign"))
                                                {
                                                    testprop = right_turn;
                                                }

                                                //pr = UnityEngine.Object.Instantiate(testprop);
                                                //pr2 = UnityEngine.Object.Instantiate(testprop);
                                                pr.m_mesh = testprop.m_mesh;
                                                pr2.m_mesh = testprop.m_mesh;

                                                pr.m_material = testprop.m_material;
                                                pr2.m_material = testprop.m_material;

                                                pr.m_lodMesh = testprop.m_lodMesh;
                                                pr2.m_lodMesh = testprop.m_lodMesh;

                                                pr.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;
                                                pr2.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;

                                                pr.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;
                                                pr2.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;

                                                pr.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;
                                                pr2.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;

                                                pr.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;
                                                pr2.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;

                                                pr.m_lodMaterialCombined = testprop.m_lodMaterialCombined;
                                                pr2.m_lodMaterialCombined = testprop.m_lodMaterialCombined;

                                                pr.m_lodLocations = testprop.m_lodLocations;
                                                pr2.m_lodLocations = testprop.m_lodLocations;

                                                pr.m_lodObjectIndices = testprop.m_lodObjectIndices;
                                                pr2.m_lodObjectIndices = testprop.m_lodObjectIndices;

                                                pr.m_lodColors = testprop.m_lodColors;
                                                pr2.m_lodColors = testprop.m_lodColors;

                                                pr.m_lodCount = testprop.m_lodCount;
                                                pr2.m_lodCount = testprop.m_lodCount;

                                                pr.m_lodMin = testprop.m_lodMin;
                                                pr2.m_lodMin = testprop.m_lodMin;

                                                pr.m_lodMax = testprop.m_lodMax;
                                                pr2.m_lodMax = testprop.m_lodMax;

                                                pr.m_lodHeightMap = testprop.m_lodHeightMap;
                                                pr2.m_lodHeightMap = testprop.m_lodHeightMap;

                                                pr.m_lodHeightMapping = testprop.m_lodHeightMapping;
                                                pr2.m_lodHeightMapping = testprop.m_lodHeightMapping;

                                                pr.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;
                                                pr2.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;

                                                pr.m_lodRenderDistance = 2000;
                                                pr2.m_lodRenderDistance = 2000;

                                                Debug.Log(prefab.name + " sign replaced");
                                                //Debug.Log(prefab.name);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public static void ReplaceParkingSigns()
        {
            PropInfo testprop = new PropInfo();
            bool propfound = false;


            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("us no parking"))
                {
                    testprop = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    Debug.Log("no parking sign found.");
                    propfound = true;
                }
            }

            if (propfound)
            {

                /*var prop_collections = UnityEngine.Object.FindObjectsOfType<PropCollection>();
foreach (var pc in prop_collections)
{
	for(int i = 0; i < pc.m_prefabs.Length; i++)
	{
		if(pc.m_prefabs[i].name.ToLower().Contains("speed limit"))
		{
			if(pc.m_prefabs[i].nam.ToLower().Contains("30")
			{
					pc.m_prefabs[if
			}
					pc.m_prefabs[i] = testprop;
			Debug.Log("prop prefab replaced");
		}
	}
}*/



                var net_collections = UnityEngine.Object.FindObjectsOfType<NetCollection>();
                foreach (var nc in net_collections)
                {
                    foreach (var prefab in nc.m_prefabs)
                    {
                        if (prefab.m_lanes != null)
                        {
                            foreach (var lane in prefab.m_lanes)
                            {
                                if (lane.m_laneProps != null)
                                {
                                    for (int i = 0; i < lane.m_laneProps.m_props.Length; i++)
                                    {
                                        if (lane.m_laneProps.m_props[i].m_prop != null)
                                        {
                                            var pr = lane.m_laneProps.m_props[i].m_prop;
                                            var pr2 = lane.m_laneProps.m_props[i].m_finalProp;
                                            if (pr.name.ToLower().Contains("no parking sign"))
                                            {
                                                //var testprop = 
                                                /*if (pr.name.ToLower ().Contains ("30")) {
												testprop = sl15;
											} else if (pr.name.ToLower ().Contains ("40")) {
												testprop = sl25;
											} else if (pr.name.ToLower ().Contains ("50")) {
												testprop = sl30;
											} else if (pr.name.ToLower ().Contains ("60")) {
												testprop = sl45;
											} else if (pr.name.ToLower ().Contains ("100")) {
												testprop = sl65;
											}*/



                                                //pr = UnityEngine.Object.Instantiate(testprop);
                                                //pr2 = UnityEngine.Object.Instantiate(testprop);
                                                pr.m_mesh = testprop.m_mesh;
                                                pr2.m_mesh = testprop.m_mesh;

                                                pr.m_material = testprop.m_material;
                                                pr2.m_material = testprop.m_material;

                                                pr.m_lodMesh = testprop.m_lodMesh;
                                                pr2.m_lodMesh = testprop.m_lodMesh;

                                                pr.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;
                                                pr2.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;

                                                pr.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;
                                                pr2.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;

                                                pr.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;
                                                pr2.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;

                                                pr.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;
                                                pr2.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;

                                                pr.m_lodMaterialCombined = testprop.m_lodMaterialCombined;
                                                pr2.m_lodMaterialCombined = testprop.m_lodMaterialCombined;

                                                pr.m_lodLocations = testprop.m_lodLocations;
                                                pr2.m_lodLocations = testprop.m_lodLocations;

                                                pr.m_lodObjectIndices = testprop.m_lodObjectIndices;
                                                pr2.m_lodObjectIndices = testprop.m_lodObjectIndices;

                                                pr.m_lodColors = testprop.m_lodColors;
                                                pr2.m_lodColors = testprop.m_lodColors;

                                                pr.m_lodCount = testprop.m_lodCount;
                                                pr2.m_lodCount = testprop.m_lodCount;

                                                pr.m_lodMin = testprop.m_lodMin;
                                                pr2.m_lodMin = testprop.m_lodMin;

                                                pr.m_lodMax = testprop.m_lodMax;
                                                pr2.m_lodMax = testprop.m_lodMax;

                                                pr.m_lodHeightMap = testprop.m_lodHeightMap;
                                                pr2.m_lodHeightMap = testprop.m_lodHeightMap;

                                                pr.m_lodHeightMapping = testprop.m_lodHeightMapping;
                                                pr2.m_lodHeightMapping = testprop.m_lodHeightMapping;

                                                pr.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;
                                                pr2.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;

                                                pr.m_lodRenderDistance = 2000;
                                                pr2.m_lodRenderDistance = 2000;

                                                Debug.Log(prefab.name + " sign replaced");
                                                //Debug.Log(prefab.name);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


    }

}