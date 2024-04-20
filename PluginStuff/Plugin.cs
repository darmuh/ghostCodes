using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace ghostCodes
{
    [BepInPlugin("darmuh.ghostCodes", "ghostCodes", "2.0.4")]

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static class PluginInfo
        {
            public const string PLUGIN_GUID = "darmuh.ghostCodes";
            public const string PLUGIN_NAME = "ghostCodes";
            public const string PLUGIN_VERSION = "2.0.4";
        }

        internal static new ManualLogSource GC;

        //variables
        
        public bool bypassGGE { get; internal set; } = false;
        public bool ghostCodeSent { get; internal set; } = false;
        public float groupSanity { get; internal set; } = 0f;
        public float maxSanity { get; internal set; } = 0f;
        internal int playersAtStart = 0;
        public int codeCount { get; internal set; } = 0;
        public int randGC { get; internal set; } = 0;
        internal PlayerControllerB[] players;

        //Terminal Instance
        internal Terminal Terminal;

        //Teleporter Instances
        internal ShipTeleporter NormalTP;
        internal ShipTeleporter InverseTP;

        //Dressgirl Instance
        public DressGirlAI DressGirl;

        //Compatibility Stuff
        internal bool facilityMeltdown = false;
        internal bool toilHead = false;

        private void Awake()
        {
            Plugin.instance = this;
            Plugin.GC = base.Logger;
            Plugin.GC.LogInfo((object)$"{PluginInfo.PLUGIN_NAME} have been detected in the terminal, version {PluginInfo.PLUGIN_VERSION}");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            ModConfig.SetConfigSettings();


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
            if (ModConfig.extensiveLogging.Value)
                Plugin.GC.LogInfo(message);
            else
                return;
        }
    }
}
