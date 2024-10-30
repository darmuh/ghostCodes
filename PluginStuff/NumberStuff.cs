using GameNetcodeStuff;
using static ghostCodes.CodeStuff;

namespace ghostCodes
{
    internal class NumberStuff
    {
        internal static System.Random Rand = new();
        internal static int GetInt(int num1, int num2)
        {
            return Rand.Next(num1, num2);
        }

        internal static float GetFloat(float num1, float num2)
        {
            return UnityEngine.Random.Range(num1, num2);
        }

        internal static float GetClampedInsanityPercent(float originalPercentage, float multiplier)
        {
            if (originalPercentage * multiplier >= 100f)
            {
                return 100f;
            }
            else
                return originalPercentage * multiplier;
        }

        internal static int GetAlivePlayers()
        {
            int alivePlayers = 0;

            Plugin.MoreLogs("Getting alive players");
            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (!player.isPlayerDead && player.isPlayerControlled)
                    alivePlayers++;
            }

            return alivePlayers;
        }

        internal static int GetWait(bool useRandom, int min, int max, int setInterval)
        {
            int wait;

            if (useRandom)
            {
                wait = GetInt(min, max);
                Plugin.Spam("using random intervals for wait");
            }
            else
                wait = setInterval;

            return wait;
        }

        internal static bool TryGetObjectNum(out int randomObjectNum)
        {
            if (myTerminalObjects.Count == 0)
            {
                randomObjectNum = -1;
                return false;
            }
            else
            {
                randomObjectNum = Rand.Next(0, myTerminalObjects.Count);
                return true;
            }
        }

        internal static int GetNumberPlayersEmoting()
        {
            int numberPlayersEmoting = 0;

            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if (player.performingEmote)
                    numberPlayersEmoting++;
            }

            Plugin.MoreLogs($"# players emoting: {numberPlayersEmoting}");
            return numberPlayersEmoting;
        }
    }
}
