using BepInEx.Configuration;
using System.Collections.Generic;
using static OpenLib.ConfigManager.ConfigSetup;

namespace ghostCodes.Configs
{
    public static class ModConfig
    {
        public static List<ConfigFile> configFiles = [];
        public static ConfigEntry<bool> ModNetworking { get; internal set; } //For users who only want to use this as the host
        public static ConfigEntry<bool> ExtensiveLogging { get; internal set; } //enable extensive logging messages\
        internal static ConfigEntry<string> SetupConfigCode;
        internal static ConfigEntry<string> InsanityConfigCode;
        internal static ConfigEntry<string> SoloAssistConfigCode;
        internal static ConfigEntry<string> InteractionsConfigCode;

        public static void SetConfigSettings()
        {

            // NETWORKING
            ModNetworking = MakeBool(Plugin.instance.Config, "__NETWORKING", "ModNetworking", true, "Disable this if you want to disable networking and use this mod as a Host-Only mod. This feature will completely disable GhostGirl Enhanced Mode.");

            // General
            ExtensiveLogging = MakeBool(Plugin.instance.Config, "General", "ExtensiveLogging", false, "Enable or Disable extensive log messages for this mod.");

            SetupConfigCode = MakeString(Plugin.instance.Config, "General", "SetupConfigCode", "", "Paste a config code here from the setupconfig generation page");
            InsanityConfigCode = MakeString(Plugin.instance.Config, "General", "InsanityConfigCode", "", "Paste a config code here from the insanityconfig generation page");
            SoloAssistConfigCode = MakeString(Plugin.instance.Config, "General", "SoloAssistConfigCode", "", "Paste a config code here from the soloassistconfig generation page");
            InteractionsConfigCode = MakeString(Plugin.instance.Config, "General", "InteractionsConfigCode", "", "Paste a config code here from the interactionsconfig generation page");


            Plugin.GC.LogInfo("ghostCodes main config initialized");
            SetupConfig.Init();
            InteractionsConfig.Init();
            InsanityConfig.Init();
            SoloAssistConfig.Init();
            
            
        }
    }
}
