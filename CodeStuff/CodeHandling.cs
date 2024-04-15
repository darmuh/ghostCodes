using System;
using System.Collections.Generic;
using static ghostCodes.CodeStuff;
using static ghostCodes.CodeActions;
using static ghostCodes.NumberStuff;

namespace ghostCodes
{
    internal class CodeHandling
    {
        internal static List<ActionPercentage> possibleActions = new List<ActionPercentage>();
        internal static List<Action> chosenActions = new List<Action>();
        
        internal static void HandleGhostCodeSending(StartOfRound instance)
        {
            Misc.LogTime();
            int randomObjectNum = GetObjectNum();
            chosenActions.Clear();
            InitPossibleActions(instance, randomObjectNum);

            chosenActions = ActionPercentage.ChooseActionsFromPercentages(possibleActions);
            HandleChosenActions(randomObjectNum);
            SoundSystem.InsanityAmbient();
            SoundSystem.HandleCodeEffects();
        }

        internal static void HandleRapidFireCodeChoices(StartOfRound instance, int number)
        {
            Misc.LogTime();
            chosenActions.Clear();
            InitPossibleActions(instance, number);

            chosenActions = ActionPercentage.ChooseActionsFromPercentages(possibleActions);
            HandleChosenActions(number);
            SoundSystem.InsanityAmbient();
            SoundSystem.HandleCodeEffects();
        }

        private static void HandleChosenActions(int randomObjectNum)
        {
            if (chosenActions == null && myTerminalObjects.Count == 0)
            {
                Bools.endAllCodes = true;
                return;
            }

            if (chosenActions.Count > 0)
            {
                foreach (var action in chosenActions)
                {
                    action.Invoke();
                }
            }
            else
            {
                myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                Plugin.GC.LogInfo("No Special action chosen, calling function from terminal");
            }
        }

    }
}
