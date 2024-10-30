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
