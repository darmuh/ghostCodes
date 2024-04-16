using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class InitPlugin
    {
        internal static Dictionary<string,ConfigEntry<bool>> identifiedCommands = new Dictionary<string,ConfigEntry<bool>>();
        internal static List<ConfigEntry<bool>> modifiedCommands = new List<ConfigEntry<bool>>();
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
            if (!gcConfig.GGEbypass.Value)
                return;

            Plugin.MoreLogs("Initializing GhostGirlEnhanced Bypass List");

            List<string> noGhostPlanets = gcConfig.GGEbypassList.Value
                                      .Split(',')
                                      .Select(item => item.TrimStart())
                                      .ToList();

            if (gcConfig.GGEbypass.Value && (noGhostPlanets.Any(planet => StartOfRound.Instance.currentLevel.PlanetName.Contains(planet))))
            {
                Plugin.instance.bypassGGE = true;
                Plugin.MoreLogs("Ghost Girl Enhanced is being bypassed on this moon");
            }
            else
                Plugin.instance.bypassGGE = false;

        }

        internal static void GhostGirlOnlyCheck()
        {
            // Check if the feature is enabled
            if (!gcConfig.gcGhostGirlOnly.Value)
                return;

            Plugin.MoreLogs("Checking gcGhostGirlOnlyList");

            List<string> ghostGirlOnly = gcConfig.gcGhostGirlOnlyList.Value.Split(',')
                                      .Select(item => item.TrimStart())
                                      .ToList();

            // Check if ghost girl enhancement is disabled or bypassed
            if (!gcConfig.ghostGirlEnhanced.Value || Plugin.instance.bypassGGE)
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
                            entry.Value = false;
                            modifiedCommands.Add(entry);
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
                        if (Plugin.instance.Config.TryGetEntry<bool>(item.Value.Definition, out ConfigEntry<bool> entry) && modifiedCommands.Contains(entry))
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

            if (gcConfig.soloAssist.Value)
                InsanityStuff.InitsoloAssist();
        }

        internal static void NetworkingCheck()
        {
            if (!gcConfig.ModNetworking.Value)
            {
                gcConfig.ghostGirlEnhanced.Value = false;
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
