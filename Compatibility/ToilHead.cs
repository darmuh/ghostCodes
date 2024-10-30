using com.github.zehsteam.ToilHead.MonoBehaviours;
using ghostCodes.Configs;
using System.Collections.Generic;
using System.Linq;
using static ghostCodes.CodeHandling;
using static ghostCodes.NumberStuff;
using Object = UnityEngine.Object;

namespace ghostCodes.Compatibility
{
    internal class ToilHead
    {
        internal static List<ToilHeadTurretBehaviour> toilHeadSprings = [];
        internal static List<FollowTerminalAccessibleObjectBehaviour> toilHeadObjects = [];

        internal static void CheckForToilHeadObjects()
        {
            if (!Plugin.instance.ToilHead || InteractionsConfig.ToilHeadTurretBerserk.Value < 1 || InteractionsConfig.ToilHeadTurretDisable.Value < 1)
                return;

            Plugin.MoreLogs("Checking for ToilHead Terminal-Accessible-Objects");
            CheckForToilHead();

            if (toilHeadSprings.Count > 0 && toilHeadObjects.Count > 0)
            {
                int randomObjectNum = GetInt(0, toilHeadSprings.Count);
                if (toilHeadObjects[randomObjectNum].inCooldown)
                    return;

                Plugin.MoreLogs("Adding toilhead actions");
                if (InteractionsConfig.ToilHeadTurretDisable.Value < 1)
                    possibleActions.Add(new ActionPercentage("toilHeadDisable", () => HandleToilHeadCodeAction(randomObjectNum), InteractionsConfig.ToilHeadTurretDisable.Value));
                if (InteractionsConfig.ToilHeadTurretBerserk.Value < 1)
                    possibleActions.Add(new ActionPercentage("toilHeadBerserk", () => HandleToilHeadBerserkAction(randomObjectNum), InteractionsConfig.ToilHeadTurretBerserk.Value));
            }
        }

        internal static void CheckForToilHead()
        {
            Plugin.MoreLogs("looking for toilheads...");

            toilHeadObjects = Object.FindObjectsOfType<FollowTerminalAccessibleObjectBehaviour>().ToList();
            toilHeadSprings = Object.FindObjectsOfType<ToilHeadTurretBehaviour>().ToList();

            if (toilHeadObjects.Count > 0)
                Plugin.MoreLogs($"Found FollowTerminalAccessibleObjectBehaviour(s) [{toilHeadObjects.Count}]");

            if (toilHeadSprings.Count > 0)
                Plugin.MoreLogs($"Found ToilHeadTurretBehaviour(s) [{toilHeadSprings.Count}]");

            if (toilHeadObjects.Count != toilHeadSprings.Count)
                Plugin.MoreLogs($"Toilheads objects mismatch! toilHeadObjects: {toilHeadObjects.Count} toilHeadSprings: {toilHeadSprings.Count}");
        }

        internal static void HandleToilHeadCodeAction(int randomObjectNum)
        {
            if (!Plugin.instance.ToilHead)
                return;

            if (toilHeadSprings.Count == 0)
                return;

            toilHeadObjects[randomObjectNum].CallFunctionFromTerminal();
            SignalTranslator.MessWithSignalTranslator();
            Plugin.MoreLogs("Turret, ON COILHEAD, disabled");
        }

        internal static void HandleToilHeadBerserkAction(int randomObjectNum)
        {
            if (!Plugin.instance.ToilHead)
                return;

            if (toilHeadSprings.Count == 0)
                return;

            toilHeadSprings[randomObjectNum].EnterBerserkModeServerRpc();
            SignalTranslator.MessWithSignalTranslator();
            Plugin.MoreLogs("Turret, ON COILHEAD, going BERSERK");

        }
    }
}
