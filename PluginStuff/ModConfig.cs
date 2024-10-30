using BepInEx.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace ghostCodes
{
    public static class ModConfig
    {
        //establish config options here
        public static ConfigEntry<bool> ModNetworking { get; internal set; } //For users who only want to use this as the host
        public static ConfigEntry<bool> useRandomIntervals{ get; internal set; } //Toggle whether the intervals between codes are random or set intervals
        public static ConfigEntry<bool> gcIgnoreLandmines{ get; internal set; } //Toggle whether ghostCodes ignore landmines
        public static ConfigEntry<bool> gcIgnoreTurrets{ get; internal set; } //Toggle whether ghostCodes ignore turrets
        public static ConfigEntry<bool> gcIgnoreDoors{ get; internal set; } //Toggle whether ghostCodes ignore doors (this should always be false lol)
        public static ConfigEntry<bool> gcInsanity{ get; internal set; } //Toggle Insanity affected codes
        public static ConfigEntry<bool> ghostGirlEnhanced{ get; internal set; } //Toggle Ghost Girl Enhanced affected codes
        public static ConfigEntry<int> gcFirstSetInterval{ get; internal set; } //First wait configuration
        public static ConfigEntry<int> gcSecondSetInterval{ get; internal set; } //Second wait config
        public static ConfigEntry<int> gcSetIntervalAC{ get; internal set; } //last wait config after code was sent
        public static ConfigEntry<int> gcFirstRandIntervalMin{ get; internal set; } //First wait configuration
        public static ConfigEntry<int> gcFirstRandIntervalMax{ get; internal set; } //First wait configuration
        public static ConfigEntry<int> gcSecondRandIntervalMin{ get; internal set; } //Second wait config
        public static ConfigEntry<int> gcSecondRandIntervalMax{ get; internal set; } //Second wait config
        public static ConfigEntry<int> gcRandIntervalACMin{ get; internal set; } //last wait config after code was sent
        public static ConfigEntry<int> gcRandIntervalACMax{ get; internal set; } //last wait config after code was sent
        public static ConfigEntry<int> gcMaxCodes{ get; internal set; } //Maximum amount of codes to send in one round
        public static ConfigEntry<int> gcMinCodes{ get; internal set; } //Minimum amount of codes to send in one round
        public static ConfigEntry<bool> ggIgnoreCodeCount{ get; internal set; } //Determine whether hauntings can be affected by code count
        public static ConfigEntry<bool> insanityRapidFire{ get; internal set; } //enable/disable rapidfire
        //public static ConfigEntry<bool> rapidFireCooldown{ get; internal set; } //adds time limit to rapidFire during insanity mode
        public static ConfigEntry<int> rapidFireMaxHours{ get; internal set; } //adds time limit to rapidFire during insanity mode, time limit variable
        public static ConfigEntry<bool> fixGhostGirlBreakers{ get; internal set; } // enable/disable the ghostgirl code from vanilla
        public static ConfigEntry<bool> extensiveLogging{ get; internal set; } //enable extensive logging messages

        //Turret Stuff
        public static ConfigEntry<int> turretInsaneBChance{ get; internal set; }
        public static ConfigEntry<int> turretNormalBChance{ get; internal set; }


        //Regular Doors
        public static ConfigEntry<bool> regularDoorsSectionStuff { get; internal set; }
        public static ConfigEntry<bool> openAllRegularDoorsEvent{ get; internal set; }
        public static ConfigEntry<int> openAllRegularDoorsChance{ get; internal set; }
        public static ConfigEntry<bool> closeAllRegularDoorsEvent{ get; internal set; }
        public static ConfigEntry<int> closeAllRegularDoorsChance{ get; internal set; }
        public static ConfigEntry<bool> openSingleDoorEvent{ get; internal set; }
        public static ConfigEntry<int> openSingleDoorChance{ get; internal set; }
        public static ConfigEntry<bool> closeSingleDoorEvent{ get; internal set; }
        public static ConfigEntry<int> closeSingleDoorChance{ get; internal set; }
        public static ConfigEntry<bool> unlockSingleDoorEvent{ get; internal set; }
        public static ConfigEntry<int> unlockSingleDoorChance{ get; internal set; }
        public static ConfigEntry<bool> lockSingleDoorEvent{ get; internal set; }
        public static ConfigEntry<int> lockSingleDoorChance{ get; internal set; }

        //ship stuff
        public static ConfigEntry<bool> shipstuffSection { get; internal set; }
        //public static ConfigEntry<bool> emptyShipEvent{ get; internal set; }
        //public static ConfigEntry<int> emptyShipChance{ get; internal set; }
        public static ConfigEntry<bool> normalTpEvent{ get; internal set; }
        public static ConfigEntry<int> normalTpChance{ get; internal set; }
        public static ConfigEntry<bool> inverseTpEvent{ get; internal set; }
        public static ConfigEntry<int> inverseTpChance{ get; internal set; }
        public static ConfigEntry<bool> lightsOnShipEvent{ get; internal set; }
        public static ConfigEntry<int> lightsOnShipChance{ get; internal set; }
        public static ConfigEntry<bool> doorsOnShipEvent{ get; internal set; }
        public static ConfigEntry<int> doorsOnShipChance{ get; internal set; }
        public static ConfigEntry<bool> monitorsOnShipEvent{ get; internal set; }
        public static ConfigEntry<int> monitorsOnShipChance{ get; internal set; }
        public static ConfigEntry<bool> shockTerminalUserEvent{ get; internal set; }
        public static ConfigEntry<int> shockTerminalUserChance{ get; internal set; }

        //Walkie Talkie Stuff
        public static ConfigEntry<bool> walkieSectionStuff { get; internal set; }
        public static ConfigEntry<bool> ggBreatheOnWalkies { get; internal set; }
        public static ConfigEntry<int> ggBreatheOnWalkiesChance { get; internal set; }
        public static ConfigEntry<bool> gcGarbleWalkies { get; internal set; }
        public static ConfigEntry<int> gcGarbleWalkiesChance { get; internal set; }

        //Item Stuff
        public static ConfigEntry<bool> itemSectionStuff { get; internal set; }
        public static ConfigEntry<bool> gcDrainAllBatteries { get; internal set; }
        public static ConfigEntry<int> gcDrainAllBatteriesChance { get; internal set; }
        public static ConfigEntry<bool> gcDrainHauntedPlayerBatteries { get; internal set; }
        public static ConfigEntry<int> gcDrainHauntedPlayerBatteriesChance { get; internal set; }
        public static ConfigEntry<bool> gcDrainRandomPlayerBatteries { get; internal set; }
        public static ConfigEntry<int> gcDrainRandomPlayerBatteriesChance { get; internal set; }
        public static ConfigEntry<int> gcBatteryDrainPercentage { get; internal set; }


        //Sound System
        public static ConfigEntry<int> gcAddInsanitySounds{ get; internal set; } //Chance insanity sounds are added each time a code is run
        public static ConfigEntry<bool> gcEnableTerminalSound{ get; internal set; } //Toggle whether the terminal makes a sound when a ghost code is submitted
        public static ConfigEntry<int> gcTerminalSoundChance{ get; internal set; } //Chance that a sound is played on the terminal each time a code is run
        public static ConfigEntry<bool> gcUseTerminalAlarmSound{ get; internal set; } //Toggle using the terminal alarm sound
        public static ConfigEntry<bool> gcUseGirlSounds{ get; internal set; } //Toggle using ghost girl audio for codes being run
        

        //Other Action Chances
        public static ConfigEntry<int> hungryDoorIChance{ get; internal set; }
        public static ConfigEntry<int> hungryDoorNChance{ get; internal set; }
        public static ConfigEntry<int> mineInsaneBChance{ get; internal set; }
        public static ConfigEntry<int> mineNormalBChance{ get; internal set; }

        //Insanity Mode Stuff
        public static ConfigEntry<int> sanityPercentL1{ get; internal set; }
        public static ConfigEntry<int> sanityPercentL2{ get; internal set; }
        public static ConfigEntry<int> sanityPercentL3{ get; internal set; }
        public static ConfigEntry<int> sanityPercentMAX{ get; internal set; }
        public static ConfigEntry<int> waitPercentL1{ get; internal set; }
        public static ConfigEntry<int> waitPercentL2{ get; internal set; }
        public static ConfigEntry<int> waitPercentL3{ get; internal set; }
        public static ConfigEntry<int> waitPercentMAX{ get; internal set; }

        //Solo Assist Stuff
        public static ConfigEntry<int> saS1percent{ get; internal set; }
        public static ConfigEntry<int> saS1inside{ get; internal set; }
        public static ConfigEntry<int> saS1outside{ get; internal set; }
        
        public static ConfigEntry<int> saS2percent{ get; internal set; }
        public static ConfigEntry<int> saS2inside{ get; internal set; }
        public static ConfigEntry<int> saS2outside{ get; internal set; }

        public static ConfigEntry<int> saS3percent{ get; internal set; }
        public static ConfigEntry<int> saS3inside{ get; internal set; }
        public static ConfigEntry<int> saS3outside{ get; internal set; }

        //More insanity
        public static ConfigEntry<bool> deathBonus{ get; internal set; }
        public static ConfigEntry<int> deathBonusPercent{ get; internal set; }
        public static ConfigEntry<bool> ggBonus{ get; internal set; }
        public static ConfigEntry<int> ggBonusPercent{ get; internal set; }
        public static ConfigEntry<bool> soloAssist{ get; internal set; }
        public static ConfigEntry<bool> emoteBuff{ get; internal set; }
        public static ConfigEntry<int> emoteBuffPercent{ get; internal set; }

        //ghostGirl stuff
        public static ConfigEntry<bool> GGEbypass{ get; internal set; }
        public static ConfigEntry<string> GGEbypassList{ get; internal set; }
        public static ConfigEntry<bool> gcGhostGirlOnly{ get; internal set; }
        public static ConfigEntry<string> gcGhostGirlOnlyList{ get; internal set; }
        public static ConfigEntry<string> modifiedConfigItemsList { get; internal set; }
        public static ConfigEntry<int> ggPlayerLightsPercent{ get; internal set; } //chance that flashlights toggle
        public static ConfigEntry<int> ggVanillaBreakerChance{ get; internal set; }
        public static ConfigEntry<int> ggCodeBreakerChance{ get; internal set; }
        public static ConfigEntry<int> ggEmoteStopChasingChance{ get; internal set; }
        public static ConfigEntry<int> ggEmoteStopChasePlayers{ get; internal set; }
        public static ConfigEntry<bool> ggEmoteCheck{ get; internal set; }
        public static ConfigEntry<int> ggShowerStopChasingChance{ get; internal set; }
        public static ConfigEntry<bool> ggShowerCheck{ get; internal set; }
        public static ConfigEntry<int> ggDeathNoteChance{ get; internal set; }
        public static ConfigEntry<bool> ggDeathNote{ get; internal set; }
        public static ConfigEntry<int> ggDeathNoteMaxStrikes{ get; internal set; }
        public static ConfigEntry<bool> ggDeathNoteFailChase{ get; internal set; }
        public static ConfigEntry<bool> gcRebootTerminal{ get; internal set; }
        public static ConfigEntry<int> gcRebootEfficacy{ get; internal set; }

        //signal translator
        public static ConfigEntry<bool> canSendMessages{ get; internal set; }
        public static ConfigEntry<bool> onlyUniqueMessages{ get; internal set; }
        public static ConfigEntry<bool> onlyGGSendMessages{ get; internal set; }
        public static ConfigEntry<int> messageFrequency{ get; internal set; }
        public static ConfigEntry<string> signalMessages{ get; internal set; }
        public static ConfigEntry<string> monitorMessages{ get; internal set; }
        public static ConfigEntry<bool> enableBroadcastEffect{ get; internal set; }

        //rapidFire stuff
        public static ConfigEntry<bool> rfRapidLights{ get; internal set; }
        public static ConfigEntry<float> rfRLmin{ get; internal set; }
        public static ConfigEntry<float> rfRLmax{ get; internal set; }
        public static ConfigEntry<string> rfRLcolorValue{ get; internal set; }
        public static ConfigEntry<bool> rfRLcolorChange{ get; internal set; }

        //external mod stuff
        public static ConfigEntry<bool> toilHeadStuff{ get; internal set; }
        public static ConfigEntry<bool> toilHeadTurretDisable{ get; internal set; }
        public static ConfigEntry<int> toilHeadTurretDisableChance{ get; internal set; }
        public static ConfigEntry<bool> toilHeadTurretBerserk{ get; internal set; }
        public static ConfigEntry<int> toilHeadTurretBerserkChance{ get; internal set; }

        public static void SetConfigSettings()
        {

            // NETWORKING
            ModNetworking = MakeBool("__NETWORKING", "ModNetworking", true, "Disable this if you want to disable networking and use this mod as a Host-Only mod. This feature will completely disable GhostGirl Enhanced Mode.");

            // General
            extensiveLogging = MakeBool("General", "extensiveLogging", false, "Enable or Disable extensive log messages for this mod.");

            //Mode Settings
            ghostGirlEnhanced = MakeBool("Mode Settings", "ghostGirlEnhanced", true, "Ghost Girl Enhanced Mode, will replace insanity & normal ghost codes modes");
            useRandomIntervals = MakeBool("Mode Settings", "useRandomIntervals", true, "Disable this to use the 'set' intervals, all modes use intervals on first load.");
            gcInsanity = MakeBool("Mode Settings", "gcInsanity", true, "This will enable insanity mode, a secondary mode that sends ghost codes based on group sanity levels. (Can be used alongside GGE mode with GGEBypass)");
            insanityRapidFire = MakeBool("Mode Settings", "insanityRapidFire", true, "Set this to false to disable rapidFire on Max Insanity (Insanity Mode ONLY)");
            rapidFireMaxHours = MakeClampedInt("Mode Settings", "rapidFireMaxHours", 1, "Set the maximum amount of hours for rapidFire to be active when on Max Insanity (Insanity Mode ONLY).", 1, 24);

            //Code stuff
            gcMinCodes = MakeInt("Codes", "gcMinCodes", 0, "This is the minimum amount of ghost codes that WILL be sent in one round. If you'd like for there to be the possibility of no codes set this to 0.");
            gcMaxCodes = MakeInt("Codes", "gcMaxCodes", 100, "This is the maximum amount of ghost codes that CAN be sent in one round, a random number of codes will be chosen with this value set as the maximum");
            ggIgnoreCodeCount = MakeBool("Codes", "ggIgnoreCodeCount", true, "Set this to false if you want to have a maximum amount of codes to send when codes depend on hauntings in GGE mode.");
            enableBroadcastEffect = MakeBool("Codes", "enableBroadcastEffect", true, "Set this to false if you want to disable the code broadcast effect on the terminal during a ghost code event.");

            //GGE Mode Specific
            GGEbypass = MakeBool("Ghost Girl Enhanced", "GGEbypass", true, "Enable or Disable bypassing Ghost Girl Enhanced on moons listed in GGEbypassList."); 
            GGEbypassList = MakeString("Ghost Girl Enhanced", "GGEbypassList", "Vow,Offense,March,Embrion", "When using Ghost Girl Enhanced mode, this comma-separated list of moons will be bypassed for the next enabled mode (set to moons that the ghost girl cant spawn on by default).");
            gcGhostGirlOnly = MakeBool("Ghost Girl Enhanced", "gcGhostGirlOnly", false, "When enabled, any config options listed in [gcGhostGirlOnlyList] will be set to disabled when GhostGirlEnhanced Mode is not active.");
            gcGhostGirlOnlyList = MakeString("Ghost Girl Enhanced", "gcGhostGirlOnlyList", "ggShowerCheck,ggDeathNote,gcRebootTerminal", "when gcGhostGirlOnly is enabled, this comma-separated list of config options will be disabled when landing on moons in [GGEbypassList].");
            modifiedConfigItemsList = MakeString("Ghost Girl Enhanced", "modifiedConfigItemsList", "", "===[[[***DONT TOUCH THIS***]]]=== when gcGhostGirlOnly is enabled, this list is used to set config items back to what they were before being modified by [gcGhostGirlOnlyList].");

            //GhostGirl Interactions
            fixGhostGirlBreakers = MakeBool("Ghost Girl Interactions", "fixGhostGirlBreakers", true, "Fix the vanilla code so that the ghost girl can flip the breakers at the start of a chase.");
            ggVanillaBreakerChance = MakeClampedInt("Ghost Girl Interactions", "ggVanillaBreakerChance", 20, "Percent chance the ghost girl will flip the breakers when beginning a chase.", 0, 100);
            ggPlayerLightsPercent = MakeClampedInt("Ghost Girl Interactions", "ggPlayerLightsPercent", 45, "Set the percentage chance that the ghostGirl will flicker a player's lights during a ghostCode event.", 0, 100);

            //emote
            ggEmoteCheck = MakeBool("Ghost Girl Interactions", "ggEmoteCheck", true, "Enable or Disable emoting to stop the ghost girl from chasing you.");
            ggEmoteStopChasingChance = MakeClampedInt("Ghost Girl Interactions", "ggEmoteStopChasingChance", 95, "How effective emoting to get the ghost girl to stop chasing you is.", 0, 100);
            ggEmoteStopChasePlayers = MakeClampedInt("Ghost Girl Interactions", "ggEmoteStopChasePlayers", 75, "Percentage of living players required to stop ghost girl from chasing.", 0, 100);
            
            //shower use
            ggShowerCheck = MakeBool("Ghost Girl Interactions", "ggShowerCheck", true, "Enable or Disable taking a shower to stop the ghost girl from chasing you.");
            ggShowerStopChasingChance = MakeClampedInt("Ghost Girl Interactions", "ggShowerStopChasingChance", 95, "How effective taking a shower to get the ghost girl to stop chasing you is.", 0, 100);

            //deathNote
            ggDeathNote = MakeBool("Ghost Girl Interactions", "ggDeathNote", true, "Enable or Disable typing a name in the terminal to transfer the ghost girl to another player.");
            ggDeathNoteChance = MakeClampedInt("Ghost Girl Interactions", "ggDeathNoteChance", 65, "How effective typing a player's name in the terminal is to transfer the haunting.", 0, 100);
            ggDeathNoteMaxStrikes = MakeClampedInt("Ghost Girl Interactions", "ggDeathNoteMaxStrikes", 3, "Amount of times you can attempt to use the death note to transfer hauntings, use -1 for infinite attempts.", -1, 25);
            ggDeathNoteFailChase = MakeBool("Ghost Girl Interactions", "ggDeathNoteFailChase", true, "Enable or Disable triggering a ghost girl chase on failed attempt to transfer the haunting.");

            //Terminal Reboot
            gcRebootTerminal = MakeBool("Terminal Reboot", "gcRebootTerminal", true, "Enable or Disable adding a reboot command to fight back against ghostCodes by rebooting the terminal.");
            gcRebootEfficacy = MakeClampedInt("Terminal Reboot", "ggRebootEfficacy", 75, "How effective rebooting the terminal is to delay ghostCodes.", 0, 100);

            //Signal Translator Stuff
            canSendMessages = MakeBool("Signal Translator", "canSendMessages", true, "Enable or Disable codes triggering the signal translator to send messages during special codes.");
            signalMessages = MakeString("Signal Translator", "signalMessages", "RUN, LETS PLAY, BOO, FIND ME, I SEE YOU", "Comma-separated list of messages the ghostGirl will send over the signal translator when sending a code.");
            messageFrequency = MakeClampedInt("Signal Translator", "messageFrequency", 45, "How frequent the signal translator will send messages during code events (percentage)", 0, 100);
            onlyUniqueMessages = MakeBool("Signal Translator", "onlyUniqueMessages", true, "Will ensure the each message sent is unique (as long as there is more than 1 message in signalMessages).");
            onlyGGSendMessages = MakeBool("Signal Translator", "onlyGGSendMessages", false, "With this setting enabled, signal translator messages will only be sent during codes triggered by the ghost girl");

            //Turret Stuff
            gcIgnoreTurrets = MakeBool("Turret", "gcIgnoreTurrets", false, "Toggle whether ghostCodes ignore turrets");
            turretNormalBChance = MakeClampedInt("Turret", "turretNormalBChance", 20, "How rare it is for the turret to go berserk from a normal ghostCode.", 0, 100);
            turretInsaneBChance = MakeClampedInt("Turret", "turretInsaneBChance", 65, "How rare it is for the turret to go berserk during rapidFire event.", 0, 100);

            //Mine Stuff
            gcIgnoreLandmines = MakeBool("Mine", "gcIgnoreLandmines", false, "Toggle whether ghostCodes ignore landmines");
            mineNormalBChance = MakeClampedInt("Mine", "mineNormalBChance", 10, "How rare it is for a mine to blow itself up from a normal ghostCode.", 0, 100);
            mineInsaneBChance = MakeClampedInt("Mine", "mineInsaneBChance", 65, "How rare it is for a mine to blow itself up during rapidFire event.", 0, 100);

            //Blast Door Stuff
            gcIgnoreDoors = MakeBool("Blast Door", "gcIgnoreDoors", false, "Toggle whether ghostCodes ignore blast doors");
            hungryDoorNChance = MakeClampedInt("Blast Door", "hungryDoorNChance", 10, "How rare it is for a blast door to start biting from a normal ghostCode.", 0, 100);
            hungryDoorIChance = MakeClampedInt("Blast Door", "hungryDoorIChance", 65, "How rare it is for a blast door to start biting during rapidFire event.", 0, 100);

            //Facility Lights & RapidFire
            ggCodeBreakerChance = MakeClampedInt("Facility Lights", "ggCodeBreakerChance", 3, "How rare it is for a ghostCode to flip the breaker and turn off the facility lights.", 0, 100);
            rfRapidLights = MakeBool("RapidFire", "rfRapidLights", true, "Disable this to remove light flashing effect during RapidFire event. (DISABLE THIS IF YOU HAVE SEVERE EPILEPSY)");
            rfRLmin = MakeClampedFloat("RapidFire", "rfRLmin", 0.2f, "Set shortest possible time between each light flickering effect during rapidFire event.", 0.1f, 10f);
            rfRLmax = MakeClampedFloat("RapidFire", "rfRLmax", 0.6f, "Set longest possible time between each light flickering effect during rapidFire event.", 0.1f, 20f);
            rfRLcolorChange = MakeBool("RapidFire", "rfRLcolorChange", true, "Disable this to remove changing the color of the lights during RapidFire event.");
            rfRLcolorValue = MakeString("RapidFire", "rfRLcolorValue", "226, 4, 63, 1", "Determine color lights will change to during rapidFire event. (Must be 4 numbers separated by commas. Red, Green, Blue, Alpha)");


            //Regular Facility Doors
            regularDoorsSectionStuff = MakeBool("Regular Doors", "regularDoorsSectionStuff", true, "Disable or Enable this entire section. (Regular Doors)");
            closeAllRegularDoorsEvent = MakeBool("Regular Doors", "closeAllRegularDoorsEvent", true, "Disable or Enable ghostCodes being able to close all doors in the facility at once");
            closeAllRegularDoorsChance = MakeClampedInt("Regular Doors", "closeAllRegularDoorsChance", 15, "How rare it is for the closeAllRegularDoorsEvent to be called from a normal ghostCode.", 0, 100);
            openAllRegularDoorsEvent = MakeBool("Regular Doors", "openAllRegularDoorsEvent", true, "Disable or Enable ghostCodes being able to open all doors in the facility at once");
            openAllRegularDoorsChance = MakeClampedInt("Regular Doors", "openAllRegularDoorsChance", 15, "How rare it is for the openAllRegularDoorsChance to be called from a normal ghostCode.", 0, 100);
            openSingleDoorEvent = MakeBool("Regular Doors", "openSingleDoorEvent", true, "Disable or Enable ghostCodes being able to open a single door in the facility at random");
            openSingleDoorChance = MakeClampedInt("Regular Doors", "openSingleDoorChance", 70, "How rare it is for the openSingleDoorEvent to be called from a normal ghostCode.", 0, 100);
            closeSingleDoorEvent = MakeBool("Regular Doors", "closeSingleDoorEvent", true, "Disable or Enable ghostCodes being able to close a single door in the facility at random");
            closeSingleDoorChance = MakeClampedInt("Regular Doors", "closeSingleDoorChance", 70, "How rare it is for the closeSingleDoorChance to be called from a normal ghostCode.", 0, 100);
            lockSingleDoorEvent = MakeBool("Regular Doors", "lockSingleDoorEvent", true, "Disable or Enable ghostCodes being able to lock a single door in the facility at random");
            lockSingleDoorChance = MakeClampedInt("Regular Doors", "lockSingleDoorChance", 35, "How rare it is for the lockSingleDoorChance to be called from a normal ghostCode.", 0, 100);
            unlockSingleDoorEvent = MakeBool("Regular Doors", "unlockSingleDoorEvent", true, "Disable or Enable ghostCodes being able to unlock a single door in the facility at random");
            unlockSingleDoorChance = MakeClampedInt("Regular Doors", "unlockSingleDoorChance", 35, "How rare it is for the unlockSingleDoorChance to be called from a normal ghostCode.", 0, 100);

            //ShipStuff
            shipstuffSection = MakeBool("Ship Stuff", "shipstuffSection", true, "Disable or Enable this entire section. (Ship Stuff)");
            //emptyShipEvent = Plugin.instance.Config.Bind<bool>("Ship Stuff", "emptyShipEvent", true, "Disable or Enable ghostCodes being able to suck any players that are currently in the ship");
            //emptyShipChance = Plugin.instance.Config.Bind("Ship Stuff", "emptyShipChance", 25, new ConfigDescription("How rare it is to call the emptyShipEvent ghostcode.", new AcceptableValueRange<int>(0, 100)));
            normalTpEvent = MakeBool("Ship Stuff", "normalTpEvent", true, "Disable or Enable ghostCodes being able to mess with the normal teleporter");
            normalTpChance = MakeClampedInt("Ship Stuff", "normalTpChance", 5, "How rare it is to call the normalTpEvent ghostcode.", 0, 100);
            inverseTpEvent = MakeBool(  "Ship Stuff", "inverseTpEvent", true, "Disable or Enable ghostCodes being able to mess with the inverse teleporter");
            inverseTpChance = MakeClampedInt("Ship Stuff", "inverseTpChance", 15, "How rare it is to call the inverseTpEvent ghostcode.", 0, 100);
            lightsOnShipEvent = MakeBool("Ship Stuff", "lightsOnShipEvent", true, "Disable or Enable ghostCodes being able to mess with the lights on the ship");
            lightsOnShipChance = MakeClampedInt("Ship Stuff", "lightsOnShipChance", 30, "How rare it is to call the lightsOnShipEvent ghostcode.", 0, 100);
            doorsOnShipEvent = MakeBool("Ship Stuff", "doorsOnShipEvent", true, "Disable or Enable ghostCodes being able to mess with the doors on the ship");
            doorsOnShipChance = MakeClampedInt("Ship Stuff", "doorsOnShipChance", 30, "How rare it is to call the doorsOnShipEvent ghostcode.", 0, 100);
            monitorsOnShipEvent = MakeBool("Ship Stuff", "monitorsOnShipEvent", true, "Disable or Enable ghostCodes being able to mess with the monitors on the ship and add custom messages [monitorMessages]");
            monitorsOnShipChance = MakeClampedInt("Ship Stuff", "monitorsOnShipChance", 45, "How rare it is to call the monitorsOnShipEvent ghostcode.", 0, 100);
            monitorMessages = MakeString("Ship Stuff", "monitorMessages", "BEHIND YOU, HAVING FUN?, TAG YOU'RE IT, DANCE FOR ME, IM HIDING, #######, ERROR, DEATH, NO MORE SCRAP", "Comma-separated list of messages the ghostGirl can display on the ship monitors when sending a code.");
            shockTerminalUserEvent = MakeBool("Ship Stuff", "shockTerminalUserEvent", true, "Disable or Enable ghostCodes being able to shock any active terminal user on the ship");
            shockTerminalUserChance = MakeClampedInt("Ship Stuff", "shockTerminalUserChance", 10, "How rare it is to call the shockTerminalUserEvent ghostcode.", 0, 100);

            //walkies
            walkieSectionStuff = MakeBool("Walkies", "walkieSectionStuff", true, "Disable or Enable this entire section. (Walkies)");
            ggBreatheOnWalkies = MakeBool("Walkies", "ggBreatheOnWalkies", true, "Disable or Enable ghost girl transmiting the ghost girl breathing sound over all walkies");
            ggBreatheOnWalkiesChance = MakeClampedInt("Walkies", "ggBreatheOnWalkiesChance", 35, "How rare it is to call the ggBreatheOnWalkies ghostcode.", 0, 100);
            gcGarbleWalkies = MakeBool("Walkies", "gcGarbleWalkies", true, "Disable or Enable ghost codes being able to garble the walkie talkies transmissions for a period of time");
            gcGarbleWalkiesChance = MakeClampedInt("Walkies", "gcGarbleWalkiesChance", 15, "How rare it is to call the gcGarbleWalkies ghostcode.", 0, 100);



            //Items
            itemSectionStuff = MakeBool("Items", "itemSectionStuff", true, "Disable or Enable this entire section. (Items)");
            gcDrainAllBatteries = MakeBool("Items", "gcDrainAllBatteries", true, "Disable or Enable ghost codes being able to drain the battery of all items in all player's inventories as long as they're not on the ship.");
            gcDrainAllBatteriesChance = MakeClampedInt("Items", "gcDrainAllBatteriesChance", 5, "How rare it is to call the gcDrainAllBatteries ghostcode.", 0, 100);
            gcDrainHauntedPlayerBatteries = MakeBool("Items", "gcDrainHauntedPlayerBatteries", true, "Disable or Enable ghost codes being able to drain the battery of all items in a haunted player's inventories as long as they're not on the ship.");
            gcDrainHauntedPlayerBatteriesChance = MakeClampedInt("Items", "gcDrainHauntedPlayerBatteriesChance", 10, "How rare it is to call the gcDrainHauntedPlayerBatteries ghostcode.", 0, 100);
            gcDrainRandomPlayerBatteries = MakeBool("Items", "gcDrainRandomPlayerBatteries", true, "Disable or Enable ghost codes being able to drain the battery of all items in a random player's inventories as long as they're not on the ship.");
            gcDrainRandomPlayerBatteriesChance = MakeClampedInt("Items", "gcDrainRandomPlayerBatteriesChance", 10, "How rare it is to call the gcDrainRandomPlayerBatteries ghostcode.", 0, 100);
            gcBatteryDrainPercentage = MakeClampedInt("Items", "gcBatteryDrainPercentage", 2, "Percentage of battery to instantly drain when any of the battery drain ghostcodes are called.", 0, 100);

            //Sounds
            gcEnableTerminalSound = MakeBool("Sounds", "gcEnableTerminalSound", true, "This setting determines whether the terminal will play any sound when a ghost code is entered.");
            gcUseTerminalAlarmSound = MakeBool("Sounds", "gcUseTerminalAlarmSound", true, "Enable or disable the terminal alarm sound playing when a ghostCode is run. NOTE: With networking disabled, this is the only possible sound to play.");
            gcTerminalSoundChance = MakeClampedInt("Sounds", "gcTerminalSoundChance", 100, "How rare it is for ghostCodes to play a sound over the terminal.", 0, 100);
            gcAddInsanitySounds = MakeClampedInt("Sounds", "gcAddInsanitySounds", 75, "How rare it is for ghostCodes to trigger insanity sounds.", 0, 100);
            gcUseGirlSounds = MakeBool("Sounds", "gcUseGirlSounds", true, "When a ghost girl is present, enabling this will have the terminal play some of the sounds she makes when a ghostCode is run.");
            
            //set interval
            gcFirstSetInterval = MakeInt("Set Interval Configurations", "gcFirstSetInterval", 90, "This is the initial time it takes to start loading codes after landing the ship");
            gcSecondSetInterval = MakeInt("Set Interval Configurations", "gcSecondSetInterval", 30, "This is the time it takes to load the code once it has been picked");
            gcSetIntervalAC = MakeInt("Set Interval Configurations", "gcSetIntervalAC", 180, "This is the time it waits to pick another code");
            
            //random interval
            gcFirstRandIntervalMin = MakeInt("Random Interval Configurations", "gcFirstRandIntervalMin", 15, "This is the minimum time it should take to start loading codes after landing the ship");
            gcSecondRandIntervalMin = MakeInt("Random Interval Configurations", "gcSecondRandIntervalMin", 5, "This is the minimum time it should take to load the code once it has been picked");
            gcRandIntervalACMin = MakeInt("Random Interval Configurations", "gcRandIntervalACMin", 20, "This is the minimum time it should wait to pick another code");
            gcFirstRandIntervalMax = MakeInt("Random Interval Configurations", "gcFirstRandIntervalMax", 120, "This is the maximum time it should take to start loading codes after landing the ship");
            gcSecondRandIntervalMax = MakeInt("Random Interval Configurations", "gcSecondRandIntervalMax", 60, "This is the maximum time it should take to load the code once it has been picked");
            gcRandIntervalACMax = MakeInt("Random Interval Configurations", "gcRandIntervalACMax", 240, "This is the maximum time it should wait to pick another code");

            //Insanity Mode Values
            sanityPercentL1 = MakeClampedInt("Insanity Mode", "sanityPercentL1", 25, "Set the percentage of the maximum sanity level required to reach Level 1 of Insanity Mode.", 0, 100);
            sanityPercentL2 = MakeClampedInt("Insanity Mode", "sanityPercentL2", 50, "Set the percentage of the maximum sanity level required to reach Level 2 of Insanity Mode.", 0, 100);
            sanityPercentL3 = MakeClampedInt("Insanity Mode", "sanityPercentL3", 75, "Set the percentage of the maximum sanity level required to reach Level 3 of Insanity Mode.", 0, 100);
            sanityPercentMAX = MakeClampedInt("Insanity Mode", "sanityPercentMAX", 95, "Set the percentage of the maximum sanity level required to reach MAX Level of Insanity Mode and trigger rapid fire.", 0, 100);
            waitPercentL1 = MakeClampedInt("Insanity Mode", "waitPercentL1", 90, "Set the percentage of the wait time to use after reaching Level 1 of Insanity Mode.", 0, 100);
            waitPercentL2 = MakeClampedInt("Insanity Mode", "waitPercentL2", 50, "Set the percentage of the wait time to use after reaching Level 2 of Insanity Mode.", 0, 100);
            waitPercentL3 = MakeClampedInt("Insanity Mode", "waitPercentL3", 10, "Set the percentage of the wait time to use after reaching Level 3 of Insanity Mode.", 0, 100);
            waitPercentMAX = MakeClampedInt("Insanity Mode", "waitPercentMAX", 2, "Set the percentage of the wait time to use after reaching Max Level of Insanity Mode. (This is only triggered if rapidFire is disabled)", 0, 100);

            //Insanity Buffs and Debuffs
            deathBonus = MakeBool("Insanity Mode", "deathBonus", false, "Toggle whether player deaths adds an insanity level bonus or not");
            deathBonusPercent = MakeClampedInt("Insanity Mode", "deathBonusPercent", 10, "Percentage of current group insanity value that will be added for each dead player. (example: if group insanity value is 100 and this is set to 10, then for each dead player 10 will be added to 100. So if there are 4 players in the group and 2 are dead the new group sanity will be 120)", 0, 100);
            ggBonus = MakeBool("Insanity Mode", "ggBonus", false, "Toggle whether the ghost girl being spawned adds an insanity level bonus or not");
            ggBonusPercent = MakeClampedInt("Insanity Mode", "ggBonusPercent", 10, "Percentage of current group insanity value to increase for the group once a ghostGirl has been spawned. (example: if group insanity value is 100 and this is set to 10, then group insanity value will be 110 when a ghost girl is detected)", 0, 100);
            emoteBuff = MakeBool("Insanity Mode", "emoteBuff", true, "Enable this to lower sanity whenever any living player is emoting.");
            emoteBuffPercent = MakeClampedInt("Insanity Mode", "emoteBuffPercent", 25, "Percentage of current group insanity value to decrease from the group for each person that is emoting. (example: if group insanity value is 100 and this is set to 10, then for each player that is emoting 10 will be subtracted from 100. So if there are 4 players in the group and 2 are emoting the new group sanity will be 80)", 0, 100);

            //Solo Assist Debuff Values
            soloAssist = MakeBool("Solo Assistance", "soloAssist", true, "Enable this setting to reduce Insanity gains during solo play");
            saS1percent = MakeClampedInt("Solo Assistance", "saS1percent", 50, "Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 1 (beginning of day to Noon)", 0, 100);
            saS1inside = MakeClampedInt("Solo Assistance", "saS1inside", 20, "Decrease of insanity level when inside the factory during Stage 1 (beginning of day to Noon)", 0, 40);
            saS1outside = MakeClampedInt("Solo Assistance", "saS1outside", 25, "Decrease of insanity level when outside the factory during Stage 1 (beginning of day to Noon)", 0, 40);
            saS2percent = MakeClampedInt("Solo Assistance", "saS2percent", 75, "Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 2 (Noon to 5PM)", 0, 100);
            saS2inside = MakeClampedInt("Solo Assistance", "saS2inside", 10, "Decrease of insanity level when inside the factory during Stage 2 (Noon to 5PM)", 0, 40);
            saS2outside = MakeClampedInt("Solo Assistance", "saS2outside", 15, "Decrease of insanity level when outside the factory during Stage 2 (Noon to 5PM)", 0, 40);
            saS3percent = MakeClampedInt("Solo Assistance", "saS3percent", 90, "Percentage of Max Insanity required to hit Solo Assistance Buff during Stage 3 (5PM - 10PM)", 0, 100);
            saS3inside = MakeClampedInt("Solo Assistance", "saS3inside", 5, "Decrease of insanity level when inside the factory during Stage 3 (5PM - 10PM)", 0, 40);
            saS3outside = MakeClampedInt("Solo Assistance", "saS3outside", 10, "Decrease of insanity level when outside the factory during Stage 3 (5PM - 10PM)", 0, 40);

            //external mod stuff
            toilHeadStuff = MakeBool("External Mod Stuff", "toilHeadStuff", false, "If ToilHead by Zehs is detected, check for any living toilheads and add their turrets to potential ghost code interactions.");
            toilHeadTurretDisable = MakeBool("External Mod Stuff", "toilHeadTurretDisable", false, "If ToilHead by Zehs is detected, allows ghost codes to disable the turret on top of the coil head.");
            toilHeadTurretDisableChance = MakeClampedInt("External Mod Stuff", "toilHeadTurretDisableChance", 30, "Chance of toilHeadTurretDisable being called in a ghostCode.", 0, 100);
            toilHeadTurretBerserk = MakeBool("External Mod Stuff", "toilHeadTurretBerserk", false, "If ToilHead by Zehs is detected, allows ghost codes to set the turret on top of the coil head into berserk mode.");
            toilHeadTurretBerserkChance = MakeClampedInt("External Mod Stuff", "toilHeadTurretBerserkChance", 5, "Chance of toilHeadTurretBerserk being called in a ghostCode.", 0, 100);


            Plugin.GC.LogInfo("ghostCodes config initialized");
            RemoveOrphanedEntries();
            NetworkingCheck();
        }

        private static ConfigEntry<bool> MakeBool(string section, string configItemName, bool defaultValue, string configDescription)
        {
            return Plugin.instance.Config.Bind<bool>(section, configItemName, defaultValue, configDescription);
        }

        private static ConfigEntry<int> MakeInt(string section, string configItemName, int defaultValue, string configDescription)
        {
            return Plugin.instance.Config.Bind<int>(section, configItemName, defaultValue, configDescription);
        }

        private static ConfigEntry<string> MakeClampedString(string section, string configItemName, string defaultValue, string configDescription, AcceptableValueList<string> acceptedValues)
        {
            return Plugin.instance.Config.Bind(section, configItemName, defaultValue, new ConfigDescription(configDescription, acceptedValues));
        }

        private static ConfigEntry<int> MakeClampedInt(string section, string configItemName, int defaultValue, string configDescription, int minValue, int maxValue)
        {
            return Plugin.instance.Config.Bind(section, configItemName, defaultValue, new ConfigDescription(configDescription, new AcceptableValueRange<int>(minValue, maxValue)));
        }

        private static ConfigEntry<float> MakeClampedFloat(string section, string configItemName, float defaultValue, string configDescription, float minValue, float maxValue)
        {
            return Plugin.instance.Config.Bind(section, configItemName, defaultValue, new ConfigDescription(configDescription, new AcceptableValueRange<float>(minValue, maxValue)));
        }

        private static ConfigEntry<string> MakeString(string section, string configItemName, string defaultValue, string configDescription)
        {
            //monitorMessages = Plugin.instance.Config.Bind("Ship Stuff", "monitorMessages", "BEHIND YOU, HAVING FUN?, TAG YOU'RE IT, DANCE FOR ME, IM HIDING, #######, ERROR, DEATH, NO MORE SCRAP", "Comma-separated list of messages the ghostGirl can display on the ship monitors when sending a code.");

            return Plugin.instance.Config.Bind(section, configItemName, defaultValue, configDescription);
        }

        private static void NetworkingCheck()
        {
            Plugin.GC.LogInfo("Checking if networking is disabled...");

            if(!ModNetworking.Value)
            {
                List<ConfigEntry<bool>> networkingRequiredConfigOptions = new List<ConfigEntry<bool>>() { fixGhostGirlBreakers, ggEmoteCheck, ggShowerCheck, ggDeathNote, gcRebootTerminal, rfRapidLights, rfRLcolorChange, monitorsOnShipEvent, shockTerminalUserEvent, gcUseGirlSounds, walkieSectionStuff, itemSectionStuff };

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
