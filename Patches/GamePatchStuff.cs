using ghostCodes.Configs;
using ghostCodes.Patches;
using HarmonyLib;
using System.Collections.Generic;
using static ghostCodes.Bools;

namespace ghostCodes
{
    public class GamePatchStuff
    {

        [HarmonyPatch(typeof(StartOfRound), "PassTimeToNextDay")]
        public class NextDayPatch
        {
            static void Postfix()
            {
                Plugin.Spam("Next day detected!");
                OpenLibEvents.GhostCodesReset.Invoke();
            }
        }

        [HarmonyPatch(typeof(TerminalAccessibleObject), "Start")]
        public class TerminalAccessibleObject_StartPatch
        {
            static void Postfix(TerminalAccessibleObject __instance)
            {
                CodeStuff.NewUsableCode(__instance);
            }
        }

        [HarmonyPatch(typeof(TerminalAccessibleObject), "OnDestroy")]
        public class TerminalAccessibleObject_DestroyPatch
        {
            static void Postfix(TerminalAccessibleObject __instance)
            {
                CodeStuff.RemoveUsableCode(__instance);
            }
        }

        [HarmonyPatch(typeof(LungProp), "DisconnectFromMachinery", MethodType.Enumerator)]
        public class ApparatusPullPatch
        {
            static bool instructionAdded = false;

            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> ApparatusPullPatch_Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                CodeInstruction myMethod = CodeInstruction.Call("ghostCodes.Patches.OpenLibEvents:GetInvoke");

                foreach (CodeInstruction instruction in instructions)
                {
                    if (!instructionAdded)
                    {
                        Plugin.GC.LogInfo("Custom Instruction added in DisconnectFromMachinery enumerator");
                        yield return myMethod;
                        yield return instruction;
                        instructionAdded = true;
                    }
                    else
                        yield return instruction;
                }

                Plugin.GC.LogInfo("Transpiler Success! - Added ApparatusPull Invoke Event");
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "Start")]
        public class DressGirlExists : DressGirlAI //Start
        {
            static void Postfix(DressGirlAI __instance)
            {
                Plugin.instance.DressGirl = __instance;
                Plugin.MoreLogs("Dressgirl instance detected.");
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "SetHauntStarePosition")]
        public class StaringInHauntPatch : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                    return;

                if (__instance == null)
                {
                    Plugin.GC.LogError("Failed to grab dressgirl instance");
                    return;
                }

                if (!__instance.staringInHaunt)
                    return;

                if (CanSendCodes() && DressGirlStartCodes())
                {
                    Plugin.instance.DressGirl = __instance;
                    Plugin.GC.LogInfo("The ghostGirl wants to play!");
                    Plugin.instance.DressGirl.StartCoroutine(Coroutines.GhostGirlEnhanced(StartOfRound.Instance));
                }
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "FlipLightsBreakerServerRpc")]
        public class DressGirlRPCFix : DressGirlAI
        {
            static void Postfix(DressGirlAI __instance)
            {
                if (SetupConfig.FixGhostGirlBreakers.Value)
                {
                    Plugin.MoreLogs("Fixing lightbreaker Rpc");
                    __instance.FlipLightsBreakerClientRpc();
                }
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "BeginChasing")]
        public class TheChaseBegins
        {
            static void Postfix(DressGirlAI __instance)
            {

                DressGirl.BreakersFix(__instance);

                if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                    return;

                RapidFire.startRapidFire = true;
                DressGirl.girlIsChasing = true;
                Plugin.MoreLogs("Girl has begun chasing, setting rapidFire to TRUE");
                StartOfRound.Instance.StartCoroutine(Coroutines.RapidFireStart(StartOfRound.Instance));
            }
        }

        [HarmonyPatch(typeof(DressGirlAI), "StopChasing")]
        public class TheChaseEnds : DressGirlAI
        {
            static void Postfix()
            {
                if (SetupConfig.GhostCodesSettings.CurrentMode.ToLower() != "hauntings")
                    return;

                RapidFire.startRapidFire = false;
                DressGirl.girlIsChasing = false;
                Plugin.MoreLogs("Chase sequence has ended.");
            }
        }

        [HarmonyPatch(typeof(VehicleController), "Awake")]
        public class GetVehicleController //Get Instance
        {
            static void Postfix(VehicleController __instance)
            {
                Plugin.instance.Cruiser = __instance;
                Plugin.MoreLogs("Cruiser instance detected.");
            }
        }
    }
}
