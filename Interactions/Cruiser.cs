using GameNetcodeStuff;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ghostCodes.Interactions
{
    internal class Cruiser
    {
        internal static System.Random Rand = new();
        internal static void Windshield() //might need rpc
        {
            Plugin.instance.Cruiser.BreakWindshield();
        }

        internal static void Headlights(int times)
        {
            Plugin.instance.Cruiser.StartCoroutine(FlickerHeadLights(times));
        }

        internal static IEnumerator FlickerHeadLights(int times)
        {
            float rand = Random.Range(0.2f, 0.8f);
            for (int i = 0; i < times; i++)
            {
                Plugin.instance.Cruiser.ToggleHeadlightsLocalClient(); //flicker lights
                yield return new WaitForSeconds(rand);
            }
        }

        internal static void GearShift()
        {
            int newGear;
            do { newGear = Rand.Next(1, 4); } while (newGear == (int)Plugin.instance.Cruiser.gear); //get random number that is NOT current gear

            Plugin.instance.Cruiser.ShiftToGearAndSync(newGear);
        }

        internal static void Doors()
        {
            int num = Rand.Next(1,4);
            if (num == 0)
                BackDoor();
            else if (num == 1)
                Plugin.instance.Cruiser.driverSideDoorTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            else if (num == 2)
                Plugin.instance.Cruiser.passengerSideDoorTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            else
            {
                BackDoor();
                Plugin.instance.Cruiser.driverSideDoorTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                Plugin.instance.Cruiser.passengerSideDoorTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
            }
        }

        internal static void BackDoor()
        {
            // Get the InteractTrigger component from the backdoor
            InteractTrigger interactTrigger = Plugin.instance.Cruiser.backDoorContainer.GetComponentInChildren<InteractTrigger>();

            // Invoke the onInteract event if the backdoor and event are found
            if (interactTrigger != null)
            {
                UnityEvent<PlayerControllerB> onInteractEvent = interactTrigger.onInteract;

                onInteractEvent?.Invoke(GameNetworkManager.Instance.localPlayerController);
            }
        }

        internal static void Push()
        {
            Vector3 rand = new(Rand.Next(2), Rand.Next(2), Rand.Next(2));
            Vector3 pos = Plugin.instance.Cruiser.syncedPosition + rand;
            Vector3 dir;
            int randomDir = Rand.Next(101);
            if (randomDir <= 25)
                dir = Plugin.instance.Cruiser.transform.forward;
            else if (randomDir > 25 && randomDir <= 50)
                dir = Plugin.instance.Cruiser.transform.right;
            else if (randomDir > 50 && randomDir <= 75)
                dir = -Plugin.instance.Cruiser.transform.forward;
            else
                dir = -Plugin.instance.Cruiser.transform.right;

            Plugin.instance.Cruiser.PushTruckServerRpc(pos, dir);
        }

    }
}
