using BepInEx.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace ghostCodes
{
    public static class gcConfig
    {
        //establish config options here
        internal static ConfigEntry<bool> ModNetworking; //For users who only want to use this as the host
        internal static ConfigEntry<bool> useRandomIntervals; //Toggle whether the intervals between codes are random or set intervals
        internal static ConfigEntry<bool> gcIgnoreLandmines; //Toggle whether ghostCodes ignore landmines
        internal static ConfigEntry<bool> gcIgnoreTurrets; //Toggle whether ghostCodes ignore turrets
        internal static ConfigEntry<bool> gcIgnoreDoors; //Toggle whether ghostCodes ignore doors (this should always be false lol)
        internal static ConfigEntry<bool> gcInsanity; //Toggle Insanity affected codes
        internal static ConfigEntry<bool> ghostGirlEnhanced; //Toggle Insanity affected codes
        internal static ConfigEntry<int> gcFirstSetInterval; //First wait configuration
        internal static ConfigEntry<int> gcSecondSetInterval; //Second wait config
        internal static ConfigEntry<int> gcSetIntervalAC; //last wait config after code was sent
        internal static ConfigEntry<int> gcFirstRandIntervalMin; //First wait configuration
        internal static ConfigEntry<int> gcFirstRandIntervalMax; //First wait configuration
        internal static ConfigEntry<int> gcSecondRandIntervalMin; //Second wait config
        internal static ConfigEntry<int> gcSecondRandIntervalMax; //Second wait config
        internal static ConfigEntry<int> gcRandIntervalACMin; //last wait config after code was sent
        internal static ConfigEntry<int> gcRandIntervalACMax; //last wait config after code was sent
        internal static ConfigEntry<int> gcMaxCodes; //Maximum amount of codes to send in one round
        internal static ConfigEntry<int> gcMinCodes; //Minimum amount of codes to send in one round
        internal static ConfigEntry<bool> ggIgnoreCodeCount; //Determine whether hauntings can be affected by code count
        internal static ConfigEntry<bool> insanityRapidFire; //enable/disable rapidfire
        internal static ConfigEntry<bool> rapidFireCooldown; //adds time limit to rapidFire during insanity mode
        internal static ConfigEntry<int> rapidFireMaxHours; //time limit variable
        internal static ConfigEntry<bool> fixGhostGirlBreakers; // enable/disable the ghostgirl code from vanilla
        internal static ConfigEntry<bool> extensiveLogging; //enable extensive logging messages

        //Turret Stuff
        internal static ConfigEntry<int> turretNormalBChance;
        internal static ConfigEntry<int> turretInsaneBChance;

        //Regular Doors
        internal static ConfigEntry<bool> openAllRegularDoorsEvent;
        internal static ConfigEntry<int> openAllRegularDoorsChance;
        internal static ConfigEntry<bool> closeAllRegularDoorsEvent;
        internal static ConfigEntry<int> closeAllRegularDoorsChance;
        internal static ConfigEntry<bool> openSingleDoorEvent;
        internal static ConfigEntry<int> openSingleDoorChance;
        internal static ConfigEntry<bool> closeSingleDoorEvent;
        internal static ConfigEntry<int> closeSingleDoorChance;
        internal static ConfigEntry<bool> unlockSingleDoorEvent;
        internal static ConfigEntry<int> unlockSingleDoorChance;
        internal static ConfigEntry<bool> lockSingleDoorEvent;
        internal static ConfigEntry<int> lockSingleDoorChance;

        //ship stuff
        //internal static ConfigEntry<bool> emptyShipEvent;
        //internal static ConfigEntry<int> emptyShipChance;
        internal static ConfigEntry<bool> lightsOnShipEvent;
        internal static ConfigEntry<int> lightsOnShipChance;
        internal static ConfigEntry<bool> doorsOnShipEvent;
        internal static ConfigEntry<int> doorsOnShipChance;
        internal static ConfigEntry<bool> monitorsOnShipEvent;
        internal static ConfigEntry<int> monitorsOnShipChance;
        internal static ConfigEntry<bool> shockTerminalUserEvent;
        internal static ConfigEntry<int> shockTerminalUserChance;

        //Sound System
        internal static ConfigEntry<bool> gcEnableTerminalSound; //Toggle whether the terminal makes a sound when a ghost code is submitted
        internal static ConfigEntry<int> gcTerminalSoundChance; //Chance that a sound is played on the terminal each time a code is run
        internal static ConfigEntry<int> gcAddInsanitySounds; //Chance insanity sounds are added each time a code is run
        internal static ConfigEntry<bool> gcUseGirlSounds; //Toggle using ghost girl audio for codes being run
        internal static ConfigEntry<bool> gcUseTerminalAlarmSound; //Toggle using the terminal alarm sound

        //Other Action Chances
        internal static ConfigEntry<int> hungryDoorIChance;
        internal static ConfigEntry<int> hungryDoorNChance;
        internal static ConfigEntry<int> mineInsaneBChance;
        internal static ConfigEntry<int> mineNormalBChance;

        //Insanity Mode Stuff
        internal static ConfigEntry<int> sanityPercentL1;
        internal static ConfigEntry<int> sanityPercentL2;
        internal static ConfigEntry<int> sanityPercentL3;
        internal static ConfigEntry<int> sanityPercentMAX;
        internal static ConfigEntry<int> waitPercentL1;
        internal static ConfigEntry<int> waitPercentL2;
        internal static ConfigEntry<int> waitPercentL3;
        internal static ConfigEntry<int> waitPercentMAX;

        //Solo Assist Stuff
        internal static ConfigEntry<int> saS1percent;
        internal static ConfigEntry<int> saS1inside;
        internal static ConfigEntry<int> saS1outside;
        
        internal static ConfigEntry<int> saS2percent;
        internal static ConfigEntry<int> saS2inside;
        internal static ConfigEntry<int> saS2outside;

        internal static ConfigEntry<int> saS3percent;
        internal static ConfigEntry<int> saS3inside;
        internal static ConfigEntry<int> saS3outside;

        //More insanity
        internal static ConfigEntry<bool> deathBonus;
        internal static ConfigEntry<int> deathBonusNum;
        internal static ConfigEntry<bool> ggBonus;
        internal static ConfigEntry<int> ggBonusNum;
        internal static ConfigEntry<bool> soloAssist;
        internal static ConfigEntry<bool> emoteBuff;
        internal static ConfigEntry<int> emoteBuffNum;

        //ghostGirl stuff
        internal static ConfigEntry<bool> GGEbypass;
        internal static ConfigEntry<string> GGEbypassList;
        internal static ConfigEntry<bool> gcGhostGirlOnly;
        internal static ConfigEntry<string> gcGhostGirlOnlyList;
        internal static ConfigEntry<int> ggPlayerLightsPercent; //chance that flashlights toggle
        internal static ConfigEntry<int> ggVanillaBreakerChance;
        internal static ConfigEntry<int> ggCodeBreakerChance;
        internal static ConfigEntry<int> ggEmoteStopChasingChance;
        internal static ConfigEntry<int> ggEmoteStopChasePlayers;
        internal static ConfigEntry<bool> ggEmoteCheck;
        internal static ConfigEntry<int> ggShowerStopChasingChance;
        internal static ConfigEntry<bool> ggShowerCheck;
        internal static ConfigEntry<int> ggDeathNoteChance;
        internal static ConfigEntry<bool> ggDeathNote;
        internal static ConfigEntry<int> ggDeathNoteMaxStrikes;
        internal static ConfigEntry<bool> gcRebootTerminal;
        internal static ConfigEntry<int> gcRebootEfficacy;

        //signal translator
        internal static ConfigEntry<bool> canSendMessages;
        internal static ConfigEntry<bool> onlyUniqueMessages;
        internal static ConfigEntry<bool> onlyGGSendMessages;
        internal static ConfigEntry<int> messageFrequency;
        internal static ConfigEntry<string> signalMessages;
        internal static ConfigEntry<string> monitorMessages;
        internal static ConfigEntry<bool> enableBroadcastEffect;

        //rapidFire stuff
        internal static ConfigEntry<bool> rfRapidLights;
        internal static ConfigEntry<float> rfRLmin;
        internal static ConfigEntry<float> rfRLmax;
        internal static ConfigEntry<string> rfRLcolorValue;
        internal static ConfigEntry<bool> rfRLcolorChange;

        public static void SetConfigSettings()
        {
            //Insanity Mode Values
            sanityPercentL1 = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentL1", 25, new ConfigDescription("Set the percentage of the maximum sanity level required to reach Level 1 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            sanityPercentL2 = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentL2", 50, new ConfigDescription("Set the percentage of the maximum sanity level required to reach Level 2 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            sanityPercentL3 = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentL3", 75, new ConfigDescription("Set the percentage of the maximum sanity level required to reach Level 3 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            sanityPercentMAX = Plugin.instance.Config.Bind("Insanity Mode", "sanityPercentMAX", 95, new ConfigDescription("Set the percentage of the maximum sanity level required to reach MAX Level of Insanity Mode and trigger rapid fire.", new AcceptableValueRange<int>(0, 100)));
            waitPercentL1 = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentL1", 90, new ConfigDescription("Set the percentage of the wait time to use after reaching Level 1 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            waitPercentL2 = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentL2", 50, new ConfigDescription("Set the percentage of the wait time to use after reaching Level 2 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            waitPercentL3 = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentL3", 10, new ConfigDescription("Set the percentage of the wait time to use after reaching Level 3 of Insanity Mode.", new AcceptableValueRange<int>(0, 100)));
            waitPercentMAX = Plugin.instance.Config.Bind("Insanity Mode", "waitPercentMAX", 2, new ConfigDescription("Set the percentage of the wait time to use after reaching Max Level of Insanity Mode. (This is only triggered if rapidFire is disabled)", new AcceptableValueRange<int>(0, 100)));

            //Insanity Buffs and Debuffs
            deathBonus = Plugin.instance.Config.Bind<bool>("Insanity Mode", "deathBonus", false, "Toggle whether player deaths adds an insanity level bonus or not");
            deathBonusNum = Plugin.instance.Config.Bind("Insanity Mode", "deathBonusNum", 5, new ConfigDescription("Increase of insanity level after a dead player is detected (lowered from previous max of 50).", new AcceptableValueRange<int>(0, 25)));
            ggBonus = Plugin.instance.Config.Bind<bool>("Insanity Mode", "ggBonus", false, "Toggle whether the ghost girl being spawned adds an insanity level bonus or not");
            ggBonusNum = Plugin.instance.Config.Bind("Insanity Mode", "ggBonusNum", 5, new ConfigDescription("Increase of insanity level once a ghostGirl has been spawned.", new AcceptableValueRange<int>(0, 25)));
            soloAssist = Plugin.instance.Config.Bind<bool>("Solo Assistance", "soloAssist", true, "Enable this setting to reduce Insanity gains during solo play");
            emoteBuff = Plugin.instance.Config.Bind<bool>("Insanity Mode", "emoteBuff", true, "Enable this to lower sanity whenever any living player is emoting.");
            emoteBuffNum = Plugin.instance.Config.Bind("Insanity Mode", "emoteBuffNum", 10, new ConfigDescription("Decrease Amount of insanity level for each person that is emoting.", new AcceptableValueRange<int>(0, 25)));

            //Solo Assist Debuff Values
            saS1percent = Plugin.instance.Config.Bind("Solo Assistance", "saS1percent", 50, new ConfigDescription("Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 1 (beginning of day to Noon)", new AcceptableValueRange<int>(0, 100)));
            saS1inside = Plugin.instance.Config.Bind("Solo Assistance", "saS1inside", 20, new ConfigDescription("Decrease of insanity level when inside the factory during Stage 1 (beginning of day to Noon)", new AcceptableValueRange<int>(0, 40)));
            saS1outside = Plugin.instance.Config.Bind("Solo Assistance", "saS1outside", 25, new ConfigDescription("Decrease of insanity level when outside the factory during Stage 1 (beginning of day to Noon)", new AcceptableValueRange<int>(0, 40)));
            saS2percent = Plugin.instance.Config.Bind("Solo Assistance", "saS2percent", 75, new ConfigDescription("Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 2 (Noon to 5PM)", new AcceptableValueRange<int>(0, 100)));
            saS2inside = Plugin.instance.Config.Bind("Solo Assistance", "saS2inside", 10, new ConfigDescription("Decrease of insanity level when inside the factory during Stage 2 (Noon to 5PM)", new AcceptableValueRange<int>(0, 40)));
            saS2outside = Plugin.instance.Config.Bind("Solo Assistance", "saS2outside", 15, new ConfigDescription("Decrease of insanity level when outside the factory during Stage 2 (Noon to 5PM)", new AcceptableValueRange<int>(0, 40)));
            saS3percent = Plugin.instance.Config.Bind("Solo Assistance", "saS3percent", 90, new ConfigDescription("Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 3 (5PM - 10PM)", new AcceptableValueRange<int>(0, 100)));
            saS3inside = Plugin.instance.Config.Bind("Solo Assistance", "saS3inside", 5, new ConfigDescription("Decrease of insanity level when inside the factory during Stage 3 (5PM - 10PM)", new AcceptableValueRange<int>(0, 40)));
            saS3outside = Plugin.instance.Config.Bind("Solo Assistance", "saS3outside", 10, new ConfigDescription("Decrease of insanity level when outside the factory during Stage 3 (5PM - 10PM)", new AcceptableValueRange<int>(0, 40)));

            //GhostGirl Interactions
            extensiveLogging = Plugin.instance.Config.Bind<bool>("General", "extensiveLogging", false, "Enable or Disable extensive log messages for this mod.");
            GGEbypassList = Plugin.instance.Config.Bind("Ghost Girl", "GGEbypassList", "Vow, Offense, March", "Comma-separated list of moons Ghost Girl Enhanced mode will be bypassed for another mode (set to moons that ghostgirl cant spawn on by default).");
            GGEbypass = Plugin.instance.Config.Bind<bool>("Ghost Girl", "GGEbypass", true, "Enable or Disable bypassing Ghost Girl Enhanced on moons listed in GGEbypassList.");
            gcGhostGirlOnlyList = Plugin.instance.Config.Bind("Ghost Girl", "gcGhostGirlOnlyList", "ggShowerCheck, ggDeathNote, gcRebootTerminal", "Comma-separated list of moons Ghost Girl Enhanced mode will be bypassed for another mode (set to moons that ghostgirl cant spawn on by default).");
            gcGhostGirlOnly = Plugin.instance.Config.Bind<bool>("Ghost Girl", "gcGhostGirlOnly", false, "Enable or Disable bypassing Ghost Girl Enhanced on moons listed in GGEbypassList.");
            ggPlayerLightsPercent = Plugin.instance.Config.Bind("Ghost Girl", "ggPlayerLightsPercent", 45, new ConfigDescription("Set the percentage chance that the ghostGirl will flicker a player's lights during a ghostCode event.", new AcceptableValueRange<int>(0, 100)));
            fixGhostGirlBreakers = Plugin.instance.Config.Bind<bool>("Ghost Girl", "fixGhostGirlBreakers", true, "Fix the vanilla code so that the ghost girl can flip the breakers at the start of a chase.");
            ggEmoteStopChasingChance = Plugin.instance.Config.Bind("Ghost Girl", "ggEmoteStopChasingChance", 95, new ConfigDescription("How effective emoting to get the ghost girl to stop chasing you is.", new AcceptableValueRange<int>(0, 100)));
            ggEmoteStopChasePlayers = Plugin.instance.Config.Bind("Ghost Girl", "ggEmoteStopChasePlayers", 75, new ConfigDescription("Percentage of living players required to stop ghost girl from chasing.", new AcceptableValueRange<int>(0, 100)));
            ggEmoteCheck = Plugin.instance.Config.Bind<bool>("Ghost Girl", "ggEmoteCheck", true, "Enable or Disable emoting to stop the ghost girl from chasing you.");
            ggShowerCheck = Plugin.instance.Config.Bind<bool>("Ghost Girl", "ggShowerCheck", true, "Enable or Disable taking a shower to stop the ghost girl from chasing you.");
            ggShowerStopChasingChance = Plugin.instance.Config.Bind("Ghost Girl", "ggShowerStopChasingChance", 95, new ConfigDescription("How effective taking a shower to get the ghost girl to stop chasing you is.", new AcceptableValueRange<int>(0, 100)));
            ggDeathNoteChance = Plugin.instance.Config.Bind("Ghost Girl", "ggDeathNoteChance", 65, new ConfigDescription("How effective typing a player's name in the terminal is to transfer the haunting.", new AcceptableValueRange<int>(0, 100)));
            ggDeathNote = Plugin.instance.Config.Bind<bool>("Ghost Girl", "ggDeathNote", true, "Enable or Disable typing a name in the terminal to transfer the ghost girl to another player.");
            ggDeathNoteMaxStrikes = Plugin.instance.Config.Bind("Ghost Girl", "ggDeathNoteMaxStrikes", 3, new ConfigDescription("Amount of times you can sucessfully use the death note to transfer hauntings, use -1 for infinite transfers.", new AcceptableValueRange<int>(-1, 25)));
            gcRebootEfficacy = Plugin.instance.Config.Bind("Terminal Reboot", "ggRebootEfficacy", 75, new ConfigDescription("How effective rebooting the terminal is to delay ghostCodes.", new AcceptableValueRange<int>(0, 100)));
            gcRebootTerminal = Plugin.instance.Config.Bind<bool>("Terminal Reboot", "gcRebootTerminal", true, "Enable or Disable adding a reboot command to fight back against ghostCodes by rebooting the terminal.");


            //Signal Translator Stuff
            signalMessages = Plugin.instance.Config.Bind("Signal Translator", "signalMessages", "RUN, LETS PLAY, BOO, FIND ME, I SEE YOU", "Comma-separated list of messages the ghostGirl will send over the signal translator when sending a code.");
            canSendMessages = Plugin.instance.Config.Bind<bool>("Signal Translator", "canSendMessages", true, "Enable or Disable codes triggering the signal translator to send messages during special codes.");
            messageFrequency = Plugin.instance.Config.Bind("Signal Translator", "messageFrequency", 45, new ConfigDescription("How frequent the signal translator will send messages during code events (percentage)", new AcceptableValueRange<int>(0, 100)));
            onlyUniqueMessages = Plugin.instance.Config.Bind<bool>("Signal Translator", "onlyUniqueMessages", true, "Will ensure the each message sent is unique (as long as there is more than 1 message in signalMessages).");
            onlyGGSendMessages = Plugin.instance.Config.Bind<bool>("Signal Translator", "onlyGGSendMessages", false, "With this setting enabled, signal translator messages will only be sent during codes triggered by the ghost girl");
            
            //Turret Berserk Stuff
            turretNormalBChance = Plugin.instance.Config.Bind("Turret", "turretNormalBChance", 20, new ConfigDescription("How rare it is for the turret to go berserk from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            turretInsaneBChance = Plugin.instance.Config.Bind("Turret", "turretInsaneBChance", 65, new ConfigDescription("How rare it is for the turret to go berserk during rapidFire event.", new AcceptableValueRange<int>(0, 100)));
            mineNormalBChance = Plugin.instance.Config.Bind("Mine", "mineNormalBChance", 10, new ConfigDescription("How rare it is for a mine to blow itself up from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            mineInsaneBChance = Plugin.instance.Config.Bind("Mine", "mineInsaneBChance", 65, new ConfigDescription("How rare it is for a mine to blow itself up during rapidFire event.", new AcceptableValueRange<int>(0, 100)));
            hungryDoorNChance = Plugin.instance.Config.Bind("Blast Door", "hungryDoorNChance", 10, new ConfigDescription("How rare it is for a blast door to start biting from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            hungryDoorIChance = Plugin.instance.Config.Bind("Blast Door", "hungryDoorIChance", 65, new ConfigDescription("How rare it is for a blast door to start biting during rapidFire event.", new AcceptableValueRange<int>(0, 100)));

            //Facility Lights & RapidFire
            ggVanillaBreakerChance = Plugin.instance.Config.Bind("Ghost Girl", "ggVanillaBreakerChance", 20, new ConfigDescription("Percent chance the ghost girl will flip the breakers when beginning a chase.", new AcceptableValueRange<int>(0, 100)));
            ggCodeBreakerChance = Plugin.instance.Config.Bind("Facility Lights", "ggCodeBreakerChance", 3, new ConfigDescription("How rare it is for a ghostCode to flip the breaker and turn off the facility lights.", new AcceptableValueRange<int>(0, 100)));
            rfRapidLights = Plugin.instance.Config.Bind<bool>("RapidFire", "rfRapidLights", true, "Disable this to remove light flashing effect during RapidFire event. (DISABLE THIS IF YOU HAVE SEVERE EPILEPSY)");
            rfRLmin = Plugin.instance.Config.Bind("RapidFire", "rfRLmin", 0.2f, new ConfigDescription("Set shortest possible time between each light flickering effect during rapidFire event.", new AcceptableValueRange<float>(0.1f, 10f)));
            rfRLmax = Plugin.instance.Config.Bind("RapidFire", "rfRLmax", 0.6f, new ConfigDescription("Set longest possible time between each light flickering effect during rapidFire event.", new AcceptableValueRange<float>(0.1f, 20f)));
            rfRLcolorValue = Plugin.instance.Config.Bind("RapidFire", "rfRLcolorValue", "226, 4, 63, 1", "Determine color lights will change to during rapidFire event. (Must be 4 numbers separated by commas. Red, Green, Blue, Alpha)");
            rfRLcolorChange = Plugin.instance.Config.Bind<bool>("RapidFire", "rfRLcolorChange", true, "Disable this to remove changing the color of the lights during RapidFire event.");


            //Regular Facility Doors
            closeAllRegularDoorsEvent = Plugin.instance.Config.Bind<bool>("Regular Doors", "closeAllRegularDoorsEvent", true, "Disable or Enable ghostCodes being able to close all doors in the facility at once");
            closeAllRegularDoorsChance = Plugin.instance.Config.Bind("Regular Doors", "closeAllRegularDoorsChance", 15, new ConfigDescription("How rare it is for the closeAllRegularDoorsEvent to be called from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            openAllRegularDoorsEvent = Plugin.instance.Config.Bind<bool>("Regular Doors", "openAllRegularDoorsEvent", true, "Disable or Enable ghostCodes being able to open all doors in the facility at once");
            openAllRegularDoorsChance = Plugin.instance.Config.Bind("Regular Doors", "openAllRegularDoorsChance", 15, new ConfigDescription("How rare it is for the openAllRegularDoorsChance to be called from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            openSingleDoorEvent = Plugin.instance.Config.Bind<bool>("Regular Doors", "openSingleDoorEvent", true, "Disable or Enable ghostCodes being able to open a single door in the facility at random");
            openSingleDoorChance = Plugin.instance.Config.Bind("Regular Doors", "openSingleDoorChance", 70, new ConfigDescription("How rare it is for the openSingleDoorEvent to be called from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            closeSingleDoorEvent = Plugin.instance.Config.Bind<bool>("Regular Doors", "closeSingleDoorEvent", true, "Disable or Enable ghostCodes being able to close a single door in the facility at random");
            closeSingleDoorChance = Plugin.instance.Config.Bind("Regular Doors", "closeSingleDoorChance", 70, new ConfigDescription("How rare it is for the closeSingleDoorChance to be called from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            lockSingleDoorEvent = Plugin.instance.Config.Bind<bool>("Regular Doors", "lockSingleDoorEvent", true, "Disable or Enable ghostCodes being able to lock a single door in the facility at random");
            lockSingleDoorChance = Plugin.instance.Config.Bind("Regular Doors", "lockSingleDoorChance", 35, new ConfigDescription("How rare it is for the lockSingleDoorChance to be called from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            unlockSingleDoorEvent = Plugin.instance.Config.Bind<bool>("Regular Doors", "unlockSingleDoorEvent", true, "Disable or Enable ghostCodes being able to unlock a single door in the facility at random");
            unlockSingleDoorChance = Plugin.instance.Config.Bind("Regular Doors", "unlockSingleDoorChance", 35, new ConfigDescription("How rare it is for the unlockSingleDoorChance to be called from a normal ghostCode.", new AcceptableValueRange<int>(0, 100)));
            
            //ShipStuff
            //emptyShipEvent = Plugin.instance.Config.Bind<bool>("Ship Stuff", "emptyShipEvent", true, "Disable or Enable ghostCodes being able to suck any players that are currently in the ship");
            //emptyShipChance = Plugin.instance.Config.Bind("Ship Stuff", "emptyShipChance", 25, new ConfigDescription("How rare it is to call the emptyShipEvent ghostcode.", new AcceptableValueRange<int>(0, 100)));
            lightsOnShipEvent = Plugin.instance.Config.Bind<bool>("Ship Stuff", "lightsOnShipEvent", true, "Disable or Enable ghostCodes being able to mess with the lights on the ship");
            lightsOnShipChance = Plugin.instance.Config.Bind("Ship Stuff", "lightsOnShipChance", 25, new ConfigDescription("How rare it is to call the lightsOnShipEvent ghostcode.", new AcceptableValueRange<int>(0, 100)));
            doorsOnShipEvent = Plugin.instance.Config.Bind<bool>("Ship Stuff", "doorsOnShipEvent", true, "Disable or Enable ghostCodes being able to mess with the doors on the ship");
            doorsOnShipChance = Plugin.instance.Config.Bind("Ship Stuff", "doorsOnShipChance", 25, new ConfigDescription("How rare it is to call the doorsOnShipEvent ghostcode.", new AcceptableValueRange<int>(0, 100)));
            monitorsOnShipEvent = Plugin.instance.Config.Bind<bool>("Ship Stuff", "monitorsOnShipEvent", true, "Disable or Enable ghostCodes being able to mess with the monitors on the ship");
            monitorsOnShipChance = Plugin.instance.Config.Bind("Ship Stuff", "monitorsOnShipChance", 25, new ConfigDescription("How rare it is to call the monitorsOnShipEvent ghostcode.", new AcceptableValueRange<int>(0, 100)));
            shockTerminalUserEvent = Plugin.instance.Config.Bind<bool>("Ship Stuff", "shockTerminalUserEvent", true, "Disable or Enable ghostCodes being able to shock any active terminal user on the ship");
            shockTerminalUserChance = Plugin.instance.Config.Bind("Ship Stuff", "shockTerminalUserChance", 25, new ConfigDescription("How rare it is to call the shockTerminalUserEvent ghostcode.", new AcceptableValueRange<int>(0, 100)));
            monitorMessages = Plugin.instance.Config.Bind("Ship Stuff", "monitorMessages", "BEHIND YOU, HAVING FUN?, TAG YOU'RE IT, DANCE FOR ME, IM HIDING, #######, ERROR, DEATH, NO MORE SCRAP", "Comma-separated list of messages the ghostGirl can display on the ship monitors when sending a code.");


            //Misc
            ModNetworking = Plugin.instance.Config.Bind<bool>("__NETWORKING", "ModNetworking", true, "Disable this if you want to disable networking and use this mod as a Host-Only mod. This feature will completely disable GhostGirl Enhanced Mode.");
            
            //Sounds
            gcEnableTerminalSound = Plugin.instance.Config.Bind<bool>("Sounds", "gcEnableTerminalSound", true, "This setting determines whether the terminal will play any sound when a ghost code is entered.");
            gcTerminalSoundChance = Plugin.instance.Config.Bind("Sounds", "gcTerminalSoundChance", 100, new ConfigDescription("How rare it is for ghostCodes to play a sound over the terminal.", new AcceptableValueRange<int>(0, 100)));
            gcAddInsanitySounds = Plugin.instance.Config.Bind("Sounds", "gcAddInsanitySounds", 75, new ConfigDescription("How rare it is for ghostCodes to trigger insanity sounds.", new AcceptableValueRange<int>(0, 100)));
            gcUseGirlSounds = Plugin.instance.Config.Bind<bool>("Sounds", "gcUseGirlSounds", true, "When a ghost girl is present, enabling this will have the terminal play some of the sounds she makes when a ghostCode is run.");
            gcUseTerminalAlarmSound = Plugin.instance.Config.Bind<bool>("Sounds", "gcUseTerminalAlarmSound", true, "Enable or disable the terminal alarm sound playing when a ghostCode is run. NOTE: With networking disabled, this is the only possible sound to play.");


            //TerminalObjects Filter
            gcIgnoreLandmines = Plugin.instance.Config.Bind<bool>("Mine", "gcIgnoreLandmines", false, "Toggle whether ghostCodes ignore landmines");
            gcIgnoreTurrets = Plugin.instance.Config.Bind<bool>("Turret", "gcIgnoreTurrets", false, "Toggle whether ghostCodes ignore turrets");
            gcIgnoreDoors = Plugin.instance.Config.Bind<bool>("Blast Door", "gcIgnoreDoors", false, "Toggle whether ghostCodes ignore blast doors");

            //Mode Settings
            ghostGirlEnhanced = Plugin.instance.Config.Bind<bool>("Mode Settings", "ghostGirlEnhanced", true, "Ghost Girl Enhanced Mode, will replace insanity & normal ghost codes modes");
            useRandomIntervals = Plugin.instance.Config.Bind<bool>("Mode Settings", "useRandomIntervals", true, "Disable this to use the 'set' intervals");
            gcInsanity = Plugin.instance.Config.Bind<bool>("Mode Settings", "gcInsanity", true, "Enable this to have player insanity levels affect the frequency of ghost codes sent.");
            insanityRapidFire = Plugin.instance.Config.Bind<bool>("Mode Settings", "insanityRapidFire", true, "Set this to false to disable rapidFire on Max Insanity (Insanity Mode ONLY)");
            rapidFireCooldown = Plugin.instance.Config.Bind<bool>("Mode Settings", "rapidFireCooldown", true, "rapidFireCooldown adds a cooldown to rapidFire when on Max Insanity (Insanity Mode ONLY)");
            rapidFireMaxHours = Plugin.instance.Config.Bind("Mode Settings", "rapidFireMaxHours", 1, new ConfigDescription("Set the maximum amount of hours for rapidFire to be active when on Max Insanity (Insanity Mode ONLY).", new AcceptableValueRange<int>(1, 12)));

            //Max Codes per round
            gcMinCodes = Plugin.instance.Config.Bind<int>("Codes", "gcMinCodes", 0, "This is the minimum amount of ghost codes that WILL be sent in one round. If you'd like for there to be the possibility of no codes set this to 0.");
            gcMaxCodes = Plugin.instance.Config.Bind<int>("Codes", "gcMaxCodes", 100, "This is the maximum amount of ghost codes that CAN be sent in one round, a random number of codes will be chosen with this value set as the maximum");
            ggIgnoreCodeCount = Plugin.instance.Config.Bind<bool>("Codes", "ggIgnoreCodeCount", true, "Set this to false if you want to have a maximum amount of codes to send when codes depend on hauntings.");
            enableBroadcastEffect = Plugin.instance.Config.Bind<bool>("Codes", "enableBroadcastEffect", true, "Set this to false if you want to disable the code broadcast effect during a ghost code event.");

            //set interval
            gcFirstSetInterval = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcFirstSetInterval", 90, "This is the initial time it takes to start loading codes after landing the ship");
            gcSecondSetInterval = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcSecondSetInterval", 30, "This is the time it takes to load the code once it has been picked");
            gcSetIntervalAC = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcSetIntervalAC", 180, "This is the time it waits to pick another code");
            
            //random interval
            gcFirstRandIntervalMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcFirstRandIntervalMin", 15, "This is the minimum time it should take to start loading codes after landing the ship");
            gcSecondRandIntervalMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcSecondRandIntervalMin", 5, "This is the minimum time it should take to load the code once it has been picked");
            gcRandIntervalACMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcRandIntervalACMin", 20, "This is the minimum time it should wait to pick another code");
            gcFirstRandIntervalMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcFirstRandIntervalMax", 120, "This is the maximum time it should take to start loading codes after landing the ship");
            gcSecondRandIntervalMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcSecondRandIntervalMax", 60, "This is the maximum time it should take to load the code once it has been picked");
            gcRandIntervalACMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcRandIntervalACMax", 240, "This is the maximum time it should wait to pick another code");

            Plugin.GC.LogInfo("ghostCodes config initialized");
            RemoveOrphanedEntries();
            NetworkingCheck();
        }

        private static void NetworkingCheck()
        {
            Plugin.GC.LogInfo("Checking if networking is disabled...");

            if(!ModNetworking.Value)
            {
                List<ConfigEntry<bool>> networkingRequiredConfigOptions = new List<ConfigEntry<bool>>() { fixGhostGirlBreakers, ggEmoteCheck, ggShowerCheck, ggDeathNote, gcRebootTerminal, rfRapidLights, rfRLcolorChange, doorsOnShipEvent, monitorsOnShipEvent, shockTerminalUserEvent, gcUseGirlSounds };

                foreach(ConfigEntry<bool> configItem in networkingRequiredConfigOptions)
                {
                    if(configItem.Value)
                    {
                        configItem.Value = false;
                        Plugin.GC.LogInfo($"Setting {configItem.Definition.Key} to false, networking is disabled.");
                    }
                }
            }

            Plugin.GC.LogInfo("Check complete.");
        }
    
        private static void RemoveOrphanedEntries()
        {
            Plugin.MoreLogs("removing orphaned entries (credits to Kittenji)");
            PropertyInfo orphanedEntriesProp = Plugin.instance.Config.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);

            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(Plugin.instance.Config, null);

            orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
            Plugin.instance.Config.Save(); // Save the config file
        }
    }
}
