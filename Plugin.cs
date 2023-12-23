using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;


namespace ghostCodes
{
    [BepInPlugin("darmuh.ghostCodes", "ghostCodes", "1.0.0")]

    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static class PluginInfo
        {
            public const string PLUGIN_GUID = "darmuh.ghostCodes";
            public const string PLUGIN_NAME = "ghostCodes";
            public const string PLUGIN_VERSION = "1.0.0";
        }

        internal static new ManualLogSource GC;


        private void Awake()
        {
            Plugin.instance = this;
            Plugin.GC = base.Logger;
            Plugin.GC.LogInfo((object)"ghostCodes have been detected in the terminal");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            gcConfig.SetConfigSettings();
        }
    }
}
