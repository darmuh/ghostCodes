using GameNetcodeStuff;
using HarmonyLib;
using System.Linq;
using static ghostCodes.TerminalAdditions;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class TerminalPatchStuff
    {
        [HarmonyPatch(typeof(Terminal), "Start")]
        public class StartTerminal : Terminal
        {
            static void Postfix(Terminal __instance)
            {
                Plugin.instance.Terminal = __instance;
                Plugin.MoreLogs("Grabbed Terminal instance for use in mod");
            }
        }

        [HarmonyPatch(typeof(Terminal), "LoadNewNode")]
        public class HauntedTerminal
        {
            static void Postfix(TerminalNode node)
            {
                if (node == null)
                {
                    Plugin.MoreLogs("node is null");
                    return;
                }
                //SpookyTerminalColors(false);
                Plugin.MoreLogs("Handling node.");
                HandleNodeSpecialLogic(node);
            }

            internal static void HandleNodeSpecialLogic(TerminalNode node)
            {
                if (ghostCodeNodes == null)
                {
                    Plugin.MoreLogs("ghostCodeNodes is null");
                    return;
                }

                if (ghostCodeNodes.Contains(node))
                {
                    if (node == rebootNode)
                    {
                        Plugin.MoreLogs("reboot questions being asked");

                        if (!KeepSendingCodes())
                            Plugin.instance.Terminal.LoadNewNode(Plugin.instance.Terminal.terminalNodes.specialNodes[5]);

                        return;
                    }
                    else if (node.name.Contains("ghostCodes_confirm_reboot"))
                    {
                        Plugin.MoreLogs("terminal reboot started");
                        HandleRebootNode(node);
                        return;
                    }
                    else if (node.name.Contains("ghostCodes_haunt_"))
                    {
                        Plugin.MoreLogs("player name detected");
                        PlayerNameNode(node);
                        return;
                    }
                    else
                    {
                        Plugin.MoreLogs($"Node [{node.name}] detected");
                    }
                }
                else
                    Plugin.MoreLogs($"Node [{node.name} is not within the ghostCodeNodes list]");
            }

            internal static PlayerControllerB GetPlayerFromString(string playerName)
            {
                Plugin.MoreLogs($"Attempting to find player {playerName}");
                foreach (PlayerControllerB player in Plugin.instance.players)
                {
                    string friendlyName = StringStuff.TerminalFriendlyString(player.playerUsername);
                    Plugin.MoreLogs($"Checking [{friendlyName}]");
                    if (friendlyName == playerName)
                        return player;
                }

                return null;
            }

        }
    }
}
