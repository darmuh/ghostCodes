using GameNetcodeStuff;
using ghostCodes.Configs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ghostCodes.Misc;

namespace ghostCodes.Interactions
{
    internal class Items
    {
        internal static List<GrabbableObject> AllItems = [];
        internal static System.Random Rand = new();
        internal static AudioClip Adjuster;

        internal static void AdjustAllPlayersBattery()
        {
            List<PlayerControllerB> allPlayers = GetAllLivingPlayers();

            foreach (PlayerControllerB player in allPlayers)
            {
                if (player.isInHangarShipRoom)
                {
                    Plugin.MoreLogs("Player is on the ship, not changing battery status");
                }
                else
                    NetHandler.Instance.BatteryAdjustServerRpc((int)player.actualClientId);

            }
        }

        internal static void AffectHauntedPlayersBatterys()
        {
            if (Plugin.instance.DressGirl == null || !Plugin.instance.DressGirl.hauntingLocalPlayer)
                return;

            if (Plugin.instance.DressGirl.hauntingPlayer.isInHangarShipRoom)
                return;

            PlayerAffectBatteries(Plugin.instance.DressGirl.hauntingPlayer);
            Plugin.MoreLogs("haunted player battery drain called");
        }

        internal static void AffectRandomPlayersBatterys()
        {
            List<PlayerControllerB> allPlayers = GetAllLivingPlayers();
            int randomPlayer = NumberStuff.GetInt(0, allPlayers.Count);
            if (allPlayers[randomPlayer].isInHangarShipRoom)
                return;

            NetHandler.Instance.BatteryAdjustServerRpc((int)allPlayers[randomPlayer].actualClientId);
            Plugin.MoreLogs($"Draining battery of {allPlayers[randomPlayer].playerUsername}");
        }

        internal static void PlayerAffectBatteries(PlayerControllerB player)
        {
            if (player == null)
                return;

            foreach (GrabbableObject item in player.ItemSlots)
            {
                if (item == null)
                    continue;

                if (!item.isPocketed)
                    ItemAdjustBattery(item, player);
                else
                {
                    if (Rand.Next(101) >= 50)
                        ItemAdjustBattery(item, player);
                }
            }
        }

        private static void ItemAdjustBattery(GrabbableObject item, PlayerControllerB player)
        {
            if (item == null || RoundManager.Instance == null)
                return;

            if (item.itemProperties.requiresBattery)
            {
                Plugin.MoreLogs("found item has a battery");
                Plugin.MoreLogs($"Current Battery: {item.insertedBattery.charge}");
                float adjuster = item.insertedBattery.charge * (InteractionsConfig.BatteryPercentageModifier.Value / 100f);
                float newCharge;
                if (Rand.Next(2) == 0)
                {
                    newCharge = item.insertedBattery.charge - adjuster;
                    newCharge = Mathf.Clamp(newCharge, 0, 100);
                }
                else
                {
                    newCharge = item.insertedBattery.charge + adjuster;
                    newCharge = Mathf.Clamp(newCharge, 0, 100);
                }

                NetHandler.Instance.BatteryAdjustSoundServerRpc((int)player.actualClientId);
                item.insertedBattery.charge = newCharge;
                Plugin.MoreLogs($"New Battery: {item.insertedBattery.charge}");
            }
        }

        internal static void HauntItemUse(bool isHeld)
        {
            List<GrabbableObject> SoundMakers = [.. Object.FindObjectsOfType<GrabbableObject>()];

            List<string> noTouch = OpenLib.Common.CommonStringStuff.GetKeywordsPerConfigItem(InteractionsConfig.NoTouchItems.stringValue.ToLower(), ',');

            SoundMakers.RemoveAll(x => noTouch.Contains(x.name.ToLower())); //remove deny list items

            if (isHeld)
            {
                foreach (GrabbableObject item in SoundMakers)
                {
                    if (item.isHeld && item.scrapValue > 0 && Rand.Next(101) >= 50)
                        item.ActivateItemServerRpc(true, true);
                }
            }
            else
            {
                foreach (GrabbableObject item in SoundMakers)
                {
                    if (item.isInFactory && item.scrapValue > 0 && Rand.Next(101) >= 50)
                        item.ActivateItemServerRpc(true, true);
                }
            }
        }
    }
}
