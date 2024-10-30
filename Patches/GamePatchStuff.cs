using BepInEx.Bootstrap;
using HarmonyLib;
using static ghostCodes.Bools;

namespace ghostCodes
{
    public class GamePatchStuff
    {
        [HarmonyPatch(typeof(StartOfRound), "OnShipLandedMiscEvents")]
        public class StartPatch : StartOfRound
        {

            static void Postfix()
            {
                if (!StartOfRound.Instance.shipHasLanded)
                {
                    if (StartOfRound.Instance.currentLevel.name == "CompanyBuildingLevel" || StartOfRound.Instance.currentLevel.riskLevel == "Safe")
                        return;

                    InitPlugin.StartTheRound();
                }
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

        [HarmonyPatch(typeof(DressGirlAI), "Update")]
        public class OnUpdatePatch : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (!gcConfig.ghostGirlEnhanced.Value)
                    return;

                if (__instance == null)
                {
                    Plugin.GC.LogError("Failed to grab dressgirl instance");
                    return;
                }


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
                if (gcConfig.fixGhostGirlBreakers.Value)
                {
                    Plugin.MoreLogs("Fixing lightbreaker Rpc");
                    __instance.FlipLightsBreakerClientRpc();
                }
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "BeginChasing")]
        public class TheChaseBegins
        {
            static bool Prefix(DressGirlAI __instance)
            {
                if (gcConfig.fixGhostGirlBreakers.Value)
                {
                    Plugin.MoreLogs("Fixing breakers");
                    DressGirl.BreakersFix(__instance);
                    return false;
                }
                else
                    return true;
            }

            static void Postfix(DressGirlAI __instance)
            {
                if (!gcConfig.ghostGirlEnhanced.Value)
                    return;

                RapidFire.startRapidFire = true;
                DressGirl.girlIsChasing = true;
                //DressGirl.hauntStare = __instance.staringInHaunt;
                Plugin.MoreLogs("Girl has begun chasing, setting rapidFire to TRUE");
                StartOfRound.Instance.StartCoroutine(Coroutines.RapidFireStart(StartOfRound.Instance));
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "StopChasing")]
        public class TheChaseEnds : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (!gcConfig.ghostGirlEnhanced.Value)
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
