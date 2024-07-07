using BepInEx.Bootstrap;
using HarmonyLib;
using static ghostCodes.Bools;
using Random = UnityEngine.Random;

namespace ghostCodes
{
    public class GamePatchStuff
    {
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
                    Plugin.MoreLogs("Codes should not be generated...");
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

        [HarmonyPatch(typeof(ShipTeleporter), "Awake")]
        public class TeleporterInit : ShipTeleporter
        {
            static void Postfix(ShipTeleporter __instance)
            {
                if (__instance.isInverseTeleporter)
                {
                    Plugin.instance.InverseTP = __instance;
                    Plugin.MoreLogs("InverseTP instance detected and set.");
                }
                    
                else
                {
                    Plugin.instance.NormalTP = __instance;
                    Plugin.MoreLogs("NormalTP instance detected and set.");
                }
                    
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "SetHauntStarePosition")]
        public class StaringInHauntPatch : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (!ModConfig.ghostGirlEnhanced.Value)
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
                if (ModConfig.fixGhostGirlBreakers.Value)
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

                if (!ModConfig.ghostGirlEnhanced.Value)
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
            static void Postfix(DressGirlAI __instance)
            {
                if (!ModConfig.ghostGirlEnhanced.Value)
                    return;

                RapidFire.startRapidFire = false;
                DressGirl.girlIsChasing = false;
                Plugin.MoreLogs("Chase sequence has ended.");
            }
        }

        [HarmonyPatch(typeof(GameNetworkManager), "Start")]
        public class GameStartPatch
        {
            public static void Postfix()
            {
                CompatibilityCheck();
            }

            private static void CompatibilityCheck()
            {
                if (Chainloader.PluginInfos.ContainsKey("me.loaforc.facilitymeltdown"))
                {
                    Plugin.MoreLogs("Facility Meltdown mod detected!");
                    Plugin.instance.facilityMeltdown = true;
                }
                if (Chainloader.PluginInfos.ContainsKey("com.github.zehsteam.ToilHead"))
                {
                    Plugin.MoreLogs("ToilHeads mod detected!");
                    Plugin.instance.toilHead = true;
                }
            }
        }
    }
}
