using Object = UnityEngine.Object;

namespace ghostCodes
{
    internal class Doors
    {
        internal static void OpenOrCloseALLDoors(bool stateOpened)
        {
            Plugin.MoreLogs($"All doors changing state from {stateOpened}");
            DoorLock[] doors = Object.FindObjectsOfType<DoorLock>();

            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].isDoorOpened == stateOpened)
                {
                    doors[i].OpenOrCloseDoor(StartOfRound.Instance.localPlayerController);
                }
            }
            Plugin.MoreLogs($"All {stateOpened} (isDoorOpened) doors have been opened or closed");
        }

        internal static void OpenorClose1RandomDoor(bool stateOpened)
        {
            DoorLock[] doors = Object.FindObjectsOfType<DoorLock>();      
            for (int i = 0; i < doors.Length; i++)
            {
                int doorNumber = NumberStuff.GetInt(0, doors.Length - 1);
                if (doors[doorNumber].isDoorOpened == stateOpened)
                {
                    doors[doorNumber].OpenOrCloseDoor(StartOfRound.Instance.localPlayerController);
                    break;
                }
            }
            Plugin.MoreLogs("Opened or Closed one door");
        }

        internal static void LockorUnlockARandomDoor(bool stateLocked)
        {
            DoorLock[] doors = Object.FindObjectsOfType<DoorLock>();

            for (int i = 0; i < doors.Length; i++)
            {
                int doorNumber = NumberStuff.GetInt(0, doors.Length - 1);
                if (doors[doorNumber].isLocked && stateLocked)
                {
                    doors[doorNumber].UnlockDoorServerRpc();
                    Plugin.MoreLogs("Unlocked random door");
                    break;
                }
                else if (!doors[doorNumber].isLocked && !stateLocked)
                {
                    doors[doorNumber].LockDoor(); //might need a serverRpc
                    Plugin.MoreLogs("Locked random door");
                    break;
                }
            }
        }
    }
}
