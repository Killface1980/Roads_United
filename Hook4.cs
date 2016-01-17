using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RoadsUnited
{
	public class Hook4 : MonoBehaviour
	{
		public bool hookEnabled = false;

		private Dictionary<MethodInfo, RedirectCallsState> redirects = new Dictionary<MethodInfo, RedirectCallsState>();

		public static Material invertedBridgeMat;

		public void Update()
		{
			if (!this.hookEnabled && !RoadsUnitedModLoader.exor)
			{
				this.EnableHook();
			}
		}

/*		public void EnableHook()
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			MethodInfo methodInfo = typeof(NetSegment).GetMethods(bindingAttr).Single((MethodInfo c) => c.Name == "RenderInstance" && c.GetParameters().Length == 3);
			this.redirects.Add(methodInfo, RedirectionHelper.RedirectCalls(methodInfo, typeof(Hook4).GetMethod("RenderInstanceSegment", bindingAttr)));
			methodInfo = typeof(NetSegment).GetMethods(bindingAttr).Single((MethodInfo c) => c.Name == "RenderLod");
			this.redirects.Add(methodInfo, RedirectionHelper.RedirectCalls(methodInfo, typeof(Hook4).GetMethod("RenderInstanceSegment", bindingAttr)));
			methodInfo = typeof(NetNode).GetMethods(bindingAttr).Single((MethodInfo c) => c.Name == "RenderInstance" && c.GetParameters().Length == 3);
			this.redirects.Add(methodInfo, RedirectionHelper.RedirectCalls(methodInfo, typeof(Hook4).GetMethods(bindingAttr).Single((MethodInfo c) => c.Name == "RenderInstanceNode" && c.GetParameters().Length == 3)));
			methodInfo = typeof(NetNode).GetMethods(bindingAttr).Single((MethodInfo c) => c.Name == "RenderLod");
			this.redirects.Add(methodInfo, RedirectionHelper.RedirectCalls(methodInfo, typeof(Hook4).GetMethods(bindingAttr).Single((MethodInfo c) => c.Name == "RenderInstanceNode" && c.GetParameters().Length == 3)));
			this.hookEnabled = true;
		}
*/
		private MethodInfo GetMethod(string name, uint argCount)
		{
			MethodInfo[] methods = typeof(NetNode).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo[] array = methods;
			MethodInfo result;
			for (int i = 0; i < array.Length; i++)
			{
				MethodInfo methodInfo = array[i];
				if (methodInfo.Name == name && (long)methodInfo.GetParameters().Length == (long)((ulong)argCount))
				{
					result = methodInfo;
					return result;
				}
			}
			result = null;
			return result;
		}

/*		public void DisableHook()
		{
			if (this.hookEnabled)
			{
				foreach (KeyValuePair<MethodInfo, RedirectCallsState> current in this.redirects)
				{
					RedirectionHelper.RevertRedirect(current.Key, current.Value);
				}
				this.redirects.Clear();
				this.hookEnabled = false;
			}
		}
*/
		private void RefreshJunctionData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex)
		{
			MethodInfo method = this.GetMethod("RefreshJunctionData", 3u);
			object[] parameters = new object[]
			{
				nodeID,
				info,
				instanceIndex
			};
			method.Invoke(netnode, parameters);
		}

		private void RefreshBendData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex, ref RenderManager.Instance data)
		{
			MethodInfo method = this.GetMethod("RefreshBendData", 4u);
			object[] array = new object[]
			{
				nodeID,
				info,
				instanceIndex,
				data
			};
			method.Invoke(netnode, array);
			data = (RenderManager.Instance)array[3];
		}

		private void RefreshJunctionData(NetNode netnode, ushort nodeID, int segmentIndex, ushort nodeSegment, Vector3 centerPos, ref uint instanceIndex, ref RenderManager.Instance data)
		{
			NetManager instance = Singleton<NetManager>.instance;
			data.m_position = netnode.m_position;
			data.m_rotation = Quaternion.identity;
			data.m_initialized = true;
			float vScale = 0.05f;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 zero3 = Vector3.zero;
			Vector3 zero4 = Vector3.zero;
			Vector3 zero5 = Vector3.zero;
			Vector3 zero6 = Vector3.zero;
			Vector3 zero7 = Vector3.zero;
			Vector3 zero8 = Vector3.zero;
			Vector3 zero9 = Vector3.zero;
			Vector3 zero10 = Vector3.zero;
			Vector3 zero11 = Vector3.zero;
			Vector3 zero12 = Vector3.zero;
			NetSegment netSegment = instance.m_segments.m_buffer[(int)nodeSegment];
			NetInfo info = netSegment.Info;
			ItemClass connectionClass = info.GetConnectionClass();
			Vector3 vector = (nodeID != netSegment.m_startNode) ? netSegment.m_endDirection : netSegment.m_startDirection;
			float num = -4f;
			float num2 = -4f;
			ushort num3 = 0;
			ushort num4 = 0;
			for (int i = 0; i < 8; i++)
			{
				ushort segment = netnode.GetSegment(i);
				if (segment != 0 && segment != nodeSegment)
				{
					ItemClass connectionClass2 = instance.m_segments.m_buffer[(int)segment].Info.GetConnectionClass();
					if (connectionClass.m_service == connectionClass2.m_service)
					{
						NetSegment netSegment2 = instance.m_segments.m_buffer[(int)segment];
						Vector3 vector2 = (nodeID != netSegment2.m_startNode) ? netSegment2.m_endDirection : netSegment2.m_startDirection;
						float num5 = (float)((double)vector.x * (double)vector2.x + (double)vector.z * (double)vector2.z);
						if ((double)vector2.z * (double)vector.x - (double)vector2.x * (double)vector.z < 0.0)
						{
							if ((double)num5 > (double)num)
							{
								num = num5;
								num3 = segment;
							}
							float num6 = -2f - num5;
							if ((double)num6 > (double)num2)
							{
								num2 = num6;
								num4 = segment;
							}
						}
						else
						{
							if ((double)num5 > (double)num2)
							{
								num2 = num5;
								num4 = segment;
							}
							float num6 = -2f - num5;
							if ((double)num6 > (double)num)
							{
								num = num6;
								num3 = segment;
							}
						}
					}
				}
			}
			bool start = netSegment.m_startNode == nodeID;
			bool flag;
			netSegment.CalculateCorner(nodeSegment, true, start, false, out zero, out zero3, out flag);
			netSegment.CalculateCorner(nodeSegment, true, start, true, out zero2, out zero4, out flag);
			if (num3 != 0 && num4 != 0)
			{
				float num7 = (float)((double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5);
				float num8 = 1f;
				if (num3 != 0)
				{
					NetSegment netSegment2 = instance.m_segments.m_buffer[(int)num3];
					NetInfo info2 = netSegment2.Info;
					bool start2 = netSegment2.m_startNode == nodeID;
					netSegment2.CalculateCorner(num3, true, start2, true, out zero5, out zero7, out flag);
					netSegment2.CalculateCorner(num3, true, start2, false, out zero6, out zero8, out flag);
					float num5 = (float)((double)info2.m_pavementWidth / (double)info2.m_halfWidth * 0.5);
					num7 = (float)(((double)num7 + (double)num5) * 0.5);
					num8 = (float)(2.0 * (double)info.m_halfWidth / ((double)info.m_halfWidth + (double)info2.m_halfWidth));
				}
				float num9 = (float)((double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5);
				float num10 = 1f;
				if (num4 != 0)
				{
					NetSegment netSegment2 = instance.m_segments.m_buffer[(int)num4];
					NetInfo info2 = netSegment2.Info;
					bool start2 = netSegment2.m_startNode == nodeID;
					netSegment2.CalculateCorner(num4, true, start2, true, out zero9, out zero11, out flag);
					netSegment2.CalculateCorner(num4, true, start2, false, out zero10, out zero12, out flag);
					float num5 = (float)((double)info2.m_pavementWidth / (double)info2.m_halfWidth * 0.5);
					num9 = (float)(((double)num9 + (double)num5) * 0.5);
					num10 = (float)(2.0 * (double)info.m_halfWidth / ((double)info.m_halfWidth + (double)info2.m_halfWidth));
				}
				Vector3 vector3;
				Vector3 vector4;
				NetSegment.CalculateMiddlePoints(zero, -zero3, zero5, -zero7, true, true, out vector3, out vector4);
				Vector3 vector5;
				Vector3 vector6;
				NetSegment.CalculateMiddlePoints(zero2, -zero4, zero6, -zero8, true, true, out vector5, out vector6);
				Vector3 vector7;
				Vector3 vector8;
				NetSegment.CalculateMiddlePoints(zero, -zero3, zero9, -zero11, true, true, out vector7, out vector8);
				Vector3 vector9;
				Vector3 vector10;
				NetSegment.CalculateMiddlePoints(zero2, -zero4, zero10, -zero12, true, true, out vector9, out vector10);
				data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(zero, vector3, vector4, zero5, zero, vector3, vector4, zero5, netnode.m_position, vScale);
				data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(zero2, vector5, vector6, zero6, zero2, vector5, vector6, zero6, netnode.m_position, vScale);
				data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(zero, vector7, vector8, zero9, zero, vector7, vector8, zero9, netnode.m_position, vScale);
				data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(zero2, vector9, vector10, zero10, zero2, vector9, vector10, zero10, netnode.m_position, vScale);
				data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, (float)(0.5 - (double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5), (float)((double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5));
				data.m_dataVector1 = centerPos - data.m_position;
				data.m_dataVector1.w = (float)(((double)data.m_dataMatrix0.m33 + (double)data.m_extraData.m_dataMatrix2.m33 + (double)data.m_extraData.m_dataMatrix3.m33 + (double)data.m_dataMatrix1.m33) * 0.25);
				data.m_dataVector2 = new Vector4(num7, num8, num9, num10);
				data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(86016u + (uint)nodeID);
			}
			else
			{
				centerPos.x = (float)(((double)zero.x + (double)zero2.x) * 0.5);
				centerPos.z = (float)(((double)zero.z + (double)zero2.z) * 0.5);
				Vector3 vector2 = zero2;
				Vector3 vector11 = zero;
				Vector3 vector12 = zero4;
				Vector3 vector13 = zero3;
				float num5 = Mathf.Min(info.m_halfWidth * 1.333333f, 16f);
				Vector3 vector14 = zero - zero3 * num5;
				Vector3 vector15 = vector2 - vector12 * num5;
				Vector3 vector16 = zero2 - zero4 * num5;
				Vector3 vector17 = vector11 - vector13 * num5;
				Vector3 vector18 = zero + zero3 * num5;
				Vector3 vector19 = vector2 + vector12 * num5;
				Vector3 vector20 = zero2 + zero4 * num5;
				Vector3 vector21 = vector11 + vector13 * num5;
				data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(zero, vector14, vector15, vector2, zero, vector14, vector15, vector2, netnode.m_position, vScale);
				data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(zero2, vector20, vector21, vector11, zero2, vector20, vector21, vector11, netnode.m_position, vScale);
				data.m_extraData.m_dataMatrix3 = NetSegment.CalculateControlMatrix(zero, vector18, vector19, vector2, zero, vector18, vector19, vector2, netnode.m_position, vScale);
				data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(zero2, vector16, vector17, vector11, zero2, vector16, vector17, vector11, netnode.m_position, vScale);
				data.m_dataMatrix0.SetRow(3, data.m_dataMatrix0.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
				data.m_extraData.m_dataMatrix2.SetRow(3, data.m_extraData.m_dataMatrix2.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
				data.m_extraData.m_dataMatrix3.SetRow(3, data.m_extraData.m_dataMatrix3.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
				data.m_dataMatrix1.SetRow(3, data.m_dataMatrix1.GetRow(3) + new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
				data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, (float)(0.5 - (double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5), (float)((double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5));
				data.m_dataVector1 = centerPos - data.m_position;
				data.m_dataVector1.w = (float)(((double)data.m_dataMatrix0.m33 + (double)data.m_extraData.m_dataMatrix2.m33 + (double)data.m_extraData.m_dataMatrix3.m33 + (double)data.m_dataMatrix1.m33) * 0.25);
				data.m_dataVector2 = new Vector4((float)((double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5), 1f, (float)((double)info.m_pavementWidth / (double)info.m_halfWidth * 0.5), 1f);
				data.m_extraData.m_dataVector4 = RenderManager.GetColorLocation(86016u + (uint)nodeID);
			}
			data.m_dataInt0 = segmentIndex;
			data.m_dataColor0 = info.m_color;
			data.m_dataColor0.a = 0f;
			if (info.m_requireSurfaceMaps)
			{
				Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector3);
			}
			instanceIndex = (uint)data.m_nextInstance;
		}

		private void RefreshJunctionData(NetNode netnode, ushort nodeID, int segmentIndex, NetInfo info, ushort nodeSegment, ushort nodeSegment2, ref uint instanceIndex, ref RenderManager.Instance data)
		{
			data.m_position = netnode.m_position;
			data.m_rotation = Quaternion.identity;
			data.m_initialized = true;
			float vScale = 0.05f;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 zero3 = Vector3.zero;
			Vector3 zero4 = Vector3.zero;
			Vector3 zero5 = Vector3.zero;
			Vector3 zero6 = Vector3.zero;
			Vector3 zero7 = Vector3.zero;
			Vector3 zero8 = Vector3.zero;
			bool start = Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].m_startNode == nodeID;
			bool flag;
			Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].CalculateCorner(nodeSegment, true, start, false, out zero, out zero5, out flag);
			Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment].CalculateCorner(nodeSegment, true, start, true, out zero2, out zero6, out flag);
			bool start2 = Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].m_startNode == nodeID;
			Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].CalculateCorner(nodeSegment2, true, start2, true, out zero3, out zero7, out flag);
			Singleton<NetManager>.instance.m_segments.m_buffer[(int)nodeSegment2].CalculateCorner(nodeSegment2, true, start2, false, out zero4, out zero8, out flag);
			Vector3 vector;
			Vector3 vector2;
			NetSegment.CalculateMiddlePoints(zero, -zero5, zero3, -zero7, true, true, out vector, out vector2);
			Vector3 vector3;
			Vector3 vector4;
			NetSegment.CalculateMiddlePoints(zero2, -zero6, zero4, -zero8, true, true, out vector3, out vector4);
			data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(zero, vector, vector2, zero3, zero2, vector3, vector4, zero4, netnode.m_position, vScale);
			data.m_extraData.m_dataMatrix2 = NetSegment.CalculateControlMatrix(zero2, vector3, vector4, zero4, zero, vector, vector2, zero3, netnode.m_position, vScale);
			data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
			data.m_dataVector3 = RenderManager.GetColorLocation(86016u + (uint)nodeID);
			data.m_dataInt0 = (8 | segmentIndex);
			data.m_dataColor0 = info.m_color;
			data.m_dataColor0.a = 0f;
			if (info.m_requireSurfaceMaps)
			{
				Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);
			}
			instanceIndex = (uint)data.m_nextInstance;
		}

		private int CalculateRendererCount(NetNode netnode, NetInfo info)
		{
			int result;
			if ((netnode.m_flags & NetNode.Flags.Junction) == NetNode.Flags.None)
			{
				result = 1;
			}
			else
			{
				int num = 0;
				if (info.m_requireSegmentRenderers)
				{
					num += netnode.CountSegments();
				}
				if (info.m_requireDirectRenderers)
				{
					num += (int)netnode.m_connectCount;
				}
				result = num;
			}
			return result;
		}

		public void RenderInstanceNode(RenderManager.CameraInfo cameraInfo, ushort nodeID, int layerMask)
		{
			NetManager instance = Singleton<NetManager>.instance;
			NetNode netnode = instance.m_nodes.m_buffer[(int)nodeID];
			if (netnode.m_flags != NetNode.Flags.None)
			{
				NetInfo info = netnode.Info;
				if (cameraInfo.Intersect(netnode.m_bounds))
				{
					if (netnode.m_problems != Notification.Problem.None && (layerMask & 1 << Singleton<NotificationManager>.instance.m_notificationLayer) != 0)
					{
						Vector3 position = netnode.m_position;
						position.y += Mathf.Max(5f, info.m_maxHeight);
						Notification.RenderInstance(cameraInfo, netnode.m_problems, position, 1f);
					}
					if ((layerMask & info.m_netLayers) != 0 && (netnode.m_flags & (NetNode.Flags.End | NetNode.Flags.Bend | NetNode.Flags.Junction)) != NetNode.Flags.None)
					{
						if ((netnode.m_flags & NetNode.Flags.Bend) != NetNode.Flags.None)
						{
							if (info.m_segments == null || info.m_segments.Length == 0)
							{
								return;
							}
						}
						else if (info.m_nodes == null || info.m_nodes.Length == 0)
						{
							return;
						}
						uint count = (uint)this.CalculateRendererCount(netnode, info);
						RenderManager instance2 = Singleton<RenderManager>.instance;
						uint num;
						if (instance2.RequireInstance(86016u + (uint)nodeID, count, out num))
						{
							int num2 = 0;
							while (num != 65535u)
							{
								this.RenderInstanceNode(cameraInfo, nodeID, info, num2, netnode.m_flags, ref num, ref instance2.m_instances[(int)((UIntPtr)num)]);
								if (++num2 > 36)
								{
									CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
									break;
								}
							}
						}
					}
				}
			}
		}

		public void RenderInstanceNode(RenderManager.CameraInfo cameraInfo, ushort nodeID, NetInfo info, int iter, NetNode.Flags flags, ref uint instanceIndex, ref RenderManager.Instance data)
		{
			NetManager instance = Singleton<NetManager>.instance;
			NetNode netnode = instance.m_nodes.m_buffer[(int)nodeID];
			if (data.m_dirty)
			{
				data.m_dirty = false;
				if (iter == 0)
				{
					if ((flags & NetNode.Flags.Junction) != NetNode.Flags.None)
					{
						this.RefreshJunctionData(netnode, nodeID, info, instanceIndex);
					}
					else if ((flags & NetNode.Flags.Bend) != NetNode.Flags.None)
					{
						this.RefreshBendData(netnode, nodeID, info, instanceIndex, ref data);
					}
					else if ((flags & NetNode.Flags.End) != NetNode.Flags.None)
					{
						this.RefreshEndData(netnode, nodeID, info, instanceIndex, ref data);
					}
				}
			}
			if (data.m_initialized)
			{
				if ((flags & NetNode.Flags.Junction) != NetNode.Flags.None)
				{
					if ((data.m_dataInt0 & 8) != 0)
					{
						ushort segment = netnode.GetSegment(data.m_dataInt0 & 7);
						if (segment != 0)
						{
							NetManager instance2 = Singleton<NetManager>.instance;
							info = instance2.m_segments.m_buffer[(int)segment].Info;
							for (int i = 0; i < info.m_nodes.Length; i++)
							{
								NetInfo.Node node = info.m_nodes[i];
								if (node.CheckFlags(flags) && node.m_directConnect)
								{
									instance2.m_materialBlock.Clear();
									instance2.m_materialBlock.AddMatrix(instance2.ID_LeftMatrix, data.m_dataMatrix0);
									instance2.m_materialBlock.AddMatrix(instance2.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
									instance2.m_materialBlock.AddVector(instance2.ID_MeshScale, data.m_dataVector0);
									instance2.m_materialBlock.AddVector(instance2.ID_ObjectIndex, data.m_dataVector3);
									instance2.m_materialBlock.AddColor(instance2.ID_Color, data.m_dataColor0);
									if (info.m_requireSurfaceMaps && data.m_dataTexture1 != null)
									{
										instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexA, data.m_dataTexture0);
										instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexB, data.m_dataTexture1);
										instance2.m_materialBlock.AddVector(instance2.ID_SurfaceMapping, data.m_dataVector1);
									}
									NetManager expr_25A_cp_0 = instance2;
									expr_25A_cp_0.m_drawCallData.m_defaultCalls = expr_25A_cp_0.m_drawCallData.m_defaultCalls + 1;
									Graphics.DrawMesh(node.m_nodeMesh, data.m_position, data.m_rotation, node.m_nodeMaterial, node.m_layer, null, 0, instance2.m_materialBlock);
								}
							}
						}
					}
					else
					{
						ushort segment = netnode.GetSegment(data.m_dataInt0 & 7);
						if (segment != 0)
						{
							NetManager instance2 = Singleton<NetManager>.instance;
							info = instance2.m_segments.m_buffer[(int)segment].Info;
							for (int i = 0; i < info.m_nodes.Length; i++)
							{
								NetInfo.Node node = info.m_nodes[i];
								if (node.CheckFlags(flags) && !node.m_directConnect)
								{
									instance2.m_materialBlock.Clear();
									instance2.m_materialBlock.AddMatrix(instance2.ID_LeftMatrix, data.m_dataMatrix0);
									instance2.m_materialBlock.AddMatrix(instance2.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
									instance2.m_materialBlock.AddMatrix(instance2.ID_LeftMatrixB, data.m_extraData.m_dataMatrix3);
									instance2.m_materialBlock.AddMatrix(instance2.ID_RightMatrixB, data.m_dataMatrix1);
									instance2.m_materialBlock.AddVector(instance2.ID_MeshScale, data.m_dataVector0);
									instance2.m_materialBlock.AddVector(instance2.ID_CenterPos, data.m_dataVector1);
									instance2.m_materialBlock.AddVector(instance2.ID_SideScale, data.m_dataVector2);
									instance2.m_materialBlock.AddVector(instance2.ID_ObjectIndex, data.m_extraData.m_dataVector4);
									instance2.m_materialBlock.AddColor(instance2.ID_Color, data.m_dataColor0);
									if (info.m_requireSurfaceMaps && data.m_dataTexture1 != null)
									{
										instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexA, data.m_dataTexture0);
										instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexB, data.m_dataTexture1);
										instance2.m_materialBlock.AddVector(instance2.ID_SurfaceMapping, data.m_dataVector3);
									}
									NetManager expr_49F_cp_0 = instance2;
									expr_49F_cp_0.m_drawCallData.m_defaultCalls = expr_49F_cp_0.m_drawCallData.m_defaultCalls + 1;
									Graphics.DrawMesh(node.m_nodeMesh, data.m_position, data.m_rotation, node.m_nodeMaterial, node.m_layer, null, 0, instance2.m_materialBlock);
								}
							}
						}
					}
				}
				else if ((flags & NetNode.Flags.End) != NetNode.Flags.None)
				{
					NetManager instance2 = Singleton<NetManager>.instance;
					for (int i = 0; i < info.m_nodes.Length; i++)
					{
						NetInfo.Node node = info.m_nodes[i];
						if (node.CheckFlags(flags) && !node.m_directConnect)
						{
							instance2.m_materialBlock.Clear();
							instance2.m_materialBlock.AddMatrix(instance2.ID_LeftMatrix, data.m_dataMatrix0);
							instance2.m_materialBlock.AddMatrix(instance2.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
							instance2.m_materialBlock.AddMatrix(instance2.ID_LeftMatrixB, data.m_extraData.m_dataMatrix3);
							instance2.m_materialBlock.AddMatrix(instance2.ID_RightMatrixB, data.m_dataMatrix1);
							instance2.m_materialBlock.AddVector(instance2.ID_MeshScale, data.m_dataVector0);
							instance2.m_materialBlock.AddVector(instance2.ID_CenterPos, data.m_dataVector1);
							instance2.m_materialBlock.AddVector(instance2.ID_SideScale, data.m_dataVector2);
							instance2.m_materialBlock.AddVector(instance2.ID_ObjectIndex, data.m_extraData.m_dataVector4);
							instance2.m_materialBlock.AddColor(instance2.ID_Color, data.m_dataColor0);
							if (info.m_requireSurfaceMaps && data.m_dataTexture1 != null)
							{
								instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexA, data.m_dataTexture0);
								instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexB, data.m_dataTexture1);
								instance2.m_materialBlock.AddVector(instance2.ID_SurfaceMapping, data.m_dataVector3);
							}
							NetManager expr_6BF_cp_0 = instance2;
							expr_6BF_cp_0.m_drawCallData.m_defaultCalls = expr_6BF_cp_0.m_drawCallData.m_defaultCalls + 1;
							Graphics.DrawMesh(node.m_nodeMesh, data.m_position, data.m_rotation, node.m_nodeMaterial, node.m_layer, null, 0, instance2.m_materialBlock);
						}
					}
				}
				else if ((flags & NetNode.Flags.Bend) != NetNode.Flags.None)
				{
					NetManager instance2 = Singleton<NetManager>.instance;
					for (int i = 0; i < info.m_segments.Length; i++)
					{
						NetInfo.Segment segment2 = info.m_segments[i];
						bool flag;
						if (segment2.CheckFlags(NetSegment.Flags.None, out flag))
						{
							instance2.m_materialBlock.Clear();
							instance2.m_materialBlock.AddMatrix(instance2.ID_LeftMatrix, data.m_dataMatrix0);
							instance2.m_materialBlock.AddMatrix(instance2.ID_RightMatrix, data.m_extraData.m_dataMatrix2);
							instance2.m_materialBlock.AddVector(instance2.ID_MeshScale, data.m_dataVector0);
							instance2.m_materialBlock.AddVector(instance2.ID_ObjectIndex, data.m_dataVector3);
							instance2.m_materialBlock.AddColor(instance2.ID_Color, data.m_dataColor0);
							if (info.m_requireSurfaceMaps && data.m_dataTexture1 != null)
							{
								instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexA, data.m_dataTexture0);
								instance2.m_materialBlock.AddTexture(instance2.ID_SurfaceTexB, data.m_dataTexture1);
								instance2.m_materialBlock.AddVector(instance2.ID_SurfaceMapping, data.m_dataVector1);
							}
							NetManager expr_866_cp_0 = instance2;
							expr_866_cp_0.m_drawCallData.m_defaultCalls = expr_866_cp_0.m_drawCallData.m_defaultCalls + 1;
							Graphics.DrawMesh(segment2.m_segmentMesh, data.m_position, data.m_rotation, segment2.m_segmentMaterial, segment2.m_layer, null, 0, instance2.m_materialBlock);
						}
					}
				}
			}
			instanceIndex = (uint)data.m_nextInstance;
		}

		private void RefreshEndData(NetNode netnode, ushort nodeID, NetInfo info, uint instanceIndex, ref RenderManager.Instance data)
		{
			MethodInfo method = this.GetMethod("RefreshEndData", 4u);
			object[] array = new object[]
			{
				nodeID,
				info,
				instanceIndex,
				data
			};
			method.Invoke(netnode, array);
			data = (RenderManager.Instance)array[3];
		}

		public void RenderInstanceSegment(RenderManager.CameraInfo cameraInfo, ushort segmentID, int layerMask)
		{
			NetManager instance = Singleton<NetManager>.instance;
			NetSegment netSegment = instance.m_segments.m_buffer[(int)segmentID];
			if (netSegment.m_flags != NetSegment.Flags.None)
			{
				NetInfo info = netSegment.Info;
				if ((layerMask & info.m_netLayers) != 0 && cameraInfo.Intersect(netSegment.m_bounds))
				{
					RenderManager instance2 = Singleton<RenderManager>.instance;
					uint num;
					if (instance2.RequireInstance((uint)(49152 + segmentID), 1u, out num))
					{
						this.RenderInstanceSegmentNew(cameraInfo, segmentID, layerMask, info, ref instance2.m_instances[(int)((UIntPtr)num)]);
					}
				}
			}
		}

		private void RenderInstanceSegmentNew(RenderManager.CameraInfo cameraInfo, ushort segmentID, int layerMask, NetInfo info, ref RenderManager.Instance data)
		{
			NetManager instance = Singleton<NetManager>.instance;
			bool flag = false;
			if ((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None && info.name.Contains("Highway") && !info.name.ToLower().Contains("tunnel") && !info.name.ToLower().Contains("slope"))
			{
				flag = true;
				ushort endNode = instance.m_segments.m_buffer[(int)segmentID].m_endNode;
				instance.m_segments.m_buffer[(int)segmentID].m_endNode = instance.m_segments.m_buffer[(int)segmentID].m_startNode;
				instance.m_segments.m_buffer[(int)segmentID].m_startNode = endNode;
				instance.m_segments.m_buffer[(int)segmentID].m_flags = (instance.m_segments.m_buffer[(int)segmentID].m_flags & ~NetSegment.Flags.Invert);
				Vector3 endDirection = instance.m_segments.m_buffer[(int)segmentID].m_endDirection;
				instance.m_segments.m_buffer[(int)segmentID].m_endDirection = instance.m_segments.m_buffer[(int)segmentID].m_startDirection;
				instance.m_segments.m_buffer[(int)segmentID].m_startDirection = endDirection;
			}
			if (data.m_dirty)
			{
				data.m_dirty = false;
				Vector3 position = instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].m_position;
				Vector3 position2 = instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].m_position;
				data.m_position = (position + position2) * 0.5f;
				data.m_rotation = Quaternion.identity;
				data.m_dataColor0 = info.m_color;
				data.m_dataColor0.a = 0f;
				data.m_dataVector0 = new Vector4(0.5f / info.m_halfWidth, 1f / info.m_segmentLength, 1f, 1f);
				data.m_dataVector3 = RenderManager.GetColorLocation((uint)(49152 + segmentID));
				data.m_dataVector3.w = Singleton<WindManager>.instance.GetWindSpeed(data.m_position);
				if (info.m_segments == null || info.m_segments.Length == 0)
				{
					if (info.m_lanes != null)
					{
						bool invert;
						if ((flag && !info.name.Contains("Highway")) || (!flag && info.name.Contains("Highway") && !info.name.ToLower().Contains("tunnel") && !info.name.ToLower().Contains("slope")))
						{
							invert = true;
							NetNode.Flags flags;
							Color color;
							instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags, out color);
							NetNode.Flags flags2;
							Color color2;
							instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);
						}
						else if ((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
						{
							invert = true;
							NetNode.Flags flags;
							Color color;
							instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags, out color);
							NetNode.Flags flags2;
							Color color2;
							instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);
						}
						else
						{
							invert = false;
							NetNode.Flags flags;
							Color color;
							instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags, out color);
							NetNode.Flags flags2;
							Color color2;
							instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out flags2, out color2);
						}
						float startAngle = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleStart * 0.02454369f;
						float endAngle = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleEnd * 0.02454369f;
						int num = 0;
						uint num2 = instance.m_segments.m_buffer[(int)segmentID].m_lanes;
						int i = 0;
						while (i < info.m_lanes.Length && num2 != 0u)
						{
							instance.m_lanes.m_buffer[(int)((UIntPtr)num2)].RefreshInstance(num2, info.m_lanes[i], startAngle, endAngle, invert, ref data, ref num);
							num2 = instance.m_lanes.m_buffer[(int)((UIntPtr)num2)].m_nextLane;
							i++;
						}
					}
				}
				else
				{
					bool flag2 = false;
					int i = 0;
					while (i < info.m_segments.Length && !info.m_segments[i].CheckFlags(instance.m_segments.m_buffer[(int)segmentID].m_flags, out flag2))
					{
						i++;
					}
					float vScale = 0.05f;
					Vector3 vector;
					Vector3 startDir;
					bool smoothStart;
					instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, true, true, out vector, out startDir, out smoothStart);
					Vector3 vector2;
					Vector3 endDir;
					bool smoothEnd;
					instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, false, true, out vector2, out endDir, out smoothEnd);
					Vector3 vector3;
					Vector3 startDir2;
					instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, true, false, out vector3, out startDir2, out smoothStart);
					Vector3 vector4;
					Vector3 endDir2;
					instance.m_segments.m_buffer[(int)segmentID].CalculateCorner(segmentID, true, false, false, out vector4, out endDir2, out smoothEnd);
					Vector3 vector5;
					Vector3 vector6;
					NetSegment.CalculateMiddlePoints(vector, startDir, vector4, endDir2, smoothStart, smoothEnd, out vector5, out vector6);
					Vector3 vector7;
					Vector3 vector8;
					NetSegment.CalculateMiddlePoints(vector3, startDir2, vector2, endDir, smoothStart, smoothEnd, out vector7, out vector8);
					if (flag2)
					{
						data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(vector2, vector8, vector7, vector3, vector4, vector6, vector5, vector, data.m_position, vScale);
						data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(vector4, vector6, vector5, vector, vector2, vector8, vector7, vector3, data.m_position, vScale);
					}
					else
					{
						data.m_dataMatrix0 = NetSegment.CalculateControlMatrix(vector, vector5, vector6, vector4, vector3, vector7, vector8, vector2, data.m_position, vScale);
						data.m_dataMatrix1 = NetSegment.CalculateControlMatrix(vector3, vector7, vector8, vector2, vector, vector5, vector6, vector4, data.m_position, vScale);
					}
				}
				if (info.m_requireSurfaceMaps)
				{
					Singleton<TerrainManager>.instance.GetSurfaceMapping(data.m_position, out data.m_dataTexture0, out data.m_dataTexture1, out data.m_dataVector1);
				}
			}
			if (info.m_segments != null)
			{
				for (int i = 0; i < info.m_segments.Length; i++)
				{
					NetInfo.Segment segment = info.m_segments[i];
					bool flag2;
					if (segment.CheckFlags(instance.m_segments.m_buffer[(int)segmentID].m_flags, out flag2))
					{
						instance.m_materialBlock.Clear();
						instance.m_materialBlock.AddMatrix(instance.ID_LeftMatrix, data.m_dataMatrix0);
						instance.m_materialBlock.AddMatrix(instance.ID_RightMatrix, data.m_dataMatrix1);
						instance.m_materialBlock.AddVector(instance.ID_MeshScale, data.m_dataVector0);
						instance.m_materialBlock.AddVector(instance.ID_ObjectIndex, data.m_dataVector3);
						instance.m_materialBlock.AddColor(instance.ID_Color, data.m_dataColor0);
						if (info.m_requireSurfaceMaps && data.m_dataTexture0 != null)
						{
							instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexA, data.m_dataTexture0);
							instance.m_materialBlock.AddTexture(instance.ID_SurfaceTexB, data.m_dataTexture1);
							instance.m_materialBlock.AddVector(instance.ID_SurfaceMapping, data.m_dataVector1);
						}
						NetManager expr_A97_cp_0 = instance;
						expr_A97_cp_0.m_drawCallData.m_defaultCalls = expr_A97_cp_0.m_drawCallData.m_defaultCalls + 1;
						if (info.name.Contains("Bridge") && i == 1 && flag)
						{
							Graphics.DrawMesh(segment.m_segmentMesh, data.m_position, Quaternion.Euler(0f, 180f, 0f), segment.m_segmentMaterial, segment.m_layer, null, 0, instance.m_materialBlock);
						}
						else
						{
							Graphics.DrawMesh(segment.m_segmentMesh, data.m_position, data.m_rotation, segment.m_segmentMaterial, segment.m_layer, null, 0, instance.m_materialBlock);
						}
					}
				}
			}
			if (flag)
			{
				ushort endNode = instance.m_segments.m_buffer[(int)segmentID].m_endNode;
				instance.m_segments.m_buffer[(int)segmentID].m_endNode = instance.m_segments.m_buffer[(int)segmentID].m_startNode;
				instance.m_segments.m_buffer[(int)segmentID].m_startNode = endNode;
				NetSegment[] expr_BD9_cp_0 = instance.m_segments.m_buffer;
				expr_BD9_cp_0[(int)segmentID].m_flags = (expr_BD9_cp_0[(int)segmentID].m_flags | NetSegment.Flags.Invert);
				Vector3 endDirection = instance.m_segments.m_buffer[(int)segmentID].m_endDirection;
				instance.m_segments.m_buffer[(int)segmentID].m_endDirection = instance.m_segments.m_buffer[(int)segmentID].m_startDirection;
				instance.m_segments.m_buffer[(int)segmentID].m_startDirection = endDirection;
			}
			if (info.m_lanes != null && ((layerMask & info.m_treeLayers) != 0 || cameraInfo.CheckRenderDistance(data.m_position, info.m_maxPropDistance + 128f)))
			{
				bool invert2;
				NetNode.Flags startFlags;
				Color startColor;
				NetNode.Flags endFlags;
				Color endColor;
				if ((instance.m_segments.m_buffer[(int)segmentID].m_flags & NetSegment.Flags.Invert) != NetSegment.Flags.None)
				{
					invert2 = true;
					instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out startFlags, out startColor);
					instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out endFlags, out endColor);
				}
				else
				{
					invert2 = false;
					instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_startNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_startNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out startFlags, out startColor);
					instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode].Info.m_netAI.GetNodeState(instance.m_segments.m_buffer[(int)segmentID].m_endNode, ref instance.m_nodes.m_buffer[(int)instance.m_segments.m_buffer[(int)segmentID].m_endNode], segmentID, ref instance.m_segments.m_buffer[(int)segmentID], out endFlags, out endColor);
				}
				float startAngle2 = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleStart * 0.02454369f;
				float endAngle2 = (float)instance.m_segments.m_buffer[(int)segmentID].m_cornerAngleEnd * 0.02454369f;
				Vector4 dataVector = data.m_dataVector3;
				InfoManager.InfoMode currentMode = Singleton<InfoManager>.instance.CurrentMode;
				if (currentMode != InfoManager.InfoMode.None && !info.m_netAI.ColorizeProps(currentMode))
				{
					dataVector.z = 0f;
				}
				int num3 = (info.m_segments == null || info.m_segments.Length == 0) ? 0 : -1;
				uint num4 = instance.m_segments.m_buffer[(int)segmentID].m_lanes;
				int i = 0;
				while (i < info.m_lanes.Length && num4 != 0u)
				{
					instance.m_lanes.m_buffer[(int)((UIntPtr)num4)].RenderInstance(cameraInfo, segmentID, num4, info.m_lanes[i], startFlags, endFlags, startColor, endColor, startAngle2, endAngle2, invert2, layerMask, dataVector, ref data, ref num3);
					num4 = instance.m_lanes.m_buffer[(int)((UIntPtr)num4)].m_nextLane;
					i++;
				}
			}
		}
	}
}
