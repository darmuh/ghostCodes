using BepInEx;
using BepInEx.Configuration;
using ghostCodes.Compatibility;
using System;
using System.Collections.Generic;
using System.IO;
using static OpenLib.ConfigManager.ConfigSetup;

namespace ghostCodes.Configs
{
    public static class InteractionsConfig
    {
        public static ConfigFile PrimaryInteractions = new(Path.Combine(Paths.ConfigPath, $"ghostCodes.PrimaryInteractions.cfg"), true);
        public static ConfigFile SecondaryInteractions = new(Path.Combine(Paths.ConfigPath, $"ghostCodes.SecondaryInteractions.cfg"), true);
        public static List<InteractionSetting> Settings = [];


        // MainInteractions //
        public static InteractionSetting TurretBerserk = new("TurretBerserk", "MainInteractions", typeof(int));
        public static InteractionSetting HungryBlastDoor = new("HungryBlastDoor", "MainInteractions", typeof(int));
        public static InteractionSetting MineBoom = new("MineBoom", "MainInteractions", typeof(int));
        public static InteractionSetting PlayerLights = new("PlayerLights", "MainInteractions", typeof(int), true);
        public static InteractionSetting FlipBreaker = new("FlipBreaker", "MainInteractions", typeof(int));
        public static InteractionSetting SignalTranslator = new("SignalTranslator", "MainInteractions", typeof(int));
        public static InteractionSetting AffectAllBatteries = new("AffectAllBatteries", "MainInteractions", typeof(int), true);
        public static InteractionSetting AffectRandomPlayerBatteries = new("AffectRandomPlayerBatteries", "MainInteractions", typeof(int), true);
        public static InteractionSetting HauntFactoryScrap = new("HauntFactoryScrap", "MainInteractions", typeof(int)); //new
        public static InteractionSetting HauntHeldScrap = new("HauntHeldScrap", "MainInteractions", typeof(int)); //new

        // DoorInteractions //
        public static InteractionSetting ToggleAllRegularDoors = new("ToggleAllRegularDoors", "DoorInteractions", typeof(int));
        public static InteractionSetting ToggleRegularDoor = new("ToggleRegularDoor", "DoorInteractions", typeof(int));
        public static InteractionSetting UnlockSingleDoor = new("UnlockSingleDoor", "DoorInteractions", typeof(int));
        public static InteractionSetting LockSingleDoor = new("LockSingleDoor", "DoorInteractions", typeof(int));
        public static InteractionSetting TryHauntSingleDoor = new("TryHauntSingleDoor", "DoorInteractions", typeof(int)); //NEW
        public static InteractionSetting TryHauntHalfAllDoors = new("TryHauntHalfAllDoors", "DoorInteractions", typeof(int)); //NEW
        public static InteractionSetting TryHauntAllDoors = new("TryHauntAllDoors", "DoorInteractions", typeof(int)); //NEW

        // ShipInteractions //
        public static InteractionSetting TeleportPlayer = new("TeleportPlayer", "ShipInteractions", typeof(int));
        public static InteractionSetting InverseTeleporter = new("InverseTeleporter", "ShipInteractions", typeof(int));
        public static InteractionSetting LightsOnShip = new("LightsOnShip", "ShipInteractions", typeof(int));
        public static InteractionSetting DoorsOnShip = new("DoorsOnShip", "ShipInteractions", typeof(int));
        public static InteractionSetting MonitorsOnShip = new("MonitorsOnShip", "ShipInteractions", typeof(int), true);
        public static InteractionSetting ShockTerminalUser = new("ShockTerminalUser", "ShipInteractions", typeof(int), true);
        public static InteractionSetting CorruptedCredits = new("CorruptedCredits", "ShipInteractions", typeof(int)); //new
        public static InteractionSetting HeavyLever = new("HeavyLever", "ShipInteractions", typeof(int), true); //new
        public static InteractionSetting HauntedOrder = new("HauntedOrder", "ShipInteractions", typeof(int)); //new

        // DressGirlInteractions //
        public static InteractionSetting BreatheOnWalkies = new("BreatheOnWalkies", "DressGirlInteractions", typeof(int), true);
        public static InteractionSetting GarbleWalkies = new("GarbleWalkies", "DressGirlInteractions", typeof(int), true);
        public static InteractionSetting AffectHauntedPlayerBatteries = new("AffectHauntedPlayerBatteries", "DressGirlInteractions", typeof(int));

        // CruiserInteractions // **ALL NEW**
        public static InteractionSetting ChangeCruiserRadio = new("ChangeCruiserRadio", "CruiserInteractions", typeof(int));
        public static InteractionSetting CruiserEjectDriver = new("CruiserEjectDriver", "CruiserInteractions", typeof(int));
        public static InteractionSetting CruiserUseBoost = new("CruiserUseBoost", "CruiserInteractions", typeof(int));
        public static InteractionSetting ToggleCruiserLights = new("ToggleCruiserLights", "CruiserInteractions", typeof(int));
        public static InteractionSetting FlickerCruiserLights = new("FlickerCruiserLights", "CruiserInteractions", typeof(int));
        public static InteractionSetting ToggleCruiserHood = new("ToggleCruiserHood", "CruiserInteractions", typeof(int));
        public static InteractionSetting CruiserWindshield = new("CruiserWindshield", "CruiserInteractions", typeof(int));
        public static InteractionSetting CruiserPush = new("CruiserPush", "CruiserInteractions", typeof(int));
        public static InteractionSetting ToggleCruiserDoors = new("ToggleCruiserDoors", "CruiserInteractions", typeof(int));
        public static InteractionSetting CruiserShiftGears = new("CruiserShiftGears", "CruiserInteractions", typeof(int));


        // CounterPlay //
        public static InteractionSetting EmoteStopChasing = new("EmoteStopChasing", "CounterPlay", typeof(int));
        public static InteractionSetting EmoteStopChaseRequiredPlayers = new("EmoteStopChaseRequiredPlayers", "CounterPlay", typeof(int));
        public static InteractionSetting ShowerStopChasing = new("ShowerStopChasing", "CounterPlay", typeof(int));
        public static InteractionSetting DeathNote = new("DeathNote", "CounterPlay", typeof(int), true);
        public static InteractionSetting DeathNoteMaxStrikes = new("DeathNoteMaxStrikes", "CounterPlay", typeof(int));
        public static InteractionSetting DeathNoteFailChase = new("DeathNoteFailChase", "CounterPlay", typeof(bool));
        public static InteractionSetting TerminalReboot = new("TerminalReboot", "CounterPlay", typeof(int), true);
        public static InteractionSetting TerminalRebootUses = new("TerminalRebootUses", "CounterPlay", typeof(int));


        // Modded Interactions //
        public static InteractionSetting ToilHeadTurretDisable = new("ToilHeadTurretDisable", "ModdedInteractions", typeof(int));
        public static InteractionSetting ToilHeadTurretBerserk = new("ToilHeadTurretBerserk", "ModdedInteractions", typeof(int));

        // InteractionModifiers //
        public static InteractionSetting BatteryPercentageModifier = new("BatteryPercentageModifier", "InteractionModifiers", typeof(int));
        public static InteractionSetting OnlyUniqueMessages = new("OnlyUniqueMessages", "InteractionModifiers", typeof(bool));
        public static InteractionSetting AllSignalMessages = new("AllSignalMessages", "InteractionModifiers", typeof(string));
        public static InteractionSetting AllMonitorMessages = new("AllMonitorMessages", "InteractionModifiers", typeof(string));
        public static InteractionSetting NoTouchItems = new("NoTouchItems", "InteractionModifiers", typeof(string));

        internal static void SetConfig(ConfigFile Interactions, string id = "")
        {
            // MainInteractions //
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"TurretBerserk{id}", 20, "How rare it is for the turret to go berserk from a ghostCode.", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"HungryBlastDoor{id}", 10, "How rare it is for a blast door to start biting from a ghostCode.", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"MineBoom{id}", 10, "How rare it is for a mine to blow itself up from a ghostCode.", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"PlayerLights{id}", 45, "Set the percentage chance that the ghostGirl will flicker a player's lights during a ghostCode event.", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"FlipBreaker{id}", 3, "How rare it is for a ghostCode to flip the breaker and turn off the facility lights.", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"SignalTranslator{id}", 45, "How frequent the signal translator will send messages during code events (percentage)", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"HauntFactoryScrap{id}", 25, "Chance of a ghostcode interacting with noisy scrap items within the factory", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"HauntHeldScrap{id}", 25, "Chance of a ghostcode interacting with any noisy scrap that is held by a player", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"AffectAllBatteries{id}", 5, $"How rare it is for all battery charges to be affected when a ghost code is called.", 0, 100);
            MakeClampedInt(Interactions, $"MainInteractions{id}", $"AffectRandomPlayerBatteries{id}", 10, $"How rare it is for a random player's batteries to be affected by a ghost code.", 0, 100);


            // DoorInteractions //
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"ToggleAllRegularDoors{id}", 10, "How rare it is for a ghostcode to close or open ALL doors.", 0, 100);
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"ToggleRegularDoor{id}", 65, "How rare it is for a ghostcode to close or open a regular door.", 0, 100);
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"UnlockSingleDoor{id}", 15, "How rare it is for the unlockSingleDoorChance to be called from a ghostCode.", 0, 100);
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"LockSingleDoor{id}", 5, "How rare it is for the LockSingleDoor to be called from a ghostCode.", 0, 100);
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"TryHauntSingleDoor{id}", 35, "How rare it is to try to haunt a single door (rapidly open/shut) from a ghostCode.\nThis utilizes the function added by zeekers which does not guarantee a haunted door.", 0, 100);
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"TryHauntHalfAllDoors{id}", 10, "How rare it is to try to haunt half of all inside doors (rapidly open/shut) from a ghostCode.\nThis utilizes the function added by zeekers which does not guarantee a haunted door.", 0, 100);
            MakeClampedInt(Interactions, $"DoorInteractions{id}", $"TryHauntAllDoors{id}", 5, "How rare it is to try to haunt all inside doors (rapidly open/shut) from a ghostCode.\nThis utilizes the function added by zeekers which does not guarantee a haunted door.", 0, 100);

            // CruiserInteractions // **ALL NEW**
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"ChangeCruiserRadio{id}", 55, $"How rare it is to change cruiser radio station from a ghostCode.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"CruiserEjectDriver{id}", 3, $"How rare it is for a ghostcode to eject the druver from the cruiser.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"CruiserUseBoost{id}", 15, $"How rare it is for a ghostcode to use cruiser boost.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"ToggleCruiserLights{id}", 30, $"How rare it is for a ghostcode to toggle cruiser headlights on or off.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"FlickerCruiserLights{id}", 20, $"How rare it is for a ghostcode to flicker the cruiser headlights on/off.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"ToggleCruiserHood{id}", 10, $"How rare it is for a ghostcode to toggle the cruiser hood open/closed.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"CruiserWindshield{id}", 3, $"How rare it is for a ghostcode to break the cruiser windshield.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"CruiserPush{id}", 45, $"How rare it is for a ghostcode to push the cruiser like a player.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"CruiserShiftGears{id}", 2, $"How rare it is for a ghostcode to make the cruiser change gears.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);
            MakeClampedInt(Interactions, $"CruiserInteractions{id}", $"ToggleCruiserDoors{id}", 10, $"How rare it is for a ghostcode to toggle a random amount of the doors open/closed.\nThis interaction will only be picked if a cruiser is present and active.", 0, 100);

            // ShipInteractions //
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"TeleportPlayer{id}", 5, $"How rare it is to call the normalTpEvent ghostcode.", 0, 100);
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"InverseTeleporter{id}", 15, $"How rare it is to call the inverseTpEvent ghostcode.", 0, 100);
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"LightsOnShip{id}", 20, $"How rare it is to call the lightsOnShipEvent ghostcode.", 0, 100);
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"DoorsOnShip{id}", 20, $"How rare it is to call the doorsOnShipEvent ghostcode.", 0, 100);
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"MonitorsOnShip{id}", 25, $"How rare it is to call the monitorsOnShipEvent ghostcode.", 0, 100);
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"ShockTerminalUser{id}", 10, $"How rare it is for a ghostcode to shock the terminal user.", 0, 100);
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"CorruptedCredits{id}", 5, $"How rare it is for a ghostcode to corrupt the terminal credits.\nThis will last until the ship leaves.", 0, 100); //new
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"HeavyLever{id}", 10, $"How rare it is for a ghostcode to change the lever's weight(faster OR slower time to hold).\nThis will last until the ship leaves.", 0, 100); //new
            MakeClampedInt(Interactions, $"ShipInteractions{id}", $"HauntedOrder{id}", 15, $"How rare it is for a ghostcode to call a drop ship with a chance for random items from the store (ship credits will remain unaffected).", 0, 100);

            // DressGirlInteractions //
            MakeClampedInt(Interactions, $"DressGirlInteractions{id}", $"BreatheOnWalkies{id}", 35, $"How rare it is for the ghost girl to start breathing on walkies when a ghost code is called.", 0, 100);
            MakeClampedInt(Interactions, $"DressGirlInteractions{id}", $"GarbleWalkies{id}", 15, $"How rare it is for the ghost girl to start garbling walkies when a ghost code is called.", 0, 100);
            MakeClampedInt(Interactions, $"DressGirlInteractions{id}", $"AffectHauntedPlayerBatteries{id}", 15, $"How rare it is for the haunted player's batteries to be affected by a ghostcode.", 0, 100);

            // CounterPlay //
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"EmoteStopChasing{id}", 95, $"How effective emoting to get the ghost girl to stop chasing you is.", 0, 100);
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"EmoteStopChaseRequiredPlayers{id}", 75, $"Percentage of living players required to stop ghost girl from chasing.", 0, 100);
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"ShowerStopChasing{id}", 95, $"How effective taking a shower to get the ghost girl to stop chasing you is.", 0, 100);
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"DeathNote{id}", 65, $"How effective typing a player's name in the terminal is to transfer the haunting.", 0, 100);
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"DeathNoteMaxStrikes{id}", 3, $"Amount of times you can attempt to use the death note to transfer hauntings, use -1 for infinite attempts.", -1, 25);
            MakeBool(Interactions, $"CounterPlay{id}", $"DeathNoteFailChase{id}", true, $"Enable or Disable triggering a ghost girl chase on failed attempt to transfer the haunting.");
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"TerminalReboot{id}", 75, $"How effective rebooting the terminal is to delay ghostCodes.", 0, 100);
            MakeClampedInt(Interactions, $"CounterPlay{id}", $"TerminalRebootUses{id}", 1, $"How many times you can run a successful terminal reboot.\n0 is Unlimited", 0, 10);

            // ModdedInteractions //
            MakeClampedInt(Interactions, $"ModdedInteractions{id}", $"ToilHeadTurretDisable{id}", 30, $"Chance of toilHeadTurretDisable being called in a ghostCode.", 0, 100);
            MakeClampedInt(Interactions, $"ModdedInteractions{id}", $"ToilHeadTurretBerserk{id}", 5, $"Chance of toilHeadTurretBerserk being called in a ghostCode.", 0, 100);

            // InteractionModifiers //
            MakeClampedInt(Interactions, $"InteractionModifiers{id}", $"BatteryPercentageModifier{id}", 20, $"Percentage of battery to instantly adjust (negative or positive) when any of the battery adjust ghostcodes are called.", 0, 100); ;
            MakeBool(Interactions, $"InteractionModifiers{id}", $"OnlyUniqueMessages{id}", true, $"Will ensure the each message sent is unique (as long as there is more than 1 message in signalMessages)."); ;
            MakeString(Interactions, $"InteractionModifiers{id}", $"AllSignalMessages{id}", $"RUN, LETS PLAY, BOO, FIND ME, I SEE YOU", $"Comma-separated list of messages the ghostGirl will send over the signal translator when sending a code."); ;
            MakeString(Interactions, $"InteractionModifiers{id}", $"AllMonitorMessages{id}", $"BEHIND YOU, HAVING FUN?, TAG YOU'RE IT, DANCE FOR ME, IM HIDING, #######, ERROR, DEATH, NO MORE SCRAP", "Comma-separated list of messages the ghostGirl can display on the ship monitors when sending a code.");
            MakeString(Interactions, $"InteractionModifiers{id}", $"NoTouchItems{id}", "", "Comma separated list of items that ghostCodes should NOT interact with in any scenario");

            Interactions.Save();
            if (id.Length > 0 && OpenLib.Plugin.instance.LethalConfig)
                LethalConfigStuff.QueueAndLoad(Interactions);
            else
                ModConfig.configFiles.Add(Interactions);

            Interactions.SettingChanged += OnSettingChanged;

            Plugin.GC.LogInfo($"ghostCodes interactions {id} config initialized");
        }

        internal static void OnSettingChanged(object sender, SettingChangedEventArgs settingChangedArg)
        {
            InteractionSetting setting = Settings.Find(x => settingChangedArg.ChangedSetting.Definition.Key.Contains(x.name)); //grab matching setting name
            ConfigFile currentConfig;
            string id = "";

            if (SetupConfig.GhostCodesSettings.CurrentMode == SetupConfig.GhostCodesSettings.SecondaryMode)
            {
                currentConfig = SecondaryInteractions;
                id = " (Secondary)";
            }
            else
                currentConfig = PrimaryInteractions;

            if (UpdateSetting(setting, currentConfig, id))
                Plugin.Spam("Setting updated from config change event!");
        }

        internal static bool UpdateSetting(InteractionSetting setting, ConfigFile Interactions, string id = "")
        {
            if (setting == null)
            {
                Plugin.WARNING("InteractionSetting is Null!!");
                return false;
            }

            Plugin.Spam(setting.name);

            if (setting.type == typeof(int))
            {
                if (Interactions.TryGetEntry<int>(setting.section + id, setting.name + id, out ConfigEntry<int> entry))
                {
                    if (setting.isNetworked && !ModConfig.ModNetworking.Value)
                    {
                        setting.Value = 0;
                        Plugin.Spam($"{setting.name} requires networking and networking is disabled!");
                    }
                    else
                        setting.Value = entry.Value;

                    Plugin.Spam($"{setting.name} set to {setting.Value}");
                    return true;
                }
                else
                    Plugin.WARNING($"Unable to get config value and set setting for {setting.name}");
            }
            else if (setting.type == typeof(bool))
            {
                if (Interactions.TryGetEntry<bool>(setting.section + id, setting.name + id, out ConfigEntry<bool> entry))
                {
                    if (setting.isNetworked && !ModConfig.ModNetworking.Value)
                    {
                        setting.boolValue = false;
                        Plugin.Spam($"{setting.name} requires networking and networking is disabled!");
                    }
                    else
                        setting.boolValue = entry.Value;

                    Plugin.Spam($"{setting.name} set to {setting.boolValue}");
                    return true;
                }
                else
                    Plugin.WARNING($"Unable to get config value and set setting for {setting.name}");
            }
            else if (setting.type == typeof(string))
            {
                if (Interactions.TryGetEntry<string>(setting.section + id, setting.name + id, out ConfigEntry<string> entry))
                {
                    setting.stringValue = entry.Value;
                    Plugin.Spam($"{setting.name} set to {entry.Value}");
                    return true;
                }
                else
                    Plugin.WARNING($"Unable to get config value and set setting for {setting.name}");
            }
            else
                Plugin.WARNING($"Unexpected type for {setting.name}!");

            return false;
        }

        internal static void MapToConfig(ConfigFile Interactions, List<InteractionSetting> Settings)
        {
            string id = GetIdentifier();

            foreach (InteractionSetting setting in Settings)
            {
                if (setting == null)
                {
                    Plugin.WARNING("Null InteractionSetting in listing!!");
                    continue;
                }

                if (UpdateSetting(setting, Interactions, id))
                    Plugin.Spam($"{setting.name} updated!");
            }
        }

        private static string GetIdentifier()
        {
            if (SetupConfig.GhostCodesSettings == null)
                return "";

            if (SetupConfig.GhostCodesSettings.CurrentMode == null)
                return "";

            if (SetupConfig.GhostCodesSettings.SecondaryMode == null)
                return "";

            if (SetupConfig.GhostCodesSettings.CurrentMode == SetupConfig.GhostCodesSettings.SecondaryMode)
                return " (Secondary)";
            else
                return "";
        }

    }

    public class InteractionSetting
    {
        public string name;
        public string section;
        public Type type;
        public int Value; //main value
        public bool boolValue;
        public string stringValue;

        //networking required
        public bool isNetworked;

        public InteractionSetting(string name, string section, Type type, bool networking = false)
        {
            this.name = name;
            this.section = section;
            this.type = type;
            isNetworked = networking;
            InteractionsConfig.Settings.Add(this);
        }
    }
}
