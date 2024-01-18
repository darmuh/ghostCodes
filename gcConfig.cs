using BepInEx.Configuration;
using System.Configuration;

namespace ghostCodes
{
    public static class gcConfig
    {
        //establish config options here
        public static ConfigEntry<bool> ModNetworking; //For users who only want to use this as the host
        public static ConfigEntry<bool> gcRandomIntervals; //Toggle whether the intervals between codes are random or set intervals
        public static ConfigEntry<bool> gcEnableTerminalSound; //Toggle whether the terminal makes a sound when a ghost code is submitted
        public static ConfigEntry<bool> gcIgnoreLandmines; //Toggle whether ghostCodes ignore landmines
        public static ConfigEntry<bool> gcIgnoreTurrets; //Toggle whether ghostCodes ignore turrets
        public static ConfigEntry<bool> gcIgnoreDoors; //Toggle whether ghostCodes ignore doors (this should always be false lol)
        public static ConfigEntry<bool> gcInsanityMode; //Toggle Insanity affected codes
        public static ConfigEntry<bool> ghostGirlEnhanced; //Toggle Insanity affected codes
        public static ConfigEntry<int> gcFirstSetInterval; //First wait configuration
        public static ConfigEntry<int> gcSecondSetInterval; //Second wait config
        public static ConfigEntry<int> gcSetIntervalAC; //last wait config after code was sent
        public static ConfigEntry<int> gcFirstRandIntervalMin; //First wait configuration
        public static ConfigEntry<int> gcFirstRandIntervalMax; //First wait configuration
        public static ConfigEntry<int> gcSecondRandIntervalMin; //Second wait config
        public static ConfigEntry<int> gcSecondRandIntervalMax; //Second wait config
        public static ConfigEntry<int> gcRandIntervalACMin; //last wait config after code was sent
        public static ConfigEntry<int> gcRandIntervalACMax; //last wait config after code was sent
        public static ConfigEntry<int> gcMaxCodes; //Maximum amount of codes to send in one round
        public static ConfigEntry<int> gcMinCodes; //Minimum amount of codes to send in one round
        public static ConfigEntry<bool> insanityRapidFire; //enable/disable rapidfire
        public static ConfigEntry<bool> fixGhostGirlBreakers; // enable/disable the ghostgirl code from vanilla

        //Turret Stuff
        public static ConfigEntry<int> turretNormalBChance;
        public static ConfigEntry<int> turretInsaneBChance;

        //Other Action Chances
        public static ConfigEntry<int> hungryDoorIChance;
        public static ConfigEntry<int> hungryDoorNChance;
        public static ConfigEntry<int> mineInsaneBChance;
        public static ConfigEntry<int> mineNormalBChance;
        public static ConfigEntry<int> ggVanillaBreakerChance;
        public static ConfigEntry<int> ggCodeBreakerChance;

        //Insanity Mode Stuff
        public static ConfigEntry<int> sanityPercentL1;
        public static ConfigEntry<int> sanityPercentL2;
        public static ConfigEntry<int> sanityPercentL3;
        public static ConfigEntry<int> sanityPercentMAX;
        public static ConfigEntry<int> waitPercentL1;
        public static ConfigEntry<int> waitPercentL2;
        public static ConfigEntry<int> waitPercentL3;
        public static ConfigEntry<int> waitPercentMAX;

        //Solo Assist Stuff
        public static ConfigEntry<int> saS1percent;
        public static ConfigEntry<int> saS1inside;
        public static ConfigEntry<int> saS1outside;
        
        public static ConfigEntry<int> saS2percent;
        public static ConfigEntry<int> saS2inside;
        public static ConfigEntry<int> saS2outside;

        public static ConfigEntry<int> saS3percent;
        public static ConfigEntry<int> saS3inside;
        public static ConfigEntry<int> saS3outside;

        //More insanity
        public static ConfigEntry<bool> deathBonus;
        public static ConfigEntry<int> deathBonusNum;
        public static ConfigEntry<bool> ggBonus;
        public static ConfigEntry<int> ggBonusNum;
        public static ConfigEntry<bool> soloAssist;

        //ghostGirl stuff
        //public static ConfigEntry<bool> gcGhostGirl; //enable/disable ghostGirl interactions
        //public static ConfigEntry<bool> gcOnlyGhostGirl; //toggle ONLY interacting when ghostGirl is present
        public static ConfigEntry<bool> GGEbypass;
        public static ConfigEntry<string> GGEbypassList;
        public static ConfigEntry<bool> ggCanSendMessages;
        public static ConfigEntry<string> signalMessages;
        public static ConfigEntry<int> ggPlayerLightsPercent; //chance that flashlights toggle

        //rapidFire stuff
        public static ConfigEntry<bool> rfRapidLights;
        public static ConfigEntry<float> rfRLmin;
        public static ConfigEntry<float> rfRLmax;
        //public static ConfigEntry<bool> rfRapidLightsColor;

        public static void SetConfigSettings()
        {
            //Insanity Mode
            gcConfig.sanityPercentL1 = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentL1", 25, new ConfigDescription("Set the percentage of the maximum sanity level required to reach Level 1 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.sanityPercentL2 = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentL2", 50, new ConfigDescription("Set the percentage of the maximum sanity level required to reach Level 2 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.sanityPercentL3 = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentL3", 75, new ConfigDescription("Set the percentage of the maximum sanity level required to reach Level 3 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.sanityPercentMAX = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentMAX", 95, new ConfigDescription("Set the percentage of the maximum sanity level required to reach MAX Level of Insanity Mode and trigger rapid fire.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.waitPercentL1 = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentL1", 90, new ConfigDescription("Set the percentage of the wait time to use after reaching Level 1 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.waitPercentL2 = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentL2", 50, new ConfigDescription("Set the percentage of the wait time to use after reaching Level 2 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.waitPercentL3 = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentL3", 10, new ConfigDescription("Set the percentage of the wait time to use after reaching Level 3 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.waitPercentMAX = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentMAX", 2, new ConfigDescription("Set the percentage of the wait time to use after reaching Max Level of Insanity Mode. (This is only triggered if rapidFire is disabled)", new AcceptableValueRange<int>(0, 100)));


            //more insanity
            gcConfig.deathBonus = Plugin.instance.Config.Bind<bool>("Insanity Mode", "deathBonus", false, "Toggle whether player deaths adds an insanity level bonus or not");
            gcConfig.deathBonusNum = Plugin.instance.Config.Bind("Insanity Mode", "deathBonusNum", 5, new ConfigDescription("Increase of insanity level after a dead player is detected (lowered from previous max of 50).", new AcceptableValueRange<int>(0, 25)));
            gcConfig.ggBonus = Plugin.instance.Config.Bind<bool>("Insanity Mode", "ggBonus", false, "Toggle whether the ghost girl being spawned adds an insanity level bonus or not");
            gcConfig.ggBonusNum = Plugin.instance.Config.Bind("Insanity Mode", "ggBonusNum", 5, new ConfigDescription("Increase of insanity level once a ghostGirl has been spawned.", new AcceptableValueRange<int>(0, 25)));
            gcConfig.soloAssist = Plugin.instance.Config.Bind<bool>("Solo Assistance", "soloAssist", true, "Enable this setting to reduce Insanity gains during solo play");

            //Solo Assist Values
            gcConfig.saS1percent = Plugin.instance.Config.Bind("Solo Assistance", "saS1percent", 50, new ConfigDescription("Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 1 (beginning of day to Noon)", new AcceptableValueRange<int>(0, 100)));
            gcConfig.saS1inside = Plugin.instance.Config.Bind("Solo Assistance", "saS1inside", 20, new ConfigDescription("Decrease of insanity level when inside the factory during Stage 1 (beginning of day to Noon)", new AcceptableValueRange<int>(0, 40)));
            gcConfig.saS1outside = Plugin.instance.Config.Bind("Solo Assistance", "saS1outside", 25, new ConfigDescription("Decrease of insanity level when outside the factory during Stage 1 (beginning of day to Noon)", new AcceptableValueRange<int>(0, 40)));

            gcConfig.saS2percent = Plugin.instance.Config.Bind("Solo Assistance", "saS2percent", 75, new ConfigDescription("Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 2 (Noon to 5PM)", new AcceptableValueRange<int>(0, 100)));
            gcConfig.saS2inside = Plugin.instance.Config.Bind("Solo Assistance", "saS2inside", 10, new ConfigDescription("Decrease of insanity level when inside the factory during Stage 2 (Noon to 5PM)", new AcceptableValueRange<int>(0, 40)));
            gcConfig.saS2outside = Plugin.instance.Config.Bind("Solo Assistance", "saS2outside", 15, new ConfigDescription("Decrease of insanity level when outside the factory during Stage 2 (Noon to 5PM)", new AcceptableValueRange<int>(0, 40)));

            gcConfig.saS3percent = Plugin.instance.Config.Bind("Solo Assistance", "saS3percent", 90, new ConfigDescription("Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 3 (5PM - 10PM)", new AcceptableValueRange<int>(0, 100)));
            gcConfig.saS3inside = Plugin.instance.Config.Bind("Solo Assistance", "saS3inside", 5, new ConfigDescription("Decrease of insanity level when inside the factory during Stage 3 (5PM - 10PM)", new AcceptableValueRange<int>(0, 40)));
            gcConfig.saS3outside = Plugin.instance.Config.Bind("Solo Assistance", "saS3outside", 10, new ConfigDescription("Decrease of insanity level when outside the factory during Stage 3 (5PM - 10PM)", new AcceptableValueRange<int>(0, 40)));


            //GhostGirl Interactions
            //gcConfig.gcGhostGirl = Plugin.instance.Config.Bind<bool>("General", "gcGhostGirl", true, "Toggle ghost girl interactions with insanity & normal ghost codes modes");
            gcConfig.ghostGirlEnhanced = Plugin.instance.Config.Bind<bool>("General", "ghostGirlEnhanced", true, "Ghost Girl Enhanced Mode, will replace insanity & normal ghost codes modes");
            gcConfig.GGEbypassList = Plugin.instance.Config.Bind("Ghost Girl", "GGEbypassList", "Vow, Offense, March", "Comma-separated list of moons Ghost Girl Enhanced mode will be bypassed for another mode (set to moons that ghostgirl cant spawn on by default).");
            gcConfig.GGEbypass = Plugin.instance.Config.Bind<bool>("Ghost Girl", "GGEbypass", true, "Enable or Disable bypassing Ghost Girl Enhanced on moons listed in GGEbypassList.");

            //gcConfig.gcOnlyGhostGirl = Plugin.instance.Config.Bind<bool>("Ghost Girl", "gcGhostGirl", false, "Toggle so that ghost codes ONLY interact with the ghost girl enemy ai");
            gcConfig.ggPlayerLightsPercent = Plugin.instance.Config.Bind("Ghost Girl", "ggPlayerLightsPercent", 45, new ConfigDescription("Set the percentage chance that the ghostGirl will flicker a player's lights during a ghostCode event.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.fixGhostGirlBreakers = Plugin.instance.Config.Bind<bool>("Ghost Girl", "fixGhostGirlBreakers", true, "Fix the vanilla code so that the ghost girl can flip the breakers at the start of a chase.");
            gcConfig.signalMessages = Plugin.instance.Config.Bind("Ghost Girl", "signalMessages", "RUN, LETS PLAY, BOO, FIND ME, I SEE YOU", "Comma-separated list of messages the ghostGirl will send over the signal translator when sending a code.");
            gcConfig.ggCanSendMessages = Plugin.instance.Config.Bind<bool>("Ghost Girl", "ggCanSendMessages", true, "Enable or Disable ghost girl using the signal translator to send messages during special codes.");

            //Turret Berserk Stuff
            gcConfig.turretNormalBChance = Plugin.instance.Config.Bind("Turret", "turretNormalBChance", 20, new ConfigDescription("How rare it is for the turret to go berserk from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.turretInsaneBChance = Plugin.instance.Config.Bind("Turret", "turretInsaneBChance", 65, new ConfigDescription("How rare it is for the turret to go berserk during rapidFire mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.mineNormalBChance = Plugin.instance.Config.Bind("Mine", "mineNormalBChance", 10, new ConfigDescription("How rare it is for a mine to blow itself up from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.mineInsaneBChance = Plugin.instance.Config.Bind("Mine", "mineInsaneBChance", 65, new ConfigDescription("How rare it is for a mine to blow itself up during rapidFire mode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.hungryDoorNChance = Plugin.instance.Config.Bind("Door", "hungryDoorNChance", 10, new ConfigDescription("How rare it is for a door to start biting from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.hungryDoorIChance = Plugin.instance.Config.Bind("Door", "hungryDoorIChance", 65, new ConfigDescription("How rare it is for a door to start biting during rapidFire mode.", new AcceptableValueRange<int>(0, 100)));


            //Facility Lights
            gcConfig.ggVanillaBreakerChance = Plugin.instance.Config.Bind("Ghost Girl", "ggVanillaBreakerChance", 20, new ConfigDescription("Percent chance the ghost girl will flip the breakers when beginning a chase.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.ggCodeBreakerChance = Plugin.instance.Config.Bind("Facility Lights", "ggCodeBreakerChance", 3, new ConfigDescription("How rare it is for a ghostCode to flip the breaker and turn off the facility lights.", new AcceptableValueRange<int>(0, 100)));
            gcConfig.rfRapidLights = Plugin.instance.Config.Bind<bool>("GGE Mode Settings", "ggRapidLights", true, "Disable this to remove light flashing effect during RapidFire mode. (DISABLE THIS IF YOU HAVE SEVERE EPILEPSY)");
            gcConfig.rfRLmin = Plugin.instance.Config.Bind("GGE Mode Settings", "rfRLmin", 0.2f, new ConfigDescription("Set shortest possible time between each light flickering affect.", new AcceptableValueRange<float>(0.1f, 0.9f)));
            gcConfig.rfRLmax = Plugin.instance.Config.Bind("GGE Mode Settings", "rfRLmax", 0.6f, new ConfigDescription("Set longest possible time between each light flickering affect.", new AcceptableValueRange<float>(0.1f, 1.9f)));



            //Enable or Disable random intervals
            gcConfig.gcRandomIntervals = Plugin.instance.Config.Bind<bool>("General", "gcRandomIntervals", true, "Disable this to use the 'set' intervals");
            gcConfig.ModNetworking = Plugin.instance.Config.Bind<bool>("__NETWORKING", "ModNetworking", true, "Disable this if you want to disable networking and use this mod as a Host-Only mod. This feature will completely disable GhostGirl Enhanced Mode.");
            gcConfig.gcEnableTerminalSound = Plugin.instance.Config.Bind<bool>("General", "gcEnableTerminalSound", true, "This setting determines whether the terminal plays the 'alarm' sound when a ghost code is entered.");
            gcConfig.gcIgnoreLandmines = Plugin.instance.Config.Bind<bool>("General", "gcIgnoreLandmines", false, "Toggle whether ghostCodes ignore landmines");
            gcConfig.gcIgnoreTurrets = Plugin.instance.Config.Bind<bool>("General", "gcIgnoreTurrets", false, "Toggle whether ghostCodes ignore turrets");
            gcConfig.gcIgnoreDoors = Plugin.instance.Config.Bind<bool>("General", "gcIgnoreDoors", false, "Toggle whether ghostCodes ignore doors (this should probably always be false lol)");
            gcConfig.gcInsanityMode = Plugin.instance.Config.Bind<bool>("General", "gcInsanityMode", true, "Enable this to have player insanity levels affect the frequency of ghost codes sent.");
            gcConfig.insanityRapidFire = Plugin.instance.Config.Bind<bool>("General", "insanityRapidFire", true, "Set this to false to disable rapidFire on Max Insanity (Insanity Mode ONLY)");

            //Max Codes per round
            gcConfig.gcMinCodes = Plugin.instance.Config.Bind<int>("General", "gcMinCodes", 0, "This is the minimum amount of ghost codes that WILL be sent in one round. If you'd like for there to be the possibility of no codes set this to 0.");
            gcConfig.gcMaxCodes = Plugin.instance.Config.Bind<int>("General", "gcMaxCodes", 100, "This is the maximum amount of ghost codes that CAN be sent in one round, a random number of codes will be chosen with this value set as the maximum");

            //set interval
            gcConfig.gcFirstSetInterval = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcFirstRandInterval", 90, "This is the initial time it takes to start loading codes after landing the ship");
            gcConfig.gcSecondSetInterval = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcSecondRandInterval", 30, "This is the time it takes to load the code once it has been picked");
            gcConfig.gcSetIntervalAC = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcSetIntervalAC", 180, "This is the time it waits to pick another code");
            
            //random interval
            gcConfig.gcFirstRandIntervalMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcFirstRandIntervalMin", 15, "This is the minimum time it should take to start loading codes after landing the ship");
            gcConfig.gcSecondRandIntervalMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcSecondRandIntervalMin", 5, "This is the minimum time it should take to load the code once it has been picked");
            gcConfig.gcRandIntervalACMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcRandIntervalACMin", 20, "This is the minimum time it should wait to pick another code");
            gcConfig.gcFirstRandIntervalMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcFirstRandIntervalMax", 120, "This is the maximum time it should take to start loading codes after landing the ship");
            gcConfig.gcSecondRandIntervalMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcSecondRandIntervalMax", 60, "This is the maximum time it should take to load the code once it has been picked");
            gcConfig.gcRandIntervalACMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcRandIntervalACMax", 240, "This is the maximum time it should wait to pick another code");

            Plugin.GC.LogInfo("ghostCodes config initialized");
        }
    }
}
