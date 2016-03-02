﻿using System;
using System.Reflection;
using UnityEngine;

namespace RoadsUnited.Hook
{
    public class NetInfoHook : NetInfo
    {
        private static bool deployed = false;

        private static RedirectCallsState _InitializePrefab_state;
        private static MethodInfo _InitializePrefab_original;
        private static MethodInfo _InitializePrefab_detour;

        public static event PrefabEventHandler<NetInfo> OnPreInitialization;
        public static event PrefabEventHandler<NetInfo> OnPostInitialization;

        public static void Deploy()
        {
            if (!deployed && (OnPreInitialization != null || OnPostInitialization != null))
            {
                _InitializePrefab_original = typeof(NetInfo).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
                _InitializePrefab_detour = typeof(NetInfoHook).GetMethod("InitializePrefab", BindingFlags.Instance | BindingFlags.Public);
                _InitializePrefab_state = RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);

                deployed = true;

                Debug.LogFormat("PrefabHook: {0} Methods detoured!", "NetInfo");
            }
        }

        public static void Revert()
        {
            if (deployed)
            {
                RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
                _InitializePrefab_original = null;
                _InitializePrefab_detour = null;

                OnPreInitialization = null;
                OnPostInitialization = null;

                deployed = false;

                Debug.LogFormat("PrefabHook: {0} Methods restored!", "NetInfo");
            }
        }

        public new virtual void InitializePrefab()
        {
            if (OnPreInitialization != null)
            {
                try
                {
                    OnPreInitialization(base.GetComponent<NetInfo>());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            RedirectionHelper.RevertRedirect(_InitializePrefab_original, _InitializePrefab_state);
            base.InitializePrefab();
            RedirectionHelper.RedirectCalls(_InitializePrefab_original, _InitializePrefab_detour);

            if (OnPostInitialization != null)
            {
                try
                {
                    OnPostInitialization(base.GetComponent<NetInfo>());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}