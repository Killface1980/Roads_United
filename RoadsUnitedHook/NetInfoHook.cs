using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace RoadsUnited.Hook
{
    public class NetInfoHook : NetInfo
    {
        public bool hookEnabled = false;

        private Dictionary<MethodInfo, RedirectCallsState> redirects = new Dictionary<MethodInfo, RedirectCallsState>();

        private static RedirectCallsState _InitializePrefab_state;
        private static MethodInfo _InitializePrefab_original;
        private static MethodInfo _InitializePrefab_detour;


        public static Material invertedBridgeMat;

        public void Update()
        {
            if (!hookEnabled)
            {
                EnableHook();
            }
        }

        public void EnableHook()
        {
            _InitializePrefab_original = typeof(NetInfo).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
            _InitializePrefab_detour = typeof(NetInfoHook).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
            _InitializePrefab_state = RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);
            hookEnabled = true;
            Debug.LogFormat("PrefabHook: {0} Methods detoured!", "NetInfo");

        }

        public void DisableHook()
        {
            if (this.hookEnabled)
            {
                RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
                _InitializePrefab_original = null;
                _InitializePrefab_detour = null;



                hookEnabled = false;

                Debug.LogFormat("PrefabHook: {0} Methods restored!", "NetInfo");

            }
        }

        public new virtual void InitializePrefab()
        {
            GetComponent<NetInfo>();

            RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
            base.InitializePrefab();
            RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);


        }
    }
}
