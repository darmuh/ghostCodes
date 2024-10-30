using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace ghostCodes
{
    public class ghostCodesPatch : MonoBehaviour
    {
        public static bool ghostCodeSent = false;
        public static bool startRapidFire = false;
        public static bool bypassGGE = false;
        public static float groupSanity = 0f;
        public static float maxSanity = 0f;
        public static int playersAtStart = 0;
        public static int codeCount = 0;
        public static int randGC = 0;
        public static float sPercentL1 = 0.25f;
        public static float sPercentL2 = 0.5f;
        public static float sPercentL3 = 0.75f;
        public static float sPercentMAX = 0.9f;
        public static float wPercentL1 = 0.9f;
        public static float wPercentL2 = 0.5f;
        public static float wPercentL3 = 0.1f;

        [HarmonyPatch(typeof(StartOfRound), "OnShipLandedMiscEvents")]
        public class StartPatch : StartOfRound
        {
            public static List<TerminalAccessibleObject> myTerminalObjects = new List<TerminalAccessibleObject>();
            public static Terminal patchTerminal = null;

            static void Postfix(ref Terminal __instance)
            {
                if (!StartOfRound.Instance.shipHasLanded)
                {
                    if (StartOfRound.Instance.currentLevel.name == "CompanyBuildingLevel" || StartOfRound.Instance.currentLevel.riskLevel == "Safe")
                        return;

                    List<string> noGhostPlanets = gcConfig.GGEbypassList.Value.Split(',').ToList();

                    if (gcConfig.GGEbypass.Value && (noGhostPlanets.Any(planet => StartOfRound.Instance.currentLevel.PlanetName.Contains(planet))))
                        bypassGGE = true;

                    maxSanity = 0f;
                    codeCount = 0;
                    ghostCodeSent = false;
                    startRapidFire = false;
                    dressGirlPatch.performingAction = false;

                    if (!gcConfig.ModNetworking.Value)
                    {
                        gcConfig.ghostGirlEnhanced.Value = false;
                        Plugin.GC.LogInfo("Forcing Ghost Girl Enhanced Mode to FALSE");
                    }
                        

                    if (gcConfig.gcInsanityMode.Value || bypassGGE)
                        getUsableSanityPercents();

                    TerminalAccessibleObject[] array = Object.FindObjectsOfType<TerminalAccessibleObject>();
                    if (array == null || array.Length <= 0)
                        return;
                    myTerminalObjects = array.ToList();
                    Plugin.GC.LogInfo($"Initial Loaded objects count: {myTerminalObjects.Count}");
                    // string listContents = string.Join(", ", myTerminalObjects);
                    //Plugin.GC.LogInfo($"{listContents}");
                    List<TerminalAccessibleObject> filteredObjects = new List<TerminalAccessibleObject>();

                    foreach (var obj in myTerminalObjects)
                    {
                        if (!(obj.gameObject.name.Contains("Landmine") && gcConfig.gcIgnoreLandmines.Value) &&
                            !(obj.gameObject.name.Contains("TurretScript") && gcConfig.gcIgnoreTurrets.Value) &&
                            !(obj.gameObject.name.Contains("BigDoor") && gcConfig.gcIgnoreDoors.Value))
                        {
                            filteredObjects.Add(obj);
                        }
                    }

                    myTerminalObjects = filteredObjects;

                    Plugin.GC.LogInfo($"Loaded myTerminalObjects({myTerminalObjects.Count})");
                    patchTerminal = Object.FindObjectOfType<Terminal>();
                    string listContents2 = string.Join(", ", myTerminalObjects);
                    Plugin.GC.LogInfo($"{listContents2}");
                    getPlayersAtStart();

                    if (gcConfig.gcRandomIntervals.Value)
                    {
                        __instance.StartCoroutine(RandomGO(__instance));
                        //Plugin.GC.LogInfo("The ghost is in the machine, at random intervals");
                    }
                    else
                    {
                        __instance.StartCoroutine(SetGO(__instance));
                        //Plugin.GC.LogInfo("The ghost is in the machine, at set intervals");
                    }
                }
            }

            static void getPlayersAtStart()
            {
                playersAtStart = 0;
                for(int i = 0; i < StartOfRound.Instance.allPlayerScripts.Count(); i++)
                {
                    if (StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled)
                        playersAtStart++;
                }
                Plugin.GC.LogInfo($"Players at Start: {playersAtStart}");
                if (playersAtStart > 1)
                    Plugin.GC.LogInfo("SoloAssist Buff disabled, more than one player detected.");
            }

            static IEnumerator RandomGO(Terminal instance)
            {
                int firstWait = Random.Range(gcConfig.gcFirstRandIntervalMin.Value, gcConfig.gcFirstRandIntervalMax.Value);
                randGC = Random.Range(gcConfig.gcMinCodes.Value, gcConfig.gcMaxCodes.Value);
                if (gcConfig.ghostGirlEnhanced.Value && gcConfig.ModNetworking.Value && !bypassGGE)
                {
                    Plugin.GC.LogInfo($"The ghost girl has been enhanced >:)");
                }
                else if(GameNetworkManager.Instance.localPlayerController.IsHost)
                {
                    yield return new WaitForSeconds(firstWait);
                    instance.StartCoroutine(PostfixCoroutine1(instance));
                    Plugin.GC.LogInfo("The ghost is in the machine, at random intervals");
                }
                else
                {
                    Plugin.GC.LogInfo("Ghost Girl Enhanced is disabled and you are not the host.");
                }
            }

            static IEnumerator SetGO(Terminal instance)
            {
                randGC = Random.Range(gcConfig.gcMinCodes.Value, gcConfig.gcMaxCodes.Value);

                if (gcConfig.ghostGirlEnhanced.Value && gcConfig.ModNetworking.Value && !bypassGGE)
                {
                    Plugin.GC.LogInfo($"The ghost girl has been enhanced >:)");
                }
                else if (GameNetworkManager.Instance.localPlayerController.IsHost)
                {
                    yield return new WaitForSeconds(gcConfig.gcFirstSetInterval.Value);
                    instance.StartCoroutine(PostfixCoroutine2(instance));
                    Plugin.GC.LogInfo("The ghost is in the machine, at set intervals");
                }
                else
                {
                    Plugin.GC.LogInfo("Ghost Girl Enhanced is disabled and you are not the host.");
                }
            }

            private static void getUsableSanityPercents()
            {
                if (gcConfig.sanityPercentL1.Value > 0 && gcConfig.sanityPercentL1.Value < 100)
                    sPercentL1 = gcConfig.sanityPercentL1.Value / 100f;
                if (gcConfig.sanityPercentL2.Value > 0 && gcConfig.sanityPercentL2.Value < 100)
                    sPercentL2 = gcConfig.sanityPercentL2.Value / 100f;
                if (gcConfig.sanityPercentL3.Value > 0 && gcConfig.sanityPercentL3.Value < 100)
                    sPercentL3 = gcConfig.sanityPercentL3.Value / 100f;
                if (gcConfig.sanityPercentMAX.Value > 0 && gcConfig.sanityPercentMAX.Value < 100)
                    sPercentMAX = gcConfig.sanityPercentMAX.Value / 100f;
                if (gcConfig.waitPercentL1.Value > 0 && gcConfig.waitPercentL1.Value < 100)
                    wPercentL1 = gcConfig.waitPercentL1.Value / 100f;
                if (gcConfig.waitPercentL2.Value > 0 && gcConfig.waitPercentL2.Value < 100)
                    wPercentL2 = gcConfig.waitPercentL2.Value / 100f;
                if (gcConfig.waitPercentL3.Value > 0 && gcConfig.waitPercentL3.Value < 100)
                    wPercentL3 = gcConfig.waitPercentL3.Value / 100f;


                Plugin.GC.LogInfo("Insanity mode percents set >:)");

            }


            public static void getAllSanity()
            {
                groupSanity = 0f;
                maxSanity = 0f;

                // Iterate through all players
                for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Count(); i++)
                {
                    if (!StartOfRound.Instance.allPlayerScripts[i].isPlayerDead && StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled)
                    {
                        groupSanity += StartOfRound.Instance.allPlayerScripts[i].insanityLevel;
                        maxSanity += StartOfRound.Instance.allPlayerScripts[i].maxInsanityLevel;
                    }
                }

                ApplyBonuses(ref groupSanity, ref maxSanity);

                if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * sPercentMAX)
                {
                    startRapidFire = true;
                    Plugin.GC.LogInfo("max sanity hit, getAllSanity()");
                    Plugin.GC.LogInfo($"Group Sanity Level: {Mathf.Round(groupSanity)}");
                    Plugin.GC.LogInfo($"Group Max Insanity level: {Mathf.Round(maxSanity)}");
                }
                else
                {
                    startRapidFire = false;
                    Plugin.GC.LogInfo($"Group Sanity Level: {Mathf.Round(groupSanity)}");
                    Plugin.GC.LogInfo($"Group Max Insanity level: {Mathf.Round(maxSanity)}");
                }
                    
            }

            private static void ApplyBonuses(ref float groupSanity, ref float maxSanity)
            {
                // List of bonus functions
                List<Func<float, float, Tuple<float, float>>> bonuses = new List<Func<float, float, Tuple<float, float>>>();

                // Add bonuses as needed
                if (gcConfig.deathBonus.Value)
                    bonuses.Add((g, m) => DeathBonus(g, m));
                if (gcConfig.ggBonus.Value)
                    bonuses.Add((g, m) => girlBonus(g, m));
                if (gcConfig.soloAssist.Value && playersAtStart == 1)
                    bonuses.Add((g, m) => soloAssist(g, m));

                // Apply all bonuses
                foreach (var bonus in bonuses)
                {
                    var result = bonus.Invoke(groupSanity, maxSanity);
                    groupSanity = result.Item1;
                    maxSanity = result.Item2;
                }
            }

            private static Tuple<float, float> soloAssist(float groupSanity, float maxSanity)
            {
                if (gcConfig.soloAssist.Value && playersAtStart == 1)
                {
                    int hour = TimeOfDay.Instance.hour;
                    Plugin.GC.LogInfo($"Hour: {hour}");
                    if (hour <= 6) //apply this debuff till noon (stage 1)
                    {
                        if (groupSanity >= maxSanity * (gcConfig.saS1percent.Value/100)) //percentage of max sanity check (saS1percent)
                        {
                            if(StartOfRound.Instance.localPlayerController.isInsideFactory)
                            {
                                if(groupSanity > gcConfig.saS1inside.Value)
                                {
                                    groupSanity -= gcConfig.saS1inside.Value; //stage 1 (in factory) sanity removal number (saS1inside)
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS1inside.Value} from Group Sanity Level.");
                                }
                                else
                                {
                                    groupSanity = 0; //stage 1 (in factory) sanity removal number (saS1inside)
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS1inside.Value} from Group Sanity Level which set value to 0.");
                                }
                                
                            }
                            else
                            {
                                if(groupSanity > gcConfig.saS1outside.Value)
                                {
                                    groupSanity -= gcConfig.saS1outside.Value; //stage 1 (out of factory) sanity removal number
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS1outside.Value} from Group Sanity Level.");
                                }
                                else
                                {
                                    groupSanity = 0;
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS1outside.Value} from Group Sanity Level which set value to 0.");
                                }
                            } 
                        }
                        else
                        {
                            Plugin.GC.LogInfo($"No solo assist buff added.");
                        }
                    }
                    else if (hour > 6 && hour <= 10 ) //apply this debuff between noon and 5PM (stage 2)
                    {
                        if (groupSanity >= maxSanity * (gcConfig.saS2percent.Value / 100)) //percentage of max sanity check
                        {
                            if (StartOfRound.Instance.localPlayerController.isInsideFactory)
                            {
                                if (groupSanity > gcConfig.saS2inside.Value)
                                {
                                    groupSanity -= gcConfig.saS2inside.Value; //stage 2 (in factory) sanity removal number
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS2inside.Value} from Group Sanity Level.");
                                }
                                else
                                {
                                    groupSanity = 0;
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS2inside.Value} from Group Sanity Level which set value to 0.");
                                }

                            }
                            else
                            {
                                if (groupSanity > gcConfig.saS2outside.Value)
                                {
                                    groupSanity -= gcConfig.saS2outside.Value; //stage 2 (out of factory) sanity removal number
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS2outside.Value} from Group Sanity Level.");
                                }
                                else
                                {
                                    groupSanity = 0;
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS2outside.Value} from Group Sanity Level which set value to 0.");
                                }

                            }
                                
                        }
                        else
                        {
                            Plugin.GC.LogInfo($"No solo assist buff added.");
                        }
                    }
                    else if (hour > 10 && hour <= 15) //last debuff between 5PM and 10PM (stage 3)
                    {
                        if (groupSanity >= maxSanity * (gcConfig.saS3percent.Value / 100)) //percentage of max sanity
                        {
                            if (StartOfRound.Instance.localPlayerController.isInsideFactory)
                            {
                                if(groupSanity > gcConfig.saS3inside.Value)
                                {
                                    groupSanity -= gcConfig.saS3inside.Value; //stage 3 (in factory) sanity removal number
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS3inside.Value} from Group Sanity Level.");
                                }
                                else
                                {
                                    groupSanity = 0;
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS3inside.Value} from Group Sanity Level which set value to 0.");
                                }

                            }
                            else
                            {
                                if(groupSanity > gcConfig.saS3outside.Value)
                                {
                                    groupSanity -= gcConfig.saS3outside.Value; //stage 3 (out of factory) sanity removal number
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS3outside.Value} from Group Sanity Level.");
                                }
                                else
                                {
                                    groupSanity = 0;
                                    Plugin.GC.LogInfo($"Subtracted {gcConfig.saS3outside.Value} from Group Sanity Level which set value to 0.");
                                }

                            }
                                
                        }
                        else
                        {
                            Plugin.GC.LogInfo($"No solo assist buff added.");
                        }
                    }
                    else
                        Plugin.GC.LogInfo($"No solo assist buff added.");
                }
                return Tuple.Create(groupSanity, maxSanity);
            }

            private static Tuple<float, float> DeathBonus(float groupSanity, float maxSanity)
            {
                if (gcConfig.deathBonus.Value)
                {
                    int deadPlayers = StartOfRound.Instance.allPlayerScripts.Count(player => player.isPlayerDead);

                    for (int i = 0; i < deadPlayers; i++)
                    {
                        if (maxSanity >= (groupSanity + gcConfig.deathBonusNum.Value))
                        {
                            groupSanity += gcConfig.deathBonusNum.Value;
                            Plugin.GC.LogInfo($"{gcConfig.deathBonusNum.Value} added to Group Sanity Level.");
                        }
                        else
                        {
                            groupSanity = maxSanity;
                            Plugin.GC.LogInfo($"Death bonus exceeds maximum, setting to max sanity level.");
                        }
                    }
                }
                return Tuple.Create(groupSanity, maxSanity);
            }

            private static Tuple<float, float> girlBonus(float groupSanity, float maxSanity)
            {
                if (gcConfig.ggBonus.Value)
                {
                    checkForGhostGirl();
                    if(ghostGirlExists)
                    {
                        if (maxSanity >= (groupSanity + gcConfig.ggBonusNum.Value))
                        {
                            groupSanity += gcConfig.ggBonusNum.Value;
                            Plugin.GC.LogInfo($"{gcConfig.ggBonusNum.Value} added to Group Sanity Level.");
                        }
                        else
                        {
                            groupSanity = maxSanity;
                            Plugin.GC.LogInfo($"Ghost Girl Insanity bonus exceeds maximum, setting to max sanity level.");
                        }
                    }
                }
                return Tuple.Create(groupSanity, maxSanity);
            }

            private static bool ghostGirlExists = false;

            private static void checkForGhostGirl()
            {
                DressGirlAI ghostGirl = FindObjectOfType<DressGirlAI>();
                if (ghostGirl != null)
                {
                    ghostGirlExists = true;
                }
            }

            private static IEnumerator alarmLights()
            {
                BreakerBox breakerBox = Object.FindObjectOfType<BreakerBox>();
                NetHandler.Instance.AlarmLightsServerRpc(false);

                while (startRapidFire && !StartOfRound.Instance.localPlayerController.isPlayerDead)
                {
                    if (StartOfRound.Instance.localPlayerController.isInsideFactory)
                        NetHandler.Instance.ggFlickerServerRpc();
                    yield return new WaitForSeconds(Random.Range(gcConfig.rfRLmin.Value, gcConfig.rfRLmax.Value));
                }
                NetHandler.Instance.AlarmLightsServerRpc(true);
                yield return new WaitForSeconds(1);
                if(!breakerBox.isPowerOn)
                {
                    NetHandler.Instance.ggFacilityLightsServerRpc();
                    Plugin.GC.LogInfo("returning to normal");
                }
                lightsFlickering = false;
                
            }

            public static bool lightsFlickering = false;

            public static IEnumerator rapidFire(Terminal instance)
            {
                //Plugin.GC.LogInfo("start of rapidFire");
                
                if (CanSendCodes())
                {
                    //Plugin.GC.LogInfo("can send codes");
                    while (ShouldContinueSendingCodes() && startRapidFire)
                    {
                        if(gcConfig.ModNetworking.Value && gcConfig.rfRapidLights.Value && !lightsFlickering)
                        {
                            instance.StartCoroutine(alarmLights());
                            Plugin.GC.LogInfo("networking enabled, sending alarm lights");
                            lightsFlickering = true;
                        }
                            
                        if (gcConfig.gcInsanityMode.Value && !gcConfig.ghostGirlEnhanced.Value)
                            getAllSanity(); //last check to make sure sanity levels are within needs
                        if (!ghostCodeSent)
                        {
                            for(int i = 0; i < myTerminalObjects.Count; i++)
                            {
                                StartPatch.HandleGhostCodeSending(instance, i);
                                yield return new WaitForSeconds(Random.Range(0.5f, 3f)); //rapidFire code cooldowns
                            }

                            codeCount++;
                            ghostCodeSent = true;
                        }
                        else
                        {
                            float cooldown = Random.Range(1f, 4f);
                            yield return new WaitForSeconds(cooldown);
                            ghostCodeSent = false;
                        }

                        if (StartOfRound.Instance.shipIsLeaving)
                        {
                            Plugin.GC.LogInfo($"Ship is leaving, {codeCount} codes were sent.");
                            yield break;
                        }

                        if( gcConfig.ghostGirlEnhanced.Value && StartOfRound.Instance.localPlayerController.isPlayerDead)
                        {
                            Plugin.GC.LogInfo("you died, stopping any logic you were calling");
                            yield break;
                        }
                    }
                    if(!(ShouldContinueSendingCodes()))
                        Plugin.GC.LogInfo($"the ghost is bored of sending codes, {codeCount} codes were sent.");
                    if (!startRapidFire && ShouldContinueSendingCodes())
                    {

                        Plugin.GC.LogInfo("ending rapidFire and returning to main routine");
                        if (gcConfig.gcRandomIntervals.Value && !gcConfig.ghostGirlEnhanced.Value)
                        {
                            instance.StartCoroutine(RandomGO(instance));
                            Plugin.GC.LogInfo("The ghost is in the machine, at random intervals");
                        }
                        else if (!gcConfig.gcRandomIntervals.Value && !gcConfig.ghostGirlEnhanced.Value)
                        {
                            instance.StartCoroutine(SetGO(instance));
                            Plugin.GC.LogInfo("The ghost is in the machine, at set intervals");
                        }
                        else
                        {
                            Plugin.GC.LogInfo("Codes should resume during next haunting");
                            dressGirlPatch.performingAction = false;
                        }
                            
                    }
                        
                }
                startRapidFire = false;
                yield break;
            }

            static IEnumerator PostfixCoroutine1(Terminal instance)
            {
                //float turretBerserkChance = gcConfig.turretNormalBChance.Value / 100f;

                if (CanSendCodes())
                {
                    while (ShouldContinueSendingCodes())
                    {
                        yield return new WaitForSeconds(1);

                        if (!ghostCodeSent)
                        {
                            int randomObjectNum = Random.Range(0, StartPatch.myTerminalObjects.Count);
                            float randomWaitNum = GetRandomWaitTime(ghostCodeSent);

                            if (gcConfig.gcInsanityMode.Value)
                            {
                                ApplyInsanityMode(instance, ref randomWaitNum);
                            }

                            yield return new WaitForSeconds(randomWaitNum);

                            HandleGhostCodeSending(instance, randomObjectNum);
                            
                            SoundManager.Instance.PlayAmbientSound(syncedForAllPlayers: true, SoundManager.Instance.playingInsanitySoundClipOnServer);
                            Terminal getTerm = FindObjectOfType<Terminal>();

                            if (!gcConfig.ModNetworking.Value)
                            {
                                if (getTerm != null)
                                {
                                    getTerm.PlayBroadcastCodeEffect();
                                    Plugin.GC.LogInfo("effect broadcast");

                                    if (gcConfig.gcEnableTerminalSound.Value)
                                    {
                                        getTerm.PlayTerminalAudioServerRpc(3);
                                        Plugin.GC.LogInfo("alarm sound played");
                                    }
                                    
                                }
                               
                            }   
                            else if (gcConfig.gcEnableTerminalSound.Value && gcConfig.ModNetworking.Value)
                            {
                                NetHandler.Instance.ggTermAudioServerRpc();
                                //Plugin.GC.LogInfo("networked sounds playing");
                                NetHandler.Instance.TermBroadcastFXServerRpc();
                                //Plugin.GC.LogInfo("playing effect on terminal");
                            }
                            else
                            {
                                NetHandler.Instance.TermBroadcastFXServerRpc();
                                Plugin.GC.LogInfo("playing effect on terminal, sounds are disabled");
                            }
                                
                            codeCount++;
                            ghostCodeSent = true;
                        }
                        else
                        {
                            float randomWaitNum = GetRandomWaitTime(ghostCodeSent);
                            Plugin.GC.LogInfo($"ghostCode was just sent, waiting {gcConfig.gcSetIntervalAC.Value}");

                            if (gcConfig.gcInsanityMode.Value)
                            {
                                ApplyInsanityMode(instance, ref randomWaitNum);
                                if (startRapidFire)
                                    yield break;
                            }

                            yield return new WaitForSeconds(randomWaitNum);
                            ghostCodeSent = false;
                        }

                        if (StartOfRound.Instance.shipIsLeaving)
                        {
                            Plugin.GC.LogInfo($"Ship is leaving, {codeCount} codes were sent.");
                            yield break;
                        }
                    }

                    Plugin.GC.LogInfo($"the ghost is bored of sending codes, {codeCount} codes were sent.");
                    yield break;
                }
                else
                {
                    Plugin.GC.LogInfo("No codes can be sent at this time.");
                    yield return new WaitForSeconds(5);
                }
            }

            static IEnumerator PostfixCoroutine2(Terminal instance)
            {
                //float turretBerserkChance = gcConfig.turretNormalBChance.Value / 100f;


                if (CanSendCodes())
                {
                    while (ShouldContinueSendingCodes())
                    {
                        yield return new WaitForSeconds(1);

                        if (!ghostCodeSent)
                        {
                            int randomObjectNum = Random.Range(0, StartPatch.myTerminalObjects.Count);
                            float randomWaitNum = gcConfig.gcSecondSetInterval.Value;

                            if (gcConfig.gcInsanityMode.Value)
                            {
                                ApplyInsanityMode(instance, ref randomWaitNum);
                                if (startRapidFire)
                                    yield break;
                            }

                            yield return new WaitForSeconds(randomWaitNum);

                            SoundManager.Instance.PlayAmbientSound(syncedForAllPlayers: true, SoundManager.Instance.playingInsanitySoundClipOnServer);
                            Terminal getTerm = FindObjectOfType<Terminal>();

                            if (!gcConfig.ModNetworking.Value)
                            {
                                if (getTerm != null)
                                {
                                    getTerm.PlayBroadcastCodeEffect();
                                    Plugin.GC.LogInfo("effect broadcast");

                                    if (gcConfig.gcEnableTerminalSound.Value)
                                    {
                                        getTerm.PlayTerminalAudioServerRpc(3);
                                        Plugin.GC.LogInfo("alarm sound played");
                                    }

                                }

                            }
                            else if (gcConfig.gcEnableTerminalSound.Value && gcConfig.ModNetworking.Value)
                            {
                                NetHandler.Instance.ggTermAudioServerRpc();
                                //Plugin.GC.LogInfo("networked sounds playing");
                                NetHandler.Instance.TermBroadcastFXServerRpc();
                                //Plugin.GC.LogInfo("playing effect on terminal");
                            }
                            else
                            {
                                NetHandler.Instance.TermBroadcastFXServerRpc();
                                Plugin.GC.LogInfo("playing effect on terminal, sounds are disabled");
                            }

                            HandleGhostCodeSending(instance, randomObjectNum);
                            codeCount++;
                            ghostCodeSent = true;
                        }
                        else
                        {
                            float randomWaitNum = gcConfig.gcSetIntervalAC.Value;
                            Plugin.GC.LogInfo($"ghostCode was just sent, waiting {gcConfig.gcSetIntervalAC.Value}");

                            if (gcConfig.gcInsanityMode.Value)
                            {
                                ApplyInsanityMode(instance, ref randomWaitNum);
                            }

                            yield return new WaitForSeconds(randomWaitNum);
                            ghostCodeSent = false;
                        }

                        if (StartOfRound.Instance.shipIsLeaving)
                        {
                            Plugin.GC.LogInfo($"Ship is leaving, {codeCount} codes were sent.");
                            yield break;
                        }
                    }

                    Plugin.GC.LogInfo($"the ghost is bored of sending codes, {codeCount} codes were sent.");
                    yield break;
                }
                else
                {
                    Plugin.GC.LogInfo("No codes can be sent at this time.");
                    yield return new WaitForSeconds(10);
                }
            }

            // Shared Methods

            public static bool CanSendCodes()
            {
                if (!gcConfig.ghostGirlEnhanced.Value || bypassGGE)
                    return GameNetworkManager.Instance.gameHasStarted && myTerminalObjects.Count > 0;
                else
                    return GameNetworkManager.Instance.gameHasStarted && myTerminalObjects.Count > 0 && dressGirlPatch.hauntedPlayer == StartOfRound.Instance.localPlayerController;
            }

            public static bool ShouldContinueSendingCodes()
            {
                if(!gcConfig.ghostGirlEnhanced.Value || bypassGGE)
                    return !StartOfRound.Instance.allPlayersDead && codeCount < randGC && !StartOfRound.Instance.shipIsLeaving;
                else
                    return !StartOfRound.Instance.allPlayersDead && codeCount < randGC && !StartOfRound.Instance.shipIsLeaving && !dressGirlPatch.hauntedPlayer.isPlayerDead;
            }

            public static float GetRandomWaitTime(bool ghostCodeSent)
            {
                return ghostCodeSent ? Random.Range(gcConfig.gcFirstRandIntervalMin.Value, gcConfig.gcFirstRandIntervalMax.Value) :
                                      Random.Range(gcConfig.gcSecondRandIntervalMin.Value, gcConfig.gcSecondRandIntervalMax.Value);
            }

            public static void ApplyInsanityMode(Terminal instance, ref float randomWaitNum)
            {

                getAllSanity();

                //Plugin.GC.LogInfo($"(ApplyInsanityMode)Group Sanity Level: {Mathf.Round(groupSanity)}");
                //Plugin.GC.LogInfo($"(ApplyInsanityMode)Group Max Insanity level: {Mathf.Round(maxSanity)}");

                if (startRapidFire && gcConfig.insanityRapidFire.Value)
                {
                    Plugin.GC.LogInfo("max insanity level reached!!! startRapidFire TRUE");
                    instance.StartCoroutine(rapidFire(instance));
                    return;
                }
                else if (!gcConfig.insanityRapidFire.Value && Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * sPercentMAX)
                {
                    randomWaitNum *= gcConfig.waitPercentMAX.Value / 100f;
                    Plugin.GC.LogInfo("Max Insanity Level reached (rapidFire disabled)");
                }
                else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * sPercentL3)
                {
                    randomWaitNum *= wPercentL3;
                    //Plugin.GC.LogInfo($"(ApplyInsanityMode)Group Max Insanity level: {Mathf.Round(groupSanity)} >= {Mathf.Round(maxSanity) * sPercentL3} ");
                    Plugin.GC.LogInfo("insanity level 3 reached");
                }
                else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * sPercentL2)
                {
                    randomWaitNum *= wPercentL2;
                    Plugin.GC.LogInfo("insanity level 2 reached");
                }
                else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * sPercentL1)
                {
                    randomWaitNum *= wPercentL1;
                    Plugin.GC.LogInfo("insanity level 1 reached");
                }
                else
                {
                    Plugin.GC.LogInfo("insanity levels low");
                }
                Plugin.GC.LogInfo($"waiting {randomWaitNum}");
            }



            public static void HandleGhostCodeSending(Terminal instance, int randomObjectNum)
            {
                string clockTime = HUDManager.Instance.clockNumber.text;
                string logTime = clockTime.Replace("\n", "").Replace("\r", "");
                Plugin.GC.LogInfo($"TIME: {logTime}");
                // Define a list of possible actions with their percentages
                List<ActionPercentage> possibleActions = new List<ActionPercentage>
                {
                    //new ActionPercentage("facilityLights", () => FlipLights(), gcConfig.ggCodeBreakerChance.Value)

                };

                if(gcConfig.ModNetworking.Value) //networking required for this function
                {
                    possibleActions.Add(new ActionPercentage("facilityLights", () => FlipLights(), gcConfig.ggCodeBreakerChance.Value));
                }

                //ActionPercentage(string name, Action action, float percentage)
                //new ActionPercentage("hungryDoor", () => HandleHungryDoor(randomObjectNum, instance), gcConfig.hungryDoorNChance.Value)

                if (ghostGirlExists && gcConfig.ModNetworking.Value)
                {
                    possibleActions.Add(new ActionPercentage("ggFlashlight", () => ggFlashlight(), gcConfig.ggPlayerLightsPercent.Value));
                    //ghostgirlONLY interactions
                }

                if (!gcConfig.gcIgnoreDoors.Value)
                    possibleActions.Add(new ActionPercentage("hungryDoor", () => HandleHungryDoor(randomObjectNum, instance), gcConfig.hungryDoorNChance.Value));
                if (!gcConfig.gcIgnoreTurrets.Value)
                    possibleActions.Add(new ActionPercentage("normalTurret", () => HandleTurretAction(randomObjectNum), gcConfig.turretNormalBChance.Value));
                if (!gcConfig.gcIgnoreLandmines.Value)
                    possibleActions.Add(new ActionPercentage("normalMine", () => HandleMineAction(randomObjectNum), gcConfig.mineNormalBChance.Value));

                
                if (startRapidFire)
                {
                    possibleActions.Add(new ActionPercentage("rfTurret", () => HandleTurretAction(randomObjectNum), gcConfig.turretInsaneBChance.Value));
                    possibleActions.Add(new ActionPercentage("rfMine", () => HandleMineAction(randomObjectNum), gcConfig.mineInsaneBChance.Value));
                    possibleActions.Add(new ActionPercentage("rfHungryDoor", () => HandleHungryDoor(randomObjectNum, instance), gcConfig.hungryDoorIChance.Value));
                    //rapidfireONLY interactions
                }
                else
                {
                    possibleActions.RemoveAll(actionPercentage => actionPercentage.Name == "rfTurret");
                    possibleActions.RemoveAll(actionPercentage => actionPercentage.Name == "rfMine");
                    possibleActions.RemoveAll(actionPercentage => actionPercentage.Name == "rfHungryDoor");
                    possibleActions.RemoveAll(actionPercentage => actionPercentage.Name == "facilityLights");
                }



                List<Action> chosenActions = ActionPercentage.ChooseActionsFromPercentages(possibleActions);
                if (chosenActions.Count > 0)
                {
                    foreach (var action in chosenActions)
                    {
                        action.Invoke();
                    }
                }
                else
                {
                    StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                    Plugin.GC.LogInfo("No Special action chosen, calling function from terminal");
                }
            }

            private static string OddSignalMessage(out string message)
            {
                List<string> messages = gcConfig.signalMessages.Value.Split(',').ToList();
                int rand = Random.Range(0, messages.Count);
                message = messages[rand];
                return message;
            }

            private static void MessWithSignalTranslator()
            {
                string message;
                OddSignalMessage(out message);
                HUDManager.Instance.UseSignalTranslatorServerRpc(message);
            }

            private static void ggFlashlight()
            {
                NetHandler.Instance.ggFlickerServerRpc();
            }

            public static void HandleTurretAction(int randomObjectNum)
            {
                if (StartPatch.myTerminalObjects[randomObjectNum].gameObject.name.Contains("TurretScript") && !StartPatch.myTerminalObjects[randomObjectNum].inCooldown)
                {
                    Turret turretobj = StartPatch.myTerminalObjects[randomObjectNum].gameObject.GetComponent<Turret>();
                    turretobj.SwitchTurretMode(3);
                    turretobj.EnterBerserkModeClientRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    turretobj.EnterBerserkModeServerRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    if(gcConfig.ggCanSendMessages.Value)
                        MessWithSignalTranslator();
                    Plugin.GC.LogInfo("Turrets do this?!?");
                }
                else
                {
                    StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                }
            }

            public static void HandleMineAction(int randomObjectNum)
            {
                if (StartPatch.myTerminalObjects[randomObjectNum].gameObject.name.Contains("Landmine"))
                {
                    Landmine landmine = StartPatch.myTerminalObjects[randomObjectNum].gameObject.GetComponent<Landmine>();
                    landmine.ExplodeMineClientRpc();
                    landmine.ExplodeMineServerRpc();
                    if (gcConfig.ggCanSendMessages.Value)
                        MessWithSignalTranslator();
                    StartPatch.myTerminalObjects.Remove(myTerminalObjects[randomObjectNum]);
                    Plugin.GC.LogInfo("WHAT THE FUUUUUU-");
                }
                else
                {
                    StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                }
            }

            public static void HandleHungryDoor(int randomObjectNum, Terminal instance)
            {
                if (StartPatch.myTerminalObjects[randomObjectNum].gameObject.name.Contains("BigDoor"))
                {
                    if (gcConfig.ggCanSendMessages.Value)
                        MessWithSignalTranslator();
                    Plugin.GC.LogInfo("The door is hungy");
                    instance.StartCoroutine(hungryDoor(instance, randomObjectNum));
                }
                else
                {
                    StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                }
            }

            public static void FlipLights()
            {
                if (gcConfig.ggCanSendMessages.Value)
                    MessWithSignalTranslator();
                Plugin.GC.LogInfo("who turned out the lights??");
                NetHandler.Instance.ggFacilityLightsServerRpc();
            }

            static IEnumerator hungryDoor(Terminal instance, int randomObjectNum)
            {
                float intervalTime = Random.Range(0.2f, 0.7f);
                int numBites = Random.Range(3, 9);
                if (StartPatch.myTerminalObjects[randomObjectNum].gameObject.name.Contains("BigDoor"))
                {
                    for (int i=0; i<numBites; i++)
                    {
                        StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                        yield return new WaitForSeconds(intervalTime);
                        i++;
                    }
                    yield break;
                }
                
            }
        }

    }

        // Helper class to represent an action with its percentage
        public class ActionPercentage
        {
            public Action Action { get; }
            public float Percentage { get; }
            public string Name { get; set; }

            public ActionPercentage(string name, Action action, float percentage)
            {
                Name = name;
                Action = action;
                Percentage = percentage;
            }


        // Helper method to choose actions from a list based on percentages
        public static List<Action> ChooseActionsFromPercentages(List<ActionPercentage> actions)
        {
            List<Action> chosenActions = new List<Action>();

            foreach (var action in actions)
            {
                float randomValue = Random.Range(0f, 100f);

                if (randomValue <= action.Percentage || Mathf.Approximately(action.Percentage, 100f))
                {
                    chosenActions.Add(action.Action);
                }
            }

            return chosenActions;
        }
    }

        

}
