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
        internal static ConfigEntry<string> PrimaryInteractionsConfigCode;
        internal static ConfigEntry<string> SecondaryInteractionsConfigCode;

        public static void SetConfigSettings()
        {

            // NETWORKING
            ModNetworking = MakeBool(Plugin.instance.Config, "__NETWORKING", "ModNetworking", true, "Disable this if you want to disable networking and use this mod as a Host-Only mod. This feature will completely disable GhostGirl Enhanced Mode.");

            // General
            ExtensiveLogging = MakeBool(Plugin.instance.Config, "General", "ExtensiveLogging", true, "Enable or Disable extensive log messages for this mod.");

            SetupConfigCode = MakeString(Plugin.instance.Config, "General", "SetupConfigCode", "", "Paste a config code here from the setupconfig generation page");
            InsanityConfigCode = MakeString(Plugin.instance.Config, "General", "InsanityConfigCode", "", "Paste a config code here from the insanityconfig generation page");
            SoloAssistConfigCode = MakeString(Plugin.instance.Config, "General", "SoloAssistConfigCode", "", "Paste a config code here from the soloassistconfig generation page");
            PrimaryInteractionsConfigCode = MakeString(Plugin.instance.Config, "General", "PrimaryInteractionsConfigCode", "", "Paste a config code here from the PrimaryInteractionsConfig generation page");
            SecondaryInteractionsConfigCode = MakeString(Plugin.instance.Config, "General", "SecondaryInteractionsConfigCode", "", "Paste a config code here from the SecondaryInteractionsConfig generation page");

            Plugin.GC.LogInfo("ghostCodes main config initialized");
            SetupConfig.Init();
            InteractionsConfig.SetConfig(InteractionsConfig.PrimaryInteractions);
            if (SetupConfig.UseSecondaryInteractionsConfig.Value)
                InteractionsConfig.SetConfig(InteractionsConfig.SecondaryInteractions, " (Secondary)");
            InsanityConfig.Init();
            SoloAssistConfig.Init();

            SetupConfig.GhostCodesSettings = new(SetupConfig.PrimaryMode.Value, SetupConfig.SecondaryMode.Value, SetupConfig.SecondaryModeLevels.Value);
            InteractionsConfig.MapToConfig(InteractionsConfig.PrimaryInteractions, InteractionsConfig.Settings);
            Plugin.instance.Config.SettingChanged += OnSettingChanged;
        }

        internal static void OnSettingChanged(object sender, SettingChangedEventArgs settingChangedArg)
        {
            if (settingChangedArg.ChangedSetting == ModNetworking)
            {
                if ((bool)settingChangedArg.ChangedSetting.BoxedValue && StartOfRound.Instance == null)
                {
                    Plugin.Spam("Networking detected enabled in lobby!");
                    NetObject.Init();
                    InteractionsConfig.MapToConfig(InteractionsConfig.PrimaryInteractions, InteractionsConfig.Settings);
                }
                else if (!(bool)settingChangedArg.ChangedSetting.BoxedValue && StartOfRound.Instance == null)
                {
                    NetObject.DestroyAnyNetworking();
                    Plugin.Spam("Networking detected disabled in lobby!");
                    InteractionsConfig.MapToConfig(InteractionsConfig.PrimaryInteractions, InteractionsConfig.Settings);
                }
            }
        }
    }
}
