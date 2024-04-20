using Unity.Netcode;
using UnityEngine;
using System.Linq;
using GameNetcodeStuff;
using static ghostCodes.TerminalPatchStuff;
using ghostCodes.Interactions;

namespace ghostCodes
{
    public class NetHandler : NetworkBehaviour
    {

        public static NetHandler Instance { get; private set; }

        //[ServerRpc(RequireOwnership = false)]
        //[ClientRpc]

        [ServerRpc(RequireOwnership = false)]
        public void GGFlickerServerRpc()
        {
            if (RapidFire.meltdown)
            {
                NetHandler.Instance.AlarmLightsServerRpc(true);
                return;
            }
                
            NetworkManager networkManager = base.NetworkManager;

            if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.MoreLogs($"Server: Syncing actions between players...");

            }

            else if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsHost || networkManager.IsServer))
            {
                Plugin.MoreLogs($"Exec stage not server");
            }
            else
            {
                Plugin.MoreLogs("no conditions met");
            }

            GGFlickerClientRpc();
        }

        [ClientRpc]
        public void GGFlickerClientRpc()
        {
            if (RapidFire.meltdown)
                return;

            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.MoreLogs($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient && !networkManager.IsHost))
                {
                    RoundManager.Instance.FlickerLights(flickerFlashlights: true, disableFlashlights: false);
                    Plugin.MoreLogs("flickering lights for non-HOST");
                }
                else if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient && networkManager.IsHost))
                {
                    RoundManager.Instance.FlickerLights(flickerFlashlights: true, disableFlashlights: false);
                    Plugin.MoreLogs("flickering lights for HOST");
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void BreatheOnWalkiesServerRpc()
        {
            if (Plugin.instance.DressGirl == null)
                return;

            Plugin.MoreLogs($"SERVER: ghost breathing on all walkies");
            BreatheOnWalkiesClientRpc();
        }

        [ClientRpc]
        public void BreatheOnWalkiesClientRpc()
        {
            WalkieStuff.BreatheOnWalkiesFunc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void GarbleWalkiesServerRpc()
        {

            Plugin.MoreLogs($"SERVER: garbling all walkies");
            GarbleWalkiesClientRpc();
        }

        [ClientRpc]
        public void GarbleWalkiesClientRpc()
        {
            WalkieStuff.GarbleAllWalkiesFunc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void EmptyShipServerRpc()
        {
            if (Plugin.instance.DressGirl == null)
                return;

            Plugin.MoreLogs($"SERVER: emptying ship");
            EmptyShipClientRpc();
        }

        [ClientRpc]
        public void EmptyShipClientRpc()
        {
            StartOfRound.Instance.StartCoroutine(Coroutines.EmptyShip());
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeDressGirlToPlayerServerRpc(string playerName)
        {
            if (Plugin.instance.DressGirl == null)
                return;

            Plugin.MoreLogs($"SERVER: Changing dressgirl to {playerName}");
            ChangeDressGirlToPlayerClientRpc(playerName);
        }

        [ClientRpc]
        public void ChangeDressGirlToPlayerClientRpc(string playerName)
        {
            if(Plugin.instance.DressGirl == null)
            {
                Plugin.MoreLogs("DressGirl instance is null");
                return;
            }

            PlayerControllerB changeToPlayer = HauntedTerminal.GetPlayerFromString(playerName);
            if (changeToPlayer != null)
            {
                Plugin.instance.DressGirl.switchedHauntingPlayer = true;
                Plugin.instance.DressGirl.StartCoroutine(Plugin.instance.DressGirl.setSwitchingHauntingPlayer());
                DressGirl.ChangeHauntedPlayer(changeToPlayer);
                Plugin.MoreLogs($"Changing haunted player to {playerName}");
            }

        }

        [ServerRpc(RequireOwnership = false)]
        public void AlarmLightsServerRpc(bool normal)
        {
            if (!ModConfig.rfRLcolorChange.Value)
                return;

            Plugin.MoreLogs("AlarmLights color ServerRpc");
            AlarmLightsClientRpc(normal);
        }

        [ClientRpc]
        public void AlarmLightsClientRpc(bool normalLights) //Color
        {
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
                {
                    Plugin.GC.LogError($"Network Manager failed to initialize.");
                }

                if (__rpc_exec_stage == __RpcExecStage.Client && networkManager.IsClient)
                {
                    if (normalLights == false)
                    {
                        Color32 configColor = Misc.ParseColorFromString(ModConfig.rfRLcolorValue.Value);
                        for (int i = 0; i < RoundManager.Instance.allPoweredLights.Count; i++)
                        {
                            RoundManager.Instance.allPoweredLights[i].color = configColor;
                        }
                        Plugin.MoreLogs("Alarm lights set");
                    }
                    else if (normalLights == true)
                    {
                        for (int i = 0; i < RoundManager.Instance.allPoweredLights.Count; i++)
                        {
                            RoundManager.Instance.allPoweredLights[i].color = Color.white;
                        }
                        Plugin.MoreLogs("Lights set back to normal");
                    }
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void GGFacilityLightsServerRpc()
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

            GGFacilityLightsClientRpc();
        }

        [ClientRpc]
        public void GGFacilityLightsClientRpc()
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
        public void ShockTerminalSoundServerRpc()
        {
            ShockTerminalSoundClientRpc();
        }

        [ClientRpc]
        public void ShockTerminalSoundClientRpc()
        {
            SoundSystem.PlayTerminalSound(StartOfRound.Instance.damageSFX);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RebootTerminalSpookyServerRpc()
        {
            RebootTerminalSpookyClientRpc();
        }

        [ClientRpc]
        public void RebootTerminalSpookyClientRpc()
        {
            StartOfRound.Instance.StartCoroutine(TerminalAdditions.RebootTerminalSpooky());
            Plugin.MoreLogs("Client Received Terminal Reboot");
        }

        [ServerRpc(RequireOwnership = false)]
        public void MessWithMonitorsServerRpc()
        {
            MessWithMonitorsClientRpc();
        }

        [ClientRpc]
        public void MessWithMonitorsClientRpc()
        {
            ShipStuff.MessWithMonitors();
        }

        [ServerRpc(RequireOwnership = false)]
        public void GGTermAudioServerRpc(int num)
        {
            GGTermAudioClientRpc(num);
        }

        [ClientRpc]
        public void GGTermAudioClientRpc(int num)
        {
            NetHandler.Instance.PlayGhostAudioonTerminal(num);
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

        public void PlayGhostAudioonTerminal(int num)
        {
            Plugin.instance.Terminal.terminalAudio.PlayOneShot(SoundSystem.allSounds[num]);
        }


        //DO NOT REMOVE
        public override void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                if (Instance != null && Instance.gameObject != null)
                {
                    NetworkObject networkObject = Instance.gameObject.GetComponent<NetworkObject>();

                    if (networkObject != null)
                    {
                        networkObject.Despawn();
                        Plugin.GC.LogInfo("Nethandler despawned!");
                    }
                }
            }

            Instance = this;
            base.OnNetworkSpawn();
            Plugin.GC.LogInfo("Nethandler Spawned!");
        }


    }
}

