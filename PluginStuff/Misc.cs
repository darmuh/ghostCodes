
using GameNetcodeStuff;
using System.Collections.Generic;

namespace ghostCodes
{
    internal class Misc
    {
        internal static List<PlayerControllerB> GetAllLivingPlayers()
        {
            List<PlayerControllerB> allPlayers = [];

            Plugin.MoreLogs("Getting alive players");
            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (!player.isPlayerDead && player.isPlayerControlled)
                    allPlayers.Add(player);
            }

            return allPlayers;
        }

        internal static void LogTime()
        {
            string clockTime = HUDManager.Instance.clockNumber.text;
            string logTime = clockTime.Replace("\n", "").Replace("\r", "");
            Plugin.MoreLogs($"TIME: {logTime}");
        }
    }
}
