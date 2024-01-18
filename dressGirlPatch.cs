using HarmonyLib;
using System.Collections;
using static ghostCodes.ghostCodesPatch;
using Random = UnityEngine.Random;
using UnityEngine;
using GameNetcodeStuff;

namespace ghostCodes
{
    internal class dressGirlPatch
    {
        public static bool performingAction = false;
        public static PlayerControllerB hauntedPlayer;
        static bool girlIsChasing = false;
        [HarmonyPatch(typeof(DressGirlAI), "SetHauntStarePosition")]
        public class DressGirlPatch : DressGirlAI
        {
            static void Postfix(ref DressGirlAI __instance)
            {
                Plugin.hauntStare = __instance.staringInHaunt;
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "FlipLightsBreakerServerRpc")]
        public class DressGirlRPCFix : DressGirlAI
        {
            static void Postfix(ref DressGirlAI __instance)
            {
                if (gcConfig.fixGhostGirlBreakers.Value)
                {
                    Plugin.GC.LogInfo("Fixing lightbreaker rpc");
                    __instance.FlipLightsBreakerClientRpc();
                }
                
            }
        }

        //BeginChasing() - RapidFire Patch
        //Update() - call code during hauntings

        [HarmonyPatch(typeof(DressGirlAI), "ChoosePlayerToHaunt")]
        public class TheChosenOne : DressGirlAI
        {
            static void Postfix(ref DressGirlAI __instance)
            {
                if (__instance.hauntingPlayer != null && !__instance.hauntingPlayer.isPlayerDead)
                {
                    hauntedPlayer = __instance.hauntingPlayer;
                }
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "Update")]
        public class onUpdatePatch : DressGirlAI
        {
            static void Postfix(ref DressGirlAI __instance)
            {
                if (gcConfig.ghostGirlEnhanced.Value && StartPatch.CanSendCodes() && KeepSendingCodes2(__instance))
                {
                    Terminal theTerm = FindObjectOfType<Terminal>();
                    if (theTerm != null)
                    {
                        Plugin.GC.LogInfo("The ghostGirl wants to play!");
                        __instance.StartCoroutine(ghostGirlEnhanced(theTerm, __instance));
                    }
                    else
                        Plugin.GC.LogError("Unable to find terminal instance from DressGirl Update Patch...");
                }
            }

            private static bool KeepSendingCodes(DressGirlAI instance)
            {
                return !StartOfRound.Instance.allPlayersDead && codeCount < randGC && !StartOfRound.Instance.shipIsLeaving && !hauntedPlayer.isPlayerDead;
            }
            private static bool KeepSendingCodes2(DressGirlAI instance)
            {
                return !StartOfRound.Instance.allPlayersDead && codeCount < randGC && !StartOfRound.Instance.shipIsLeaving && instance.staringInHaunt && !performingAction && !hauntedPlayer.isPlayerDead;
            }

            static IEnumerator ghostGirlEnhanced(Terminal instance, DressGirlAI girlstance)
            {
                while (KeepSendingCodes(girlstance))
                {
                    
                    float timeWait1 = Random.Range(0.75f, 1.5f);
                    if (startRapidFire)
                    {
                        instance.StartCoroutine(StartPatch.rapidFire(instance));
                        yield break;
                    }
                    while (girlstance.staringInHaunt)
                    {
                        float timeWait2 = Random.Range(1f, 4f);
                        int randomObjectNum = Random.Range(0, StartPatch.myTerminalObjects.Count);
                        performingAction = true;
                        if (!ghostCodeSent && !girlIsChasing)
                        {

                            StartPatch.HandleGhostCodeSending(instance, randomObjectNum);
                            ghostCodeSent = true;
                        }
                        else
                        {
                            //float randomWaitNum = gcConfig.gcSetIntervalAC.Value;
                            Plugin.GC.LogInfo($"ghostCode was just sent, waiting {timeWait2}");

                            yield return new WaitForSeconds(timeWait2);
                            ghostCodeSent = false;
                        }

                        if (StartOfRound.Instance.shipIsLeaving)
                        {
                            Plugin.GC.LogInfo($"Ship is leaving, {codeCount} codes were sent.");
                            yield break;
                        }
                    }
                    //Plugin.GC.LogInfo("not currently haunting, waiting.");
                    yield return new WaitForSeconds(timeWait1);
                }

                performingAction = false;
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "BeginChasing")]
        public class startChasingPatch
        {
            static bool Prefix(ref DressGirlAI __instance)
            {
                if (__instance.currentBehaviourStateIndex != 1 && gcConfig.fixGhostGirlBreakers.Value)
                {
                    __instance.SwitchToBehaviourStateOnLocalClient(1);
                    __instance.staringInHaunt = false;
                    __instance.disappearingFromStare = false;
                    __instance.disappearByVanishing = false;
                    __instance.choseDisappearingPosition = false;
                    __instance.agent.speed = 5.25f;
                    __instance.creatureAnimator.SetBool("Walk", true);
                    __instance.timesChased++;
                    if (__instance.timesChased != 1 && Random.Range(0, 100) < gcConfig.ggVanillaBreakerChance.Value)
                    {
                        __instance.FlipLightsBreakerServerRpc();
                        Plugin.GC.LogInfo("patched updated breaker chance for vanilla");
                    }
                    else
                    {
                        __instance.MessWithLightsServerRpc();
                    }

                    __instance.chaseTimer = 20f;
                    __instance.timer = 0f;
                    __instance.SetMovingTowardsTargetPlayer(__instance.hauntingPlayer);
                    __instance.moveTowardsDestination = true;
                    return false;
                }
                else
                {
                    Plugin.GC.LogInfo("girl not chasing");
                    return true;
                }     
            }

            static void Postfix(ref DressGirlAI __instance)
            {
                if (gcConfig.ghostGirlEnhanced.Value)
                {
                    ghostCodesPatch.startRapidFire = true;
                    girlIsChasing = true;
                    Plugin.GC.LogInfo("Set girl chasing boolean to true");
                }
                
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "StopChasing")]
        public class stopChasingPatch : DressGirlAI
        {
            static void Postfix(ref DressGirlAI __instance)
            {
                if(gcConfig.ghostGirlEnhanced.Value)
                {
                    ghostCodesPatch.startRapidFire = false;
                    girlIsChasing = false;
                    Plugin.GC.LogInfo("girl stopped chasing");
                }
                
            }
        }
    }
}