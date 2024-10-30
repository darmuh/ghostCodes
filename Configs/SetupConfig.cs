using BepInEx;
using BepInEx.Configuration;
using ghostCodes.PluginStuff;
using System.IO;
using static OpenLib.ConfigManager.ConfigSetup;
using static OpenLib.ConfigManager.WebHelper;


namespace ghostCodes.Configs
{
    public static class SetupConfig
    {
        public static ConfigFile Setup = new(Path.Combine(Paths.ConfigPath, $"darmuh.ghostCodes.Setup.cfg"), true);
        public static ModeSettings GhostCodesSettings { get; internal set; }

        // Mode Settings
        public static ConfigEntry<string> PrimaryMode { get; internal set; } //Primary Mode used for this mod
        public static ConfigEntry<string> SecondaryMode { get; internal set; } //Secondary Mode used for this mod
        public static ConfigEntry<string> SecondaryModeLevels { get; internal set; } //List of Levels to force Secondary Mode
        public static ConfigEntry<bool> RandomIntervals { get; internal set; } //Toggle whether the intervals between codes are random or set intervals
        public static ConfigEntry<int> RapidFireMaxHours { get; internal set; } //adds time limit to rapidFire during insanity mode, time limit variable
        public static ConfigEntry<bool> SoloAssist { get; internal set; } //adds time limit to rapidFire during insanity mode, time limit variable


        //Code Settings
        public static ConfigEntry<int> MinCodes { get; internal set; } //Minimum amount of codes to send in one round
        public static ConfigEntry<int> MaxCodes { get; internal set; } //Maximum amount of codes to send in one round
        public static ConfigEntry<bool> HauntingsCountCodes { get; internal set; } //Determine whether hauntings can be affected by code count

        //Misc
        public static ConfigEntry<bool> FixGhostGirlBreakers { get; internal set; } // enable/disable the ghostgirl code from vanilla
        public static ConfigEntry<int> VanillaBreakersChance { get; internal set; } //percentage chance for FixGhostGirlBreakers

        public static ConfigEntry<bool> IgnoreLandmines { get; internal set; } //Toggle whether ghostCodes ignore landmines
        public static ConfigEntry<bool> IgnoreTurrets { get; internal set; } //Toggle whether ghostCodes ignore turrets
        public static ConfigEntry<bool> IgnoreDoors { get; internal set; } //Toggle whether ghostCodes ignore doors


        //Effects/Sound
        public static ConfigEntry<int> AddInsanitySounds { get; internal set; } //Chance insanity sounds are added each time a code is run
        public static ConfigEntry<int> TerminalSoundChance { get; internal set; } //Chance that a sound is played on the terminal each time a code is run
        public static ConfigEntry<bool> BroadcastEffect { get; internal set; }
        public static ConfigEntry<bool> RapidLights { get; internal set; }
        public static ConfigEntry<float> RapidLightsMin { get; internal set; }
        public static ConfigEntry<float> RapidLightsMax { get; internal set; }
        public static ConfigEntry<string> RapidLightsColorValue { get; internal set; }


        //Intervals
        public static ConfigEntry<int> FirstSetInterval { get; internal set; } //First wait configuration
        public static ConfigEntry<int> SecondSetInterval { get; internal set; } //Second wait config
        public static ConfigEntry<int> SetIntervalAC { get; internal set; } //last wait config after code was sent
        public static ConfigEntry<int> FirstRandIntervalMin { get; internal set; } //First wait configuration
        public static ConfigEntry<int> FirstRandIntervalMax { get; internal set; } //First wait configuration
        public static ConfigEntry<int> SecondRandIntervalMin { get; internal set; } //Second wait config
        public static ConfigEntry<int> SecondRandIntervalMax { get; internal set; } //Second wait config
        public static ConfigEntry<int> RandIntervalACMin { get; internal set; } //last wait config after code was sent
        public static ConfigEntry<int> RandIntervalACMax { get; internal set; } //last wait config after code was sent

        internal static void Init()
        {
            //Main

            PrimaryMode = MakeClampedString(Setup, "Mode", "PrimaryMode", "Hauntings", "Set the primary mode to load on all levels.", new AcceptableValueList<string>("Hauntings", "Intervals", "Insanity"));
            SecondaryMode = MakeClampedString(Setup, "Mode", "SecondaryMode", "None", "Set the secondary mode to load on all levels.", new AcceptableValueList<string>("Hauntings", "Intervals", "Insanity", "None"));
            SecondaryModeLevels = MakeString(Setup, "Mode", "SecondaryModeLevels", "", "Comma-separated listing of all levels that will use the mode listed in [SecondaryMode]");

            RandomIntervals = MakeBool(Setup, "Mode", "RandomIntervals", true, "Disable this to use the 'set' intervals, all modes use intervals on first load."); ;
            RapidFireMaxHours = MakeClampedInt(Setup, "Mode", "RapidFireMaxHours", 1, "Set the maximum amount of hours for rapidFire to be active when on Max Insanity (Insanity Mode ONLY).\nSet to 0 to disable rapidFire", 0, 24);
            SoloAssist = MakeBool(Setup, "Mode", "SoloAssist", true, "Enable this to reduce sanity drain for single-player Insanity Mode");

            MinCodes = MakeClampedInt(Setup, "Codes", "MinCodes", 0, "This is the minimum amount of ghost codes that WILL be sent in one round. If you'd like for there to be the possibility of no codes set this to 0.", 0, 9999999);
            MaxCodes = MakeClampedInt(Setup, "Codes", "MaxCodes", 100, "This is the maximum amount of ghost codes that CAN be sent in one round. If you'd like for there to be the possibility of no codes set this to 0.", 0, 9999999);
            HauntingsCountCodes = MakeBool(Setup, "Codes", "HauntingsCountCodes", false, "Set this to true if you want hauntings to adhere to minimum/maximum code counts.");

            //Misc
            FixGhostGirlBreakers = MakeBool(Setup, "Misc", "FixGhostGirlBreakers", true, "Fix the vanilla code so that the ghost girl can flip the breakers at the start of a chase.");
            VanillaBreakersChance = MakeClampedInt(Setup, "Misc", "VanillaBreakersChance", 20, "Percent chance the ghost girl will flip the breakers when beginning a chase.", 0, 100);
            IgnoreLandmines = MakeBool(Setup, "Misc", "IgnoreLandmines", false, "Toggle whether ghostCodes ignore landmines");
            IgnoreTurrets = MakeBool(Setup, "Misc", "IgnoreTurrets", false, "Toggle whether ghostCodes ignore turrets");
            IgnoreDoors = MakeBool(Setup, "Misc", "IgnoreDoors", false, "Toggle whether ghostCodes ignore blast doors");

            //Effects/Sound
            AddInsanitySounds = MakeClampedInt(Setup, "Effects/Sound", "AddInsanitySounds", 75, "How rare it is for ghostCodes to trigger insanity sounds in the facility", 0, 100);
            TerminalSoundChance = MakeClampedInt(Setup, "Effects/Sound", "TerminalSoundChance", 100, "How rare it is for ghostCodes to play a sound over the terminal.", 0, 100);
            BroadcastEffect = MakeBool(Setup, "Effects/Sound", "BroadcastEffect", true, "Set this to false if you want to disable the code broadcast effect on the terminal during a ghost code event.");
            RapidLights = MakeBool(Setup, "Effects/Sound", "RapidLights", true, "Disable this to remove light flashing effect during RapidFire event.\n(DISABLE THIS IF YOU HAVE SEVERE EPILEPSY)");
            RapidLightsMin = MakeClampedFloat(Setup, "Effects/Sound", "RapidLightsMin", 0.2f, "Set shortest possible time between each light flickering effect during rapidFire event.", 0.1f, 10f);
            RapidLightsMax = MakeClampedFloat(Setup, "Effects/Sound", "RapidLightsMax", 0.6f, "Set longest possible time between each light flickering effect during rapidFire event.", 0.1f, 20f);
            RapidLightsColorValue = MakeString(Setup, "Effects/Sound", "RapidLightsColorValue", "nochange", "Determine color lights will change to during rapidFire event.\nSet to a valid hex code for color change or leave as nochange to disable color change"); 

            //Intervals//

            FirstSetInterval = MakeClampedInt(Setup, "Intervals", "FirstSetInterval", 90, "This is the initial time it takes to start loading codes after landing the ship", 1, 9999999);
            SecondSetInterval = MakeClampedInt(Setup, "Intervals", "SecondSetInterval", 30, "This is the time it takes to load the code once it has been picked", 1, 9999999);
            SetIntervalAC = MakeClampedInt(Setup, "Intervals", "SetIntervalAC", 180, "This is the time it waits to pick another code", 1, 9999999);
            FirstRandIntervalMin = MakeClampedInt(Setup, "Intervals", "FirstRandIntervalMin", 15, "This is the minimum time it should take to start loading codes after landing the ship", 1, 9999999);
            FirstRandIntervalMax = MakeClampedInt(Setup, "Intervals", "FirstRandIntervalMax", 120, "This is the maximum time it should take to start loading codes after landing the ship \nNOTE: Must be higher than FirstRandIntervalMin!", 1, 9999999);
            SecondRandIntervalMin = MakeClampedInt(Setup, "Intervals", "SecondRandIntervalMin", 5, "This is the minimum time it should take to load the code once it has been picked", 1, 9999999);
            SecondRandIntervalMax = MakeClampedInt(Setup, "Intervals", "SecondRandIntervalMax", 60, "This is the maximum time it should take to load the code once it has been picked\nNOTE: Must be higher than SecondRandIntervalMin!", 1, 9999999);
            RandIntervalACMin = MakeClampedInt(Setup, "Intervals", "RandIntervalACMin", 20, "This is the minimum time it should wait to pick another code", 1, 9999999);
            RandIntervalACMax = MakeClampedInt(Setup, "Intervals", "RandIntervalACMax", 240, "This is the maximum time it should wait to pick another code\nNOTE: Must be higher than RandIntervalACMin!", 1, 9999999);

            Setup.Save();
            ModConfig.configFiles.Add(Setup);
            //WebConfig(Setup);
            if (ModConfig.SetupConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.SetupConfigCode, Setup);


            GhostCodesSettings = new(PrimaryMode.Value, SecondaryMode.Value, SecondaryModeLevels.Value);
            Setup.SettingChanged += OnSettingChanged;
            Plugin.GC.LogInfo("ghostCodes setup config initialized");
        }

        internal static void OnSettingChanged(object sender, SettingChangedEventArgs settingChangedArg)
        {
            if (settingChangedArg.ChangedSetting == null)
                return;

            if (settingChangedArg.ChangedSetting.Definition != PrimaryMode.Definition && settingChangedArg.ChangedSetting.Definition != SecondaryMode.Definition && settingChangedArg.ChangedSetting.Definition != SecondaryModeLevels.Definition)
                return;

            Plugin.MoreLogs("Updating ghostcode mode settings!");
            GhostCodesSettings.ConfigUpdate(PrimaryMode.Value, SecondaryMode.Value, SecondaryModeLevels.Value);
        }

    }
}
