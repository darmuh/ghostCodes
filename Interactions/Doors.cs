﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ghostCodes
{
    internal class Doors
    {
        internal static List<DoorLock> AllDoors = [];
        internal static bool CustomHauntRunning = false;
        internal static System.Random Rand = new();

        internal static void UpdateCacheDoorsList()
        {
            AllDoors = [.. Object.FindObjectsOfType<DoorLock>()];
        }

        internal static void OpenOrCloseALLDoors()
        {
            Plugin.MoreLogs($"All doors changing state");

            for (int i = 0; i < AllDoors.Count; i++)
            {
                if (AllDoors[i] == null)
                    continue;

                AllDoors[i].OpenOrCloseDoor(StartOfRound.Instance.localPlayerController);
            }
            Plugin.MoreLogs($"All {AllDoors.Count} doors have been opened or closed");
        }

        internal static void OpenorClose1RandomDoor()
        {
            if(AllDoors.Count == 0) 
                return;

            int doorNumber = NumberStuff.GetInt(0, AllDoors.Count);

            if (AllDoors[doorNumber] == null)
                return;
            
            AllDoors[doorNumber].OpenOrCloseDoor(StartOfRound.Instance.localPlayerController);

            Plugin.MoreLogs("Opened or Closed one door");
        }

        internal static void LockorUnlockARandomDoor(bool stateLocked)
        {

            if (AllDoors.Count == 0)
                return;

            for (int i = 0; i < AllDoors.Count; i++)
            {
                int doorNumber = NumberStuff.GetInt(0, AllDoors.Count);

                if (AllDoors[doorNumber] == null)
                    continue;

                if (AllDoors[doorNumber].isLocked && stateLocked)
                {
                    AllDoors[doorNumber].UnlockDoorServerRpc();
                    Plugin.MoreLogs("Unlocked random door");
                    break;
                }
                else if (!AllDoors[doorNumber].isLocked && !stateLocked)
                {
                    AllDoors[doorNumber].LockDoor(); //might need a serverRpc
                    Plugin.MoreLogs("Locked random door");
                    break;
                }
            }
        }

        internal static void HauntDoors(int value)
        {
            if(AllDoors.Count == 0) 
                return;

            float percentage = value / 100f;
            Plugin.MoreLogs($"Attempting to Haunt {value}% of doors!");
            float count;
            if (value == 0)
                count = 1;
            else
                count = AllDoors.Count * percentage;

            for (int i = 0; i < (int)count; i++)
            {
                if (AllDoors[i] == null)
                    continue;

                MonoBehaviour mono = AllDoors[i].gameObject.GetComponent<MonoBehaviour>();
                mono.StopAllCoroutines();
                mono.StartCoroutine(CustomDoorHaunt(AllDoors[i]));
            }

            Plugin.MoreLogs($"Attempted to haunt {(int)count} doors!");
        }

        internal static IEnumerator CustomDoorHaunt(DoorLock thisDoor)
        {
            int times = Rand.Next(9);
            for (int i = 0; i < times; i++)
            {
                float wait;
                if (!thisDoor.isDoorOpened && Rand.Next(100) >= 92)
                    wait = Random.Range(2f, 7f); //extra long wait
                else if (thisDoor.isDoorOpened)
                    wait = Random.Range(0.15f, 0.3f); //quick shut door
                else
                    wait = Random.Range(0.2f, 1.7f); //random interval while door is closed

                yield return new WaitForSeconds(wait);
                thisDoor.OpenOrCloseDoor(StartOfRound.Instance.localPlayerController);
            }

            yield return new WaitForSeconds(0.2f);
            if (thisDoor.isDoorOpened)
                thisDoor.OpenOrCloseDoor(StartOfRound.Instance.localPlayerController);
        }
    }
}
