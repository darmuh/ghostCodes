namespace ghostCodes
{
    internal class Lights
    {
        internal static void GGFlashlight()
        {
            NetHandler.Instance.FlickerLightsServerRpc(true, false);
            Plugin.MoreLogs("nosfaratu is that you??");
        }

        internal static void FlipLights()
        {
            Plugin.MoreLogs("who turned out the lights??");
            NetHandler.Instance.FacilityBreakerServerRpc();
        }
    }
}
