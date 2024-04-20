using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using static ghostCodes.CodeStuff;
using static ghostCodes.Bools;
using static ghostCodes.CodeHandling;
using static ghostCodes.Blastdoors;
using static ghostCodes.Mines;
using static ghostCodes.Doors;
using static ghostCodes.Turrets;
using static ghostCodes.Lights;
using static ghostCodes.RapidFire;
using static ghostCodes.ShipStuff;
using static ghostCodes.Teleporters;
using GameNetcodeStuff;
using ghostCodes.Interactions;

namespace ghostCodes
{
    internal class CodeActions
    {
        internal static void InitPossibleActions(StartOfRound instance, int randomObjectNum = -1)
        {
            possibleActions.Clear();
            
            NetworkingRequiredActions();
            DressGirlRequiredActions();
            ShipStuff();
            MessWithItems();
            AddRegularDoorStuff();
            
            if (randomObjectNum < 0)
                return;

            AddRapidFireActions(instance, randomObjectNum);
            TerminalObjectActions(instance, randomObjectNum);
            ExternalModCheck();
        }

        private static void MessWithWalkies()
        {
            if (WalkieTalkie.allWalkieTalkies.Count == 0 || !ModConfig.walkieSectionStuff.Value) 
                return;

            Plugin.MoreLogs("Adding enabled walkie stuff");

            if(ModConfig.gcGarbleWalkies.Value)
                possibleActions.Add(new ActionPercentage("GarbleWalkies", () => NetHandler.Instance.GarbleWalkiesServerRpc(), ModConfig.gcGarbleWalkiesChance.Value));

            if (ModConfig.ggBreatheOnWalkies.Value || Plugin.instance.DressGirl == null )
                possibleActions.Add(new ActionPercentage("BreatheOnWalkies", () => NetHandler.Instance.BreatheOnWalkiesServerRpc(), ModConfig.ggBreatheOnWalkiesChance.Value));
        }

        private static void MessWithItems()
        {
            if (!ModConfig.itemSectionStuff.Value)
                return;

            if(ModConfig.gcDrainAllBatteries.Value)
                possibleActions.Add(new ActionPercentage("DrainAllBatteries", () => Items.DrainAllPlayersBattery(), ModConfig.gcDrainAllBatteriesChance.Value));

            if(ModConfig.gcDrainHauntedPlayerBatteries.Value && Plugin.instance.DressGirl != null)
                possibleActions.Add(new ActionPercentage("DrainHauntedPlayerBatteries", () => Items.DrainHauntedPlayersBatterys(), ModConfig.gcDrainHauntedPlayerBatteriesChance.Value));

            if (ModConfig.gcDrainRandomPlayerBatteries.Value)
                possibleActions.Add(new ActionPercentage("DrainRandomPlayerBatteries", () => Items.DrainRandomPlayersBatterys(), ModConfig.gcDrainRandomPlayerBatteriesChance.Value));

        }

        private static void ShipStuff()
        {
            if (!AreAnyPlayersInShip() || !ModConfig.shipstuffSection.Value)
                return;

            //Plugin.MoreLogs("players detected on ship");

            //if (gcConfig.emptyShipEvent.Value)
                //possibleActions.Add(new ActionPercentage("EmptyShipOfPlayers", () => SuckPlayersOutOfShip(), gcConfig.emptyShipChance.Value));

            if(ModConfig.lightsOnShipEvent.Value)
                possibleActions.Add(new ActionPercentage("MessWithShipLights", () => MessWithShipLights(), ModConfig.lightsOnShipChance.Value));

            if(ModConfig.doorsOnShipEvent.Value)
                possibleActions.Add(new ActionPercentage("MessWithShipDoors", () => MessWithShipDoors(), ModConfig.doorsOnShipChance.Value));

            if(ModConfig.ModNetworking.Value && ModConfig.shockTerminalUserEvent.Value && IsAnyPlayerUsingTerminal(out PlayerControllerB player))
            {
                possibleActions.Add(new ActionPercentage("Shock Terminal User", () => ShockTerminalUser(), ModConfig.shockTerminalUserChance.Value));
                Plugin.MoreLogs(player.playerUsername + " detected using terminal");
            } 

            if(ModConfig.monitorsOnShipEvent.Value && ModConfig.ModNetworking.Value)
                possibleActions.Add(new ActionPercentage("MessWithMonitors", () => NetHandler.Instance.MessWithMonitorsServerRpc(), ModConfig.monitorsOnShipChance.Value));

            if(ModConfig.normalTpEvent.Value && Plugin.instance.NormalTP != null)
            {
                Plugin.MoreLogs("Added MessWithNormalTP");
                possibleActions.Add(new ActionPercentage("MessWithNormalTP", () => InteractWithAnyTP(0), ModConfig.normalTpChance.Value));
            }
                

            if (ModConfig.inverseTpEvent.Value && Plugin.instance.InverseTP != null)
            {
                Plugin.MoreLogs("Added MessWithInverseTP");
                possibleActions.Add(new ActionPercentage("MessWithInverseTP", () => InteractWithAnyTP(1), ModConfig.inverseTpChance.Value));
            }
                
        }

        private static void AddRegularDoorStuff()
        {
            if (!ModConfig.regularDoorsSectionStuff.Value || !AreAnyPlayersInFacility())
                return;

            if(ModConfig.openAllRegularDoorsEvent.Value)
                possibleActions.Add(new ActionPercentage("OpenALLDoors", () => OpenOrCloseALLDoors(false), ModConfig.openAllRegularDoorsChance.Value));

            if (ModConfig.closeAllRegularDoorsEvent.Value)
                possibleActions.Add(new ActionPercentage("CloseALLDoors", () => OpenOrCloseALLDoors(true), ModConfig.closeAllRegularDoorsChance.Value));

            if (ModConfig.openSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("OpenONEdoor", () => OpenorClose1RandomDoor(false), ModConfig.openSingleDoorChance.Value));

            if (ModConfig.closeSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("CloseONEdoor", () => OpenorClose1RandomDoor(true), ModConfig.closeSingleDoorChance.Value));

            if (ModConfig.lockSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("LockONEdoor", () => LockorUnlockARandomDoor(true), ModConfig.lockSingleDoorChance.Value));

            if (ModConfig.unlockSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("UnlockONEdoor", () => LockorUnlockARandomDoor(false), ModConfig.unlockSingleDoorChance.Value));

        }

        private static void AddRapidFireActions(StartOfRound instance, int randomObjectNum)
        {
            if (!startRapidFire)
                return;

            if(!ModConfig.gcIgnoreTurrets.Value && isThisaTurret(randomObjectNum))
                possibleActions.Add(new ActionPercentage("rfTurret", () => HandleTurretAction(randomObjectNum), ModConfig.turretInsaneBChance.Value));

            if (!ModConfig.gcIgnoreLandmines.Value && isThisaMine(randomObjectNum))
                possibleActions.Add(new ActionPercentage("rfMine", () => HandleMineAction(randomObjectNum), ModConfig.mineInsaneBChance.Value));
            
            if(!ModConfig.gcIgnoreDoors.Value && isThisaBigDoor(randomObjectNum))
                possibleActions.Add(new ActionPercentage("rfHungryDoor", () => HandleHungryDoor(randomObjectNum, instance), ModConfig.hungryDoorIChance.Value));
        }

        private static void DressGirlRequiredActions()
        {
            if (Plugin.instance.DressGirl == null || !ModConfig.ModNetworking.Value)
                return;


            possibleActions.Add(new ActionPercentage("ggFlashlight", () => GGFlashlight(), ModConfig.ggPlayerLightsPercent.Value));

            MessWithWalkies();
        }

        private static void ExternalModCheck()
        {
            Plugin.MoreLogs("Checking for any external mods to utilize ghost codes on");
            if(Plugin.instance.toilHead)
                Compatibility.ToilHead.CheckForToilHeadObjects();
        }

        private static void NetworkingRequiredActions()
        {
            if (!ModConfig.ModNetworking.Value)
                return;

            possibleActions.Add(new ActionPercentage("facilityLights", () => FlipLights(), ModConfig.ggCodeBreakerChance.Value));
        }

        private static void TerminalObjectActions(StartOfRound instance, int randomObjectNum)
        {

            if (myTerminalObjects.Count > 0)
                possibleActions.Add(new ActionPercentage("DefaultCodes", () => DefaultTerminalAction(randomObjectNum), 100));

            if (!ModConfig.gcIgnoreDoors.Value && isThisaBigDoor(randomObjectNum))
                possibleActions.Add(new ActionPercentage("hungryDoor", () => HandleHungryDoor(randomObjectNum, instance), ModConfig.hungryDoorNChance.Value));
            if (!ModConfig.gcIgnoreTurrets.Value && isThisaTurret(randomObjectNum))
                possibleActions.Add(new ActionPercentage("normalTurret", () => HandleTurretAction(randomObjectNum), ModConfig.turretNormalBChance.Value));
            if (!ModConfig.gcIgnoreLandmines.Value && isThisaMine(randomObjectNum))
                possibleActions.Add(new ActionPercentage("normalMine", () => HandleMineAction(randomObjectNum), ModConfig.mineNormalBChance.Value));
        }

        internal static void DefaultTerminalAction(int randomObjectNum)
        {
            myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
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
            Shuffle(actions);
            List<Action> chosenActions = new List<Action>();
            int numActions = 0;

            foreach (var action in actions)
            {
                float randomValue = Random.Range(0f, 100f);

                if (numActions < 3 && (randomValue <= action.Percentage || Mathf.Approximately(action.Percentage, 100f)))
                {
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
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
