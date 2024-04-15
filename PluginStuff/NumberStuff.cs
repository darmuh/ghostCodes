using Random = UnityEngine.Random;
using static ghostCodes.CodeStuff;
using GameNetcodeStuff;

namespace ghostCodes
{
    internal class NumberStuff
    {
        internal static int GetInt(int num1, int num2)
        {
            return Random.Range(num1, num2);
        }

        internal static float GetFloat(float num1, float num2)
        {
            return Random.Range(num1, num2);
        }

        internal static int GetAlivePlayers()
        {
            int alivePlayers = 0;

            Plugin.MoreLogs("Getting alive players");
            foreach(PlayerControllerB player in Plugin.instance.players)
            {
                if(!player.isPlayerDead)
                    alivePlayers++;
            }

            return alivePlayers;
        }

        internal static int GetFirstWait()
        {
            int firstWait;

            if (gcConfig.useRandomIntervals.Value)
            {
                firstWait = GetInt(gcConfig.gcFirstRandIntervalMin.Value, gcConfig.gcFirstRandIntervalMax.Value);
                Plugin.MoreLogs("Using random intervals for first wait");
            }
            else
                firstWait = gcConfig.gcFirstSetInterval.Value;

            return firstWait;


        }

        internal static int GetSecondWait()
        {
            int secondWait;

            if (gcConfig.useRandomIntervals.Value)
            {
                secondWait = GetInt(gcConfig.gcSecondRandIntervalMin.Value, gcConfig.gcSecondRandIntervalMax.Value);
                Plugin.MoreLogs("Using random intervals for second wait");
            }
            else
                secondWait = gcConfig.gcSecondSetInterval.Value;

            return secondWait;
        }

        internal static int GetWaitAfterCode()
        {
            int waitAfterCode;
            if (gcConfig.useRandomIntervals.Value)
            {
                waitAfterCode = GetInt(gcConfig.gcRandIntervalACMin.Value, gcConfig.gcRandIntervalACMax.Value);
                Plugin.MoreLogs("Using random intervals for after code wait");
            }
            else
                waitAfterCode = gcConfig.gcSetIntervalAC.Value;

            return waitAfterCode;
        }

        internal static int GetObjectNum()
        {
            int randomObjectNum;

            if (myTerminalObjects.Count == 0)
                randomObjectNum = - 1;
            else            
                randomObjectNum = Random.Range(0, myTerminalObjects.Count-1);
            
            return randomObjectNum;
        }

        internal static int GetNumberPlayersEmoting()
        {
            int numberPlayersEmoting = 0;

            foreach (PlayerControllerB player in Plugin.instance.players)
            {
                if(player.performingEmote)
                    numberPlayersEmoting++;
            }

            Plugin.MoreLogs($"# players emoting: {numberPlayersEmoting}");
            return numberPlayersEmoting;
        }
    }
}
