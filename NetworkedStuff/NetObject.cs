using ghostCodes.Configs;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

namespace ghostCodes
{
    internal class NetObject
    {
        internal static void Init()
        {
            if (!ModConfig.ModNetworking.Value)
                return;

            if (networkPrefab != null)
                return;

            Plugin.Spam("Assets:");
            string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string name in names)
                Plugin.Spam(name);

            var MainAssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ghostCodes.ghostngo"));
            networkPrefab = (GameObject)MainAssetBundle.LoadAsset("ghostngo");
            networkPrefab.AddComponent<NetHandler>();

            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);

        }

        internal static void SpawnNetworkHandler()
        {
            if (!ModConfig.ModNetworking.Value)
                return;

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
                networkHandlerHost.GetComponent<NetworkObject>().Spawn();
            }

        }

        internal static void DestroyAnyNetworking()
        {
            if (networkPrefab != null)
            {
                NetworkManager.Singleton.RemoveNetworkPrefab(networkPrefab);
                Object.Destroy(networkPrefab);
            }
        }

        internal static bool NetObjectExists()
        {
            if (networkPrefab != null)
                return true;
            else
                return false;
        }

        static GameObject networkPrefab;
    }
}