using BepInEx.Configuration;
using GameNetcodeStuff;
using ghostCodes.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ghostCodes.Bools;
using static ghostCodes.RapidFire;

namespace ghostCodes
{
    internal class InsanityStuff
    {
        internal static Dictionary<string, ConfigEntry<int>> stageKeyVal = [];
        internal static Stages stage1 = SetStageInfo("Stage 1", 0, 6, SoloAssistConfig.S1percent, SoloAssistConfig.S1inside, SoloAssistConfig.S1outside);
        internal static Stages stage2 = SetStageInfo("Stage 2", 7, 10, SoloAssistConfig.S2percent, SoloAssistConfig.S2inside, SoloAssistConfig.S2outside);
        internal static Stages stage3 = SetStageInfo("Stage 3", 11, 15, SoloAssistConfig.S3percent, SoloAssistConfig.S3inside, SoloAssistConfig.S3outside);
        internal static List<Stages> allStages = [];

        public static void ApplyInsanityMode(StartOfRound instance, ref float randomWaitNum)
        {
            GetAllSanity();
            float groupSanity = Plugin.instance.GroupSanity;
            float maxSanity = Plugin.instance.MaxSanity;

            if (startRapidFire && SetupConfig.RapidFireMaxHours.Value > 0)
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
            Plugin.instance.GroupSanity = 0f;
            Plugin.instance.MaxSanity = 0f;

            // Iterate through all players
            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Count(); i++)
            {
                if (!StartOfRound.Instance.allPlayerScripts[i].isPlayerDead && StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled)
                {
                    Plugin.instance.GroupSanity += StartOfRound.Instance.allPlayerScripts[i].insanityLevel;
                    Plugin.instance.MaxSanity += StartOfRound.Instance.allPlayerScripts[i].maxInsanityLevel;
                }
            }

            float groupSanity = Plugin.instance.GroupSanity;
            ApplyBonuses(ref groupSanity, Plugin.instance.MaxSanity);
            Plugin.instance.GroupSanity = groupSanity;
            CheckSanityRapidFire();
        }

        private static void CheckSanityRapidFire()
        {
            if (SetupConfig.RapidFireMaxHours.Value < 1)
                return;

            if (Mathf.Round(Plugin.instance.GroupSanity) >= Mathf.Round(Plugin.instance.MaxSanity) * (InsanityConfig.SanityMaxLevel.Value / 100f))
            {
                startRapidFire = true;
                Plugin.MoreLogs("max sanity hit, CheckSanityRapidFire()");
                Plugin.MoreLogs($"Group Sanity Level: {Mathf.Round(Plugin.instance.GroupSanity)}");
                Plugin.MoreLogs($"Group Max Insanity level: {Mathf.Round(Plugin.instance.MaxSanity)}");
            }
            else
            {
                startRapidFire = false;
                Plugin.MoreLogs($"Group Sanity Level: {Mathf.Round(Plugin.instance.GroupSanity)}");
                Plugin.MoreLogs($"Group Max Insanity level: {Mathf.Round(Plugin.instance.MaxSanity)}");
            }
        }

        private static float AdjustWaitNum(float groupSanity, float maxSanity, ref float randomWaitNum)
        {
            if (SetupConfig.RapidFireMaxHours.Value < 1 && Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (InsanityConfig.SanityMaxLevel.Value / 100f))
            {
                randomWaitNum *= InsanityConfig.WaitMaxLevel.Value / 100f;
                Plugin.MoreLogs("Max Insanity Level reached (rapidFire disabled)");
                return randomWaitNum;
            }
            else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (InsanityConfig.SanityLevel3.Value / 100f))
            {
                randomWaitNum *= InsanityConfig.WaitLevel3.Value / 100f;
                //Plugin.GC.LogInfo($"(ApplyInsanityMode)Group Max Insanity level: {Mathf.Round(GroupSanity)} >= {Mathf.Round(MaxSanity) * sPercentL3} ");
                Plugin.MoreLogs("insanity level 3 reached");
                return randomWaitNum;
            }
            else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (InsanityConfig.SanityLevel2.Value / 100f))
            {
                randomWaitNum *= InsanityConfig.WaitLevel2.Value / 100f;
                Plugin.MoreLogs("insanity level 2 reached");
                return randomWaitNum;
            }
            else if (Mathf.Round(groupSanity) >= Mathf.Round(maxSanity) * (InsanityConfig.SanityLevel1.Value / 100f))
            {
                randomWaitNum *= InsanityConfig.WaitLevel1.Value / 100f;
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
            Plugin.MoreLogs($"Applying bonuses & debuffs\nplayersAtStart = {Plugin.instance.playersAtStart}");
            // Apply solo assist bonus if conditions are met
            if (SetupConfig.SoloAssist.Value && Plugin.instance.playersAtStart == 1)
            {
                Plugin.MoreLogs("SoloAssist Activated");
                groupSanity = SoloAssist(ref groupSanity, maxSanity);
            }

            // Apply death bonus if conditions are met
            if (InsanityConfig.DeathBonus.Value > 0)
            {
                Plugin.MoreLogs("Death Bonus Activated");
                groupSanity = ApplyDeathBonus(ref groupSanity, maxSanity);
            }

            // Apply ghost girl bonus if conditions are met
            if (InsanityConfig.GhostGirlBonus.Value > 0 && Plugin.instance.DressGirl != null)
            {
                Plugin.MoreLogs("Ghost Girl Bonus Activated");
                groupSanity = ApplyGirlBonus(ref groupSanity, maxSanity);
            }

            if (InsanityConfig.EmoteBuff.Value > 1)
            {
                Plugin.MoreLogs("Emote Buff Activated");
                groupSanity = EmoteBuff(ref groupSanity);
            }

            // Return the updated GroupSanity and MaxSanity
            return Tuple.Create(groupSanity, maxSanity);
        }

        // Method to apply the death bonus
        private static float ApplyDeathBonus(ref float groupSanity, float maxSanity)
        {
            int deadPlayers = Plugin.instance.players.Count(player => player.isPlayerDead && player.isPlayerControlled);

            for (int i = 0; i < deadPlayers; i++)
            {
                float bonusNum = groupSanity * (InsanityConfig.DeathBonus.Value / 100f);
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
            float bonusNum = groupSanity * (InsanityConfig.GhostGirlBonus.Value / 100f);
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
            if (InsanityConfig.EmoteBuff.Value > 0)
            {
                foreach (PlayerControllerB player in Plugin.instance.players)
                {
                    if (!player.isPlayerDead && player.performingEmote)
                    {
                        float buffNum = groupSanity * (InsanityConfig.EmoteBuff.Value / 100f);
                        Plugin.MoreLogs($"Emote buff removing {buffNum} from group sanity");
                        ApplySanityDebuff(ref groupSanity, buffNum);
                    }
                }
            }

            return groupSanity;
        }

        private static float SoloAssist(ref float groupSanity, float maxSanity)
        {
            if (SetupConfig.SoloAssist.Value && Plugin.instance.playersAtStart == 1)
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

                        Plugin.MoreLogs($"threshold: {threshold}, debuffAmount: {debuffAmount}, GroupSanity: {groupSanity}");

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
