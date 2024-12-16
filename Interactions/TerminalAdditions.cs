using GameNetcodeStuff;
using ghostCodes.Configs;
using System.Collections;
using System.Linq;
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
        internal static int rebootCount;
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
        internal static AudioClip Reboot;
        internal static AudioClip Success;

        internal static void ResetBools()
        {
            isTerminalRebooting = false;
            rebootCommandExpired = false;
            spookyColors = false;
            rebootCount = 0;
            deathNoteStrikes = 0;
        }

        internal static IEnumerator RebootTerminalSpooky()
        {
            if (isTerminalRebooting)
                yield break;

            Plugin.MoreLogs("Start of spooky terminal reboot coroutine.");
            SoundSystem.PlayTerminalSound(Reboot);
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_3);
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_2);
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_1);
            Plugin.instance.Terminal.QuitTerminal();
            yield return new WaitForEndOfFrame();
            IsTerminalUsable(false);
            float originalAlpha = Plugin.instance.Terminal.topRightText.alpha;
            Plugin.instance.Terminal.topRightText.alpha = 0f;
            Plugin.instance.Terminal.terminalUIScreen.gameObject.SetActive(true);
            Plugin.instance.Terminal.screenText.caretPosition = Plugin.instance.Terminal.screenText.text.Length;
            endAllCodes = true;
            isTerminalRebooting = true;
            yield return new WaitForSeconds(1);
            SetTerminalText(reboot_progress_1);
            StartOfRound.Instance.StartCoroutine(RebootLoadingTextLoop());
            yield return new WaitForSeconds(rebootTime);
            isTerminalRebooting = false;
            DressGirl.StopGirlFromChasing();
            endAllCodes = false;
            Plugin.instance.Terminal.topRightText.alpha = originalAlpha;
            Plugin.instance.Terminal.terminalAudio.Stop();
            SoundSystem.PlayTerminalSound(Success);
            SetTerminalText("\n\n\n\n\n\n\n\n\n\t\t>>Reboot <color=#ebe334>Complete.</color><<\n\n\n\n");
            IsTerminalUsable(true);
            InitPlugin.RestartPlugin();

            if (InteractionsConfig.TerminalRebootUses.Value > 0)
            {
                if (rebootCount >= InteractionsConfig.TerminalRebootUses.Value)
                    rebootCommandExpired = true;
                else
                    rebootCount++;
            }
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

        internal static void IsTerminalUsable(bool state)
        {
            Plugin.instance.Terminal.terminalTrigger.interactable = state;
            Plugin.MoreLogs($"terminal interactable set to {state}");
        }

        internal static string HandleRebootNode()
        {
            Plugin.MoreLogs("Handling reboot node");

            if (StartOfRound.Instance.inShipPhase)
                return "\n\n\n\n\n\n\t\tTerminal reboot is not necessary... (in orbit)\r\n";

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
            rebootTime = GetInt(30, 88);
            return $"You have requested to reboot the terminal.\nTotal time to reboot is {rebootTime} seconds...\n\nPlease CONFIRM or DENY.\n\n";
        }

        internal static void CreateAllNodes()
        {
            if (!ModConfig.ModNetworking.Value)
                return;

            CreateRebootNode();
            CreateDeathNoteNode();
        }

        private static void CreateRebootNode()
        {
            if (InteractionsConfig.TerminalReboot.Value < 1)
                return;

            rebootNode = OpenLib.CoreMethods.AddingThings.AddNodeManual("ghostCodes_reboot", "reboot", AskTerminalReboot, true, 1, OpenLib.ConfigManager.ConfigSetup.defaultListing, 0, HandleRebootNode, null, "", $"Terminal Reboot has been cancelled.\r\n\r\n");
        }

        private static void CreateDeathNoteNode()
        {
            if (InteractionsConfig.DeathNote.Value < 1)
                return;

            if(StartOfRound.Instance.allPlayerScripts.Any(x => x.isPlayerControlled && x != StartOfRound.Instance.localPlayerController))
            {
                Plugin.Spam("Adding deathnotemenu!");
                Interactions.DeathNote.DeathNoteMenu = OpenLib.CoreMethods.AddingThings.AddNodeManual("DeathNoteMenu", "leave me alone", OpenDeathNote, true, 0, OpenLib.ConfigManager.ConfigSetup.defaultListing);
            }
        }

        internal static string OpenDeathNote()
        {
            if (!CanUseDeathNote())
                return "Chill out, no one is after you lol\r\n\r\n";

            if (Plugin.instance.TerminalStuff)
                Compatibility.TerminalStuff.StopShortCuts(true);

            Interactions.DeathNote.currentPage = 1;
            Plugin.instance.Terminal.StartCoroutine(Interactions.DeathNote.MenuStart());
            return Interactions.DeathNote.GetPlayerList(0, 10, ref Interactions.DeathNote.currentPage);
        }

        internal static bool CanUseDeathNote()
        {
            if (Plugin.instance.DressGirl == null)
                return false;

            if (!Plugin.instance.DressGirl.hauntingLocalPlayer)
                return false;

            if (!StartOfRound.Instance.allPlayerScripts.Any(x => x.isPlayerControlled && x != StartOfRound.Instance.localPlayerController))
                return false;

            if (StartOfRound.Instance.shipIsLeaving)
                return false;

            return true;
        }

        internal static void DeathNoteFail()
        {
            if (!InteractionsConfig.DeathNoteFailChase.boolValue || Plugin.instance.DressGirl == null)
                return;

            Plugin.MoreLogs("death note failed, beginning ghost girl chase");
            Plugin.instance.DressGirl.BeginChasing();
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
            if (credsCorrupted)
                CorruptedCredits(false);
        }

    }
}
