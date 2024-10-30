using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ghostCodes.RapidFire;
using static ghostCodes.Bools;
using BepInEx.Configuration;
using GameNetcodeStuff;

namespace ghostCodes
{
    internal class InsanityStuff
    {
        internal static Dictionary<string, ConfigEntry<int>> stageKeyVal = new Dictionary<string, ConfigEntry<int>>();
        internal static Stages stage1 = SetStageInfo("Stage 1", 0, 6, ModConfig.saS1percent, ModConfig.saS1inside, ModConfig.saS1outside);
        internal static Stages stage2 = SetStageInfo("Stage 2", 7, 10, ModConfig.saS2percent, ModConfig.saS2inside, ModConfig.saS2outside);
        internal static Stages stage3 = SetStageInfo("Stage 3", 11, 15, ModConfig.saS3percent, ModConfig.saS3inside, ModConfig.saS3outside);
        internal static List<Stages> allStages = new List<Stages>();

        public static void ApplyInsanityMode(StartOfRound instance, ref float randomWaitNum)
        {
            GetAllSanity();
            float groupSanity = Plugin.instance.groupSanity;
            float maxSanity = Plugin.instance.maxSanity;

            if (startRapidFire && ModConfig.insanityRapidFire.Value)
            {
                Plugin.GC.LogInfo("max insanity level reached!!! startRapidFire TRUE");
                instance.StartCoroutine(Coroutines.RapidFireStart(instance));
                return;
            }
            else
            {
                AdjustWaitNum(groupSanity, maxSanity, ref randomWaitNum);
                Plugin.MoreLogs($"waiting {randomWaitNum}");
            }
        }

        internal static void InitsoloAssist()
        {
            allStages.Clear();
            allStages.Add(stage1);
            allStages.Add(stage2);
            allStages.Add(stage3);
        }

        internal static void GetAllSanity()
        {
            Plugin.instance.groupSanity = 0f;
            Plugin.instance.maxSanity = 0f;

            // Iterate through all players
            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Count(); i++)
            {
                if (!StartOfRound.Instance.allPlayerScripts[i].isPlayerDead && StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled)
                {
                    Plugin.instance.groupSanity += StartOfRound.Instance.allPlayerScripts[i].insanityLevel;
                    Plugin.instance.maxSanity += StartOfRound.Instance.allPlayerScripts[i].maxInsanityLevel;
                }
            }

            float groupSanity = Plugin.instance.groupSanity;
            ApplyBonuses(ref groupSanity, Plugin.instance.maxSanity);
            Plugin.instance.groupSanity = groupSanity;
            CheckSanityRapidFire();
        }

        private static void CheckSanityRapidFire()
        {
            if (!ModConfig.insanityRapidFire.Value)
                return;

            if (Mathf.Round(Plugin.instance.groupSanity) >= Mathf.Round(Plugin.instance.maxSanity) * (ModConfig.sanityPercentMAX.Value / 100f))
            {
                startRapidFire = true;
                Plugin.MoreLogs("max sanity hit, CheckSanityRapidFire()");
                Plugin.MoreLogs($"Group Sanity Level: {Mathf.Round(Plugin.instance.groupSanity)}");
                Plugin.MoreLogs($"Group Max Insanity level: {Mathf.Round(Plugin.instance.maxSanity)}");
            }
            else
            {
                startRapidFire = false;
                Plugin.MoreLogs($"Group Sanity Level: {Mathf.Round(Plugin.instance.groupSanity)}");
                Plugin.MoreLogs($"Group Max Insanity level: {Mathf.Round(Plugin.instance.maxSanity)}");
            }
        }

        private static float AdjustWaitNum(float groupSanity, float maxSanity, ref float randomWaitNum)
        {
            if (!ModConfig.insanityRapidFire.Value && Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (ModConfig.sanityPercentMAX.Value/100f))
            {
                randomWaitNum *= ModConfig.waitPercentMAX.Value / 100f;
                Plugin.MoreLogs("Max Insanity Level reached (rapidFire disabled)");
                return randomWaitNum;
            }
            else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (ModConfig.sanityPercentL3.Value / 100f))
            {
                randomWaitNum *= ModConfig.waitPercentL3.Value / 100f;
                //Plugin.GC.LogInfo($"(ApplyInsanityMode)Group Max Insanity level: {Mathf.Round(groupSanity)} >= {Mathf.Round(maxSanity) * sPercentL3} ");
                Plugin.MoreLogs("insanity level 3 reached");
                return randomWaitNum;
            }
            else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (ModConfig.sanityPercentL2.Value / 100f))
            {
                randomWaitNum *= ModConfig.waitPercentL2.Value / 100f;
                Plugin.MoreLogs("insanity level 2 reached");
                return randomWaitNum;
            }
            else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (ModConfig.sanityPercentL1.Value / 100f))
            {
                randomWaitNum *= ModConfig.waitPercentL1.Value / 100f;
                Plugin.MoreLogs("insanity level 1 reached");
                return randomWaitNum;
            }
            else
            {
                Plugin.MoreLogs("insanity levels low");
                return randomWaitNum;
            }
        }

        // Start of Insanity Bonus Handling Logic
        private static Tuple<float, float> ApplyBonuses(ref float groupSanity, float maxSanity)
        {
            Plugin.MoreLogs("Applying bonuses & debuffs");
            // Apply solo assist bonus if conditions are met
            if (ModConfig.soloAssist.Value && Plugin.instance.playersAtStart == 1)
            {
                Plugin.MoreLogs("SoloAssist Activated");
                groupSanity = SoloAssist(ref groupSanity, maxSanity);
            }

            // Apply death bonus if conditions are met
            if (ModConfig.deathBonus.Value)
            {
                Plugin.MoreLogs("Death Bonus Activated");
                groupSanity = ApplyDeathBonus(ref groupSanity, maxSanity);
            }

            // Apply ghost girl bonus if conditions are met
            if (ModConfig.ggBonus.Value && Plugin.instance.DressGirl != null)
            {
                Plugin.MoreLogs("Ghost Girl Bonus Activated");
                groupSanity = ApplyGirlBonus(ref groupSanity, maxSanity);
            }

            if(ModConfig.emoteBuff.Value)
            {
                Plugin.MoreLogs("Emote Buff Activated");
                groupSanity = EmoteBuff(ref groupSanity);
            }

            // Return the updated groupSanity and maxSanity
            return Tuple.Create(groupSanity, maxSanity);
        }

        // Method to apply the death bonus
        private static float ApplyDeathBonus(ref float groupSanity, float maxSanity)
        {
            int deadPlayers = Plugin.instance.players.Count(player => player.isPlayerDead);

            for (int i = 0; i < deadPlayers; i++)
            {
                float bonusNum = groupSanity * (ModConfig.deathBonusPercent.Value / 100f);
                Plugin.MoreLogs($"Dead Player detected. Adding {bonusNum} to group sanity");
                groupSanity = ApplyBonus(groupSanity, maxSanity, bonusNum, (_) => true);
            }

            Plugin.MoreLogs("Death bonus applied to insanity values");
            return groupSanity;
        }

        // Method to apply the ghost girl bonus
        private static float ApplyGirlBonus(ref float groupSanity, float maxSanity)
        {
            Plugin.MoreLogs("Ghost Girl Bonus applied to insanity values");
            float bonusNum = groupSanity * (ModConfig.ggBonusPercent.Value / 100f);
            Plugin.MoreLogs($"Ghost Girl Bonus adding {bonusNum} to group sanity");
            groupSanity = ApplyBonus(groupSanity, maxSanity, bonusNum, (_) => true);
            return groupSanity;
        }

        // Method to apply bonus if condition is true
        private static float ApplyBonus(float groupSanity, float maxSanity, float bonusValue, Func<float, bool> condition)
        {
            if (condition(groupSanity) && maxSanity >= (groupSanity + bonusValue))
            {
                groupSanity += bonusValue;
            }
            return groupSanity;
        }

        private static Stages SetStageInfo(string name, int startHour, int endHour, ConfigEntry<int> percentKey, ConfigEntry<int> insideKey, ConfigEntry<int> outsideKey)
        {
            Stages stage = new Stages
            {
                Name = name,
                StartHour = startHour,
                EndHour = endHour,
                PercentKey = percentKey,
                InsideKey = insideKey,
                OutsideKey = outsideKey,
            };
            return stage;
        }

        private static float EmoteBuff(ref float groupSanity)
        {
            if(ModConfig.emoteBuff.Value)
            {
                foreach(PlayerControllerB player in Plugin.instance.players)
                {
                    if (!player.isPlayerDead && player.performingEmote)
                    {
                        float buffNum = groupSanity * (ModConfig.emoteBuffPercent.Value / 100f);
                        Plugin.MoreLogs($"Emote buff removing {buffNum} from group sanity");
                        ApplySanityDebuff(ref groupSanity, buffNum);
                    }
                }
            }

            return groupSanity;
        }

        private static float SoloAssist(ref float groupSanity, float maxSanity)
        {
            if (ModConfig.soloAssist.Value && Plugin.instance.playersAtStart == 1)
            {
                int hour = TimeOfDay.Instance.hour;
                Plugin.GC.LogInfo($"Hour: {hour}");

                foreach (Stages stage in allStages)
                {
                    if (IsHourInRange(hour, stage.StartHour, stage.EndHour))
                    {
                        float threshold = maxSanity * (stage.PercentKey.Value / 100f);
                        bool isInsideFactory = IsInsideFactory();
                        float debuffAmount = isInsideFactory ? stage.InsideKey.Value : stage.OutsideKey.Value;

                        Plugin.MoreLogs($"threshold: {threshold}, debuffAmount: {debuffAmount}, groupSanity: {groupSanity}");

                        if (groupSanity >= threshold)
                        {
                            ApplySanityDebuff(ref groupSanity, debuffAmount);
                            Plugin.MoreLogs($"{stage.Name} detected and debuffs applied");
                        }
                        else
                        {
                            Plugin.MoreLogs($"No solo assist buff added.");
                        }

                        break; // Exit loop after finding the appropriate stage
                    }
                }
            }

            return groupSanity;
        }

        // Helper method to apply sanity debuff
        private static void ApplySanityDebuff(ref float groupSanity, float debuffAmount)
        {
            if (groupSanity > debuffAmount)
            {
                groupSanity -= debuffAmount;
                Plugin.GC.LogInfo($"Subtracted {debuffAmount} from Group Sanity Level.");
            }
            else
            {
                groupSanity = 0;
                Plugin.GC.LogInfo($"Subtracted {debuffAmount} from Group Sanity Level which set value to 0.");
            }
        }


    }

    internal class Stages
    {
        internal string Name { get; set; }
        internal int StartHour { get; set; }
        internal int EndHour { get; set; }
        internal ConfigEntry<int> PercentKey { get; set; }
        internal ConfigEntry<int> InsideKey { get; set; }
        internal ConfigEntry<int> OutsideKey { get; set; }
    }
}
