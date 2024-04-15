using FacilityMeltdown.API;

namespace ghostCodes.Compatibility
{
    internal class FacilityMeltdown
    {
        internal static bool CheckForMeltdown()
        {
            if (!Plugin.instance.facilityMeltdown)
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
