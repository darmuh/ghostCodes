using BepInEx.Configuration;
using HarmonyLib;
using Steamworks.Ugc;
using System.Collections.Generic;
using System.Linq;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class InitPlugin
    {
        internal static Dictionary<ConfigEntry<bool>, bool> modifiedCommands = new Dictionary<ConfigEntry<bool>, bool>();
        internal static List<ConfigEntry<bool>> bannedConfigItems = new List<ConfigEntry<bool>>();
        internal static void StartTheRound()
        {
            NetworkingCheck();
            GetPlayersAtStart();
            GhostGirlEnhancedBypassInit();
            GhostGirlOnlyCheck();
            DressGirl.InitDressGirl();
            StartPlugin();
        }

        private static void StartPlugin()
        {
            if (ShouldInitPlugin())
                StartOfRound.Instance.StartCoroutine(Coroutines.InitEnumerator(StartOfRound.Instance));
        }

        internal static void RestartPlugin()
        {
            if (ShouldInitPlugin())
                StartOfRound.Instance.StartCoroutine(Coroutines.RestartEnum(StartOfRound.Instance));
        }

        internal static void GhostGirlEnhancedBypassInit()
        {
            if (!ModConfig.GGEbypass.Value || !ModConfig.ghostGirlEnhanced.Value)
                return;

            Plugin.MoreLogs("Initializing GhostGirlEnhanced Bypass List");

            List<string> noGhostPlanets = ModConfig.GGEbypassList.Value
                                      .Split(',')
                                      .Select(item => item.TrimStart())
                                      .ToList();

            if (ModConfig.GGEbypass.Value && (noGhostPlanets.Any(planet => StartOfRound.Instance.currentLevel.PlanetName.Contains(planet))))
            {
                Plugin.instance.bypassGGE = true;
                Plugin.MoreLogs("Ghost Girl Enhanced is being bypassed on this moon");
            }
            else
                Plugin.instance.bypassGGE = false;

        }

        private static void BannedConfigItems()
        {
            Plugin.MoreLogs("initializing banned config items to NOT modify with [gcGhostGirlOnlyList]");
            bannedConfigItems.Clear();
            bannedConfigItems.Add(ModConfig.ModNetworking);
            bannedConfigItems.Add(ModConfig.ghostGirlEnhanced);
            bannedConfigItems.Add(ModConfig.useRandomIntervals);
            bannedConfigItems.Add(ModConfig.gcGhostGirlOnly);
            bannedConfigItems.Add(ModConfig.gcInsanity);
            bannedConfigItems.Add(ModConfig.GGEbypass);
        }

        private static void DeserializeConfigOptions(string serializedData)
        {

            if (!ModConfig.gcGhostGirlOnly.Value)
                return;

            // Clear existing configOptions dictionary
            modifiedCommands.Clear();

            // Deserialize the serialized data into dictionary
            var pairs = serializedData.Split(';');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    foreach (var item in Plugin.instance.Config)
                    {
                        if(item.Key.Key == keyValue[0] && bool.TryParse(keyValue[1], out bool boolValue))
                        {
                            if (Plugin.instance.Config.TryGetEntry<bool>(item.Value.Definition, out ConfigEntry<bool> entry) && entry.Value != boolValue)
                            {
                                Plugin.MoreLogs($"Resetting {keyValue[0]} to {keyValue[1]}");
                                entry.Value = boolValue;
                            }
                            else
                                Plugin.MoreLogs("Correct value assigned to {keyValue[0]}");
                        }

                    }
                }
            }
        }

        private static string SerializeConfigOptions()
        {
            // Serialize the dictionary into a delimited string
            return string.Join(";", modifiedCommands.Select(kv => $"{kv.Key.Definition.Key}={kv.Value}"));
        }

        private static void SaveModifiedListToConfig()
        {
            // Serialize the dictionary into a format that can be stored in the configuration
            ModConfig.modifiedConfigItemsList.Value = SerializeConfigOptions();

            Plugin.MoreLogs("Config options saved to config");
        }

        internal static void GhostGirlOnlyCheck()
        {
            // Check if the feature is enabled
            if (!ModConfig.gcGhostGirlOnly.Value)
                return;

            BannedConfigItems();
            DeserializeConfigOptions(ModConfig.modifiedConfigItemsList.Value);

            Plugin.MoreLogs("Checking gcGhostGirlOnlyList");

            List<string> ghostGirlOnly = ModConfig.gcGhostGirlOnlyList.Value.Split(',')
                                      .Select(item => item.TrimStart())
                                      .ToList();

            // Check if ghost girl enhancement is disabled or bypassed
            if (!ModConfig.ghostGirlEnhanced.Value || Plugin.instance.bypassGGE)
            {
                Plugin.MoreLogs("Disabling interactions that require a ghost girl from gcGhostGirlOnlyList.");
                foreach (var item in Plugin.instance.Config)
                {
                    // Check if the item is in the ghostGirlOnly list
                    if (ghostGirlOnly.Contains(item.Key.Key))
                    {
                        Plugin.MoreLogs("Matching config item found from gcGhostGirlOnlyList");
                        // Try to get the config entry and set it to false if it's not already false
                        if (Plugin.instance.Config.TryGetEntry<bool>(item.Value.Definition, out ConfigEntry<bool> entry) && entry.Value)
                        {
                            modifiedCommands.Add(entry,entry.Value);
                            entry.Value = false;
                            Plugin.MoreLogs($"Setting: {item.Key.Key} to false");
                        }
                    }
                }
            }
            // Check if bypassGGE is false
            else if (!Plugin.instance.bypassGGE)
            {
                Plugin.MoreLogs("Enabling any disabled interactions that have been added to the gcGhostGirlOnlyList.");
                foreach (var item in Plugin.instance.Config)
                {
                    // Check if the item is in the ghostGirlOnly list
                    if (ghostGirlOnly.Contains(item.Key.Key))
                    {
                        // Try to get the config entry and restore its original value if it was modified
                        if (Plugin.instance.Config.TryGetEntry<bool>(item.Value.Definition, out ConfigEntry<bool> entry) && modifiedCommands.ContainsKey(entry))
                        {
                            entry.Value = true;
                            modifiedCommands.Remove(entry);
                            Plugin.MoreLogs($"Setting: {item.Key.Key} to true");
                        }
                    }
                }
            }
            else
            {
                Plugin.GC.LogError("Issue with ghost girl only list encountered...");
            }

            SaveModifiedListToConfig();
        }

        internal static void CodesInit()
        {
            Plugin.MoreLogs("Initializing GhostCode core variables");

            Plugin.instance.ghostCodeSent = false;
            RapidFire.startRapidFire = false;
            RapidFire.meltdown = false;
            Coroutines.rapidFireStart = false;
            Coroutines.codeLooperStarted = false;
            Plugin.instance.maxSanity = 0f;
            Plugin.instance.groupSanity = 0f;
            Plugin.instance.playersAtStart = 0;
            Plugin.instance.codeCount = 0;
            Plugin.instance.randGC = 0;
            endAllCodes = false;

            TerminalAdditions.ResetBools();
            CodeStuff.GetUsableCodes();
            CodeStuff.GetRandomCodeAmount();

            if (ModConfig.soloAssist.Value)
                InsanityStuff.InitsoloAssist();
        }

        internal static void NetworkingCheck()
        {
            if (!ModConfig.ModNetworking.Value)
            {
                ModConfig.ghostGirlEnhanced.Value = false;
                Plugin.MoreLogs("Forcing Ghost Girl Enhanced Mode to FALSE");
            }
        }

        internal static void GetPlayersAtStart()
        {
            Plugin.instance.playersAtStart = 0;
            Plugin.instance.players = StartOfRound.Instance.allPlayerScripts;

            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Count(); i++)
            {
                if (StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled)
                    Plugin.instance.playersAtStart++;          
            }

            Plugin.MoreLogs($"Players at Start: {Plugin.instance.playersAtStart}");

            if (Plugin.instance.playersAtStart > 1)
                Plugin.MoreLogs("SoloAssist Buff disabled, more than one player detected.");
        }


    }
}
