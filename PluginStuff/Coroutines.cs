using GameNetcodeStuff;
using ghostCodes.Configs;
using System.Collections;
using UnityEngine;
using static ghostCodes.Bools;
using static ghostCodes.CodeHandling;
using static ghostCodes.CodeStuff;
using static ghostCodes.DressGirl;
using static ghostCodes.InsanityStuff;
using static ghostCodes.NumberStuff;
using static ghostCodes.RapidFire;
using Random = UnityEngine.Random;

namespace ghostCodes
{
    internal class Coroutines
    {
        internal static bool rapidFireStart = false;
        internal static bool codeLooperStarted = false; //enusre only one instance of coroutine being run
        internal static bool activeFlicker = false;

        internal static IEnumerator InitEnumerator(StartOfRound instance)
        {
            int firstWait = GetWait(SetupConfig.RandomIntervals.Value, SetupConfig.FirstRandIntervalMin.Value, SetupConfig.FirstRandIntervalMax.Value, SetupConfig.FirstSetInterval.Value);
            InitPlugin.CodesInit();
            Plugin.MoreLogs($"Codes Amount: {Plugin.instance.RandCodeAmount}");

            if (SetupConfig.GhostCodesSettings.HauntingsMode && ModConfig.ModNetworking.Value)
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
            int restartWait = GetInt(20, 100);
            yield return new WaitForSeconds(restartWait);
            int firstWait = GetWait(SetupConfig.RandomIntervals.Value, SetupConfig.FirstRandIntervalMin.Value, SetupConfig.FirstRandIntervalMax.Value, SetupConfig.FirstSetInterval.Value);
            Plugin.MoreLogs($"Codes: {Plugin.instance.CodeCount} / {Plugin.instance.RandCodeAmount}");

            if (SetupConfig.GhostCodesSettings.HauntingsMode && ModConfig.ModNetworking.Value)
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
                        float randomWaitNum = GetWait(SetupConfig.RandomIntervals.Value, SetupConfig.SecondRandIntervalMin.Value, SetupConfig.SecondRandIntervalMax.Value, SetupConfig.SecondSetInterval.Value);

                        if (SetupConfig.GhostCodesSettings.InsanityMode)
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
                        Plugin.instance.CodeCount++;
                        Plugin.instance.CodeSent = true;
                        Plugin.MoreLogs($"Code Sent, count: {Plugin.instance.CodeCount}");
                    }
                    else
                    {
                        float randomWaitNum = GetWait(SetupConfig.RandomIntervals.Value, SetupConfig.RandIntervalACMin.Value, SetupConfig.RandIntervalACMax.Value, SetupConfig.SetIntervalAC.Value);
                        Plugin.MoreLogs($"ghostCode was just sent, waiting {randomWaitNum}");

                        if (SetupConfig.GhostCodesSettings.InsanityMode)
                        {
                            ApplyInsanityMode(instance, ref randomWaitNum);
                        }

                        yield return new WaitForSeconds(randomWaitNum);
                        Plugin.instance.CodeSent = false;
                    }

                    if (StartOfRound.Instance.shipIsLeaving)
                    {
                        Plugin.MoreLogs($"Ship is leaving, {Plugin.instance.CodeCount} codes were sent.");
                        codeLooperStarted = false;
                        yield break;
                    }
                }

                Plugin.MoreLogs($"the ghost is bored of sending codes, {Plugin.instance.CodeCount} codes were sent.");
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
                                RapidFireStoppers(startTime, TimeOfDay.Instance.hour);
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
                        Plugin.instance.CodeSent = true;
                    }
                    else
                    {
                        RapidFireStoppers(startTime, TimeOfDay.Instance.hour);
                        float cooldown = Random.Range(1f, 4f);
                        RapidFireStoppers(startTime, TimeOfDay.Instance.hour);
                        yield return new WaitForSeconds(cooldown);
                        Plugin.instance.CodeSent = false;
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
            if (lightsFlickering)
                yield break;

            lightsFlickering = true;
            BreakerBox breakerBox = Object.FindObjectOfType<BreakerBox>();
            if (breakerBox == null)
                yield break;

            Plugin.Spam("Starting light flicker loop");
            NetHandler.Instance.AlarmLightsServerRpc(false);

            while (startRapidFire && !StartOfRound.Instance.localPlayerController.isPlayerDead && !endAllCodes)
            {
                if (StartOfRound.Instance.localPlayerController.isInsideFactory)
                    NetHandler.Instance.FlickerLightsServerRpc(false, false);
                yield return new WaitForSeconds(Random.Range(SetupConfig.RapidLightsMin.Value, SetupConfig.RapidLightsMax.Value));
            }

            NetHandler.Instance.AlarmLightsServerRpc(true);
            yield return new WaitForSeconds(1);
            if (!breakerBox.isPowerOn)
            {
                breakerBox.SwitchBreaker(true);
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
                if (!Plugin.instance.CodeSent && !girlIsChasing)
                {
                    HandleGhostCodeSending(instance);
                    Plugin.instance.CodeSent = true;
                }
                else
                {
                    Plugin.GC.LogInfo($"ghostCode was just sent, waiting {timeWait2}");
                    yield return new WaitForSeconds(timeWait2);
                    Plugin.instance.CodeSent = false;
                }

                if (LoopBreakers())
                    yield break;
            }

            Plugin.MoreLogs("End of GhostGirlEnhanced Coroutine...");
        }
    }
}
