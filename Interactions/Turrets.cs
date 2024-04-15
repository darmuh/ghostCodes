using static ghostCodes.CodeStuff;

namespace ghostCodes
{
    internal class Turrets
    {
        internal static void HandleTurretAction(int randomObjectNum)
        {
            if (myTerminalObjects.Count == 0)
                return;

            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("TurretScript") && !myTerminalObjects[randomObjectNum].inCooldown)
            {
                Turret turretobj = myTerminalObjects[randomObjectNum].gameObject.GetComponent<Turret>();
                turretobj.SwitchTurretMode(3);
                turretobj.EnterBerserkModeClientRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                turretobj.EnterBerserkModeServerRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                SignalTranslator.MessWithSignalTranslator();
                Plugin.MoreLogs("Turrets do this?!?");
            }
            else
            {
                myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
            }
        }
    }
}
