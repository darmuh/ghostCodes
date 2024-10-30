using System.Collections;
using UnityEngine;
using static ghostCodes.CodeStuff;
using Random = UnityEngine.Random;

namespace ghostCodes
{
    internal class Blastdoors
    {
        internal static void HandleHungryDoor(int randomObjectNum, StartOfRound instance)
        {
            if (myTerminalObjects.Count == 0)
                return;

            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("BigDoor"))
            {
                SignalTranslator.MessWithSignalTranslator();
                Plugin.MoreLogs("The door is hungy");
                instance.StartCoroutine(HungryDoor(randomObjectNum));
            }
            else
            {
                myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
            }
        }

        private static IEnumerator HungryDoor(int randomObjectNum)
        {

            float intervalTime = Random.Range(0.2f, 0.7f);
            int numBites = Random.Range(3, 9);
            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("BigDoor"))
            {
                for (int i = 0; i < numBites; i++)
                {
                    myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                    yield return new WaitForSeconds(intervalTime);
                    i++;
                }
                yield break;
            }

        }
    }
}
