using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ghostCodes
{
    public class NetHandler : NetworkBehaviour
    {

        public static NetHandler Instance { get; private set; }

        //[ServerRpc(RequireOwnership = false)]
        //[ClientRpc]

        [ServerRpc(RequireOwnership = false)]
        public void ggFlickerServerRpc()
        {
            NetworkManager networkManager = base.NetworkManager;

            if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Server: Syncing actions between players...");

            }

            else if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Exec stage not server");
            }
            else
            {
                Plugin.GC.LogInfo("no conditions met");
            }

            ggFlickerClientRpc();
        }

        [ClientRpc]
        public void ggFlickerClientRpc()
        {
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.GC.LogInfo($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient && !networkManager.IsHost))
                {
                    RoundManager.Instance.FlickerLights(flickerFlashlights: true, disableFlashlights: true);
                    Plugin.GC.LogInfo("flickering lights for non-HOST");
                }
                else if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient && networkManager.IsHost))
                {
                    RoundManager.Instance.FlickerLights(flickerFlashlights: true, disableFlashlights: true);
                    Plugin.GC.LogInfo("flickering lights for HOST");
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void AlarmLightsServerRpc(bool normal)
        {
            NetworkManager networkManager = base.NetworkManager;

            if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Server: Syncing actions between players...");

            }

            else if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Exec stage not server");
            }
            else
            {
                Plugin.GC.LogInfo("no conditions met");
            }

            AlarmLightsClientRpc(normal);
        }

        [ClientRpc]
        public void AlarmLightsClientRpc(bool normalLights)
        {
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.GC.LogInfo($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && networkManager.IsClient)
                {
                    if (normalLights == false)
                    {
                        for (int i = 0; i < RoundManager.Instance.allPoweredLights.Count; i++)
                        {
                            //RoundManager.Instance.allPoweredLights[i].color = new Color32(255, 143, 18, 1);
                            
                        }
                        Plugin.GC.LogInfo("Alarm lights set");
                    }
                    else if (normalLights == true)
                    {
                        for (int i = 0; i < RoundManager.Instance.allPoweredLights.Count; i++)
                        {
                            //RoundManager.Instance.allPoweredLights[i].color = new Color32(255, 255, 255, 1);
                            
                        }
                        Plugin.GC.LogInfo("Lights set back to normal");
                    }
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void ggFacilityLightsServerRpc()
        {
            NetworkManager networkManager = base.NetworkManager;

            if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Server: Syncing actions between players...");

            }

            else if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Exec stage not server");
            }
            else
            {
                Plugin.GC.LogInfo("no conditions met");
            }

            ggFacilityLightsClientRpc();
        }

        [ClientRpc]
        public void ggFacilityLightsClientRpc()
        {
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.GC.LogInfo($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient && !networkManager.IsHost))
                {
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    BreakerBox breakerBox = Object.FindObjectOfType<BreakerBox>();
                    if (breakerBox != null)
                    {
                        Plugin.GC.LogInfo("setting facility lights for non-HOST");
                        
                        if (breakerBox.isPowerOn)
                        {
                            breakerBox.SetSwitchesOff();
                            RoundManager.Instance.TurnOnAllLights(on: false);
                            GameNetworkManager.Instance.localPlayerController.JumpToFearLevel(0.2f);
                        }
                        else
                        {
                            breakerBox.SwitchBreaker(on: true);
                            RoundManager.Instance.TurnOnAllLights(on: true);
                        }
                    }
                    
                }
                else if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient && networkManager.IsHost))
                {
                    BreakerBox breakerBox = Object.FindObjectOfType<BreakerBox>();
                    if (breakerBox != null)
                    {
                        Plugin.GC.LogInfo("setting facility lights for HOST");

                        if (breakerBox.isPowerOn)
                        {
                            breakerBox.SetSwitchesOff();
                            RoundManager.Instance.TurnOnAllLights(on: false);
                            GameNetworkManager.Instance.localPlayerController.JumpToFearLevel(0.2f);
                        }
                        else
                        {
                            breakerBox.SwitchBreaker(on: true);
                            RoundManager.Instance.TurnOnAllLights(on: true);
                        }
                        
                    }
                    
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void ggTermAudioServerRpc()
        {
            NetworkManager networkManager = base.NetworkManager;

            if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Server: Syncing actions between players...");

            }

            else if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Exec stage not server");
            }
            else
            {
                Plugin.GC.LogInfo("no conditions met");
            }

            ggTermAudioClientRpc();
        }

        [ClientRpc]
        public void ggTermAudioClientRpc()
        {
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.GC.LogInfo($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient || networkManager.IsHost))
                {
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    NetHandler.Instance.playGhostAudioonTerminal();
                    //Plugin.GC.LogInfo("ghost making noise on the terminal");
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void TermBroadcastFXServerRpc()
        {
            NetworkManager networkManager = base.NetworkManager;

            if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Server: Syncing actions between players...");

            }

            else if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.GC.LogInfo($"Exec stage not server");
            }
            else
            {
                Plugin.GC.LogInfo("no conditions met");
            }

            TermBroadcastFXClientRpc();
        }

        [ClientRpc]
        public void TermBroadcastFXClientRpc()
        {
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.GC.LogInfo($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient || networkManager.IsHost))
                {
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    //string stringTest = "TEST - isHost/isServer (exec stage not client)";
                    NetHandler.Instance.BroadcastEffect();
                    Plugin.GC.LogInfo("ghost making it's mark");
                }
            }
        }

        public void BroadcastEffect()
        {
            Terminal getTerm = FindObjectOfType<Terminal>();
            if (getTerm != null)
            {
                getTerm.codeBroadcastAnimator.SetTrigger("display");
                //Plugin.GC.LogInfo("effect broadcasted, no sound");
            }
                
        }

        public void playGhostAudioonTerminal()
        {
            //Plugin.GC.LogInfo($"start of playghost method");
            Terminal getTerm = FindObjectOfType<Terminal>();
            DressGirlAI dressGirl = FindObjectOfType<DressGirlAI>();
            if (getTerm != null)
            {
                //Plugin.GC.LogInfo($"getting audioclips");
                AudioClip[] audioClips = { getTerm.syncedAudios[3], getTerm.codeBroadcastSFX };
                if (dressGirl != null)
                {
                    AudioClip[] ghostAudios = { dressGirl.breathingSFX, dressGirl.heartbeatMusic.clip };
                    //Plugin.GC.LogInfo("ghost detected, adding dressgirl audios");

                    audioClips = audioClips.Concat(dressGirl.appearStaringSFX).ToArray();
                    audioClips = audioClips.Concat(ghostAudios).ToArray();
                }
                
                //if(StartOfRound.Instance.currentLevel.levelAmbienceClips.insideAmbience.Length > 0)
                  //  audioClips = audioClips.Concat(StartOfRound.Instance.currentLevel.levelAmbienceClips.insideAmbience).ToArray();

                //audioClips = audioClips.Concat(SoundManager.Instance.heartbeatClips).ToArray();

                //Plugin.GC.LogInfo($"concated all available audioclips");
                //Plugin.GC.LogInfo($"{audioClips}");
                // Choose a random audio clip from the combined array
                getTerm.terminalAudio.clip = audioClips[Random.Range(0, audioClips.Length)];
                Plugin.GC.LogInfo($"Playing on Terminal: {getTerm.terminalAudio.clip}");
                getTerm.terminalAudio.PlayOneShot(getTerm.terminalAudio.clip);
            }
            else
                Plugin.GC.LogInfo($"null objects detected");
        }


        //DO NOT REMOVE
        public override void OnNetworkSpawn()
        {

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
                Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
            Instance = this;

            base.OnNetworkSpawn();
        }


    }
}

