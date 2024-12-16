using ghostCodes.Configs;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class RapidFire
    {
        internal static bool startRapidFire = false;

        internal static void RapidFireCheck()
        {

            if (startRapidFire && SetupConfig.RapidFireMaxHours.Value > 0)
            {
                Plugin.GC.LogInfo("max insanity level reached!!! startRapidFire TRUE");
                StartOfRound.Instance.StartCoroutine(Coroutines.RapidFireStart(StartOfRound.Instance));
                return;
            }
        }

        internal static void ReturnFromRapidFire()
        {
            Plugin.MoreLogs("ending rapidFire and returning to main routine");
            if (ShouldRunCodeLooper())
            {
                StartOfRound.Instance.StartCoroutine(Coroutines.CodeLooper(StartOfRound.Instance));
                Coroutines.rapidFireStart = false;
            }
            else
            {
                Plugin.GC.LogInfo("Codes should resume during next haunting");
                Coroutines.rapidFireStart = false;
            }
        }

        internal static void HandleLights()
        {
            //if (Plugin.instance.FacilityMeltdown && !meltdown)
            //meltdown = CheckForMeltdown();

            if (!ModConfig.ModNetworking.Value || !NetObject.NetObjectExists())
                return;

            if (SetupConfig.RapidLights.Value && !lightsFlickering && !endAllCodes)
            {
                NetHandler.Instance.StartRapidFireLightsServerRpc(StartOfRound.Instance.localPlayerController.actualClientId);
                StartOfRound.Instance.StartCoroutine(Lights.WhileLightsFlicker());
                Plugin.MoreLogs("networking enabled, sending alarm lights");
            }
        }

        internal static void TimeCheck(int startHour, int currentHour)
        {
            if (SetupConfig.GhostCodesSettings.HauntingsMode)
                return;

            if (startHour + SetupConfig.RapidFireMaxHours.Value <= currentHour)
            {
                Plugin.MoreLogs("RapidFireMaxHours hit, entering cooldown...");
                endAllCodes = true;
                InitPlugin.RestartPlugin();
            }
        }

        internal static void CheckInsanityToContinue()
        {
            if (!SetupConfig.GhostCodesSettings.InsanityMode)
                return;

            InsanityStuff.GetAllSanity(); //last check to make sure sanity levels are within required levels
        }

        internal static void CountSentCodes()
        {
            if (!SetupConfig.GhostCodesSettings.HauntingsMode || SetupConfig.HauntingsCountCodes.Value)
                Plugin.instance.CodeCount++;
        }

        internal static bool LoopBreakers()
        {
            if (StartOfRound.Instance.shipIsLeaving)
            {
                Plugin.MoreLogs($"Ship is leaving, {Plugin.instance.CodeCount} codes were sent.");
                startRapidFire = false;
                Coroutines.rapidFireStart = false;
                return true;
            }

            if (SetupConfig.GhostCodesSettings.HauntingsMode && StartOfRound.Instance.localPlayerController.isPlayerDead)
            {
                Plugin.MoreLogs("you died, stopping any logic you were calling");
                startRapidFire = false;
                Coroutines.rapidFireStart = false;
                return true;
            }

            if (StartOfRound.Instance.inShipPhase)
            {
                Plugin.MoreLogs($"Ship has already left, {Plugin.instance.CodeCount} codes were sent.");
                startRapidFire = false;
                Coroutines.rapidFireStart = false;
                return true;
            }

            return false;
        }
    }
}
