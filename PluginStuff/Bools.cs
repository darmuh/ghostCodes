using GameNetcodeStuff;
using ghostCodes.Configs;
using static ghostCodes.CodeStuff;
using static ghostCodes.Coroutines;

namespace ghostCodes
{
    internal class Bools
    {
        internal static bool lightsFlickering = false;
        internal static bool appPullInvoked = false;
        public static bool endAllCodes = false;

        internal static bool IsCodeSent()
        {
            return Plugin.instance.CodeSent;
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
            if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                return true;

            if (!ModConfig.ModNetworking.Value)
                return true;

            if (Plugin.instance.Terminal == null)
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

        public static bool GhostCodesShouldNotBePresentEver()
        {
            if (StartOfRound.Instance.currentLevel.name == "CompanyBuildingLevel" || StartOfRound.Instance.currentLevel.riskLevel == "Safe")
                return true;

            if (StartOfRound.Instance.inShipPhase)
                return true;

            if (Plugin.instance.CodeCount > Plugin.instance.RandCodeAmount)
                return true;

            return false;
        }

        public static bool CanSendCodes()
        {
            if (endAllCodes || GhostCodesShouldNotBePresentEver())
                return false;

            if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                return GameNetworkManager.Instance.gameHasStarted;
            else if (Plugin.instance.DressGirl == null)
            {
                Plugin.MoreLogs("dressgirl instance is null");
                return false;
            }
            else
                return GameNetworkManager.Instance.gameHasStarted && Plugin.instance.DressGirl.hauntingLocalPlayer == StartOfRound.Instance.localPlayerController;
        }

        internal static bool KeepSendingCodes()
        {
            if (endAllCodes || GhostCodesShouldNotBePresentEver())
                return false;

            if (StartOfRound.Instance.shipIsLeaving || StartOfRound.Instance.allPlayersDead)
                return false;

            if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                return true;
            else
            {
                if (Plugin.instance.DressGirl == null)
                    return false;

                if (!Plugin.instance.DressGirl.hauntingLocalPlayer)
                    return false;

                if (Plugin.instance.DressGirl.hauntingPlayer.isPlayerDead)
                    return false;

                if (rapidFireStart && !DressGirl.girlIsChasing)
                    return false;

                return true;
            }

        }

        internal static bool DressGirlStartCodes()
        {
            if (endAllCodes || RapidFire.startRapidFire || GhostCodesShouldNotBePresentEver())
                return false;

            if (StartOfRound.Instance.shipIsLeaving || StartOfRound.Instance.allPlayersDead)
                return false;

            if (!Plugin.instance.DressGirl.staringInHaunt || Plugin.instance.DressGirl.hauntingPlayer.isPlayerDead)
                return false;

            return true;
        }

        internal static bool IsThisaMine(int randomObjectNum)
        {
            if (randomObjectNum < 0)
                return false;

            if (myTerminalObjects.Count == 0)
                return false;

            if (myTerminalObjects[randomObjectNum] == null)
                return false;

            if (myTerminalObjects[randomObjectNum].gameObject == null)
                return false;

            if (myTerminalObjects[randomObjectNum].GetComponent<Landmine>() != null)
                return true;

            return false;

        }

        internal static bool IsThisaTurret(int randomObjectNum)
        {
            if (randomObjectNum < 0)
                return false;

            if (myTerminalObjects.Count == 0)
                return false;

            if (myTerminalObjects[randomObjectNum] == null)
                return false;

            if (myTerminalObjects[randomObjectNum].gameObject == null)
                return false;

            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("TurretScript"))
                return true;

            return false;

        }

        internal static bool IsThisaBigDoor(int randomObjectNum)
        {
            if (randomObjectNum < 0)
                return false;

            if (myTerminalObjects.Count == 0)
                return false;

            if (myTerminalObjects[randomObjectNum] == null)
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
            foreach (PlayerControllerB player in Plugin.instance.players)
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
