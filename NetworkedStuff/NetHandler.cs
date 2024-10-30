using GameNetcodeStuff;
using ghostCodes.Configs;
using ghostCodes.Interactions;
using Unity.Netcode;
using UnityEngine;

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
                Instance.AlarmLightsServerRpc(true);
                return;
            }

            GGFlickerClientRpc();
        }

        [ClientRpc]
        public void GGFlickerClientRpc()
        {
            if (RapidFire.meltdown)
                return;

            RoundManager.Instance.FlickerPoweredLights(flickerFlashlights: true, disableFlashlights: false);
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
            if (Plugin.instance.DressGirl == null)
            {
                Plugin.MoreLogs("DressGirl instance is null");
                return;
            }

            if (OpenLib.Common.Misc.TryGetPlayerFromName(playerName, out PlayerControllerB changeToPlayer))
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
            if (!SetupConfig.RapidLightsColorValue.Value.StartsWith("#"))
                return;

            Plugin.MoreLogs("AlarmLights color ServerRpc");
            AlarmLightsClientRpc(normal);
        }

        [ClientRpc]
        public void AlarmLightsClientRpc(bool normalLights) //Color
        {
            if (normalLights == false)
            {
                Color32 configColor = OpenLib.Common.Misc.HexToColor(SetupConfig.RapidLightsColorValue.Value);
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


        [ServerRpc(RequireOwnership = false)]
        public void GGFacilityLightsServerRpc()
        {
            GGFacilityLightsClientRpc();
        }

        [ClientRpc]
        public void GGFacilityLightsClientRpc()
        {
            BreakerBox breakerBox = FindObjectOfType<BreakerBox>();
            if (breakerBox != null)
            {
                Plugin.GC.LogInfo("setting facility lights for this client");

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

        [ServerRpc(RequireOwnership = false)]
        public void ShockTerminalSoundServerRpc()
        {
            ShockTerminalSoundClientRpc();
        }

        [ClientRpc]
        public void ShockTerminalSoundClientRpc()
        {
            SoundSystem.PlayTerminalSound(TerminalAdditions.Shock);
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
            Instance.PlayGhostAudioonTerminal(num);
        }


        [ServerRpc(RequireOwnership = false)]
        public void TermBroadcastFXServerRpc()
        {
            TermBroadcastFXClientRpc();
        }

        [ClientRpc]
        public void TermBroadcastFXClientRpc()
        {
            Plugin.instance.Terminal.codeBroadcastAnimator.SetTrigger("display");
            Plugin.GC.LogInfo("ghost making it's mark");
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

