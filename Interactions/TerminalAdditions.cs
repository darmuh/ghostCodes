using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static ghostCodes.NumberStuff;
using static ghostCodes.Bools;
using static ghostCodes.StringStuff;
using static ghostCodes.TerminalPatchStuff.HauntedTerminal;

namespace ghostCodes
{
    internal class TerminalAdditions
    {
        internal static TerminalKeyword rebootKeyword;
        internal static TerminalNode rebootNode;
        internal static List<TerminalNode> ghostCodeNodes = new List<TerminalNode>();
        internal static bool rebootCommandExpired = false;
        internal static int rebootTime;
        internal static int deathNoteStrikes = 0;
        public static bool isTerminalRebooting = false;
        internal static bool spookyColors = false;

        internal static string rebootConfirmName = "ghostCodes_confirm_reboot";
        internal static string rebootDenyName = "ghostCodes_deny_reboot";

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
            SpookyTerminalColors(true);
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
            SetTerminalText("\n\n\n\n\n\n\n\n\n\t\t>>Reboot Completed.<<\n\n\n\n");
            IsTerminalUsable(true);
            SpookyTerminalColors(false);
            rebootCommandExpired = true;
            InitPlugin.RestartPlugin();
        }

        internal static IEnumerator RebootLoadingTextLoop()
        {
            while (isTerminalRebooting)
            {
                int i = GetInt(0, rebootProgress.Count - 1);
                SetTerminalText(rebootProgress[i]);
                yield return new WaitForSeconds(0.1f);
            }
        }

        internal static void SetTerminalText(string newText)
        {
            Plugin.instance.Terminal.currentText = newText;
            Plugin.instance.Terminal.screenText.text = newText;
        }

        internal static void SpookyTerminalColors(bool state)
        {
            if (Plugin.instance.Terminal.screenText == null || Plugin.instance.Terminal.topRightText == null) //null reference handling
                return;

            if (state)
            {
                Plugin.instance.Terminal.screenText.textComponent.color = Color.red;
                Plugin.instance.Terminal.topRightText.color = Color.red;
                Plugin.instance.Terminal.screenText.caretColor = Color.red;
                spookyColors = true;
            }
            else
            {
                Plugin.instance.Terminal.screenText.textComponent.color = TerminalPatchStuff.StartTerminal.screenTextDefaultColor;
                Plugin.instance.Terminal.topRightText.color = TerminalPatchStuff.StartTerminal.topRightDefaultColor;
                Plugin.instance.Terminal.screenText.caretColor = TerminalPatchStuff.StartTerminal.screenTextDefaultColor;
                spookyColors = false;
            }
        }

        internal static IEnumerator DelayedReturnToNormalText() //quick fix to a null reference error in loadnode patch, not needed anymore
        {
            yield return new WaitForSeconds(3);
            SpookyTerminalColors(false);
        }

        internal static void TerminalResetSound()
        {
            SoundSystem.PlayTerminalSound(StartOfRound.Instance.HUDSystemAlertSFX);
        }

        internal static void IsTerminalUsable(bool state)
        {
            Plugin.instance.Terminal.gameObject.SetActive(state);
            Plugin.MoreLogs($"terminal game object set to {state}");
        }

        internal static void PlayerNameNode(TerminalNode node)
        {
            if (Plugin.instance.DressGirl == null)
            {
                Plugin.instance.Terminal.LoadNewNode(Plugin.instance.Terminal.terminalNodes.specialNodes[5]);
                return;
            }
                
            if(!Plugin.instance.DressGirl.hauntingLocalPlayer)
            {
                Plugin.instance.Terminal.LoadNewNode(Plugin.instance.Terminal.terminalNodes.specialNodes[5]);
                return;
            }

            Plugin.MoreLogs("handling player name node");

            if (ModConfig.ggDeathNoteMaxStrikes.Value != -1 && deathNoteStrikes > ModConfig.ggDeathNoteMaxStrikes.Value)
            {
                string newText = "\n\n\n\n\n\n\t\tDeath note is out of space...\r\n";
                SetTerminalText(newText);
            }

            string nodeName = node.name;
            string playerName = nodeName.Replace("ghostCodes_haunt_", "");
            playerName = TerminalFriendlyString(playerName);
            ValidPlayerNameInput(playerName);
        }

        internal static void HandleRebootNode(TerminalNode node)
        {
            Plugin.MoreLogs("Handling reboot node");

            int chance = GetInt(0, 100);
            if (ModConfig.gcRebootEfficacy.Value < chance || rebootCommandExpired)
            {
                string newText = "\n\n\n\n\n\n\t\tTerminal reboot has failed...\r\n";
                SetTerminalText(newText);
                rebootCommandExpired = true;
                return;
            }
            else
            {
                StartOfRound.Instance.StartCoroutine(RebootTerminalSpooky());
                Plugin.MoreLogs("rebooting terminal");
                return;
            }

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
            if (!ModConfig.gcRebootTerminal.Value)
                return;

            rebootTime = GetInt(30, 60);
            MakeCommand("ghostCodes_reboot", "reboot", $"You have requested to reboot the terminal.\nTotal time of reboot is {rebootTime}.\n\nPlease CONFIRM or DENY.\n\n", false, true, true, false, "ghostCodes_confirm_reboot", "ghostCodes_deny_reboot", "Rebooting Terminal...\r\n", "reboot rejected.", 0, out rebootNode, out rebootKeyword);
        }

        private static void CreatePlayerNameNodes()
        {
            if (!ModConfig.ggDeathNote.Value)
                return;

            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (player != StartOfRound.Instance.localPlayerController && player.isPlayerControlled)
                {
                    string playerName = player.playerUsername;
                    playerName = TerminalFriendlyString(playerName);
                    MakeCommand("ghostCodes_haunt_" + playerName, playerName, "this should not display", false, true, false, true, "", "", "", "", 0, out TerminalNode playerNode, out TerminalKeyword playerKeyword);
                    Plugin.MoreLogs($"Command created for haunting {playerName}");
                }
                else
                    Plugin.MoreLogs("skipping self and non-player controlled players");
                
            }
        }

        private static void DeathNoteFail()
        {
            if (!ModConfig.ggDeathNoteFailChase.Value)
                return;

            Plugin.MoreLogs("death note failed, beginning ghost girl chase");
            Plugin.instance.DressGirl.BeginChasing();
        }

        private static void ValidPlayerNameInput(string s)
        {
            int chance = GetInt(0, 100);
            PlayerControllerB changeToPlayer = GetPlayerFromString(s);
            if(changeToPlayer == null)
            {
                string newText = $"\n\n\n\n\n\nI don't know anyone by that name! Guess you're stuck with me ^.^\n\n";
                SetTerminalText(newText);
                SpookyTerminalColors(true);
                deathNoteStrikes++;
                DeathNoteFail();
                //Plugin.instance.Terminal.StartCoroutine(DelayedReturnToNormalText());
                return;
            }

            if (ModConfig.ggDeathNoteChance.Value < chance)
            {
                string newText = $"\n\n\n\n\n\nYou're not getting rid of me that easy! :3\n\n";
                SetTerminalText(newText);
                SpookyTerminalColors(true);
                deathNoteStrikes++;
                DeathNoteFail();
                //Plugin.instance.Terminal.StartCoroutine(DelayedReturnToNormalText());
                return;
            }
            else if (changeToPlayer.isPlayerControlled && !changeToPlayer.isPlayerDead)
            {
                string newText = $"\n\n\n\n\n\nOkay! I'll go play with {s} now! :3\n\n";
                DressGirl.EndAllGirlStuff();
                NetHandler.Instance.ChangeDressGirlToPlayerServerRpc(s);
                SetTerminalText(newText);
                SpookyTerminalColors(true);
                deathNoteStrikes++;
                //Plugin.instance.Terminal.StartCoroutine(DelayedReturnToNormalText());
                Plugin.MoreLogs("You should not be haunted anymore.");
                return; // make sure you only skip if really necessary
            }
            else
            {
                string newText = $"\n\n\n\n\n\nI think i'll keep having fun with you! :3\n\n";
                SetTerminalText(newText);
                SpookyTerminalColors(true);
                deathNoteStrikes++;
                Plugin.MoreLogs("picked player is likely dead, this is an else statement");
                DeathNoteFail();
                //Plugin.instance.Terminal.StartCoroutine(DelayedReturnToNormalText());
                return;
            }
        }

        internal static void CheckForKeyWord(string keyWord, ref List<TerminalKeyword> keyWordList)
        {
            foreach(TerminalKeyword terminalKeyword in keyWordList)
            {
                if (terminalKeyword.word.Equals(keyWord))
                {
                    keyWordList.Remove(terminalKeyword);
                    Plugin.MoreLogs($"Keyword: [{keyWord}] removed");
                    break;
                }
            }

            return;
        }

        internal static void MakeCommand(string nodeName, string keyWord, string displayText, bool isVerb, bool clearText, bool needsConfirm, bool acceptAnything, string confirmResultName, string denyResultName, string confirmDisplayText, string denyDisplayText, int price, out TerminalNode terminalNode, out TerminalKeyword terminalKeyword)
        {
            List<TerminalKeyword> allKeywordsList = Plugin.instance.Terminal.terminalNodes.allKeywords.ToList();
            CheckForKeyWord(keyWord, ref allKeywordsList);
               
            terminalNode = ScriptableObject.CreateInstance<TerminalNode>();
            terminalNode.name = nodeName;
            terminalNode.displayText = displayText;
            terminalNode.clearPreviousText = clearText;
            terminalNode.itemCost = price;
            terminalNode.overrideOptions = needsConfirm;
            terminalNode.acceptAnything = acceptAnything;

            terminalKeyword = ScriptableObject.CreateInstance<TerminalKeyword>();
            terminalKeyword.name = nodeName+"_keyword";
            terminalKeyword.word = keyWord.ToLower();
            terminalKeyword.isVerb = isVerb;
            terminalKeyword.specialKeywordResult = terminalNode;

            if (needsConfirm)
            {
                MakeConfirmationNode(confirmResultName, denyResultName, confirmDisplayText, denyDisplayText, price, out CompatibleNoun confirm, out CompatibleNoun deny);
                terminalNode.terminalOptions = new CompatibleNoun[] { confirm, deny };
                allKeywordsList.Add(terminalKeyword);
                allKeywordsList.Add(confirm.noun);
                allKeywordsList.Add(deny.noun);
                ghostCodeNodes.Add(terminalNode);
                Plugin.MoreLogs("Node/Keyword added with confirmation nodes");
            }
            else
            {
                allKeywordsList.Add(terminalKeyword);
                ghostCodeNodes.Add(terminalNode);
                Plugin.MoreLogs("Node/Keyword added without confirmation nodes");
            }

            Plugin.instance.Terminal.terminalNodes.allKeywords = allKeywordsList.ToArray();

        }

        internal static void MakeConfirmationNode(string confirmResultName, string denyResultName, string confirmDisplayText, string denyDisplayText, int price, out CompatibleNoun confirm, out CompatibleNoun deny)
        {
            confirm = new CompatibleNoun
            {
                noun = ScriptableObject.CreateInstance<TerminalKeyword>()
            };
            confirm.noun.word = "confirm";
            confirm.noun.isVerb = true;

            confirm.result = ScriptableObject.CreateInstance<TerminalNode>();
            confirm.result.name = confirmResultName;
            confirm.result.displayText = confirmDisplayText;
            confirm.result.clearPreviousText = true;
            confirm.result.itemCost = price;
            ghostCodeNodes.Add(confirm.result);


            deny = new CompatibleNoun
            {
                noun = ScriptableObject.CreateInstance<TerminalKeyword>()
            };
            deny.noun.word = "deny";
            deny.noun.isVerb = true;

            deny.result = ScriptableObject.CreateInstance<TerminalNode>();
            deny.result.name = denyResultName;
            deny.result.displayText = denyDisplayText;
            ghostCodeNodes.Add(deny.result);
        }
    }
}
