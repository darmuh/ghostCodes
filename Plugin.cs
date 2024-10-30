using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;


namespace ghostCodes
{
    [BepInPlugin("darmuh.ghostCodes", "ghostCodes", "1.5.1")]

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static class PluginInfo
        {
            public const string PLUGIN_GUID = "darmuh.ghostCodes";
            public const string PLUGIN_NAME = "ghostCodes";
            public const string PLUGIN_VERSION = "1.5.1";
        }

        internal static new ManualLogSource GC;

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

        public static bool hauntStare = false;
    }
}
