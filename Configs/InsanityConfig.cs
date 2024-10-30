using BepInEx;
using BepInEx.Configuration;
using System.IO;
using static OpenLib.ConfigManager.ConfigSetup;
using static OpenLib.ConfigManager.WebHelper;

namespace ghostCodes.Configs
{
    public static class InsanityConfig
    {
        public static ConfigFile InsanityMode = new(Path.Combine(Paths.ConfigPath, $"darmuh.ghostCodes.InsanityMode.cfg"), true);

        // General //
        public static ConfigEntry<float> InsanityModeMultiplier { get; internal set; }

        // Values //
        public static ConfigEntry<int> SanityLevel1 { get; internal set; }
        public static ConfigEntry<int> SanityLevel2 { get; internal set; }
        public static ConfigEntry<int> SanityLevel3 { get; internal set; }
        public static ConfigEntry<int> SanityMaxLevel { get; internal set; }
        public static ConfigEntry<int> WaitLevel1 { get; internal set; }
        public static ConfigEntry<int> WaitLevel2 { get; internal set; }
        public static ConfigEntry<int> WaitLevel3 { get; internal set; }
        public static ConfigEntry<int> WaitMaxLevel { get; internal set; }

        // Bonuses //
        public static ConfigEntry<int> DeathBonus { get; internal set; }
        public static ConfigEntry<int> GhostGirlBonus { get; internal set; }
        public static ConfigEntry<int> EmoteBuff { get; internal set; }

        internal static void Init()
        {
            InsanityModeMultiplier = MakeClampedFloat(InsanityMode, "General", "InsanityModeMultiplier", 1.5f, "Multiplier for all interaction chances when at max insanity while insanity mode is active", 0.2f, 3f);

            SanityLevel1 = MakeClampedInt(InsanityMode, "Values", "SanityLevel1", 25, "Set the percentage of the maximum sanity level required to reach Level 1 of Insanity Mode.", 0, 100);
            SanityLevel2 = MakeClampedInt(InsanityMode, "Values", "SanityLevel2", 50, "Set the percentage of the maximum sanity level required to reach Level 2 of Insanity Mode.", 0, 100);
            SanityLevel3 = MakeClampedInt(InsanityMode, "Values", "SanityLevel3", 75, "Set the percentage of the maximum sanity level required to reach Level 3 of Insanity Mode.", 0, 100);
            SanityMaxLevel = MakeClampedInt(InsanityMode, "Values", "SanityMaxLevel", 95, "Set the percentage of the maximum sanity level required to reach MAX Level of Insanity Mode and trigger rapid fire.", 0, 100);
            WaitLevel1 = MakeClampedInt(InsanityMode, "Values", "WaitLevel1", 90, "Set the percentage of the wait time to use after reaching Level 1 of Insanity Mode.", 0, 100);
            WaitLevel2 = MakeClampedInt(InsanityMode, "Values", "WaitLevel2", 50, "Set the percentage of the wait time to use after reaching Level 2 of Insanity Mode.", 0, 100);
            WaitLevel3 = MakeClampedInt(InsanityMode, "Values", "WaitLevel3", 10, "Set the percentage of the wait time to use after reaching Level 3 of Insanity Mode.", 0, 100);
            WaitMaxLevel = MakeClampedInt(InsanityMode, "Values", "WaitMaxLevel", 2, "Set the percentage of the wait time to use after reaching Max Level of Insanity Mode. (This is only triggered if rapidFire is disabled)", 0, 100);

            DeathBonus = MakeClampedInt(InsanityMode, "Bonuses", "DeathBonus", 10, "Percentage of current group insanity value that will be added for each dead player.", 0, 100);
            GhostGirlBonus = MakeClampedInt(InsanityMode, "Bonuses", "GhostGirlBonus", 10, "Percentage of current group insanity value to increase for the group once a ghostGirl has been spawned.", 0, 100);
            EmoteBuff = MakeClampedInt(InsanityMode, "Bonuses", "EmoteBuff", 25, "Percentage of current group insanity value to decrease from the group for each person that is emoting.", 0, 100);

            InsanityMode.Save();
            ModConfig.configFiles.Add(InsanityMode);
            //WebConfig(InsanityMode);
            if (ModConfig.InsanityConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.InsanityConfigCode, InsanityMode);
            Plugin.GC.LogInfo("ghostCodes insanity config initialized");
        }

    }
}
