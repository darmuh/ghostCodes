using GameNetcodeStuff;
using ghostCodes.Configs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using static ghostCodes.NumberStuff;
using static ghostCodes.TerminalAdditions;

namespace ghostCodes.Interactions
{

    internal class DeathNote
    {
        internal static List<PlayerControllerB> DeathNoteList
        {
            get { return [.. StartOfRound.Instance.allPlayerScripts.Where(x => x.isPlayerControlled)]; }
            set { DeathNoteList = value; }
        }

        internal static int deathNoteStrikes = 0;

        //menu stuff
        internal static int activeSelection = 0;
        internal static int currentPage = 1;
        internal static bool inDeathNote = false;
        internal static bool ghostMessage = false;
        internal static TerminalNode DeathNoteMenu = null!;

        internal static void HandleInput()
        {
            if (Keyboard.current.anyKey.IsPressed() && ghostMessage)
            {
                ExitMenu(true);
                return;
            }    

            if (Keyboard.current[Key.UpArrow].isPressed)
                UpMenu();

            if (Keyboard.current[Key.DownArrow].isPressed)
                DownMenu();

            if (Keyboard.current[Key.Backspace].isPressed)
                ExitMenu(true);

            if (Keyboard.current[Key.LeftArrow].isPressed)
                PrevPage();

            if (Keyboard.current[Key.RightArrow].isPressed)
                NextPage();

            if (Keyboard.current[Key.Enter].isPressed)
                MenuSelect();

            

        }

        internal static void LoadPage(string displayText)
        {
            DeathNoteMenu.displayText = displayText;
            Plugin.instance.Terminal.LoadNewNode(DeathNoteMenu);
        }

        internal static void MenuSelect()
        {
            ghostMessage = true;
            int chance = GetInt(0, 100);
            PlayerControllerB changeToPlayer = DeathNoteList[activeSelection];

            if(changeToPlayer == StartOfRound.Instance.localPlayerController)
            {
                Plugin.MoreLogs("Why pick yourself? This wont count as a fail because im a forgiving mod dev");
                LoadPage($"\n\n\n\n\n\n<color=#b81b1b>You can't pick yourself silly! ^.^</color>\n\n");
                return;
            }

            if (InteractionsConfig.DeathNote.Value < chance)
            {
                deathNoteStrikes++;
                DeathNoteFail();
                Plugin.MoreLogs("Unlucky for you you're still haunted!");
                LoadPage($"\n\n\n\n\n\n<color=#b81b1b>You're not getting rid of me that easy! :3</color>\n\n");
            }
            else if (changeToPlayer.isPlayerControlled && !changeToPlayer.isPlayerDead)
            {
                
                DressGirl.EndAllGirlStuff();
                NetHandler.Instance.ChangeDressGirlToPlayerServerRpc(changeToPlayer.actualClientId);
                deathNoteStrikes++;
                Plugin.MoreLogs("You should not be haunted anymore.");
                LoadPage($"\n\n\n\n\n\n<color=#b81b1b>Okay! I'll go play with {changeToPlayer.playerUsername} now! :3</color>\n\n");
            }
            else
            {
                deathNoteStrikes++;
                Plugin.MoreLogs("picked player is likely dead, this is an else statement (really unlucky)");
                DeathNoteFail();
                LoadPage($"\n\n\n\n\n\n<color=#b81b1b>I think i'll keep having fun with you! :3</color>\n\n");
            }
        }

        internal static void UpMenu()
        {
            if (activeSelection > 0)
                activeSelection--;

            LoadPage(GetPlayerList(activeSelection, 10, ref currentPage));
        }

        internal static void DownMenu()
        {
            activeSelection++;
            LoadPage(GetPlayerList(activeSelection, 10, ref currentPage));
        }

        internal static void NextPage()
        {
            currentPage++;
            LoadPage(GetPlayerList(activeSelection, 10, ref currentPage));
        }

        internal static void PrevPage()
        {
            if (currentPage > 1)
                currentPage--;

            LoadPage(GetPlayerList(activeSelection, 10, ref currentPage));
        }

        internal static IEnumerator MenuStart()
        {
            if (inDeathNote)
                yield break;

            yield return new WaitForEndOfFrame();
            inDeathNote = true;
            yield return new WaitForEndOfFrame();
            Plugin.instance.Terminal.screenText.DeactivateInputField();
            Plugin.instance.Terminal.screenText.interactable = false;

            yield break;
        }

        internal static void ExitMenu(bool enableInput)
        {
            Plugin.instance.Terminal.StartCoroutine(MenuClose(enableInput));
        }

        internal static IEnumerator MenuClose(bool enableInput)
        {
            yield return new WaitForEndOfFrame();
            inDeathNote = false;
            ghostMessage = false;
            yield return new WaitForEndOfFrame();
            Plugin.instance.Terminal.LoadNewNode(Plugin.instance.Terminal.terminalNodes.specialNodes.ToArray()[1]);
            yield return new WaitForEndOfFrame();
            if (Plugin.instance.TerminalStuff)
                Compatibility.TerminalStuff.StopShortCuts(false);

            if (enableInput)
            {
                Plugin.instance.Terminal.screenText.ActivateInputField();
                Plugin.instance.Terminal.screenText.interactable = true;
            }

            yield break;
        }

        internal static string GetPlayerList(int activeIndex, int pageSize, ref int currentPage)
        {
            Plugin.Spam($"activeIndex: {activeIndex}\npageSize: {pageSize}\ncurrentPage: {currentPage}");
            if (DeathNoteList == null)
            {
                Plugin.ERROR("ghostcodes FATAL ERROR: DeathNoteList is NULL");
                return "ghostcodes FATAL ERROR: DeathNoteList is NULL";
            }

            int listing = DeathNoteList.Count;

            if (listing == 0)
            {
                Plugin.Spam("DeathNoteList empty!!");
                return "Empty Player Listing :(\r\n";
            }

            Plugin.Spam($"listing count: {listing}");

            // Ensure currentPage is within valid range
            currentPage = Mathf.Clamp(currentPage, 1, Mathf.CeilToInt((float)listing / pageSize));

            // Calculate the start and end indexes for the current page
            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Mathf.Min(startIndex + pageSize, listing);
            int totalItems = 0;
            int emptySpace;

            StringBuilder message = new();

            message.Append($"Who else should I play with??? :>\r\n");
            message.Append("\r\n");

            // Recalculate activeIndex based on the current page
            // Ensure activeIndex is within the range of items on the current page
            activeIndex = Mathf.Clamp(activeIndex, startIndex, endIndex - 1);
            Plugin.Spam($"activeSelection: {activeSelection} activeIndex: {activeIndex}");
            activeSelection = activeIndex;

            DeathNoteList.RemoveAll(x => x == null);

            // Iterate through each item in the current page
            for (int i = startIndex; i < endIndex; i++)
            {

                // Prepend ">" to the active item and append "[EQUIPPED]" line if applicable
                string menuItem;
                PlayerControllerB victim = DeathNoteList[i];

                if ( victim != null)
                {
                    menuItem = (i == activeIndex)
                    ? $"> "
                    : $"";

                    menuItem += $"{victim.playerUsername}";
                }
                else
                {
                    menuItem = (i == activeIndex)
                    ? $"> {i} - **Unknown (cannot select)**"
                    : $"{i} - **Unknown (cannot select)**";
                    Plugin.WARNING($"player is null at index [ {i} ] of DeathNoteList!");
                }

                // Display the menu item
                message.Append(menuItem + "\n");
                totalItems++;
            }

            emptySpace = pageSize - totalItems;

            for (int i = 0; i < emptySpace; i++)
            {
                message.Append("\r\n");
            }

            // Display pagination information
            //Page [LeftArrow] < 6/10 > [RightArrow]
            message.Append("\r\n\r\n");
            message.Append($"Page [LeftArrow] < {currentPage}/{Mathf.CeilToInt((float)listing / pageSize)} > [RightArrow]\r\n");
            message.Append($"Leave Menu (if you dare): [BackSpace]\r\nSelect Friend (victim): [Enter]\r\n\r\n");
            return message.ToString();
        }
    }
}
