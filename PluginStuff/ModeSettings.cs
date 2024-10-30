using ghostCodes.Configs;
using System.Collections.Generic;
using System.Linq;

namespace ghostCodes.PluginStuff
{
    public class ModeSettings
    {
        internal string CurrentMode = "";
        internal string PrimaryMode = "";
        internal string SecondaryMode = "";

        internal bool hasSecondaryMode = false;

        internal bool HauntingsMode;
        internal bool IntervalsMode;
        internal bool InsanityMode;
        internal List<string> SecondaryLevels;

        internal ModeSettings(string primaryMode, string secondaryMode, string secondaryLevelList)
        {
            Plugin.Spam("Creating ModeSettings!");

            PrimaryMode = primaryMode;
            UpdateToMode(primaryMode);

            if (secondaryMode.ToLower() == "none" || secondaryLevelList.Length <= 0)
            {
                SecondaryMode = "";
                hasSecondaryMode = false;
                return;
            }


            SecondaryMode = secondaryMode;
            hasSecondaryMode = true;
            SecondaryLevels = OpenLib.Common.CommonStringStuff.GetKeywordsPerConfigItem(secondaryLevelList, ',');

        }

        internal void ConfigUpdate(string primaryMode, string secondaryMode, string secondaryLevelList)
        {
            Plugin.Spam("ConfigUpdate for ModeSettings detected!");
            PrimaryMode = primaryMode;

            if (secondaryMode.ToLower() == "none" || secondaryLevelList.Length <= 0)
            {
                if (CurrentMode != PrimaryMode)
                {
                    Plugin.Spam($"Updating to primary mode! [ {PrimaryMode} ]");
                    UpdateToMode(PrimaryMode);
                    InteractionsConfig.MapToConfig(InteractionsConfig.PrimaryInteractions, InteractionsConfig.Settings);
                }

                SecondaryMode = "";
                hasSecondaryMode = false;
                return;
            }

            SecondaryMode = secondaryMode;
            hasSecondaryMode = true;
            SecondaryLevels = OpenLib.Common.CommonStringStuff.GetKeywordsPerConfigItem(secondaryLevelList, ',');
        }

        internal void UpdateSettings(SelectableLevel level)
        {
            if (!hasSecondaryMode)
                return;

            string numberlessName = new(level.PlanetName.SkipWhile(c => !char.IsLetter(c)).ToArray());

            if (SecondaryLevels.Count < 1)
                return;

            if (SecondaryLevels.Any(x => level.PlanetName.ToLower().Contains(x.ToLower())))
            {
                Plugin.Spam($"Updating to secondary mode! [ {SecondaryMode} ]");
                UpdateToMode(SecondaryMode);
                InteractionsConfig.MapToConfig(InteractionsConfig.SecondaryInteractions, InteractionsConfig.Settings);
            }
            else
            {
                if (CurrentMode != PrimaryMode)
                {
                    Plugin.Spam($"Updating to primary mode! [ {PrimaryMode} ]");
                    UpdateToMode(PrimaryMode);
                    InteractionsConfig.MapToConfig(InteractionsConfig.PrimaryInteractions, InteractionsConfig.Settings);
                }
            }
        }

        internal void UpdateToMode(string modeName)
        {
            Plugin.Spam($"UpdateToMode - {modeName}");
            CurrentMode = modeName;

            if (modeName.ToLower() == "hauntings")
            {
                HauntingsMode = true;
                IntervalsMode = false;
                InsanityMode = false;
            }
            else if (modeName.ToLower() == "intervals")
            {
                HauntingsMode = false;
                IntervalsMode = true;
                InsanityMode = false;
            }
            else if (modeName.ToLower() == "insanity")
            {
                HauntingsMode = false;
                IntervalsMode = true;
                InsanityMode = true;
            }
        }
    }
}
