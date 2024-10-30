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
            AddRegularDoorStuff();
            
            if (randomObjectNum < 0)
                return;

            AddRapidFireActions(instance, randomObjectNum);
            TerminalObjectActions(instance, randomObjectNum);
            ExternalModCheck();
        }

        private static void ShipStuff()
        {
            if (!AreAnyPlayersInShip())
                return;

            //Plugin.MoreLogs("players detected on ship");

            //if (gcConfig.emptyShipEvent.Value)
                //possibleActions.Add(new ActionPercentage("EmptyShipOfPlayers", () => SuckPlayersOutOfShip(), gcConfig.emptyShipChance.Value));

            if(gcConfig.lightsOnShipEvent.Value)
                possibleActions.Add(new ActionPercentage("MessWithShipLights", () => MessWithShipLights(), gcConfig.lightsOnShipChance.Value));

            if(gcConfig.doorsOnShipEvent.Value)
                possibleActions.Add(new ActionPercentage("MessWithShipDoors", () => MessWithShipDoors(), gcConfig.doorsOnShipChance.Value));

            if(gcConfig.ModNetworking.Value && gcConfig.shockTerminalUserEvent.Value && IsAnyPlayerUsingTerminal(out PlayerControllerB player))
            {
                possibleActions.Add(new ActionPercentage("Shock Terminal User", () => ShockTerminalUser(), gcConfig.shockTerminalUserChance.Value));
                Plugin.MoreLogs(player.playerUsername + " detected using terminal");
            } 

            if(gcConfig.monitorsOnShipEvent.Value && gcConfig.ModNetworking.Value)
                possibleActions.Add(new ActionPercentage("MessWithMonitors", () => NetHandler.Instance.MessWithMonitorsServerRpc(), gcConfig.monitorsOnShipChance.Value));

            if(gcConfig.normalTpEvent.Value && Plugin.instance.NormalTP != null)
            {
                Plugin.MoreLogs("Added MessWithNormalTP");
                possibleActions.Add(new ActionPercentage("MessWithNormalTP", () => InteractWithAnyTP(0), gcConfig.normalTpChance.Value));
            }
                

            if (gcConfig.inverseTpEvent.Value && Plugin.instance.InverseTP != null)
            {
                Plugin.MoreLogs("Added MessWithInverseTP");
                possibleActions.Add(new ActionPercentage("MessWithInverseTP", () => InteractWithAnyTP(1), gcConfig.inverseTpChance.Value));
            }
                
        }

        private static void AddRegularDoorStuff()
        {
            if(gcConfig.openAllRegularDoorsEvent.Value)
                possibleActions.Add(new ActionPercentage("OpenALLDoors", () => OpenOrCloseALLDoors(false), gcConfig.openAllRegularDoorsChance.Value));

            if (gcConfig.closeAllRegularDoorsEvent.Value)
                possibleActions.Add(new ActionPercentage("CloseALLDoors", () => OpenOrCloseALLDoors(true), gcConfig.closeAllRegularDoorsChance.Value));

            if (gcConfig.openSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("OpenONEdoor", () => OpenorClose1RandomDoor(false), gcConfig.openSingleDoorChance.Value));

            if (gcConfig.closeSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("CloseONEdoor", () => OpenorClose1RandomDoor(true), gcConfig.closeSingleDoorChance.Value));

            if (gcConfig.lockSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("LockONEdoor", () => LockorUnlockARandomDoor(true), gcConfig.lockSingleDoorChance.Value));

            if (gcConfig.lockSingleDoorEvent.Value)
                possibleActions.Add(new ActionPercentage("LockONEdoor", () => LockorUnlockARandomDoor(false), gcConfig.lockSingleDoorChance.Value));

        }

        private static void AddRapidFireActions(StartOfRound instance, int randomObjectNum)
        {
            if (!startRapidFire)
                return;

            if(!gcConfig.gcIgnoreTurrets.Value && isThisaTurret(randomObjectNum))
                possibleActions.Add(new ActionPercentage("rfTurret", () => HandleTurretAction(randomObjectNum), gcConfig.turretInsaneBChance.Value));

            if (!gcConfig.gcIgnoreLandmines.Value && isThisaMine(randomObjectNum))
                possibleActions.Add(new ActionPercentage("rfMine", () => HandleMineAction(randomObjectNum), gcConfig.mineInsaneBChance.Value));
            
            if(!gcConfig.gcIgnoreDoors.Value && isThisaBigDoor(randomObjectNum))
                possibleActions.Add(new ActionPercentage("rfHungryDoor", () => HandleHungryDoor(randomObjectNum, instance), gcConfig.hungryDoorIChance.Value));
        }

        private static void DressGirlRequiredActions()
        {
            if (Plugin.instance.DressGirl == null || !gcConfig.ModNetworking.Value)
                return;

            possibleActions.Add(new ActionPercentage("ggFlashlight", () => GGFlashlight(), gcConfig.ggPlayerLightsPercent.Value));
        }

        private static void ExternalModCheck()
        {
            Plugin.MoreLogs("Checking for any external mods to utilize ghost codes on");
            if(Plugin.instance.toilHead)
                Compatibility.ToilHead.CheckForToilHeadObjects();
        }

        private static void NetworkingRequiredActions()
        {
            if (!gcConfig.ModNetworking.Value)
                return;

            possibleActions.Add(new ActionPercentage("facilityLights", () => FlipLights(), gcConfig.ggCodeBreakerChance.Value));
        }

        private static void TerminalObjectActions(StartOfRound instance, int randomObjectNum)
        {
            if (!AreAnyPlayersInFacility())
                return;

            if (myTerminalObjects.Count > 0)
                possibleActions.Add(new ActionPercentage("DefaultCodes", () => DefaultTerminalAction(randomObjectNum), 100));

            if (!gcConfig.gcIgnoreDoors.Value && isThisaBigDoor(randomObjectNum))
                possibleActions.Add(new ActionPercentage("hungryDoor", () => HandleHungryDoor(randomObjectNum, instance), gcConfig.hungryDoorNChance.Value));
            if (!gcConfig.gcIgnoreTurrets.Value && isThisaTurret(randomObjectNum))
                possibleActions.Add(new ActionPercentage("normalTurret", () => HandleTurretAction(randomObjectNum), gcConfig.turretNormalBChance.Value));
            if (!gcConfig.gcIgnoreLandmines.Value && isThisaMine(randomObjectNum))
                possibleActions.Add(new ActionPercentage("normalMine", () => HandleMineAction(randomObjectNum), gcConfig.mineNormalBChance.Value));
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
