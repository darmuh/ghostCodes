using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using Steamworks.ServerList;

namespace ghostCodes
{
    public class ghostCodesPatch : MonoBehaviour
    {
        public static bool ghostCodeSent = false;

        [HarmonyPatch(typeof(StartOfRound), "OnShipLandedMiscEvents")]
        public class StartPatch : StartOfRound
        {
            public static List<TerminalAccessibleObject> myTerminalObjects = new List<TerminalAccessibleObject>();
            public static Terminal patchTerminal = null;

            static void Postfix(ref Terminal __instance)
            {
                if (!StartOfRound.Instance.shipHasLanded)
                {
                    if(GameNetworkManager.Instance.localPlayerController.IsHost)
                    {
                        TerminalAccessibleObject[] array = Object.FindObjectsOfType<TerminalAccessibleObject>();
                        patchTerminal = Object.FindObjectOfType<Terminal>();
                        myTerminalObjects = array.ToList();
                        Plugin.GC.LogInfo($"Loaded myTerminalObjects({myTerminalObjects.Count})");
                        string listContents = string.Join(", ", myTerminalObjects);
                        Plugin.GC.LogInfo($"{listContents}");
                        if (gcConfig.gcRandomIntervals.Value)
                        {
                            __instance.StartCoroutine(PostfixCoroutine1());
                            Plugin.GC.LogInfo("The ghost is in the machine, at random intervals");
                        }
                        else
                        {
                            __instance.StartCoroutine(PostfixCoroutine2());
                            Plugin.GC.LogInfo("The ghost is in the machine, at set intervals");
                        }
                    }
                    else
                    {
                        Plugin.GC.LogInfo($"ghost codes only enabled for host client.");
                    }
                    
                    
                }
            }

            static IEnumerator PostfixCoroutine1() //Random
            {
                //Plugin.GC.LogInfo("start of random coroutine");

                int firstWait = Random.Range(gcConfig.gcFirstRandIntervalMin.Value, gcConfig.gcFirstRandIntervalMax.Value);
                int codeCount = 0;
                int randGC = 0;
                randGC = Random.Range(1, gcConfig.gcMaxCodes.Value);

                yield return new WaitForSeconds(firstWait);

                if (GameNetworkManager.Instance.gameHasStarted)
                {
                    while (StartOfRound.Instance.allPlayersDead == false && codeCount < randGC)
                    {
                        yield return new WaitForSeconds(1);
                        if (!ghostCodeSent)
                        {
                            //Plugin.GC.LogInfo("loading new ghost code");
                            int randomObjectNum = Random.Range(0, StartPatch.myTerminalObjects.Count);
                            int randomWaitNum = Random.Range(gcConfig.gcSecondRandIntervalMin.Value, gcConfig.gcSecondRandIntervalMax.Value);

                            yield return new WaitForSeconds(randomWaitNum);

                            //patchTerminal.broadcastedCodeThisFrame = true;

                            if (!StartPatch.myTerminalObjects[randomObjectNum].inCooldown)
                            {
                                //patchTerminal.PlayBroadcastCodeEffect();
                                if (gcConfig.gcEnableTerminalSound.Value && patchTerminal != null)
                                    patchTerminal.PlayTerminalAudioServerRpc(3);
                                StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                                //patchTerminal.LoadNewNode(patchTerminal.terminalNodes.specialNodes[19]);
                                ghostCodeSent = true;
                                codeCount++;
                                Plugin.GC.LogInfo($"ghostCode#{codeCount} sent to {StartPatch.myTerminalObjects[randomObjectNum].name}");
                            }
                            else
                            {
                                Plugin.GC.LogInfo($"ghostCode not Sent, {StartPatch.myTerminalObjects[randomObjectNum].name} on cooldown");
                            }

                        }
                        else
                        {
                            int randomWaitNum = Random.Range(gcConfig.gcRandIntervalACMin.Value, gcConfig.gcRandIntervalACMax.Value);
                            //Plugin.GC.LogInfo($"ghostCode already sent, waiting {randomWaitNum}");
                            yield return new WaitForSeconds(randomWaitNum);
                            ghostCodeSent = false;
                        }



                    }
                    Plugin.GC.LogInfo("the ghost is bored of sending codes");
                    yield break;
                }
                else
                {
                    Plugin.GC.LogInfo("Game hasn't started yet...");
                    yield return new WaitForSeconds(5);
                }
            }

            static IEnumerator PostfixCoroutine2() //Set
            {
                //Plugin.GC.LogInfo("start of set coroutine");
                int codeCount = 0;
                int randGC = 0;
                randGC = Random.Range(0, gcConfig.gcMaxCodes.Value);

                yield return new WaitForSeconds(gcConfig.gcFirstSetInterval.Value);

                if (GameNetworkManager.Instance.gameHasStarted)
                {
                    while(StartOfRound.Instance.allPlayersDead == false && codeCount < randGC)
                    {
                        yield return new WaitForSeconds(1);
                        if (!ghostCodeSent)
                        {
                            //Plugin.GC.LogInfo("loading new ghost code");
                            int randomObjectNum = Random.Range(0, StartPatch.myTerminalObjects.Count);


                            yield return new WaitForSeconds(gcConfig.gcSecondSetInterval.Value);

                            //Plugin.GC.LogInfo("end of wait");
                            //patchTerminal.broadcastedCodeThisFrame = true;

                            if (!StartPatch.myTerminalObjects[randomObjectNum].inCooldown)
                            {
                                //Plugin.GC.LogInfo($"Object selected: {StartPatch.myTerminalObjects[randomObjectNum]}");
                                //patchTerminal.PlayBroadcastCodeEffect();
                                if (gcConfig.gcEnableTerminalSound.Value && patchTerminal != null)
                                    patchTerminal.PlayTerminalAudioServerRpc(3);

                                StartPatch.myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
                                //patchTerminal.LoadNewNode(patchTerminal.terminalNodes.specialNodes[19]);
                                ghostCodeSent = true;
                                codeCount++;
                                Plugin.GC.LogInfo($"ghostCode# {codeCount} sent to {StartPatch.myTerminalObjects[randomObjectNum].name}");
                            }
                            else
                            {
                                Plugin.GC.LogInfo($"ghostCode not Sent, {StartPatch.myTerminalObjects[randomObjectNum].name} on cooldown");
                            }
                                
                        }
                        else
                        {
                            Plugin.GC.LogInfo($"ghostCode was just sent, waiting {gcConfig.gcSetIntervalAC.Value}");
                            yield return new WaitForSeconds(gcConfig.gcSetIntervalAC.Value);
                            ghostCodeSent = false;
                        }

                        
                            
                    }
                    Plugin.GC.LogInfo("the ghost is bored of sending codes");
                    yield break;
                }
                else
                {
                    Plugin.GC.LogInfo("Game hasn't started yet...");
                    yield return new WaitForSeconds(10);
                }
            }
        }
    }
}
