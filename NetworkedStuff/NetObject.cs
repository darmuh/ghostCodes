﻿using HarmonyLib;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

namespace ghostCodes
{
    [HarmonyPatch]
    public class NetObject
    {

        [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Start))]
        public static void Init()
        {
            if (ModConfig.ModNetworking.Value)
            {
                if (networkPrefab != null)
                    return;

                var MainAssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ghostCodes.ghostngo"));
                networkPrefab = (GameObject)MainAssetBundle.LoadAsset("ghostngo");
                networkPrefab.AddComponent<NetHandler>();

                NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
            }
            
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
        static void SpawnNetworkHandler()
        {
            if (ModConfig.ModNetworking.Value)
            {
                if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
                {
                    var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
                    networkHandlerHost.GetComponent<NetworkObject>().Spawn();
                }
            }
           
        }

        static GameObject networkPrefab;
    }
}