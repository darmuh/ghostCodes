using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace ghostCodes
{
    internal class Teleporters
    {

        internal static void InteractWithAnyTP(int tpNum)
        {
            if(tpNum == 0 && Plugin.instance.NormalTP != null)
            {
                Plugin.MoreLogs("Messing with NormalTP");
                Plugin.instance.NormalTP.PressTeleportButtonServerRpc();
                Plugin.MoreLogs("NormalTP button pressed via ServerRpc");
            }
            else if(tpNum == 1 && Plugin.instance.InverseTP != null)
            {
                Plugin.MoreLogs("Messing with InverseTP");
                Plugin.instance.InverseTP.PressTeleportButtonServerRpc();
                Plugin.MoreLogs("InverseTP button pressed via ServerRpc");
            }
        }

    }
}
