using GameNetcodeStuff;
using ghostCodes.Configs;
using ghostCodes.Interactions;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ghostCodes.Blastdoors;
using static ghostCodes.Bools;
using static ghostCodes.CodeHandling;
using static ghostCodes.CodeStuff;
using static ghostCodes.Configs.InteractionsConfig;
using static ghostCodes.Doors;
using static ghostCodes.Lights;
using static ghostCodes.Mines;
using static ghostCodes.RapidFire;
using static ghostCodes.ShipStuff;
using static ghostCodes.Teleporters;
using static ghostCodes.Turrets;

namespace ghostCodes
{
    internal class CodeActions
    {
        internal static System.Random Rand = new();
        internal static void InitPossibleActions(StartOfRound instance, int randomObjectNum = -1)
        {
            possibleActions.Clear();

            NetworkingRequiredActions();
            ShipStuff();
            GhostGirlRequired();
            AddRegularDoorStuff();
            CruiserStuff();
            Maininteractions();

            if (randomObjectNum < 0)
                return;

            TerminalObjectActions(instance, randomObjectNum);
            ExternalModCheck();
        }

        private static void Maininteractions()
        {
            if (AffectAllBatteries.Value > 0)
                possibleActions.Add(new ActionPercentage("AffectAllBatteries", () => Items.AdjustAllPlayersBattery(), AffectAllBatteries.Value));

            if (AffectRandomPlayerBatteries.Value > 0)
                possibleActions.Add(new ActionPercentage("AffectRandomPlayerBatteries", () => Items.AffectRandomPlayersBatterys(), AffectRandomPlayerBatteries.Value));

            if (HauntFactoryScrap.Value > 0)
                possibleActions.Add(new ActionPercentage("HauntFactoryScrap", () => Items.HauntItemUse(false), HauntFactoryScrap.Value));

            if (HauntHeldScrap.Value > 0)
                possibleActions.Add(new ActionPercentage("HauntHeldScrap", () => Items.HauntItemUse(true), HauntHeldScrap.Value));

        }

        private static void MessWithWalkies()
        {
            if (WalkieTalkie.allWalkieTalkies.Count == 0 || !SetupConfig.GhostCodesSettings.HauntingsMode)
                return;

            Plugin.Spam("Adding enabled walkie stuff");

            if (GarbleWalkies.Value > 0)
                possibleActions.Add(new ActionPercentage("GarbleWalkies", () => NetHandler.Instance.GarbleWalkiesServerRpc(), GarbleWalkies.Value));

            if (BreatheOnWalkies.Value > 0)
                possibleActions.Add(new ActionPercentage("BreatheOnWalkies", () => NetHandler.Instance.BreatheOnWalkiesServerRpc(), BreatheOnWalkies.Value));
        }

        private static void GhostGirlRequired()
        {
            if (!SetupConfig.GhostCodesSettings.HauntingsMode)
                return;

            if (PlayerLights.Value > 0 && !startRapidFire)
                possibleActions.Add(new ActionPercentage("ggFlashlight", () => GGFlashlight(), PlayerLights.Value));

            if (AffectHauntedPlayerBatteries.Value > 0)
                possibleActions.Add(new ActionPercentage("AffectHauntedPlayerBatteries", () => Items.AffectHauntedPlayersBatterys(), AffectHauntedPlayerBatteries.Value));

            MessWithWalkies();

        }

        private static void ShipStuff()
        {
            if (!AreAnyPlayersInShip())
                return;

            Plugin.Spam("players detected on ship");

            //if (gcConfig.emptyShipEvent.Value)
            //possibleActions.Add(new ActionPercentage("EmptyShipOfPlayers", () => SuckPlayersOutOfShip(), gcConfig.emptyShipChance.Value));

            if (LightsOnShip.Value > 0)
                possibleActions.Add(new ActionPercentage("MessWithShipLights", () => MessWithShipLights(), LightsOnShip.Value));

            if (DoorsOnShip.Value > 0)
                possibleActions.Add(new ActionPercentage("MessWithShipDoors", () => MessWithShipDoors(), DoorsOnShip.Value));

            if (ModConfig.ModNetworking.Value && ShockTerminalUser.Value > 0 && IsAnyPlayerUsingTerminal(out PlayerControllerB player))
            {
                possibleActions.Add(new ActionPercentage("Shock Terminal User", () => ShockTerminal(), ShockTerminalUser.Value));
                Plugin.MoreLogs(player.playerUsername + " detected using terminal");
            }

            if (MonitorsOnShip.Value > 0 && ModConfig.ModNetworking.Value)
                possibleActions.Add(new ActionPercentage("MessWithMonitors", () => NetHandler.Instance.MessWithMonitorsServerRpc(), MonitorsOnShip.Value));

            if (TeleportPlayer.Value > 0 && OpenLib.Common.Teleporter.NormalTP != null)
            {
                Plugin.MoreLogs("Added MessWithNormalTP");
                possibleActions.Add(new ActionPercentage("MessWithNormalTP", () => InteractWithAnyTP(0), TeleportPlayer.Value));
            }

            if (InverseTeleporter.Value > 0 && OpenLib.Common.Teleporter.InverseTP != null)
            {
                Plugin.MoreLogs("Added MessWithInverseTP");
                possibleActions.Add(new ActionPercentage("MessWithInverseTP", () => InteractWithAnyTP(1), InverseTeleporter.Value));
            }

            if (CorruptedCredits.Value > 0 && !TerminalAdditions.credsCorrupted)
                possibleActions.Add(new ActionPercentage("CorruptedCredits", () => TerminalAdditions.CorruptedCredits(true), CorruptedCredits.Value));

            if (HeavyLever.Value > 0)
                possibleActions.Add(new ActionPercentage("HeavyLever", () => HeavyLeverFunc(), HeavyLever.Value));

            if (HauntedOrder.Value > 0 && Plugin.instance.Terminal.orderedItemsFromTerminal.Count == 0 && Plugin.instance.Terminal.numberOfItemsInDropship == 0)
                possibleActions.Add(new ActionPercentage("HauntedOrder", () => HauntedOrderFunc(), HauntedOrder.Value));

        }

        private static void AddRegularDoorStuff()
        {
            if (!AreAnyPlayersInFacility())
                return;

            if (ToggleAllRegularDoors.Value > 0)
                possibleActions.Add(new ActionPercentage("ToggleAllRegularDoors", () => OpenOrCloseALLDoors(), ToggleAllRegularDoors.Value));

            if (ToggleRegularDoor.Value > 0)
                possibleActions.Add(new ActionPercentage("ToggleRegularDoor", () => OpenorClose1RandomDoor(), ToggleRegularDoor.Value));

            if (LockSingleDoor.Value > 0)
                possibleActions.Add(new ActionPercentage("LockONEdoor", () => LockorUnlockARandomDoor(true), LockSingleDoor.Value));

            if (UnlockSingleDoor.Value > 0)
                possibleActions.Add(new ActionPercentage("UnlockONEdoor", () => LockorUnlockARandomDoor(false), UnlockSingleDoor.Value));

            if (TryHauntSingleDoor.Value > 0)
                possibleActions.Add(new ActionPercentage("TryHauntSingleDoor", () => HauntDoors(0), TryHauntSingleDoor.Value));

            if (TryHauntHalfAllDoors.Value > 0)
                possibleActions.Add(new ActionPercentage("TryHauntHalfAllDoors", () => HauntDoors(50), TryHauntHalfAllDoors.Value));

            if (TryHauntAllDoors.Value > 0)
                possibleActions.Add(new ActionPercentage("TryHauntAllDoors", () => HauntDoors(100), TryHauntAllDoors.Value));
        }

        private static void CruiserStuff()
        {
            if (Plugin.instance.Cruiser == null)
                return;

            if (!Plugin.instance.Cruiser.ignitionStarted)
                return;

            Plugin.MoreLogs("Adding cruiser interactions!!");

            if (ChangeCruiserRadio.Value > 0)
                possibleActions.Add(new ActionPercentage("ChangeCruiserRadio", () => Plugin.instance.Cruiser.ChangeRadioStation(), ChangeCruiserRadio.Value));

            if (CruiserEjectDriver.Value > 0)
                possibleActions.Add(new ActionPercentage("CruiserEjectDriver", () => Plugin.instance.Cruiser.SpringDriverSeatServerRpc(), CruiserEjectDriver.Value));

            if (CruiserUseBoost.Value > 0 && Plugin.instance.Cruiser.IsOwner)
                possibleActions.Add(new ActionPercentage("CruiserUseBoost", () => Plugin.instance.Cruiser.UseTurboBoostLocalClient(), CruiserUseBoost.Value));

            if (CruiserPush.Value > 0)
                possibleActions.Add(new ActionPercentage("CruiserPush", () => Cruiser.Push(), CruiserPush.Value));

            if (ToggleCruiserDoors.Value > 0)
                possibleActions.Add(new ActionPercentage("CruiserDoors", () => Cruiser.Doors(), ToggleCruiserDoors.Value));

            if (ToggleCruiserLights.Value > 0)
                possibleActions.Add(new ActionPercentage("ToggleCruiserLights", () => Cruiser.Headlights(1), ToggleCruiserLights.Value));

            if (FlickerCruiserLights.Value > 0)
            {
                possibleActions.Add(new ActionPercentage("FlickerCruiserLights", () => Cruiser.Headlights(Rand.Next(11)), FlickerCruiserLights.Value));
            }

            if (ToggleCruiserHood.Value > 0)
                possibleActions.Add(new ActionPercentage("ToggleCruiserHood", () => Plugin.instance.Cruiser.ToggleHoodOpenLocalClient(), ToggleCruiserHood.Value));

            if (CruiserWindshield.Value > 0)
                possibleActions.Add(new ActionPercentage("CruiserWindshield", () => Cruiser.Windshield(), CruiserWindshield.Value));

            if (CruiserShiftGears.Value > 0)
                possibleActions.Add(new ActionPercentage("CruiserShiftGears", () => Cruiser.GearShift(), CruiserShiftGears.Value));

        }

        private static void ExternalModCheck()
        {
            Plugin.MoreLogs("Checking for any external mods to utilize ghost codes on");
            if (Plugin.instance.ToilHead)
                Compatibility.ToilHead.CheckForToilHeadObjects();
        }

        private static void NetworkingRequiredActions()
        {
            if (!ModConfig.ModNetworking.Value)
                return;

            if (FlipBreaker.Value > 0)
                possibleActions.Add(new ActionPercentage("facilityLights", () => FlipLights(), FlipBreaker.Value));
        }

        private static void TerminalObjectActions(StartOfRound instance, int randomObjectNum)
        {

            if (myTerminalObjects.Count > 0)
                possibleActions.Add(new ActionPercentage("DefaultCodes", () => DefaultTerminalAction(randomObjectNum), 100));

            if (!SetupConfig.IgnoreDoors.Value && IsThisaBigDoor(randomObjectNum))
                possibleActions.Add(new ActionPercentage("hungryDoor", () => HandleHungryDoor(randomObjectNum, instance), HungryBlastDoor.Value));
            if (!SetupConfig.IgnoreTurrets.Value && IsThisaTurret(randomObjectNum))
                possibleActions.Add(new ActionPercentage("normalTurret", () => HandleTurretAction(randomObjectNum), TurretBerserk.Value));
            if (!SetupConfig.IgnoreLandmines.Value && IsThisaMine(randomObjectNum))
                possibleActions.Add(new ActionPercentage("normalMine", () => HandleMineAction(randomObjectNum), MineBoom.Value));
        }

        internal static void DefaultTerminalAction(int randomObjectNum)
        {
            if (randomObjectNum == -1)
                return;

            myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
        }
    }

    // Helper class to represent an action with its percentage
    public class ActionPercentage(string name, Action action, float percentage)
    {
        public Action Action { get; } = action;
        public float Percentage { get; } = percentage;
        public string Name { get; set; } = name;


        // Helper method to choose actions from a list based on percentages
        public static List<Action> ChooseActionsFromPercentages(List<ActionPercentage> actions)
        {
            Shuffle(actions);
            List<Action> chosenActions = [];
            int numActions = 0;

            foreach (var action in actions)
            {
                Plugin.Spam($"Checking {action.Name} against percentage");
                int randomValue = NumberStuff.Rand.Next(101);
                float chance = action.Percentage;
                if (startRapidFire)
                    chance = NumberStuff.GetClampedInsanityPercent(chance, InsanityConfig.InsanityModeMultiplier.Value);

                if (numActions < 3 && (randomValue <= chance || Mathf.Approximately(chance, 100f)))
                {
                    Plugin.Spam($"Action {action.Name} was chosen!");
                    chosenActions.Add(action.Action);
                    numActions++;
                }
            }

            return chosenActions;
        }

        // Fisher-Yates shuffle algorithm
        private static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = NumberStuff.Rand.Next(n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
