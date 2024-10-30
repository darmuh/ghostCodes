using FacilityMeltdown.API;

namespace ghostCodes.Compatibility
{
    internal class FacilityMeltdown
    {
        internal static bool CheckForMeltdown()
        {
            if (!Plugin.instance.FacilityMeltdown)
                return false;

            if (MeltdownAPI.MeltdownStarted)
            {
                Plugin.MoreLogs("Meltdown detected.");
                return true;
            }

            else
                return false;
        }
    }
}
