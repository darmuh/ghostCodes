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
            if (SetupConfig.RapidLightsColorValue.Value.ToLower() == "nochange" || SetupConfig.RapidLightsColorValue.Value.ToLower() == "default" || SetupConfig.RapidLightsColorValue.Value.Length == 0)
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

        //RoundManager.Instance.FlickerLights(flickerFlashlights: true, disableFlashlights: false);
        [ServerRpc(RequireOwnership = false)]
        public void FlickerLightsServerRpc(bool flickerFlash, bool disableFlash)
        {

            Plugin.MoreLogs($"SERVER: garbling all walkies");
            FlickerLightsClientRpc(flickerFlash, disableFlash);
        }

        [ClientRpc]
        public void FlickerLightsClientRpc(bool flickerFlash, bool disableFlash)
        {
            RoundManager.Instance.FlickerLights(flickerFlashlights: flickerFlash, disableFlashlights: disableFlash);
        }

        [ServerRpc(RequireOwnership = false)]
        public void FacilityBreakerServerRpc()
        {
            FacilityBreakerClientRpc();
        }

        [ClientRpc]
        public void FacilityBreakerClientRpc()
        {
            BreakerBox breakerBox = FindObjectOfType<BreakerBox>();
            if (breakerBox != null)
            {
                Plugin.GC.LogInfo("flipping facility breaker for this client");

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
        public void BatteryAdjustSoundServerRpc(int clientID)
        {
            BatteryAdjustSoundClientRpc(clientID);
        }

        [ClientRpc]
        public void BatteryAdjustSoundClientRpc(int clientID)
        {
            PlayerControllerB player = Misc.GetAllLivingPlayers().Find(x => (int)x.actualClientId == clientID);
            player.itemAudio.PlayOneShot(Items.Adjuster, 0.8f);
        }

        [ServerRpc(RequireOwnership = false)]
        public void BatteryAdjustServerRpc(int clientID)
        {
            BatteryAdjustClientRpc(clientID);
        }

        [ClientRpc]
        public void BatteryAdjustClientRpc(int clientID)
        {
            PlayerControllerB player = Misc.GetAllLivingPlayers().Find(x => (int)x.actualClientId == clientID);
            Items.PlayerAffectBatteries(player);
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
        public void HeavyLeverServerRpc(float newValue)
        {
            HeavyLeverClientRpc(newValue);
        }

        [ClientRpc]
        public void HeavyLeverClientRpc(float newValue)
        {
            if (ShipStuff.leverChanged)
                return;

            StartMatchLever startMatchLever = FindObjectOfType<StartMatchLever>();
            startMatchLever.triggerScript.timeToHoldSpeedMultiplier = newValue;
            startMatchLever.triggerScript.holdTip = "[ This lever has a ghostly presence... ]";
            ShipStuff.leverChanged = true;
            Plugin.Spam("HeavyLeverClient Success!");
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
            if (SoundSystem.allSounds.Count < num)
                SoundSystem.InitSounds();

            if (SoundSystem.allSounds.Count >= num)
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

