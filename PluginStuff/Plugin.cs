using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using ghostCodes.Configs;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace ghostCodes
{
    [BepInPlugin("darmuh.ghostCodes", "ghostCodes", (PluginInfo.PLUGIN_VERSION))]
    [BepInDependency("darmuh.OpenLib", "0.2.5")] //hard dependency for my library

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static class PluginInfo
        {
            public const string PLUGIN_GUID = "darmuh.ghostCodes";
            public const string PLUGIN_NAME = "ghostCodes";
            public const string PLUGIN_VERSION = "2.5.1";
        }

        internal static new ManualLogSource GC;

        //variables
        public bool CodeSent { get; internal set; } = false;
        public float GroupSanity { get; internal set; } = 0f;
        public float MaxSanity { get; internal set; } = 0f;
        internal int playersAtStart = 0;
        public int CodeCount { get; internal set; } = 0;
        public int RandCodeAmount { get; internal set; } = 0;
        internal PlayerControllerB[] players;

        //Terminal Instance
        internal Terminal Terminal;

        //Dressgirl Instance
        public DressGirlAI DressGirl;

        //Cruiser Instance
        public VehicleController Cruiser;

        //Compatibility Stuff
        internal bool FacilityMeltdown = false;
        internal bool ToilHead = false;

        private void Awake()
        {
            Plugin.instance = this;
            Plugin.GC = base.Logger;
            Plugin.GC.LogInfo($"{PluginInfo.PLUGIN_NAME} have been detected in the terminal, version {PluginInfo.PLUGIN_VERSION}");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            ModConfig.SetConfigSettings();
            Patches.OpenLibEvents.Subscribers();
            //start of networking stuff

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            //end of networking stuff
        }

        public static void MoreLogs(string message)
        {
            if (ModConfig.ExtensiveLogging.Value)
                GC.LogDebug(message);
            else
                return;
        }

        public static void Spam(string log)
        {
            MoreLogs(log);
        }

        public static void WARNING(string warning)
        {
            GC.LogWarning(warning);
        }

        public static void ERROR(string error)
        {
            GC.LogError(error);
        }

    }
}
