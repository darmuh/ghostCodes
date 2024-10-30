using BepInEx;
using BepInEx.Configuration;
using System.IO;
using static OpenLib.ConfigManager.ConfigSetup;
using static OpenLib.ConfigManager.WebHelper;

namespace ghostCodes.Configs
{
    public static class InteractionsConfig
    {
        public static ConfigFile Interactions = new(Path.Combine(Paths.ConfigPath, $"darmuh.ghostCodes.Interactions.cfg"), true);

        // MainInteractions //
        public static ConfigEntry<int> TurretBerserk { get; internal set; }
        public static ConfigEntry<int> HungryBlastDoor { get; internal set; }
        public static ConfigEntry<int> MineBoom { get; internal set; }
        public static ConfigEntry<int> PlayerLights { get; internal set; } //chance that flashlights toggle
        public static ConfigEntry<int> FlipBreaker { get; internal set; }
        public static ConfigEntry<int> SignalTranslator { get; internal set; }
        public static ConfigEntry<int> HauntFactoryScrap { get; internal set; }
        public static ConfigEntry<int> HauntHeldScrap { get; internal set; }

        // DoorInteractions //
        public static ConfigEntry<int> ToggleAllRegularDoors { get; internal set; }
        public static ConfigEntry<int> ToggleRegularDoor { get; internal set; }
        public static ConfigEntry<int> UnlockSingleDoor { get; internal set; }
        public static ConfigEntry<int> LockSingleDoor { get; internal set; }
        public static ConfigEntry<int> TryHauntSingleDoor { get; internal set; } //NEW
        public static ConfigEntry<int> TryHauntHalfAllDoors { get; internal set; } //NEW
        public static ConfigEntry<int> TryHauntAllDoors { get; internal set; } //NEW

        // ShipInteractions //
        public static ConfigEntry<int> TeleportPlayer { get; internal set; }
        public static ConfigEntry<int> InverseTeleporter { get; internal set; }
        public static ConfigEntry<int> LightsOnShip { get; internal set; }
        public static ConfigEntry<int> DoorsOnShip { get; internal set; }
        public static ConfigEntry<int> MonitorsOnShip { get; internal set; }
        public static ConfigEntry<int> ShockTerminalUser { get; internal set; }
        public static ConfigEntry<int> CorruptedCredits { get; internal set; }
        public static ConfigEntry<int> HeavyLever { get; internal set; }

        // HauntingInteractions //
        public static ConfigEntry<int> BreatheOnWalkies { get; internal set; }
        public static ConfigEntry<int> GarbleWalkies { get; internal set; }
        public static ConfigEntry<int> AffectAllBatteries { get; internal set; }
        public static ConfigEntry<int> AffectHauntedPlayerBatteries { get; internal set; }
        public static ConfigEntry<int> AffectRandomPlayerBatteries { get; internal set; }

        // CruiserInteractions // **ALL NEW**
        public static ConfigEntry<int> ChangeCruiserRadio { get; internal set; }
        public static ConfigEntry<int> CruiserEjectDriver { get; internal set; }
        public static ConfigEntry<int> CruiserUseBoost { get; internal set; }
        public static ConfigEntry<int> ToggleCruiserLights { get; internal set; }
        public static ConfigEntry<int> FlickerCruiserLights { get; internal set; }
        public static ConfigEntry<int> ToggleCruiserHood { get; internal set; }
        public static ConfigEntry<int> CruiserWindshield { get; internal set; }
        public static ConfigEntry<int> CruiserPush { get; internal set; }
        public static ConfigEntry<int> ToggleCruiserDoors { get; internal set; }
        public static ConfigEntry<int> CruiserShiftGears { get; internal set; }


        // CounterPlay //
        public static ConfigEntry<int> EmoteStopChasing { get; internal set; }
        public static ConfigEntry<int> EmoteStopChaseRequiredPlayers { get; internal set; }
        public static ConfigEntry<int> ShowerStopChasing { get; internal set; }
        public static ConfigEntry<int> DeathNote { get; internal set; }
        public static ConfigEntry<int> DeathNoteMaxStrikes { get; internal set; }
        public static ConfigEntry<bool> DeathNoteFailChase { get; internal set; }
        public static ConfigEntry<int> TerminalReboot { get; internal set; }

        // Modded Interactions //
        public static ConfigEntry<int> ToilHeadTurretDisable { get; internal set; }
        public static ConfigEntry<int> ToilHeadTurretBerserk { get; internal set; }

        // InteractionModifiers //
        public static ConfigEntry<int> BatteryPercentageModifier { get; internal set; }
        public static ConfigEntry<bool> OnlyUniqueMessages { get; internal set; }
        public static ConfigEntry<string> AllSignalMessages { get; internal set; }
        public static ConfigEntry<string> AllMonitorMessages { get; internal set; }

        internal static void Init()
        {
            // MainInteractions //

            TurretBerserk = MakeClampedInt(Interactions, "MainInteractions", "TurretBerserk", 20, "How rare it is for the turret to go berserk from a ghostCode.", 0, 100);
            HungryBlastDoor = MakeClampedInt(Interactions, "MainInteractions", "HungryBlastDoor", 10, "How rare it is for a blast door to start biting from a ghostCode.", 0, 100);
            MineBoom = MakeClampedInt(Interactions, "MainInteractions", "MineBoom", 10, "How rare it is for a mine to blow itself up from a ghostCode.", 0, 100);
            PlayerLights = MakeClampedInt(Interactions, "MainInteractions", "PlayerLights", 45, "Set the percentage chance that the ghostGirl will flicker a player's lights during a ghostCode event.", 0, 100);
            FlipBreaker = MakeClampedInt(Interactions, "MainInteractions", "FlipBreaker", 3, "How rare it is for a ghostCode to flip the breaker and turn off the facility lights.", 0, 100);
            SignalTranslator = MakeClampedInt(Interactions, "MainInteractions", "SignalTranslator", 45, "How frequent the signal translator will send messages during code events (percentage)", 0, 100);
            HauntFactoryScrap = MakeClampedInt(Interactions, "MainInteractions", "HauntFactoryScrap", 25, "Chance of a ghostcode interacting with random scrap items within the factory", 0, 100);
            HauntHeldScrap = MakeClampedInt(Interactions, "MainInteractions", "HauntHeldScrap", 25, "Chance of a ghostcode interacting with random scrap that is held by any player", 0, 100);


            // DoorInteractions //

            ToggleAllRegularDoors = MakeClampedInt(Interactions, "DoorInteractions", "ToggleAllRegularDoors", 15, "How rare it is for a ghostcode to close or open ALL doors.", 0, 100);
            ToggleRegularDoor = MakeClampedInt(Interactions, "DoorInteractions", "ToggleRegularDoor", 70, "How rare it is for a ghostcode to close or open a regular door.", 0, 100);
            UnlockSingleDoor = MakeClampedInt(Interactions, "DoorInteractions", "UnlockSingleDoor", 35, "How rare it is for the unlockSingleDoorChance to be called from a ghostCode.", 0, 100);
            LockSingleDoor = MakeClampedInt(Interactions, "DoorInteractions", "lockSingleDoorChance", 35, "How rare it is for the lockSingleDoorChance to be called from a ghostCode.", 0, 100);
            TryHauntSingleDoor = MakeClampedInt(Interactions, "DoorInteractions", "TryHauntSingleDoor", 35, "How rare it is to try to haunt a single door (rapidly open/shut) from a ghostCode.\nThis utilizes the function added by zeekers which does not guarantee a haunted door.", 0, 100);
            TryHauntHalfAllDoors = MakeClampedInt(Interactions, "DoorInteractions", "TryHauntHalfAllDoors", 30, "How rare it is to try to haunt half of all inside doors (rapidly open/shut) from a ghostCode.\nThis utilizes the function added by zeekers which does not guarantee a haunted door.", 0, 100);
            TryHauntAllDoors = MakeClampedInt(Interactions, "DoorInteractions", "TryHauntAllDoors", 25, "How rare it is to try to haunt all inside doors (rapidly open/shut) from a ghostCode.\nThis utilizes the function added by zeekers which does not guarantee a haunted door.", 0, 100);

            // CruiserInteractions // **ALL NEW**
            ChangeCruiserRadio = MakeClampedInt(Interactions, "CruiserInteractions", "ChangeCruiserRadio", 55, "How rare it is to change cruiser radio station from a ghostCode.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            CruiserEjectDriver = MakeClampedInt(Interactions, "CruiserInteractions", "CruiserEjectDriver", 3, "How rare it is for a ghostcode to eject the druver from the cruiser.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            CruiserUseBoost = MakeClampedInt(Interactions, "CruiserInteractions", "CruiserUseBoost", 15, "How rare it is for a ghostcode to use cruiser boost.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            ToggleCruiserLights = MakeClampedInt(Interactions, "CruiserInteractions", "ToggleCruiserLights", 30, "How rare it is for a ghostcode to toggle cruiser headlights on or off.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            FlickerCruiserLights = MakeClampedInt(Interactions, "CruiserInteractions", "FlickerCruiserLights", 20, "How rare it is for a ghostcode to flicker the cruiser headlights on/off.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            ToggleCruiserHood = MakeClampedInt(Interactions, "CruiserInteractions", "ToggleCruiserHood", 10, "How rare it is for a ghostcode to toggle the cruiser hood open/closed.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            CruiserWindshield = MakeClampedInt(Interactions, "CruiserInteractions", "CruiserWindshield", 3, "How rare it is for a ghostcode to break the cruiser windshield.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            CruiserPush = MakeClampedInt(Interactions, "CruiserInteractions", "CruiserPush", 45, "How rare it is for a ghostcode to push the cruiser like a player.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            CruiserShiftGears = MakeClampedInt(Interactions, "CruiserInteractions", "CruiserShiftGears", 2, "How rare it is for a ghostcode to make the cruiser change gears.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            ToggleCruiserDoors = MakeClampedInt(Interactions, "CruiserInteractions", "ToggleCruiserDoors", 10, "How rare it is for a ghostcode to toggle a random amount of the doors open/closed.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);

            // ShipInteractions //

            TeleportPlayer = MakeClampedInt(Interactions, "ShipInteractions", "TeleportPlayer", 5, "How rare it is to call the normalTpEvent ghostcode.", 0, 100);
            InverseTeleporter = MakeClampedInt(Interactions, "ShipInteractions", "InverseTeleporter", 15, "How rare it is to call the inverseTpEvent ghostcode.", 0, 100);
            LightsOnShip = MakeClampedInt(Interactions, "ShipInteractions", "LightsOnShip", 30, "How rare it is to call the lightsOnShipEvent ghostcode.", 0, 100);
            DoorsOnShip = MakeClampedInt(Interactions, "ShipInteractions", "DoorsOnShip", 30, "How rare it is to call the doorsOnShipEvent ghostcode.", 0, 100);
            MonitorsOnShip = MakeClampedInt(Interactions, "ShipInteractions", "MonitorsOnShip", 45, "How rare it is to call the monitorsOnShipEvent ghostcode.", 0, 100); ;
            ShockTerminalUser = MakeClampedInt(Interactions, "ShipInteractions", "ShockTerminalUser", 10, "How rare it is for a ghostcode to shock the terminal user.", 0, 100);
            CorruptedCredits = MakeClampedInt(Interactions, "ShipInteractions", "CorruptedCredits", 5, "How rare it is for a ghostcode to corrupt the terminal credits.\nThis will last until the ship leaves.", 0, 100); //new
            HeavyLever = MakeClampedInt(Interactions, "ShipInteractions", "HeavyLever", 15, "How rare it is for a ghostcode to change the lever's weight(faster OR slower time to hold).\nThis will last until the ship leaves.", 0, 100); //new

            // HauntingInteractions //

            BreatheOnWalkies = MakeClampedInt(Interactions, "HauntingInteractions", "BreatheOnWalkies", 35, "How rare it is for the ghost girl to start breathing on walkies when a ghost code is called.", 0, 100);
            GarbleWalkies = MakeClampedInt(Interactions, "HauntingInteractions", "GarbleWalkies", 15, "How rare it is for the ghost girl to start garbling walkies when a ghost code is called.", 0, 100);
            AffectAllBatteries = MakeClampedInt(Interactions, "HauntingInteractions", "DrainAllBatteries", 5, "How rare it is for the ghost girl to drain all batteries when a ghost code is called.", 0, 100);
            AffectHauntedPlayerBatteries = MakeClampedInt(Interactions, "HauntingInteractions", "DrainHauntedPlayerBatteries", 10, "How rare it is for the ghost girl to drain the haunted player's batteries.", 0, 100);
            AffectRandomPlayerBatteries = MakeClampedInt(Interactions, "HauntingInteractions", "DrainRandomPlayerBatteries", 10, "How rare it is for the ghost girl to drain a random player's batteries.", 0, 100);

            // CounterPlay //

            EmoteStopChasing = MakeClampedInt(Interactions, "CounterPlay", "EmoteStopChasing", 95, "How effective emoting to get the ghost girl to stop chasing you is.", 0, 100);
            EmoteStopChaseRequiredPlayers = MakeClampedInt(Interactions, "CounterPlay", "EmoteStopChaseRequiredPlayers", 75, "Percentage of living players required to stop ghost girl from chasing.", 0, 100);
            ShowerStopChasing = MakeClampedInt(Interactions, "CounterPlay", "ShowerStopChasing", 95, "How effective taking a shower to get the ghost girl to stop chasing you is.", 0, 100);
            DeathNote = MakeClampedInt(Interactions, "CounterPlay", "DeathNote", 65, "How effective typing a player's name in the terminal is to transfer the haunting.", 0, 100);
            DeathNoteMaxStrikes = MakeClampedInt(Interactions, "CounterPlay", "DeathNoteMaxStrikes", 3, "Amount of times you can attempt to use the death note to transfer hauntings, use -1 for infinite attempts.", -1, 25);
            DeathNoteFailChase = MakeBool(Interactions, "CounterPlay", "DeathNoteFailChase", true, "Enable or Disable triggering a ghost girl chase on failed attempt to transfer the haunting.");
            TerminalReboot = MakeClampedInt(Interactions, "CounterPlay", "TerminalReboot", 75, "How effective rebooting the terminal is to delay ghostCodes.", 0, 100);

            // ModdedInteractions //

            ToilHeadTurretDisable = MakeClampedInt(Interactions, "ModdedInteractions", "ToilHeadTurretDisable", 30, "Chance of toilHeadTurretDisable being called in a ghostCode.", 0, 100);
            ToilHeadTurretBerserk = MakeClampedInt(Interactions, "ModdedInteractions", "ToilHeadTurretBerserk", 5, "Chance of toilHeadTurretBerserk being called in a ghostCode.", 0, 100);

            // InteractionModifiers //
            BatteryPercentageModifier = MakeClampedInt(Interactions, "InteractionModifiers", "BatteryDrainPercentageModifier", 2, "Percentage of battery to instantly drain when any of the battery drain ghostcodes are called.", 0, 100); ;
            OnlyUniqueMessages = MakeBool(Interactions, "InteractionModifiers", "OnlyUniqueMessages", true, "Will ensure the each message sent is unique (as long as there is more than 1 message in signalMessages)."); ;
            AllSignalMessages = MakeString(Interactions, "InteractionModifiers", "AllSignalMessages", "RUN, LETS PLAY, BOO, FIND ME, I SEE YOU", "Comma-separated list of messages the ghostGirl will send over the signal translator when sending a code."); ;
            AllMonitorMessages = MakeString(Interactions, "InteractionModifiers", "AllMonitorMessages", "BEHIND YOU, HAVING FUN?, TAG YOU'RE IT, DANCE FOR ME, IM HIDING, #######, ERROR, DEATH, NO MORE SCRAP", "Comma-separated list of messages the ghostGirl can display on the ship monitors when sending a code.");

            Interactions.Save();
            ModConfig.configFiles.Add(Interactions);
            //WebConfig(Interactions);
            if (ModConfig.InteractionsConfigCode.Value.Length > 1)
                ReadCompressedConfig(ref ModConfig.InteractionsConfigCode, Interactions);
            Plugin.GC.LogInfo("ghostCodes interactions config initialized");
        }

    }
}
