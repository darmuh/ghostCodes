using GameNetcodeStuff;
using System.Xml.Linq;
using UnityEngine;
using static ghostCodes.CodeStuff;

namespace ghostCodes
{
    internal class Bools
    {
        internal static bool lightsFlickering = false;
        public static bool endAllCodes = false;

        internal static bool IsCodeSent()
        {
            return Plugin.instance.ghostCodeSent;
        }

        internal static bool IsInsideFactory()
        {
            return StartOfRound.Instance.localPlayerController.isInsideFactory;
        }

        internal static bool IsHourInRange(int hour, int startHour, int endHour)
        {
            return hour > startHour && hour <= endHour;
        }

        internal static bool ShouldRunCodeLooper()
        {
            if (!gcConfig.ghostGirlEnhanced.Value)
                return true;

            if (Plugin.instance.bypassGGE)
                return true;

            if (!gcConfig.ModNetworking.Value)
                return true;

            if(Plugin.instance.Terminal == null)
            {
                Plugin.MoreLogs("Terminal instance null");
                return false;
            }

            return false;
        }

        internal static bool ShouldInitPlugin()
        {
            if (Plugin.instance.Terminal == null)
            {
                Plugin.MoreLogs("Terminal instance null");
                return false;
            }

            return true;
        }

        public static bool CanSendCodes()
        {
            if (endAllCodes)
                return false;

            if (!gcConfig.ghostGirlEnhanced.Value || Plugin.instance.bypassGGE)
                return GameNetworkManager.Instance.gameHasStarted;
            else if(Plugin.instance.DressGirl == null)
            {
                Plugin.MoreLogs("dressgirl instance is null");
                return false;
            }   
            else
                return GameNetworkManager.Instance.gameHasStarted && Plugin.instance.DressGirl.hauntingLocalPlayer == StartOfRound.Instance.localPlayerController;
        }

        internal static bool KeepSendingCodes()
        {
            if (endAllCodes)
                return false;

            if (!gcConfig.ghostGirlEnhanced.Value || Plugin.instance.bypassGGE)
                return !StartOfRound.Instance.allPlayersDead && Plugin.instance.codeCount < Plugin.instance.randGC && !StartOfRound.Instance.shipIsLeaving;
            else
                return !StartOfRound.Instance.allPlayersDead && Plugin.instance.codeCount < Plugin.instance.randGC && !StartOfRound.Instance.shipIsLeaving && !Plugin.instance.DressGirl.hauntingPlayer.isPlayerDead && Plugin.instance.DressGirl.hauntingLocalPlayer;
        }

        internal static bool DressGirlStartCodes()
        {
            if (endAllCodes || RapidFire.startRapidFire)
                return false;

            return !StartOfRound.Instance.allPlayersDead && Plugin.instance.codeCount < Plugin.instance.randGC && !StartOfRound.Instance.shipIsLeaving && Plugin.instance.DressGirl.staringInHaunt && !Plugin.instance.DressGirl.hauntingPlayer.isPlayerDead;
        }

        internal static bool isThisaMine(int randomObjectNum)
        {
            if(myTerminalObjects.Count == 0) 
                return false;

            if (myTerminalObjects[randomObjectNum].GetComponent<Landmine>() != null)
                return true;

            return false;

        }

        internal static bool isThisaTurret(int randomObjectNum)
        {
            if (myTerminalObjects.Count == 0)
                return false;

            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("TurretScript"))
                return true;

            return false;

        }

        internal static bool isThisaBigDoor(int randomObjectNum)
        {
            if (randomObjectNum < 0)
                return false;

            if (myTerminalObjects.Count == 0)
                return false;

            if (myTerminalObjects[randomObjectNum].gameObject == null)
                return false;

            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("BigDoor"))
                return true;

            return false;

        }

        internal static bool AreEnoughPlayersDancing(int totalPlayers, int playersDancing, int requiredPercentage)
        {
            if (playersDancing > totalPlayers * (requiredPercentage / 100))
                return true;

            return false;
        }

        internal static bool IsThisEffective(int percentChance)
        {
            int randomNumber = NumberStuff.GetInt(0, 100);
            if (percentChance > randomNumber)
                return true;

            return false;
        }

        internal static bool CheckForPlayerName(string playerName)
        {
            foreach(PlayerControllerB player in Plugin.instance.players)
            {
                if (playerName.Contains(player.playerUsername))
                    return true;
            }

            return false;
        }

        internal static bool AreAnyPlayersInShip()
        {
            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (!player.isPlayerDead && player.isInHangarShipRoom)
                {
                    return true;
                }
            }

            Plugin.MoreLogs("No players detected in ship");
            return false;
        }

        internal static bool AreAnyPlayersInFacility()
        {
            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (!player.isPlayerDead && player.isInsideFactory)
                {
                    return true;
                }
            }

            Plugin.MoreLogs("No players detected in facility");
            return false;
        }

        internal static bool IsAnyPlayerUsingTerminal(out PlayerControllerB terminalUser)
        {
            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (!player.isPlayerDead && player.inTerminalMenu)
                {
                    terminalUser = player;
                    return true;
                }
            }

            terminalUser = null;
            return false;
        }
    }
}
