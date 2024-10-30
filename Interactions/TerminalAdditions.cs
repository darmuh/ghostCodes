using GameNetcodeStuff;
using ghostCodes.Configs;
using ghostCodes.Patches;
using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;
using static ghostCodes.Bools;
using static ghostCodes.NumberStuff;
using static ghostCodes.StringStuff;

namespace ghostCodes
{
    internal class TerminalAdditions
    {
        internal static TerminalNode rebootNode;
        internal static bool rebootCommandExpired = false;
        internal static int rebootTime;
        internal static int deathNoteStrikes = 0;
        public static bool isTerminalRebooting = false;
        internal static bool spookyColors = false;
        internal static bool credsCorrupted = false;

        //Cached Values
        internal static int OriginalCreds = 0;
        internal static Color OriginalColor;

        internal static string rebootConfirmName = "ghostCodes_confirm_reboot";
        internal static string rebootDenyName = "ghostCodes_deny_reboot";

        //Sounds
        internal static AudioClip Shock;

        internal static void ResetBools()
        {
            isTerminalRebooting = false;
            rebootCommandExpired = false;
            spookyColors = false;
            deathNoteStrikes = 0;
        }

        internal static IEnumerator RebootTerminalSpooky()
        {
            if (isTerminalRebooting)
                yield break;

            Plugin.MoreLogs("Start of spooky terminal reboot coroutine.");
            TerminalResetSound();
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_3);
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_2);
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_1);
            Plugin.instance.Terminal.QuitTerminal();
            yield return new WaitForEndOfFrame();
            IsTerminalUsable(false);
            Plugin.instance.Terminal.terminalUIScreen.gameObject.SetActive(true);
            Plugin.instance.Terminal.screenText.caretPosition = Plugin.instance.Terminal.screenText.text.Length;
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_progress_1);
            endAllCodes = true;
            isTerminalRebooting = true;
            StartOfRound.Instance.StartCoroutine(RebootLoadingTextLoop());
            yield return new WaitForSeconds(rebootTime);
            isTerminalRebooting = false;
            DressGirl.StopGirlFromChasing();
            endAllCodes = false;
            SetTerminalText("\n\n\n\n\n\n\n\n\n\t\t>>Reboot <color=#ebe334>Complete.</color><<\n\n\n\n"); //<color=#e6b800>darmuh</color>
            IsTerminalUsable(true);
            rebootCommandExpired = true;
            InitPlugin.RestartPlugin();
        }

        internal static IEnumerator RebootLoadingTextLoop()
        {
            while (isTerminalRebooting)
            {
                int i = GetInt(0, rebootProgress.Count);
                SetTerminalText($"<color=#b81b1b>{rebootProgress[i]}</color>");
                yield return new WaitForSeconds(0.1f);
            }
        }

        internal static void SetTerminalText(string newText)
        {
            Plugin.instance.Terminal.currentText = newText;
            Plugin.instance.Terminal.screenText.text = newText;
        }

        internal static void TerminalResetSound()
        {
            SoundSystem.PlayTerminalSound(StartOfRound.Instance.HUDSystemAlertSFX);
        }

        internal static void IsTerminalUsable(bool state)
        {
            Plugin.instance.Terminal.terminalTrigger.interactable = state;
            Plugin.MoreLogs($"terminal interactable set to {state}");
        }

        internal static string PlayerNameNode()
        {
            if (Plugin.instance.DressGirl == null)
            {
                Plugin.instance.Terminal.LoadNewNode(Plugin.instance.Terminal.terminalNodes.specialNodes[5]);
                return "";
            }

            if (!Plugin.instance.DressGirl.hauntingLocalPlayer)
            {
                Plugin.instance.Terminal.LoadNewNode(Plugin.instance.Terminal.terminalNodes.specialNodes[5]);
                return "";
            }

            Plugin.MoreLogs("handling player name node");

            if (InteractionsConfig.DeathNoteMaxStrikes.Value != -1 && deathNoteStrikes > InteractionsConfig.DeathNoteMaxStrikes.Value)
            {
                string newText = "\n\n\n\n\n\n\t\tDeath note is out of space...\r\n";
                return newText;
            }

            string playerName = OpenLib.Common.CommonStringStuff.GetCleanedScreenText(Plugin.instance.Terminal);
            playerName = TerminalFriendlyString(playerName);
            return ValidPlayerNameInput(playerName);
        }

        internal static string HandleRebootNode()
        {
            Plugin.MoreLogs("Handling reboot node");

            int chance = GetInt(0, 100);
            if (InteractionsConfig.TerminalReboot.Value < chance || rebootCommandExpired)
            {
                string newText = "\n\n\n\n\n\n\t\tTerminal reboot has failed...\r\n";
                rebootCommandExpired = true;
                return newText;
            }
            else
            {
                StartOfRound.Instance.StartCoroutine(RebootTerminalSpooky());
                Plugin.MoreLogs("rebooting terminal");
                return "";
            }

        }

        internal static string AskTerminalReboot()
        {
            rebootTime = GetInt(30, 90);
            return $"You have requested to reboot the terminal.\nTotal time to reboot is {rebootTime}.\n\nPlease CONFIRM or DENY.\n\n";
        }

        internal static void CreateAllNodes()
        {
            if (!ModConfig.ModNetworking.Value)
                return;

            CreateRebootNode();
            CreatePlayerNameNodes();
        }

        private static void CreateRebootNode()
        {
            if (InteractionsConfig.TerminalReboot.Value < 1)
                return;

            rebootNode = OpenLib.CoreMethods.AddingThings.AddNodeManual("ghostCodes_reboot", "reboot", AskTerminalReboot, true, 1, OpenLib.ConfigManager.ConfigSetup.defaultListing, 0, HandleRebootNode, null, "", $"Terminal Reboot has been cancelled.\r\n\r\n");
        }

        private static void CreatePlayerNameNodes()
        {
            if (InteractionsConfig.DeathNote.Value < 1)
                return;

            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (player != StartOfRound.Instance.localPlayerController && player.isPlayerControlled)
                {
                    string playerName = player.playerUsername;
                    playerName = TerminalFriendlyString(playerName);
                    OpenLib.CoreMethods.AddingThings.AddNodeManual("ghostCodes_haunt_" + playerName, playerName, PlayerNameNode, true, 0, OpenLib.ConfigManager.ConfigSetup.defaultListing, 0, null, null, "", "", false, 1, "", true);
                    Plugin.MoreLogs($"Command created for haunting {playerName}");
                }
                else
                    Plugin.MoreLogs("skipping self and non-player controlled players");

            }
        }

        private static void DeathNoteFail()
        {
            if (!InteractionsConfig.DeathNoteFailChase.Value)
                return;

            Plugin.MoreLogs("death note failed, beginning ghost girl chase");
            Plugin.instance.DressGirl.BeginChasing();
        }

        private static string ValidPlayerNameInput(string s)
        {
            int chance = GetInt(0, 100);
            if (OpenLib.Common.Misc.TryGetPlayerFromName(s, out PlayerControllerB changeToPlayer))
            {
                if (InteractionsConfig.DeathNote.Value < chance)
                {
                    string newText = $"\n\n\n\n\n\n<color=#b81b1b>You're not getting rid of me that easy! :3</color>\n\n";
                    deathNoteStrikes++;
                    DeathNoteFail();
                    //Plugin.instance.Terminal.StartCoroutine(DelayedReturnToNormalText());
                    return newText;
                }
                else if (changeToPlayer.isPlayerControlled && !changeToPlayer.isPlayerDead)
                {
                    string newText = $"\n\n\n\n\n\n<color=#b81b1b>Okay! I'll go play with {s} now! :3</color>\n\n";
                    DressGirl.EndAllGirlStuff();
                    NetHandler.Instance.ChangeDressGirlToPlayerServerRpc(s);
                    deathNoteStrikes++;
                    //Plugin.instance.Terminal.StartCoroutine(DelayedReturnToNormalText());
                    Plugin.MoreLogs("You should not be haunted anymore.");
                    return newText; // make sure you only skip if really necessary
                }
                else
                {
                    string newText = $"\n\n\n\n\n\n<color=#b81b1b>I think i'll keep having fun with you! :3</color>\n\n";
                    deathNoteStrikes++;
                    Plugin.MoreLogs("picked player is likely dead, this is an else statement");
                    DeathNoteFail();
                    return newText;
                }
            }
            else
            {
                string newText = $"\n\n\n\n\n\n<color=#b81b1b>I don't know anyone by that name! Guess you're stuck with me ^.^</color>\n\n";
                deathNoteStrikes++;
                DeathNoteFail();
                return newText;
            }


        }

        internal static void CorruptedCredits(bool state)
        {
            if (state)
            {
                Plugin.Spam("Credits corrupted!");
                OriginalCreds = Plugin.instance.Terminal.groupCredits;
                OriginalColor = Plugin.instance.Terminal.topRightText.color;
                Plugin.instance.Terminal.SyncGroupCreditsServerRpc(0, Plugin.instance.Terminal.numberOfItemsInDropship);
                Plugin.instance.Terminal.topRightText.color = Color.red;
                credsCorrupted = true;
            }
            else
            {
                Plugin.Spam("Credits restored!");
                Plugin.instance.Terminal.SyncGroupCreditsServerRpc(OriginalCreds, Plugin.instance.Terminal.numberOfItemsInDropship);
                Plugin.instance.Terminal.topRightText.color = OriginalColor;
                credsCorrupted = false;
            }
                
        }

        internal static void RestoreCreds()
        {
            if(credsCorrupted)
                CorruptedCredits(false);
        }

    }
}
