using com.github.zehsteam.ToilHead.MonoBehaviours;
using ghostCodes.Configs;
using System.Collections.Generic;
using static ghostCodes.CodeHandling;
using static ghostCodes.NumberStuff;
using Object = UnityEngine.Object;

namespace ghostCodes.Compatibility
{
    internal class ToilHead
    {
        internal static List<ToilHeadTurretBehaviour> toilHeadSprings = [];
        internal static List<FollowTerminalAccessibleObjectBehaviour> toilHeadObjects = [];

        internal static bool CheckForToilHeadObjects()
        {
            if (!Plugin.instance.ToilHead || (InteractionsConfig.ToilHeadTurretBerserk.Value < 1 && InteractionsConfig.ToilHeadTurretDisable.Value < 1))
                return false;

            Plugin.MoreLogs("Checking for ToilHead Terminal-Accessible-Objects");
            CheckForToilHead();

            if (toilHeadSprings.Count > 0 && toilHeadObjects.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        internal static void CheckForToilHead()
        {
            Plugin.MoreLogs("looking for toilheads...");

            toilHeadObjects = [.. Object.FindObjectsOfType<FollowTerminalAccessibleObjectBehaviour>()];
            toilHeadSprings = [.. Object.FindObjectsOfType<ToilHeadTurretBehaviour>()];

            if (toilHeadObjects.Count > 0)
                Plugin.MoreLogs($"Found FollowTerminalAccessibleObjectBehaviour(s) [{toilHeadObjects.Count}]");

            if (toilHeadSprings.Count > 0)
                Plugin.MoreLogs($"Found ToilHeadTurretBehaviour(s) [{toilHeadSprings.Count}]");

            if (toilHeadObjects.Count != toilHeadSprings.Count)
                Plugin.MoreLogs($"Toilheads objects mismatch! toilHeadObjects: {toilHeadObjects.Count} toilHeadSprings: {toilHeadSprings.Count}");
        }

        internal static void HandleToilHeadCodeAction()
        {
            if (!Plugin.instance.ToilHead)
                return;

            if (toilHeadSprings.Count == 0)
                return;

            int randomObjectNum = Rand.Next(toilHeadObjects.Count);

            toilHeadObjects[randomObjectNum].CallFunctionFromTerminal();
            Plugin.MoreLogs("Turret, ON COILHEAD, disabled");
        }

        internal static void HandleToilHeadBerserkAction()
        {
            if (!Plugin.instance.ToilHead)
                return;

            if (toilHeadSprings.Count == 0)
                return;

            int randomObjectNum = Rand.Next(toilHeadObjects.Count);

            toilHeadSprings[randomObjectNum].EnterBerserkModeServerRpc();
            Plugin.MoreLogs("Turret, ON COILHEAD, going BERSERK");

        }
    }
}
