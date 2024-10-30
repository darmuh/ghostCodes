using BepInEx;
using BepInEx.Configuration;
using System.IO;
using static OpenLib.ConfigManager.ConfigSetup;
using static OpenLib.ConfigManager.WebHelper;

namespace ghostCodes.Configs
{
    public static class SoloAssistConfig
    {
        public static ConfigFile SoloAssist = new(Path.Combine(Paths.ConfigPath, $"ghostCodes.SoloAssist.cfg"), true);

        // Stage 1 //
        public static ConfigEntry<int> S1percent { get; internal set; }
        public static ConfigEntry<int> S1inside { get; internal set; }
        public static ConfigEntry<int> S1outside { get; internal set; }

        // Stage 2 //
        public static ConfigEntry<int> S2percent { get; internal set; }
        public static ConfigEntry<int> S2inside { get; internal set; }
        public static ConfigEntry<int> S2outside { get; internal set; }

        // Stage 3 //

        public static ConfigEntry<int> S3percent { get; internal set; }
        public static ConfigEntry<int> S3inside { get; internal set; }
        public static ConfigEntry<int> S3outside { get; internal set; }

        internal static void Init()
        {
            S1percent = MakeClampedInt(SoloAssist, "Solo Stage 1", "S1percent", 50, "Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 1 (beginning of day to Noon)", 0, 100);
            S1inside = MakeClampedInt(SoloAssist, "Solo Stage 1", "S1inside", 20, "Decrease of insanity level when inside the factory during Stage 1 (beginning of day to Noon)", 0, 40);
            S1outside = MakeClampedInt(SoloAssist, "Solo Stage 1", "S1outside", 25, "Decrease of insanity level when outside the factory during Stage 1 (beginning of day to Noon)", 0, 40);
            S2percent = MakeClampedInt(SoloAssist, "Solo Stage 2", "S2percent", 75, "Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 2 (Noon to 5PM)", 0, 100);
            S2inside = MakeClampedInt(SoloAssist, "Solo Stage 2", "S2inside", 10, "Decrease of insanity level when inside the factory during Stage 2 (Noon to 5PM)", 0, 40);
            S2outside = MakeClampedInt(SoloAssist, "Solo Stage 2", "S2outside", 15, "Decrease of insanity level when outside the factory during Stage 2 (Noon to 5PM)", 0, 40);
            S3percent = MakeClampedInt(SoloAssist, "Solo Stage 3", "S3percent", 90, "Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 3 (5PM - 10PM)", 0, 100);
            S3inside = MakeClampedInt(SoloAssist, "Solo Stage 3", "S3inside", 5, "Decrease of insanity level when inside the factory during Stage 3 (5PM - 10PM)", 0, 40);
            S3outside = MakeClampedInt(SoloAssist, "Solo Stage 3", "S3outside", 10, "Decrease of insanity level when outside the factory during Stage 3 (5PM - 10PM)", 0, 40);

            SoloAssist.Save();
            ModConfig.configFiles.Add(SoloAssist);
            //WebConfig(SoloAssist);
            if (ModConfig.SoloAssistConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.SoloAssistConfigCode, SoloAssist);
            Plugin.GC.LogInfo("ghostCodes solo-assist config initialized");
        }
    }
}
