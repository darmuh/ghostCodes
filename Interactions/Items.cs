using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ghostCodes.Misc;

namespace ghostCodes.Interactions
{
    internal class Items
    {
        internal static void DrainAllPlayersBattery()
        {
            List<PlayerControllerB> allPlayers = GetAllLivingPlayers();

            foreach(PlayerControllerB player in allPlayers)
            {
                if(player.isInHangarShipRoom)
                {
                    Plugin.MoreLogs("Player is on the ship, not running battery drain");
                }
                else
                    PlayerDrainAllBatteries(player);
                
            }
        }

        internal static void DrainHauntedPlayersBatterys()
        {
            if (Plugin.instance.DressGirl == null || !Plugin.instance.DressGirl.hauntingLocalPlayer)
                return;

            if (Plugin.instance.DressGirl.hauntingPlayer.isInHangarShipRoom)
                return;

            PlayerDrainAllBatteries(Plugin.instance.DressGirl.hauntingPlayer);
            Plugin.MoreLogs("haunted player battery drain called");
        }

        internal static void DrainRandomPlayersBatterys()
        {
            List<PlayerControllerB> allPlayers = GetAllLivingPlayers();
            int randomPlayer = NumberStuff.GetInt(0, allPlayers.Count - 1);
            if (allPlayers[randomPlayer].isInHangarShipRoom)
                return;

            PlayerDrainAllBatteries(allPlayers[randomPlayer]);
            Plugin.MoreLogs($"Draining battery of {allPlayers[randomPlayer].playerUsername}");
        }

        internal static void PlayerDrainAllBatteries(PlayerControllerB player)
        {
            if (player == null)
                return;

            foreach (GrabbableObject item in player.ItemSlots)
            {
                ItemDrainBattery(item, player);
            }
        }

        private static void ItemDrainBattery(GrabbableObject item, PlayerControllerB player)
        {
            if (item == null || RoundManager.Instance == null)
                return;

            if (item.itemProperties.requiresBattery)
            {
                Plugin.MoreLogs("found item has a battery");
                if (item.insertedBattery.charge > 0)
                {
                    Plugin.MoreLogs($"Current Battery: {item.insertedBattery.charge}");
                    float removeAmount = item.insertedBattery.charge * (ModConfig.gcBatteryDrainPercentage.Value / 100f);
                    float newCharge = item.insertedBattery.charge - removeAmount;
                    player.itemAudio.PlayOneShot(RoundManager.Instance.PressButtonSFX1, 0.7f);
                    item.insertedBattery.charge = newCharge;
                    Plugin.MoreLogs($"New Battery: {item.insertedBattery.charge}");
                }
            }
        }
    }
}
