using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace ghostCodes
{
    [BepInPlugin("darmuh.ghostCodes", "ghostCodes", "2.0.0")]

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static class PluginInfo
        {
            public const string PLUGIN_GUID = "darmuh.ghostCodes";
            public const string PLUGIN_NAME = "ghostCodes";
            public const string PLUGIN_VERSION = "2.0.0";
        }

        internal static new ManualLogSource GC;

        //variables
        internal bool facilityMeltdown = false;
        internal bool bypassGGE = false;
        internal bool ghostCodeSent = false;
        internal float groupSanity = 0f;
        internal float maxSanity = 0f;
        internal int playersAtStart = 0;
        internal int codeCount = 0;
        internal int randGC = 0;
        internal PlayerControllerB[] players;

        //Terminal Instance
        internal Terminal Terminal;

        //Dressgirl Instance
        public DressGirlAI DressGirl;

        private void Awake()
        {
            Plugin.instance = this;
            Plugin.GC = base.Logger;
            Plugin.GC.LogInfo((object)"ghostCodes have been detected in the terminal");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            gcConfig.SetConfigSettings();


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
            if (gcConfig.extensiveLogging.Value)
                Plugin.GC.LogInfo(message);
            else
                return;
        }
    }
}
