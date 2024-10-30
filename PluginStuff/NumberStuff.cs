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
                if(!player.isPlayerDead && player.isPlayerControlled)
                    alivePlayers++;
            }

            return alivePlayers;
        }

        internal static int GetFirstWait()
        {
            int firstWait;

            if (ModConfig.useRandomIntervals.Value)
            {
                firstWait = GetInt(ModConfig.gcFirstRandIntervalMin.Value, ModConfig.gcFirstRandIntervalMax.Value);
                Plugin.MoreLogs("Using random intervals for first wait");
            }
            else
                firstWait = ModConfig.gcFirstSetInterval.Value;

            return firstWait;


        }

        internal static int GetSecondWait()
        {
            int secondWait;

            if (ModConfig.useRandomIntervals.Value)
            {
                secondWait = GetInt(ModConfig.gcSecondRandIntervalMin.Value, ModConfig.gcSecondRandIntervalMax.Value);
                Plugin.MoreLogs("Using random intervals for second wait");
            }
            else
                secondWait = ModConfig.gcSecondSetInterval.Value;

            return secondWait;
        }

        internal static int GetWaitAfterCode()
        {
            int waitAfterCode;
            if (ModConfig.useRandomIntervals.Value)
            {
                waitAfterCode = GetInt(ModConfig.gcRandIntervalACMin.Value, ModConfig.gcRandIntervalACMax.Value);
                Plugin.MoreLogs("Using random intervals for after code wait");
            }
            else
                waitAfterCode = ModConfig.gcSetIntervalAC.Value;

            return waitAfterCode;
        }

        internal static void GetObjectNum(out int randomObjectNum)
        {
            if (myTerminalObjects.Count == 0)
                randomObjectNum = - 1;
            else            
                randomObjectNum = Random.Range(0, myTerminalObjects.Count-1);

            return;
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
