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
        static bool sm_localizationInitialized;
        static readonly string[] sm_collectionPrefixes = new string[] { "", "Europe " };
        static readonly string[] sm_thumbnailStates = new string[] { "", "Disabled", "Focused", "Hovered", "Pressed" };
        static readonly Dictionary<string, UI.UIUtils.SpriteTextureInfo> sm_thumbnailCoords = new Dictionary<string, UI.UIUtils.SpriteTextureInfo>()
        {
            /*            {"Small Busway", new UIUtils.SpriteTextureInfo() {width = 109, height = 75}},
                        {"Small Busway Decoration Grass", new UIUtils.SpriteTextureInfo() {startY = 75, width = 109, height = 75}},
                        {"Small Busway Decoration Trees", new UIUtils.SpriteTextureInfo() {startY = 150, width = 109, height = 75}},
                        {"Small Busway OneWay", new UIUtils.SpriteTextureInfo() {startY = 225, width = 109, height = 75}},
                        {"Small Busway OneWay Decoration Grass", new UIUtils.SpriteTextureInfo() {startY = 300, width = 109, height = 75}},
                        {"Small Busway OneWay Decoration Trees", new UIUtils.SpriteTextureInfo() {startY = 375, width = 109, height = 75}},
                        {"Large Road With Bus Lanes", new UIUtils.SpriteTextureInfo() {startY = 450, width = 109, height = 75}},
                        {"Large Road Decoration Grass With Bus Lanes", new UIUtils.SpriteTextureInfo() {startY = 525, width = 109, height = 75}},
                        {"Large Road Decoration Trees With Bus Lanes", new UIUtils.SpriteTextureInfo() {startY = 600, width = 109, height = 75}},
                        {"Zonable Pedestrian Pavement", new UIUtils.SpriteTextureInfo() {startY = 675, width = 109, height = 75}},
                        {"Zonable Pedestrian Gravel", new UIUtils.SpriteTextureInfo() {startY = 750, width = 109, height = 75}},
             */
        };
        public static Dictionary<string, TextureInfo> sm_fileIndex = new Dictionary<string, TextureInfo>();
        //{
        //	{"RoadLargeBusLanesTrees", new TextureInfo() {name = "RoadLargeBusLanesTrees", mainTex = "RoadLargeBusLanesGrass"}},
        //	{"RoadSmallBusway", new TextureInfo() {name = "RoadSmallBusway", mainTex = "RoadLargeBusLanesGrass"}},
        //	{"xsdaf", new TextureInfo() {name = "fdsfs", mainTex = "RoadLargeBusLanesGrass"}},
        //};
        //static readonly Dictionary<string, string> sm_fileIndex = new Dictionary<string, string>()
        //{
        //	{"RoadLargeBusLanesTrees",				"RoadLargeBusLanesGrass"},
        //	{"RoadLargeBusLanesTrees-bus",			"RoadLargeBusLanesGrass-bus"},
        //	{"RoadLargeBusLanesTrees-busBoth",		"RoadLargeBusLanesGrass-busBoth"},
        //	{"RoadLargeBusLanesElevated",			"RoadLargeBusLanesBridge"},

        //	{"RoadSmallBuswayElevated",				"RoadSmallBuswayBridge"},
        //	{"RoadSmallBuswayOneWayBridge",			"RoadSmallBuswayBridge"},
        //	{"RoadSmallBuswayOneWayElevated",		"RoadSmallBuswayBridge"},
        //	{"RoadSmallBusway-bus",					"RoadSmallBusway"},
        //	{"RoadSmallBusway-busBoth",				"RoadSmallBusway"},
        //	{"RoadSmallBuswayOneWay",				"RoadSmallBusway"},
        //	{"RoadSmallBuswayOneWay-bus",			"RoadSmallBusway"},
        //	{"RoadSmallBuswayOneWay-busBoth",		"RoadSmallBusway"},
        //	{"RoadSmallBuswayGrass-bus",			"RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayGrass-busBoth",		"RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayTrees",				"RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayTrees-bus",			"RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayTrees-busBoth",		"RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayOneWayGrass",			"RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayOneWayGrass-bus",      "RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayOneWayGrass-busBoth",  "RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayOneWayTrees",          "RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayOneWayTrees-bus",      "RoadSmallBuswayGrass"},
        //	{"RoadSmallBuswayOneWayTrees-busBoth",  "RoadSmallBuswayGrass"},
        //};
        //static readonly Dictionary<string, string> sm_lodFileIndex = new Dictionary<string, string>()
        //{
        //	{"RoadLargeBusLanesElevated",       "RoadLargeBusLanesBridge"},
        //	{"RoadSmallBuswayElevated",         "RoadSmallBuswayBridge"},
        //	{"RoadSmallBuswayOneWayBridge",     "RoadSmallBuswayBridge"},
        //	{"RoadSmallBuswayOneWayElevated",   "RoadSmallBuswayBridge"},
        //	{"RoadSmallBuswayOneWay",           "RoadSmallBusway"},
        //	{"RoadSmallBuswayOneWay-bus",       "RoadSmallBusway-bus"},
        //	{"RoadSmallBuswayOneWay-busBoth",   "RoadSmallBusway-busBoth"},
        //};

        Dictionary<string, NetLaneProps> m_customNetLaneProps;
        Dictionary<string, PrefabInfo> m_customPrefabs;
        Dictionary<string, Texture2D> m_customTextures;
        Dictionary<string, VehicleAI> m_replacedAIs;
        //Queue<Action> m_postLoadingActions;
        UITextureAtlas m_thumbnailsTextureAtlas;
        bool m_initialized;
        bool m_incompatibilityWarning;
        float m_gameStartedTime;
        int m_level;

        void Awake()
        {
            DontDestroyOnLoad(this);

            m_customNetLaneProps = new Dictionary<string, NetLaneProps>();
            m_customPrefabs = new Dictionary<string, PrefabInfo>();
            m_customTextures = new Dictionary<string, Texture2D>();
            m_replacedAIs = new Dictionary<string, VehicleAI>();
            //m_postLoadingActions = new Queue<Action>();

            LoadTextureIndex();
        }

        /*           void Start()
                   {
                       if ((RoadsUnitedModLoader.Options & OptionsManager.ModOptions.GhostMode) != OptionsManager.ModOptions.GhostMode)
                       {
                           ReplacePathManager();
                           ReplaceTransportManager();
                       }
       #if DEBUG
                       //StartCoroutine(Print());
       #endif
                   }
       */
        void OnLevelWasLoaded(int level)
        {
            this.m_level = level;

            if (level == 6)
            {
                Logger.LogInfo("Game level was loaded. Options enabled: \n\t" + RoadsUnitedMod.Options);

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

                m_customNetLaneProps.Clear();
                m_customPrefabs.Clear();
                m_replacedAIs.Clear();
                //m_postLoadingActions.Clear();
            }
        }

        public void OnLevelUnloading()
        {
            /*                if ((RoadsUnitedModLoader.Options & OptionsManager.ModOptions.UseRealisticSpeeds) == OptionsManager.ModOptions.UseRealisticSpeeds)
                            {
                                for (uint i = 0; i < PrefabCollection<CitizenInfo>.LoadedCount(); i++)
                                {
                                    CitizenInfo cit = PrefabCollection<CitizenInfo>.GetLoaded(i);
                                    cit.m_walkSpeed /= 0.25f;
                                }
                            }

                            for (uint i = 0; i < PrefabCollection<VehicleInfo>.LoadedCount(); i++)
                            {
                                //SetRealisitcSpeeds(PrefabCollection<VehicleInfo>.GetLoaded(i), false);
                                SetOriginalAI(PrefabCollection<VehicleInfo>.GetLoaded(i));
                            }
             */
        }

        void Update()
        {


            if ((RoadsUnitedMod.Options & OptionsManager.ModOptions.GhostMode) == OptionsManager.ModOptions.GhostMode)
                return;

            if (!Singleton<LoadingManager>.instance.m_loadingComplete)
                return;
            else if (m_gameStartedTime == 0f)
                m_gameStartedTime = Time.realtimeSinceStartup;

            //while (m_postLoadingActions.Count > 0)
            //	m_postLoadingActions.Dequeue().Invoke();

            // contributed by Japa




#if DEBUG
            if (Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                VehicleInfo vehicleInfo = null;
                Color color = default(Color);
                switch (count)
                {
                    case 0:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Lorry");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 1:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Bus");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 2:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Ambulance");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 3:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Police Car");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 4:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Fire Truck");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 5:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Hearse");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 6:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Garbage Truck");
                        color = vehicleInfo.m_material.color;
                        break;
                    case 7:
                        vehicleInfo = PrefabCollection<VehicleInfo>.FindLoaded("Sports-car");
                        color = Color.yellow;
                        break;
                    default:
                        break;
                }
                count = (count + 1) % 8;

                if (vehicleInfo == null)
                    Logger.LogInfo("Damn it!");
                else
                {
                    CreateVehicle(vehicleInfo.m_mesh, vehicleInfo.m_material, color);
                }
            }
#endif
        }

#if DEBUG
        int count = 0;
        GameObject vehicle;
        GameObject quad;
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

        void CreateVehicle(Mesh mesh, Material material, Color color)
        {
            if (vehicle != null)
                Destroy(vehicle);

            vehicle = new GameObject("Vehicle");
            vehicle.transform.position = new Vector3(0f, 131f, -10f);
            vehicle.transform.rotation = Quaternion.Euler(0f, 210f, 0f);
            MeshFilter mf = vehicle.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            MeshRenderer mr = vehicle.AddComponent<MeshRenderer>();
            material.color = color;
            mr.sharedMaterial = material;

            if (quad == null)
            {
                quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = new Vector3(0f, 130f, -10f);
                quad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                quad.transform.localScale = new Vector3(100, 100);
                quad.GetComponent<Renderer>().sharedMaterial.color = new Color(255f, 203f, 219f);
            }

            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            cameraController.m_targetPosition = new Vector3(0f, 139.775f, 0f);
            cameraController.m_targetSize = 40;
            cameraController.m_targetAngle = new Vector2(0f, 0f);
        }
#endif

        #region Initialization

        /*
         * In here I'm changing the prefabs to have my classes. This way, every time the game instantiates
         * a prefab that I've changed, that object will run my code.
         * The prefabs aren't available at the moment of creation of this class, that's why I keep trying to
         * run it on update. I want to make sure I make the switch as soon as they exist to prevent the game
         * from instantianting objects without my code.
         */

        //IEnumerator Print()
        //{
        //    yield return new WaitForSeconds(30f);

        //    foreach (var item in Resources.FindObjectsOfTypeAll<GameObject>().Except(GameObject.FindObjectsOfType<GameObject>()))
        //    {
        //        if (item.transform.parent == null)
        //            printGameObjects(item);
        //    }
        //}

        //void printGameObjects(GameObject go, int depth = 0)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < depth; i++)
        //    {
        //        sb.Append(">");
        //    }
        //    sb.Append("> ");
        //    sb.Append(go.name);
        //    sb.Append("\n");

        //    System.IO.File.AppendAllText("MapScenePrefabs.txt", sb.ToString());

        //    printComponents(go, depth);

        //    foreach (Transform t in go.transform)
        //    {
        //        printGameObjects(t.gameObject, depth + 1);
        //    }
        //}

        //void printComponents(GameObject go, int depth)
        //{
        //    foreach (var item in go.GetComponents<Component>())
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < depth; i++)
        //        {
        //            sb.Append(" ");
        //        }
        //        sb.Append("  -- ");
        //        sb.Append(item.GetType().Name);
        //        sb.Append("\n");

        //        System.IO.File.AppendAllText("MapScenePrefabs.txt", sb.ToString());
        //    }
        //}


        // Replace the pathfinding system for mine
        /*            void ReplacePathManager()
                    {
                        if (Singleton<PathManager>.instance as CustomPathManager != null)
                            return;

                        Logger.LogInfo("Replacing Path Manager");

                        // Change PathManager to CustomPathManager
                        FieldInfo sInstance = typeof(ColossalFramework.Singleton<PathManager>).GetFieldByName("sInstance");
                        PathManager originalPathManager = ColossalFramework.Singleton<PathManager>.instance;
                        CustomPathManager customPathManager = originalPathManager.gameObject.AddComponent<CustomPathManager>();
                        customPathManager.SetOriginalValues(originalPathManager);

                        // change the new instance in the singleton
                        sInstance.SetValue(null, customPathManager);

                        // change the manager in the SimulationManager
                        FastList<ISimulationManager> managers = (FastList<ISimulationManager>)typeof(SimulationManager).GetFieldByName("m_managers").GetValue(null);
                        managers.Remove(originalPathManager);
                        managers.Add(customPathManager);

                        // Destroy in 10 seconds to give time to all references to update to the new manager without crashing
                        GameObject.Destroy(originalPathManager, 10f);

                        Logger.LogInfo("Path Manager successfully replaced.");
                    }
        */
        /*           void ReplaceTransportManager()
                   {
                       if (Singleton<TransportManager>.instance as CustomTransportManager != null)
                           return;

                       Logger.LogInfo("Replacing Transport Manager");

                       // Change TransportManager to CustomTransportManager
                       FieldInfo sInstance = typeof(ColossalFramework.Singleton<TransportManager>).GetFieldByName("sInstance");
                       TransportManager originalTransportManager = ColossalFramework.Singleton<TransportManager>.instance;
                       CustomTransportManager customTransportManager = originalTransportManager.gameObject.AddComponent<CustomTransportManager>();
                       customTransportManager.SetOriginalValues(originalTransportManager);

                       // change the new instance in the singleton
                       sInstance.SetValue(null, customTransportManager);

                       // change the manager in the SimulationManager
                       FastList<ISimulationManager> managers = (FastList<ISimulationManager>)typeof(SimulationManager).GetFieldByName("m_managers").GetValue(null);
                       managers.Remove(originalTransportManager);
                       managers.Add(customTransportManager);

                       // add to renderable managers
                       IRenderableManager[] renderables;
                       int count;
                       RenderManager.GetManagers(out renderables, out count);
                       if (renderables != null && count != 0)
                       {
                           for (int i = 0; i < count; i++)
                           {
                               TransportManager temp = renderables[i] as TransportManager;
                               if (temp != null && temp == originalTransportManager)
                               {
                                   renderables[i] = customTransportManager;
                                   break;
                               }
                           }
                       }
                       else
                       {
                           RenderManager.RegisterRenderableManager(customTransportManager);
                       }

                       // Destroy in 10 seconds to give time to all references to update to the new manager without crashing
                       GameObject.Destroy(originalTransportManager, 10f);

                       Logger.LogInfo("Transport Manager successfully replaced.");
                   }
       */
        T TryGetComponent<T>(string name)
        {
            foreach (string prefix in sm_collectionPrefixes)
            {
                GameObject go = GameObject.Find(prefix + name);
                if (go != null)
                    return go.GetComponent<T>();
            }

            return default(T);
        }

        public static void QueuePrioritizedLoadingAction(Action action)
        {
            QueuePrioritizedLoadingAction(ActionWrapper(action));
        }

        public static void QueuePrioritizedLoadingAction(IEnumerator action)
        {
            while (!Monitor.TryEnter(sm_queueLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
            try
            {
                sm_actionQueue.Enqueue(action);
            }
            finally { Monitor.Exit(sm_queueLock); }
        }

        static void AddQueuedActionsToLoadingQueue()
        {
            LoadingManager loadingManager = Singleton<LoadingManager>.instance;
            object loadingLock = typeof(LoadingManager).GetFieldByName("m_loadingLock").GetValue(loadingManager);

            while (!Monitor.TryEnter(loadingLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
            try
            {
                FieldInfo mainThreadQueueField = typeof(LoadingManager).GetFieldByName("m_mainThreadQueue");
                Queue<IEnumerator> mainThreadQueue = (Queue<IEnumerator>)mainThreadQueueField.GetValue(loadingManager);
                if (mainThreadQueue != null)
                {
                    Queue<IEnumerator> newQueue = new Queue<IEnumerator>(mainThreadQueue.Count + 1);
                    newQueue.Enqueue(mainThreadQueue.Dequeue()); // currently running action must continue to be the first in the queue

                    while (!Monitor.TryEnter(sm_queueLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
                    try
                    {
                        while (sm_actionQueue.Count > 0)
                            newQueue.Enqueue(sm_actionQueue.Dequeue());
                    }
                    finally
                    {
                        Monitor.Exit(sm_queueLock);
                    }


                    while (mainThreadQueue.Count > 0)
                        newQueue.Enqueue(mainThreadQueue.Dequeue());

                    mainThreadQueueField.SetValue(loadingManager, newQueue);
                }
            }
            finally
            {
                Monitor.Exit(loadingLock);
            }
        }

        static IEnumerator ActionWrapper(Action a)
        {
            a.Invoke();
            yield break;
        }

        public static void QueueLoadingAction(Action action)
        {
            Singleton<LoadingManager>.instance.QueueLoadingAction(ActionWrapper(action));
        }

        public static void QueueLoadingAction(IEnumerator action)
        {
            Singleton<LoadingManager>.instance.QueueLoadingAction(action);
        }

        #endregion

        #region Clone Methods

        T ClonePrefab<T>(string prefabName, string newName, Transform customPrefabsHolder, bool replace = false, bool ghostMode = false) where T : PrefabInfo
        {
            T[] prefabs = Resources.FindObjectsOfTypeAll<T>();
            return ClonePrefab<T>(prefabName, prefabs, newName, customPrefabsHolder, replace, ghostMode);
        }

        T ClonePrefab<T>(string prefabName, T[] prefabs, string newName, Transform customPrefabsHolder, bool replace = false, bool ghostMode = false) where T : PrefabInfo
        {
            T originalPrefab = prefabs.FirstOrDefault(p => p.name == prefabName);
            if (originalPrefab == null)
                return null;

            GameObject instance = GameObject.Instantiate<GameObject>(originalPrefab.gameObject);
            instance.name = newName;
            instance.transform.SetParent(customPrefabsHolder);
            instance.transform.localPosition = new Vector3(-7500, -7500, -7500);
            T newPrefab = instance.GetComponent<T>();
            //instance.SetActive(false);

            MethodInfo initMethod = GetCollectionType(typeof(T).Name).GetMethod("InitializePrefabs", BindingFlags.Static | BindingFlags.NonPublic);
            Initializer.QueuePrioritizedLoadingAction((IEnumerator)initMethod.Invoke(null, new object[] { newName, new[] { newPrefab }, new string[] { replace ? prefabName : null } }));

            if (ghostMode)
            {
                if (newPrefab.GetType() == typeof(NetInfo))
                    (newPrefab as NetInfo).m_availableIn = ItemClass.Availability.None;
                this.m_customPrefabs.Add(newName, originalPrefab);
                return null;
            }

            newPrefab.m_prefabInitialized = false;

            return newPrefab;
        }

        NetInfo CloneRoad(string prefabName, string newName, RoadType roadType, NetCollection collection, FileManager.Folder folder = FileManager.Folder.Roads)
        {
            bool ghostMode = (RoadsUnitedMod.Options & (OptionsManager.ModOptions.GhostMode | OptionsManager.ModOptions.DisableCustomRoads)) != OptionsManager.ModOptions.None;
            NetInfo road = ClonePrefab<NetInfo>(prefabName + GetDecoratedName(roadType & ~RoadType.OneWay), collection.m_prefabs, newName, transform, false, ghostMode);
            if (road == null)
                return null;

            // Replace textures

            if (m_thumbnailsTextureAtlas != null && SetThumbnails(newName))
            {
                road.m_Atlas = m_thumbnailsTextureAtlas;
                road.m_Thumbnail = newName;
            }

            TextureInfo textureInfo;
            if (!sm_fileIndex.TryGetValue(newName, out textureInfo))
                return road;

            for (int i = 0; i < road.m_segments.Length; i++)
            {
                // FIXME: handle different kind of segments that shouldn't be touched.
                if (roadType.HasFlag(RoadType.Bridge) && i != 0)
                    break;

                TextureType textureType = TextureType.Normal;
                if ((roadType & (RoadType.Bridge | RoadType.Elevated | RoadType.Tunnel | RoadType.Slope)) == RoadType.Normal)
                {
                    if (i == 1) textureType = TextureType.Bus;
                    if (i == 2) textureType = TextureType.BusBoth;
                }

                road.m_segments[i].m_material = new Material(road.m_segments[i].m_material);
                ReplaceTextures(textureInfo, textureType, folder, road.m_segments[i].m_material);

                road.m_segments[i].m_lodMaterial = new Material(road.m_segments[i].m_lodMaterial);
                ReplaceTextures(textureInfo, textureType | TextureType.LOD, folder, road.m_segments[i].m_lodMaterial);
            }

            for (int i = 0; i < road.m_nodes.Length; i++)
            {
                // FIXME: handle different kind of nodes that shouldn't be touched.
                if (newName.Contains("Pedestrian") && i != 0)
                    break;

                road.m_nodes[i].m_material = new Material(road.m_nodes[i].m_material);
                ReplaceTextures(textureInfo, TextureType.Node, folder, road.m_nodes[i].m_material);

                road.m_nodes[i].m_lodMaterial = new Material(road.m_nodes[i].m_lodMaterial);
                ReplaceTextures(textureInfo, TextureType.NodeLOD, folder, road.m_nodes[i].m_lodMaterial);
            }

            return road;
        }

        /*           static NetLaneProps CloneNetLaneProps(string prefabName, int deltaSpace = 0)
                   {
                       NetLaneProps prefab = Resources.FindObjectsOfTypeAll<NetLaneProps>().FirstOrDefault(p => p.name == prefabName);
                       if (prefab == null)
                           return null;

                       NetLaneProps newLaneProps = ScriptableObject.CreateInstance<NetLaneProps>();
                       newLaneProps.m_props = new NetLaneProps.Prop[Mathf.Max(0, prefab.m_props.Length + deltaSpace)];
                       Array.Copy(prefab.m_props, newLaneProps.m_props, Mathf.Min(newLaneProps.m_props.Length, prefab.m_props.Length));

                       return newLaneProps;
                   }
       */
        static Type GetCollectionType(string prefabType)
        {
            switch (prefabType)
            {
                case "NetInfo":
                    return typeof(NetCollection);
                case "PropInfo":
                    return typeof(PropCollection);
                default:
                    return null;
            }
        }

        // whitespaces is for prefab names, no whitespaces is for texture file names
                   static string GetDecoratedName(RoadType roadType, bool whiteSpaces = true)
                   {
                       StringBuilder sb = new StringBuilder();

                       if (roadType.HasFlag(RoadType.OneWay))
                           sb.Append(whiteSpaces ? " OneWay" : "OneWay");

                       if (roadType.HasFlag(RoadType.Elevated))
                           sb.Append(whiteSpaces ? " Elevated" : "Elevated");
                       else if (roadType.HasFlag(RoadType.Bridge))
                           sb.Append(whiteSpaces ? " Bridge" : "Bridge");
                       else if (roadType.HasFlag(RoadType.Slope))
                           sb.Append(whiteSpaces ? " Slope" : "Slope");
                       else if (roadType.HasFlag(RoadType.Tunnel))
                           sb.Append(whiteSpaces ? " Tunnel" : "Tunnel");

                       if (roadType.HasFlag(RoadType.Grass))
                           sb.Append(whiteSpaces ? " Decoration Grass" : "Grass");
                       else if (roadType.HasFlag(RoadType.Trees))
                           sb.Append(whiteSpaces ? " Decoration Trees" : "Trees");

                       return sb.ToString();
                   }
       
        #endregion




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

        bool SetThumbnails(string name)
        {
            if (m_thumbnailsTextureAtlas == null || !sm_thumbnailCoords.ContainsKey(name))
                return false;

            return UI.UIUtils.SetThumbnails(name, sm_thumbnailCoords[name], m_thumbnailsTextureAtlas, sm_thumbnailStates);
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

#if DEBUG
        #region SceneInspectionTools

        IEnumerator Print()
        {
            while (!LoadingManager.instance.m_loadingComplete)
                yield return new WaitForEndOfFrame();

            List<GameObject> sceneObjects = GameObject.FindObjectsOfType<GameObject>().ToList();
            foreach (var item in sceneObjects)
            {
                if (item.transform.parent == null)
                    PrintGameObjects(item, "MapScene_110b.txt");
            }

            List<GameObject> prefabs = Resources.FindObjectsOfTypeAll<GameObject>().Except(sceneObjects).ToList();
            foreach (var item in prefabs)
            {
                if (item.transform.parent == null)
                    PrintGameObjects(item, "MapScenePrefabs_110b.txt");
            }
        }

        public static void PrintGameObjects(GameObject go, string fileName, int depth = 0)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }

            sb.Append(go.name);
            sb.Append(" {\n");

            System.IO.File.AppendAllText(fileName, sb.ToString());

            PrintComponents(go, fileName, depth);

            foreach (Transform t in go.transform)
            {
                PrintGameObjects(t.gameObject, fileName, depth + 1);
            }

            sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }
            sb.Append("}\n\n");
            System.IO.File.AppendAllText(fileName, sb.ToString());
        }

        public static void PrintComponents(GameObject go, string fileName, int depth)
        {
            foreach (var item in go.GetComponents<Component>())
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < depth; i++)
                {
                    sb.Append("\t");
                }
                sb.Append("\t-- ");
                sb.Append(item.GetType().Name);
                sb.Append("\n");

                System.IO.File.AppendAllText(fileName, sb.ToString());
            }
        }

        #endregion
#endif

        void LoadTextureIndex()
        {
            TextureInfo[] textureIndex = FileManager.GetTextureIndex();
            if (textureIndex == null)
                return;

            sm_fileIndex.Clear();
            foreach (TextureInfo item in textureIndex)
                sm_fileIndex.Add(item.name, item);
        }

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