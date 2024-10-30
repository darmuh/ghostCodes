namespace ghostCodes
{
    internal class Teleporters
    {
        internal static void InteractWithAnyTP(int tpNum)
        {
            if (tpNum == 0 && OpenLib.Common.Teleporter.NormalTP != null)
            {
                Plugin.MoreLogs("Messing with NormalTP");
                OpenLib.Common.Teleporter.NormalTP.PressTeleportButtonServerRpc();
                Plugin.MoreLogs("NormalTP button pressed via ServerRpc");
            }
            else if (tpNum == 1 && OpenLib.Common.Teleporter.InverseTP != null)
            {
                Plugin.MoreLogs("Messing with InverseTP");
                OpenLib.Common.Teleporter.InverseTP.PressTeleportButtonServerRpc();
                Plugin.MoreLogs("InverseTP button pressed via ServerRpc");
            }
        }

    }
}
