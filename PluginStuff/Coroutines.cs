using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using static ghostCodes.NumberStuff;
using static ghostCodes.CodeStuff;
using static ghostCodes.CodeHandling;
using static ghostCodes.Bools;
using static ghostCodes.RapidFire;
using static ghostCodes.InsanityStuff;
using static ghostCodes.DressGirl;
using static ghostCodes.Compatibility.FacilityMeltdown;
using GameNetcodeStuff;

namespace ghostCodes
{
    internal class Coroutines
    {
        internal static bool rapidFireStart = false;
        internal static bool codeLooperStarted = false; //enusre only one instance of coroutine being run

        internal static IEnumerator InitEnumerator(StartOfRound instance)
        {
            int firstWait = GetFirstWait();
            InitPlugin.CodesInit();
            Plugin.MoreLogs($"Codes Amount: {Plugin.instance.randGC}");

            if (ModConfig.ghostGirlEnhanced.Value && ModConfig.ModNetworking.Value && !Plugin.instance.bypassGGE)
            {
                Plugin.MoreLogs($"The ghost girl has been enhanced, prepare for your next haunting! >:)");
                TerminalAdditions.CreateAllNodes();
            }
            else if (GameNetworkManager.Instance.localPlayerController.IsHost)
            {
                yield return new WaitForSeconds(firstWait);
                StartOfRound.Instance.StartCoroutine(CodeLooper(instance));
                Plugin.MoreLogs("The ghost is in the machine...");
                TerminalAdditions.CreateAllNodes();
            }
            else
            {
                Plugin.MoreLogs("Ghost Girl Enhanced is disabled and you are not the host.");
                TerminalAdditions.CreateAllNodes();
            }
        }

        internal static IEnumerator RestartEnum(StartOfRound instance)
        {
            int restartWait = GetInt(20, 90);
            yield return new WaitForSeconds(restartWait);
            int firstWait = GetFirstWait();
            Plugin.MoreLogs($"Codes: {Plugin.instance.codeCount} / {Plugin.instance.randGC}");
            
            if (ModConfig.ghostGirlEnhanced.Value && ModConfig.ModNetworking.Value && !Plugin.instance.bypassGGE)
            {
                Plugin.MoreLogs($"The ghost girl has been enhanced, prepare for your next haunting! >:)");
            }
            else if (GameNetworkManager.Instance.localPlayerController.IsHost)
            {
                yield return new WaitForSeconds(firstWait);
                endAllCodes = false;
                rapidFireStart = false;
                startRapidFire = false;
                instance.StartCoroutine(CodeLooper(instance));
                Plugin.MoreLogs("The ghost is in the machine...");
            }
            else
            {
                Plugin.MoreLogs("Ghost Girl Enhanced is disabled and you are not the host.");
            }

            
        }

        internal static IEnumerator CodeLooper(StartOfRound instance)
        {
            if (codeLooperStarted)
                yield break;

            Plugin.MoreLogs("CodeLooper coroutine initiated...");
            codeLooperStarted = true;

            if (CanSendCodes())
            {
                while (KeepSendingCodes())
                {
                    yield return new WaitForSeconds(1);

                    if (!IsCodeSent())
                    {
                        float randomWaitNum = GetSecondWait();

                        if (ModConfig.gcInsanity.Value)
                        {
                            ApplyInsanityMode(instance, ref randomWaitNum);
                            if (startRapidFire)
                            {
                                Plugin.MoreLogs("startRapidFire detected from codeLooper");
                                codeLooperStarted = false;
                                yield break;
                            }
                                
                        }

                        yield return new WaitForSeconds(randomWaitNum);
                        HandleGhostCodeSending(instance);
                        Plugin.instance.codeCount++;
                        Plugin.instance.ghostCodeSent = true;
                        Plugin.MoreLogs($"Code Sent, count: {Plugin.instance.codeCount}");
                    }
                    else
                    {
                        float randomWaitNum = GetWaitAfterCode();
                        Plugin.MoreLogs($"ghostCode was just sent, waiting {randomWaitNum}");

                        if (ModConfig.gcInsanity.Value)
                        {
                            ApplyInsanityMode(instance, ref randomWaitNum);
                        }

                        yield return new WaitForSeconds(randomWaitNum);
                        Plugin.instance.ghostCodeSent = false;
                    }

                    if (StartOfRound.Instance.shipIsLeaving)
                    {
                        Plugin.MoreLogs($"Ship is leaving, {Plugin.instance.codeCount} codes were sent.");
                        codeLooperStarted = false;
                        yield break;
                    }
                }

                Plugin.MoreLogs($"the ghost is bored of sending codes, {Plugin.instance.codeCount} codes were sent.");
                codeLooperStarted = false;
                yield break;
            }
            else
            {
                Plugin.MoreLogs("No codes can be sent at this time.");
                codeLooperStarted = false;
                yield break;
            }
        }

        private static void RapidFireStoppers(int startTime, int currentTime)
        {
            GGrapidFireChecks();
            TimeCheck(startTime, currentTime);
        }

        internal static IEnumerator RapidFireStart(StartOfRound instance)
        {
            if (rapidFireStart)
                yield break;

            Plugin.MoreLogs("start of rapidFire");
            int startTime = TimeOfDay.Instance.hour;
            rapidFireStart = true;

            if (CanSendCodes())
            {
                while (KeepSendingCodes() && startRapidFire && !endAllCodes)
                {
                    CheckInsanityToContinue(); // will exit while loop if startRapidFire is set to false
                    HandleLights();

                    if (!IsCodeSent())
                    {

                        if (myTerminalObjects.Count == 0)
                        {
                            for (int i = 0; i < possibleActions.Count && KeepSendingCodes(); i++)
                            {
                                RapidFireStoppers(startTime,TimeOfDay.Instance.hour);
                                HandleRapidFireCodeChoices(instance, -1);
                                yield return new WaitForSeconds(Random.Range(0.5f, 3f)); //rapidFire code cooldowns
                            }
                        }  
                        else
                        {
                            for (int i = 0; i < myTerminalObjects.Count && KeepSendingCodes(); i++)
                            {
                                RapidFireStoppers(startTime, TimeOfDay.Instance.hour);
                                HandleRapidFireCodeChoices(instance, i);
                                yield return new WaitForSeconds(Random.Range(0.5f, 3f)); //rapidFire code cooldowns
                            }
                        }

                        CountSentCodes();
                        Plugin.instance.ghostCodeSent = true;
                    }
                    else
                    {
                        RapidFireStoppers(startTime, TimeOfDay.Instance.hour);
                        float cooldown = Random.Range(1f, 4f);
                        RapidFireStoppers(startTime, TimeOfDay.Instance.hour);
                        yield return new WaitForSeconds(cooldown);
                        Plugin.instance.ghostCodeSent = false;
                    }

                    if (LoopBreakers())
                        yield break;
                        
                }
                if (!(KeepSendingCodes()))
                    Plugin.GC.LogInfo($"end of rapidFire....");
                
                if (!startRapidFire && KeepSendingCodes() && !endAllCodes)
                {
                    ReturnFromRapidFire();
                    startRapidFire = false;
                    rapidFireStart = false;
                    yield break;
                }

                if (endAllCodes)
                {
                    startRapidFire = false;
                    rapidFireStart = false;
                    yield break;
                }     

            }
            startRapidFire = false;
            rapidFireStart = false;
            yield break;
        }

        internal static IEnumerator EmptyShip()
        {
            Plugin.MoreLogs("Emptying the ship of any players!");
            PlayerControllerB localPlayer = StartOfRound.Instance.localPlayerController;
            Vector3 playerPosition = localPlayer.transform.position;
            Vector3 shipDoorPosition = StartOfRound.Instance.shipDoorNode.position;
            float oldfriction = localPlayer.slideFriction;
            localPlayer.isPlayerSliding = true;
            localPlayer.slideFriction = localPlayer.maxSlideFriction * 2;

            while (localPlayer.isInHangarShipRoom)
            {

                if (Physics.Linecast(playerPosition, shipDoorPosition, StartOfRound.Instance.collidersAndRoomMask))
                {
                    Vector3 forceDirection = Vector3.Normalize(shipDoorPosition - playerPosition);
                    localPlayer.externalForces = forceDirection * 350f;
                    Plugin.MoreLogs("applying external forces");
                }

                yield return new WaitForSeconds(0.1f);
            }

            localPlayer.isPlayerSliding = false;
            localPlayer.slideFriction = oldfriction;
        }

        internal static IEnumerator AlarmLights()
        {
            BreakerBox breakerBox = Object.FindObjectOfType<BreakerBox>();
            if (breakerBox == null)
                yield break;

            if (Plugin.instance.facilityMeltdown)
                meltdown = CheckForMeltdown();

            NetHandler.Instance.AlarmLightsServerRpc(false);

            while (startRapidFire && !StartOfRound.Instance.localPlayerController.isPlayerDead && !meltdown)
            {
                if (StartOfRound.Instance.localPlayerController.isInsideFactory)
                    NetHandler.Instance.GGFlickerServerRpc();
                yield return new WaitForSeconds(Random.Range(ModConfig.rfRLmin.Value, ModConfig.rfRLmax.Value));
            }
            NetHandler.Instance.AlarmLightsServerRpc(true);
            yield return new WaitForSeconds(1);
            if (!breakerBox.isPowerOn)
            {
                NetHandler.Instance.GGFacilityLightsServerRpc();
                Plugin.GC.LogInfo("returning to normal");
            }
            lightsFlickering = false;

        }

        internal static IEnumerator GhostGirlEnhanced(StartOfRound instance)
        {
            Plugin.MoreLogs("GhostGirl is being enhanced");

            if (startRapidFire)
            {
                yield break;
            }

            while (Plugin.instance.DressGirl.staringInHaunt)
            {
                float timeWait2 = Random.Range(1f, 4f);
                if (!Plugin.instance.ghostCodeSent && !girlIsChasing)
                {
                    HandleGhostCodeSending(instance);
                    Plugin.instance.ghostCodeSent = true;
                }
                else
                {
                    Plugin.GC.LogInfo($"ghostCode was just sent, waiting {timeWait2}");
                    yield return new WaitForSeconds(timeWait2);
                    Plugin.instance.ghostCodeSent = false;
                }

                if (LoopBreakers())
                    yield break;
            }

            Plugin.MoreLogs("End of GhostGirlEnhanced Coroutine...");
        }
    }
}
