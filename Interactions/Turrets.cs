using System.Collections.Generic;
using UnityEngine;
using static ghostCodes.CodeStuff;

namespace ghostCodes
{
    internal class Turrets
    {
        internal static void HandleTurretAction()
        {
            if (myTerminalObjects.Count == 0)
                return;

            List<TerminalAccessibleObject> turrets = myTerminalObjects.FindAll(x => x.gameObject.name.Contains("TurretScript") && !x.inCooldown);
            if (turrets.Count > 0)
            {
                int chosen = NumberStuff.Rand.Next(turrets.Count);

                Turret turretobj = turrets[chosen].gameObject.GetComponent<Turret>();
                turretobj.SwitchTurretMode(3);
                turretobj.EnterBerserkModeClientRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                turretobj.EnterBerserkModeServerRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                Plugin.MoreLogs("Turrets do this?!?");
            }
            else
                Plugin.Spam("no turrets to go berserk");
        }
    }
}
