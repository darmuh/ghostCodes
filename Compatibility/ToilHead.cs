using Object = UnityEngine.Object;
using static ghostCodes.CodeStuff;
using System.Collections.Generic;
using UnityEngine;
using com.github.zehsteam.ToilHead.MonoBehaviours;
using static ghostCodes.NumberStuff;
using static ghostCodes.CodeHandling;
using System.Linq;

namespace ghostCodes.Compatibility
{
    internal class ToilHead
    {
        internal static List<ToilHeadTurretBehaviour> toilHeadSprings = new List<ToilHeadTurretBehaviour>();
        internal static List<FollowTerminalAccessibleObjectBehaviour> toilHeadObjects = new List<FollowTerminalAccessibleObjectBehaviour>();

        internal static void CheckForToilHeadObjects()
        {
            if (!Plugin.instance.toilHead || !gcConfig.toilHeadStuff.Value)
                return;

            Plugin.MoreLogs("Checking for ToilHead Terminal-Accessible-Objects");
            CheckForToilHead();

            if(toilHeadSprings.Count > 0 && toilHeadObjects.Count > 0)
            {
                Plugin.MoreLogs("Adding toilhead actions");
                possibleActions.Add(new ActionPercentage("toilHeadDisable", () => HandleToilHeadCodeAction(), gcConfig.toilHeadTurretDisableChance.Value));
                possibleActions.Add(new ActionPercentage("toilHeadBerserk", () => HandleToilHeadBerserkAction(), gcConfig.toilHeadTurretBerserkChance.Value));
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

        internal static void HandleToilHeadCodeAction()
        {
            if (!Plugin.instance.toilHead)
                return;

            if (toilHeadSprings.Count == 0)
                return;

            int randomObjectNum = GetInt(0, toilHeadSprings.Count - 1);
            toilHeadObjects[randomObjectNum].CallFunctionFromTerminal(); 
            SignalTranslator.MessWithSignalTranslator();
            Plugin.MoreLogs("Turret, ON COILHEAD, disabled");
        }

        internal static void HandleToilHeadBerserkAction()
        {
            if (!Plugin.instance.toilHead)
                return;

            if (toilHeadSprings.Count == 0)
                return;

            int randomObjectNum = GetInt(0, toilHeadSprings.Count - 1);
            toilHeadSprings[randomObjectNum].EnterBerserkModeServerRpc();
            SignalTranslator.MessWithSignalTranslator();
            Plugin.MoreLogs("Turret, ON COILHEAD, going BERSERK");

        }
    }
}
