using ghostCodes.Configs;
using ghostCodes.Patches;
using HarmonyLib;
using static ghostCodes.Bools;

namespace ghostCodes
{
    public class GamePatchStuff
    {
        /*
        [HarmonyPatch(typeof(RoundManager), "SetBigDoorCodes")]
        public class StartRoundPatch : RoundManager
        {

            static void Postfix()
            {
                if (!StartOfRound.Instance.inShipPhase)
                {
                    if (StartOfRound.Instance.currentLevel.name == "CompanyBuildingLevel" || StartOfRound.Instance.currentLevel.riskLevel == "Safe")
                        return;

                    InitPlugin.StartTheRound();
                }
                else
                    Plugin.WARNING("Codes should not be generated, ship is in orbit!");
            }
        }
        */

        [HarmonyPatch(typeof(StartOfRound), "PassTimeToNextDay")]
        public class NextDayPatch
        {
            static void Postfix()
            {
                Plugin.Spam("Next day detected!");
                OpenLibEvents.GhostCodesReset.Invoke();
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "Start")]
        public class DressGirlExists : DressGirlAI //Start
        {
            static void Postfix(DressGirlAI __instance)
            {
                Plugin.instance.DressGirl = __instance;
                Plugin.MoreLogs("Dressgirl instance detected.");
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "SetHauntStarePosition")]
        public class StaringInHauntPatch : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                    return;

                if (__instance == null)
                {
                    Plugin.GC.LogError("Failed to grab dressgirl instance");
                    return;
                }

                if (!__instance.staringInHaunt)
                    return;

                if (CanSendCodes() && DressGirlStartCodes())
                {
                    Plugin.instance.DressGirl = __instance;
                    Plugin.GC.LogInfo("The ghostGirl wants to play!");
                    Plugin.instance.DressGirl.StartCoroutine(Coroutines.GhostGirlEnhanced(StartOfRound.Instance));
                }
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "FlipLightsBreakerServerRpc")]
        public class DressGirlRPCFix : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (SetupConfig.FixGhostGirlBreakers.Value)
                {
                    Plugin.MoreLogs("Fixing lightbreaker Rpc");
                    __instance.FlipLightsBreakerClientRpc();
                }
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "BeginChasing")]
        public class TheChaseBegins
        {
            static void Postfix(DressGirlAI __instance)
            {

                DressGirl.BreakersFix(__instance);

                if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                    return;

                RapidFire.startRapidFire = true;
                DressGirl.girlIsChasing = true;
                Plugin.MoreLogs("Girl has begun chasing, setting rapidFire to TRUE");
                StartOfRound.Instance.StartCoroutine(Coroutines.RapidFireStart(StartOfRound.Instance));
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "StopChasing")]
        public class TheChaseEnds : DressGirlAI
        {
            static void Postfix()
            {
                if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                    return;

                RapidFire.startRapidFire = false;
                DressGirl.girlIsChasing = false;
                Plugin.MoreLogs("Chase sequence has ended.");
            }
        }

        [HarmonyPatch(typeof(VehicleController), "Awake")]
        public class GetVehicleController //Get Instance
        {
            static void Postfix(VehicleController __instance)
            {
                Plugin.instance.Cruiser = __instance;
                Plugin.MoreLogs("Cruiser instance detected.");
            }
        }
    }
}
