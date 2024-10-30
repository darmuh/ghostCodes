using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghostCodes
{
    internal class Lights
    {
        internal static void GGFlashlight()
        {
            NetHandler.Instance.GGFlickerServerRpc();
        }

        internal static void FlipLights()
        {
            SignalTranslator.MessWithSignalTranslator();
            Plugin.MoreLogs("who turned out the lights??");
            NetHandler.Instance.GGFacilityLightsServerRpc();
        }
    }
}
