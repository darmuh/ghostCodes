﻿using ghostCodes.Compatibility;
using System.Runtime.CompilerServices;
using static ghostCodes.Bools;
using static ghostCodes.Compatibility.FacilityMeltdown;

namespace ghostCodes
{
    internal class RapidFire
    {
        internal static bool startRapidFire = false;
        internal static bool meltdown = false;

        internal static void RapidFireCheck()
        {

            if (startRapidFire && ModConfig.insanityRapidFire.Value)
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
            if (Plugin.instance.facilityMeltdown && !meltdown)
                meltdown = CheckForMeltdown();

            if (ModConfig.ModNetworking.Value && ModConfig.rfRapidLights.Value && !lightsFlickering && !meltdown)
            {
                StartOfRound.Instance.StartCoroutine(Coroutines.AlarmLights());
                Plugin.MoreLogs("networking enabled, sending alarm lights");
                lightsFlickering = true;
            }
        }

        internal static void TimeCheck(int startHour, int currentHour)
        {
            if (!ModConfig.GGEbypass.Value && !ModConfig.ghostGirlEnhanced.Value)
                return;

            if (!Plugin.instance.bypassGGE)
                return;

            if (startHour + ModConfig.rapidFireMaxHours.Value <= currentHour)
            {
                Plugin.MoreLogs("rapidFireMaxHours hit, entering cooldown...");
                endAllCodes = true;
                InitPlugin.RestartPlugin();
            }
        }

        internal static void CheckInsanityToContinue()
        {
            if (!ModConfig.gcInsanity.Value)
                return;

            if (!ModConfig.ghostGirlEnhanced.Value || Plugin.instance.bypassGGE)
                InsanityStuff.GetAllSanity(); //last check to make sure sanity levels are within required levels
        }

        internal static void CountSentCodes()
        {
            if(!ModConfig.ghostGirlEnhanced.Value || !ModConfig.ggIgnoreCodeCount.Value || Plugin.instance.bypassGGE)
                Plugin.instance.codeCount++;
        }

        internal static bool LoopBreakers()
        {
            if (StartOfRound.Instance.shipIsLeaving)
            {
                Plugin.MoreLogs($"Ship is leaving, {Plugin.instance.codeCount} codes were sent.");
                startRapidFire = false;
                Coroutines.rapidFireStart = false;
                return true;
            }

            if (ModConfig.ghostGirlEnhanced.Value && StartOfRound.Instance.localPlayerController.isPlayerDead)
            {
                Plugin.MoreLogs("you died, stopping any logic you were calling");
                startRapidFire = false;
                Coroutines.rapidFireStart = false;
                return true;
            }

            if(StartOfRound.Instance.inShipPhase)
            {
                Plugin.MoreLogs($"Ship has already left, {Plugin.instance.codeCount} codes were sent.");
                startRapidFire = false;
                Coroutines.rapidFireStart = false;
                return true;
            }

            return false;
        }
    }
}
