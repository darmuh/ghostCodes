using ghostCodes.Configs;
using static ghostCodes.Bools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using GameNetcodeStuff;

namespace ghostCodes
{
    internal class Lights
    {
        internal static bool activeLightsColor = false;
        internal static bool warningLights = false;
        internal static List<Light> allLightsFixed = [];
        internal static List<Animator> allPoweredLights = [];
        internal static bool activeFlicker = false;

        internal static void InitLights()
        {
            activeLightsColor = false;
            warningLights = false;
            activeFlicker = false;
            allLightsFixed = [];
            allPoweredLights = [];

            if (RoundManager.Instance.allPoweredLightsAnimators.Count == 0)
            {
                Plugin.WARNING("INTERIOR HAS NO POWERED LIGHT ANIMATORS!!!");
                Plugin.WARNING("INTERIOR HAS NO POWERED LIGHT ANIMATORS!!!");
                Plugin.WARNING("INTERIOR HAS NO POWERED LIGHT ANIMATORS!!!");
                Plugin.WARNING("INTERIOR HAS NO POWERED LIGHT ANIMATORS!!!");
                GameObject dungeonRoot = GameObject.Find("Systems/LevelGeneration");
                if (dungeonRoot != null)
                {
                    allLightsFixed = [.. dungeonRoot.GetComponentsInChildren<Light>(includeInactive: true)];
                    Plugin.Spam($"Got [ {allLightsFixed.Count} ] lights from dungeon root!");
                }
                else
                    Plugin.WARNING("Unable to get dungeonRoot lights!");
            }

            foreach (Animator animator in RoundManager.Instance.allPoweredLightsAnimators)
            {
                if(animator.GetComponentInChildren<Light>(includeInactive: true) != null)
                    allPoweredLights.Add(animator);

                List<Light> animatorLights = [.. animator.GetComponentsInChildren<Light>(includeInactive: true)];
                allLightsFixed.AddRange(animatorLights);
            }
        }

        internal static void GGFlashlight()
        {
            NetHandler.Instance.FlickerLightsServerRpc(true, false);
            Plugin.MoreLogs("nosfaratu is that you??");
        }

        internal static void FlipLights()
        {
            Plugin.MoreLogs("who turned out the lights??");
            NetHandler.Instance.FacilityBreakerServerRpc();
        }

        internal static IEnumerator ColorTransition(Light light, Color newColor, float duration)
        {
            Color init = light.color;
            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float t = Mathf.Clamp01(counter / duration);
                light.color = Color.Lerp(init, newColor, t);
                yield return null;
            }
        }

        internal static IEnumerator PoweredLightsColor(PlayerControllerB HauntedPlayer) //Color
        {
            if (activeLightsColor)
                yield break;

            if (SetupConfig.RapidLightsColorValue.Value.ToLower() == "nochange" || SetupConfig.RapidLightsColorValue.Value.ToLower() == "default" || SetupConfig.RapidLightsColorValue.Value.Length == 0)
                yield break;

            if (allLightsFixed.Count == 0)
            {
                Plugin.WARNING("NO LIGHTS DETECTED!");
                Plugin.WARNING("NO LIGHTS DETECTED!");
                Plugin.WARNING("NO LIGHTS DETECTED!");
                yield break;
            }

            activeLightsColor = true;
            Plugin.Spam($"allLightsFixed Count: {allLightsFixed.Count}");
            Dictionary<Light, Color> allLightsColors = [];

            Color32 configColor = OpenLib.Common.Misc.HexToColor(SetupConfig.RapidLightsColorValue.Value);
            for (int i = 0; i < allLightsFixed.Count; i++)
            {
                if(!allLightsColors.ContainsKey(allLightsFixed[i]))
                    allLightsColors.Add(allLightsFixed[i], allLightsFixed[i].color);

                StartOfRound.Instance.StartCoroutine(ColorTransition(allLightsFixed[i], configColor, 5f));
            }

            warningLights = true;
            Plugin.MoreLogs("Alarm lights set");

            while (warningLights && PlayerStillAliveAndControlled(HauntedPlayer))
            {
                yield return new WaitForEndOfFrame();
            }

            if(warningLights)
                warningLights = false;

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < allLightsFixed.Count; i++)
            {
                if (allLightsColors.TryGetValue(allLightsFixed[i], out Color defaultColor))
                    StartOfRound.Instance.StartCoroutine(ColorTransition(allLightsFixed[i], defaultColor, 3f));
                else
                    Plugin.WARNING("Unable to get original light color!");
            }

            Plugin.MoreLogs("Lights set back to normal");
            activeLightsColor = false;
        }


        internal static IEnumerator WhileLightsFlicker()
        {
            if (!lightsFlickering && !warningLights)
                yield break;

            while (RapidFire.startRapidFire && !StartOfRound.Instance.localPlayerController.isPlayerDead && !endAllCodes)
            {
                yield return new WaitForEndOfFrame();
            }

            Plugin.Spam("End of LightsFlicker Coroutine!");
            NetHandler.Instance.EndRapidFireLightsServerRpc();
        }

        internal static IEnumerator FlickerPoweredLights(PlayerControllerB HauntedPlayer)
        {
            if (lightsFlickering)
                yield break;

            if(allPoweredLights.Count == 0)
            {
                Plugin.WARNING("No powered lights!");
                Plugin.WARNING("No powered lights!");
                Plugin.WARNING("No powered lights!");
                yield break;
            }

            lightsFlickering = true;
            Plugin.Spam("FlickerPoweredLights");

            while (lightsFlickering && PlayerStillAliveAndControlled(HauntedPlayer))
            {
                int count = 0;
                int maxLights;
                if (allPoweredLights.Count > 200)
                    maxLights = 85;
                else if (allPoweredLights.Count > 100)
                    maxLights = 50;
                else if (allPoweredLights.Count > 50)
                    maxLights = 15;
                else
                    maxLights = 9;

                foreach (Animator animator in allPoweredLights)
                {
                    if(count < maxLights)
                    {
                        animator.SetTrigger("Flicker");
                        count++;
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.05f);
                        count = 0;
                    }
                }
                yield return new WaitForSeconds(Random.Range(SetupConfig.RapidLightsMin.Value, SetupConfig.RapidLightsMax.Value));
            }

            if (lightsFlickering)
                lightsFlickering = false;

            Plugin.Spam("EndAlarmLights");
            yield return new WaitForSeconds(0.3f);
            RoundManager.Instance.SwitchPower(true);
            lightsFlickering = false;
        }
    }
}
