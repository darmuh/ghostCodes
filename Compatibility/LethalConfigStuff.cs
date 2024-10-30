using BepInEx.Configuration;
using ghostCodes.Configs;
using LethalConfig;
using System.Collections.Generic;
using static ghostCodes.Configs.InsanityConfig;
using static ghostCodes.Configs.SetupConfig;
using static OpenLib.ConfigManager.WebHelper;

namespace ghostCodes.Compatibility
{
    internal class LethalConfigStuff
    {
        internal static void AddLoadCodeButton()
        {
            if (!OpenLib.Plugin.instance.LethalConfig)
                return;
            Plugin.MoreLogs("AddLoadCodeButton called!");

            OpenLib.Compat.LethalConfigSoft.AddButton("General", "LoadCodes", "Click this to refresh all mod configs from code values assigned to [SetupConfigCode], [InsanityConfigCode], [SoloAssistConfigCode], [InteractionsConfigCode]", "Load Configs Codes", LoadCodes);
            OpenLib.Compat.LethalConfigSoft.AddButton("General", "GenerateWebPages", "Click this to generate web pages for all mod configs.\n These pages will be used to generate codes for [SetupConfigCode], [InsanityConfigCode], [SoloAssistConfigCode], and [InteractionsConfigCode]", "Generate Web Pages", GeneratePages);
        }

        internal static void GeneratePages()
        {
            WebConfig(Setup);
            WebConfig(InteractionsConfig.PrimaryInteractions);
            WebConfig(InteractionsConfig.SecondaryInteractions);
            WebConfig(InsanityMode);
            WebConfig(SoloAssistConfig.SoloAssist);
        }

        internal static void LoadCodes()
        {
            //code
            if (ModConfig.SetupConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.SetupConfigCode, Setup);
            if (ModConfig.PrimaryInteractionsConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.PrimaryInteractionsConfigCode, InteractionsConfig.PrimaryInteractions);
            if (ModConfig.SecondaryInteractionsConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.SecondaryInteractionsConfigCode, InteractionsConfig.SecondaryInteractions);
            if (ModConfig.InsanityConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.InsanityConfigCode, InsanityMode);
            if (ModConfig.SoloAssistConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.SoloAssistConfigCode, SoloAssistConfig.SoloAssist);

        }

        internal static void QueueAndLoad(List<ConfigFile> configList)
        {
            if (!OpenLib.Plugin.instance.LethalConfig)
                return;

            foreach (ConfigFile config in configList)
                LethalConfigManager.QueueCustomConfigFileForLateAutoGeneration(config);
            LethalConfigManager.RunLateAutoGeneration();
        }

        internal static void QueueAndLoad(ConfigFile config)
        {
            if (!OpenLib.Plugin.instance.LethalConfig)
                return;

            LethalConfigManager.QueueCustomConfigFileForLateAutoGeneration(config);
            LethalConfigManager.RunLateAutoGeneration();
        }
    }
}
