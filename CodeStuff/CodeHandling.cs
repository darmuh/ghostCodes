using System;
using System.Collections.Generic;
using static ghostCodes.CodeActions;
using static ghostCodes.CodeStuff;
using static ghostCodes.NumberStuff;

namespace ghostCodes
{
    internal class CodeHandling
    {
        internal static List<ActionPercentage> possibleActions = [];
        internal static List<Action> chosenActions = [];

        internal static void HandleGhostCodeSending(StartOfRound instance)
        {
            Misc.LogTime();
            if (TryGetObjectNum(out int randomObjectNum))
            {
                chosenActions.Clear();
                InitPossibleActions(instance, randomObjectNum);
            }
            else
                Plugin.WARNING("Unable to get randomObjectNum!");

            if (possibleActions.Count < 1)
                return;

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
            if (possibleActions.Count < 1)
                return;

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
                if (randomObjectNum == -1)
                    return;

                myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                Plugin.GC.LogInfo("No Special action chosen, calling function from terminal");
            }
        }

    }
}
