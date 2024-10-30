using GameNetcodeStuff;
using ghostCodes.Configs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ghostCodes
{
    internal class ShipStuff
    {
        internal static bool activeMonitorEvent = false;
        internal static bool activeShipDoorsEvent = false;
        internal static bool activeShipLightsEvent = false;
        internal static bool activeShockTerminalEvent = false;
        internal static System.Random Rand = new();

        internal static bool leverChanged = false;

        internal static void SuckPlayersOutOfShip()
        {
            if (Bools.AreAnyPlayersInShip())
            {
                Plugin.MoreLogs("Players detected in the ship!");
                //NetHandler.Instance.EmptyShipServerRpc();
            }
        }

        internal static void MessWithMonitors()
        {
            Plugin.MoreLogs("mess with monitors called");
            if (activeMonitorEvent)
                return;

            StartOfRound.Instance.StartCoroutine(ShipMonitorsEvent());
        }

        internal static IEnumerator ShipMonitorsEvent()
        {
            Plugin.MoreLogs("messing with monitors!");
            activeMonitorEvent = true;

            GetTextValues(StartOfRound.Instance.profitQuotaMonitorText, out Color quotaColor, out string quotaString);
            GetTextValues(StartOfRound.Instance.deadlineMonitorText, out Color deadlineColor, out string deadlineString);
            GetMonitorVals(StartOfRound.Instance.deadlineMonitorBGImage, out Color deadlineMonitorColor);
            GetMonitorVals(StartOfRound.Instance.profitQuotaMonitorBGImage, out Color quotaMonitorColor);

            List<string> messages = [.. InteractionsConfig.AllMonitorMessages.stringValue.Split(',')];

            SignalTranslator.OddSignalMessage(messages, out string message1);
            SignalTranslator.OddSignalMessage(messages, out string message2);

            float time = NumberStuff.GetFloat(1f, 10f);
            float glitch = NumberStuff.GetFloat(0.2f, 0.7f);

            ToggleMonitors(false);
            yield return new WaitForSeconds(glitch);
            ToggleMonitors(true);

            SetMonitorVals(StartOfRound.Instance.deadlineMonitorBGImage, Color.black);
            SetMonitorVals(StartOfRound.Instance.profitQuotaMonitorBGImage, Color.black);
            SetTextVals(StartOfRound.Instance.profitQuotaMonitorText, Color.red, message1);
            SetTextVals(StartOfRound.Instance.deadlineMonitorText, Color.red, message2);

            yield return new WaitForSeconds(time);

            SetMonitorVals(StartOfRound.Instance.deadlineMonitorBGImage, deadlineMonitorColor);
            SetMonitorVals(StartOfRound.Instance.profitQuotaMonitorBGImage, quotaMonitorColor);
            SetTextVals(StartOfRound.Instance.profitQuotaMonitorText, quotaColor, quotaString);
            SetTextVals(StartOfRound.Instance.deadlineMonitorText, deadlineColor, deadlineString);

            activeMonitorEvent = false;
        }

        private static void ToggleMonitors(bool state)
        {
            StartOfRound.Instance.profitQuotaMonitorBGImage.gameObject.SetActive(state);
            StartOfRound.Instance.profitQuotaMonitorText.gameObject.SetActive(state);
            StartOfRound.Instance.deadlineMonitorBGImage.gameObject.SetActive(state);
            StartOfRound.Instance.deadlineMonitorText.gameObject.SetActive(state);
        }

        internal static void SetTextVals(TextMeshProUGUI element, Color color, string text)
        {
            element.text = text;
            element.color = color;
            Plugin.MoreLogs("Set TextVals");
        }

        private static void SetMonitorVals(Image monitor, Color color)
        {
            monitor.color = color;
        }

        private static void GetMonitorVals(Image monitor, out Color color)
        {
            color = monitor.color;
        }

        internal static void GetTextValues(TextMeshProUGUI input, out Color color, out string text)
        {
            color = input.color;
            text = input.text;
            return;
        }

        internal static void ShockTerminal()
        {
            if (!Bools.IsAnyPlayerUsingTerminal(out PlayerControllerB player))
                return;

            Plugin.MoreLogs("shocking terminal user!");
            NetHandler.Instance.ShockTerminalSoundServerRpc();
            Plugin.instance.Terminal.QuitTerminal();
            HUDManager.Instance.ShakeCamera(ScreenShakeType.VeryStrong);
            float newHealth = player.health * 0.90f;
            player.health = (int)Mathf.Clamp(newHealth, 0, 100);
        }

        internal static void ToggleShipDoors()
        {
            if (!StartOfRound.Instance.shipDoorsEnabled)
                return;

            // Find the corresponding button GameObject based on the hangar doors state
            GameObject buttonObject = GameObject.Find(StartOfRound.Instance.hangarDoorsClosed ? "StartButton" : "StopButton");

            // Get the InteractTrigger component from the button
            InteractTrigger interactTrigger = buttonObject?.GetComponentInChildren<InteractTrigger>();

            // Invoke the onInteract event if the button and event are found
            if (interactTrigger != null)
            {
                UnityEvent<PlayerControllerB> onInteractEvent = interactTrigger.onInteract;

                onInteractEvent?.Invoke(GameNetworkManager.Instance.localPlayerController);
            }
        }

        internal static void ToggleShipLights()
        {

            // Find the corresponding button GameObject based on the hangar doors state
            GameObject buttonObject = GameObject.Find(StartOfRound.Instance.shipRoomLights.areLightsOn ? "LightSwitch" : "LightSwitch");

            // Get the InteractTrigger component from the button
            InteractTrigger interactTrigger = buttonObject?.GetComponentInChildren<InteractTrigger>();

            // Invoke the onInteract event if the button and event are found
            if (interactTrigger != null)
            {
                UnityEvent<PlayerControllerB> onInteractEvent = interactTrigger.onInteract;

                onInteractEvent?.Invoke(GameNetworkManager.Instance.localPlayerController);
            }
        }

        internal static void HeavyLeverFunc()
        {
            StartMatchLever startMatchLever = Object.FindObjectOfType<StartMatchLever>();
            if (Rand.Next(0, 100) > 70)
            {
                NetHandler.Instance.HeavyLeverServerRpc(Random.Range(1.5f, 5f));
                Plugin.Spam($"timetoholdspeedmultiplier boost!");
            }
            else
            {
                NetHandler.Instance.HeavyLeverServerRpc(Random.Range(0.05f, 0.5f));
                Plugin.Spam($"timetoholdspeedmultiplier slowed!");
            }
        }

        internal static void ResetLever()
        {
            if (!leverChanged)
                return;

            StartMatchLever startMatchLever = Object.FindObjectOfType<StartMatchLever>();
            if (startMatchLever == null)
                return;

            startMatchLever.triggerScript.timeToHoldSpeedMultiplier = 1;
            startMatchLever.triggerScript.holdTip = "";
            leverChanged = false;
        }

        internal static void MessWithShipDoors()
        {
            if (activeShipDoorsEvent)
                return;

            Plugin.MoreLogs("Mess with ship doors called");
            StartOfRound.Instance.StartCoroutine(MessWithShipDoorsFunc());
        }

        internal static void MessWithShipLights()
        {
            if (activeShipLightsEvent)
                return;

            Plugin.MoreLogs("Mess with ship lights called");
            StartOfRound.Instance.StartCoroutine(MessWithShipLightsFunc());
        }

        internal static IEnumerator MessWithShipDoorsFunc()
        {
            activeShipDoorsEvent = true;
            int count = NumberStuff.GetInt(1, 10);

            for (int i = 0; i < count; i++)
            {
                ToggleShipDoors();
                float time = NumberStuff.GetFloat(0.4f, 3f);
                yield return new WaitForSeconds(time);
            }

            activeShipDoorsEvent = false;
        }

        internal static IEnumerator MessWithShipLightsFunc()
        {
            activeShipLightsEvent = true;
            int count = NumberStuff.GetInt(1, 10);

            for (int i = 0; i < count; i++)
            {
                ToggleShipLights();
                float time = NumberStuff.GetFloat(0.4f, 3f);
                yield return new WaitForSeconds(time);
            }

            activeShipLightsEvent = false;
        }

        internal static void HauntedOrderFunc()
        {
            if (Plugin.instance.Terminal.orderedItemsFromTerminal.Count > 0 || Plugin.instance.Terminal.vehicleInDropship || Plugin.instance.Terminal.numberOfItemsInDropship > 0 || HasDropShipLanded())
                return;

            int itemType;
            int itemCount = Rand.Next(1, 4);
            List<int> newItemsOrdered = [];

            if (Rand.Next(101) >= 70) //make actual items rare
            {
                for (int i = 0; i < itemCount; i++)
                {
                    itemType = Rand.Next(Plugin.instance.Terminal.buyableItemsList.Length);
                    newItemsOrdered.Add(itemType);
                }
            }

            Plugin.instance.Terminal.orderedItemsFromTerminal = newItemsOrdered;
            Plugin.instance.Terminal.PlayTerminalAudioServerRpc(0);

            ItemDropship itemDropship = Object.FindObjectOfType<ItemDropship>();

            if (itemDropship == null)
                return;

            if (!itemDropship.shipLanded && StartOfRound.Instance.shipHasLanded)
                itemDropship.LandShipOnServer();
        }

        internal static bool HasDropShipLanded()
        {
            ItemDropship itemDropship = Object.FindObjectOfType<ItemDropship>();

            if (itemDropship == null)
                return false;

            return itemDropship.shipLanded;
        }

    }
}
