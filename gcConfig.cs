using BepInEx.Configuration;
using System.Configuration;

namespace ghostCodes
{
    public static class gcConfig
    {
        //establish config options here
        public static ConfigEntry<bool> gcRandomIntervals; //Toggle whether the intervals between codes are random or set intervals
        public static ConfigEntry<bool> gcEnableTerminalSound; //Toggle whether the terminal makes a sound when a ghost code is submitted
        public static ConfigEntry<int> gcFirstSetInterval; //First wait configuration
        public static ConfigEntry<int> gcSecondSetInterval; //Second wait config
        public static ConfigEntry<int> gcSetIntervalAC; //last wait config after code was sent
        public static ConfigEntry<int> gcFirstRandIntervalMin; //First wait configuration
        public static ConfigEntry<int> gcFirstRandIntervalMax; //First wait configuration
        public static ConfigEntry<int> gcSecondRandIntervalMin; //Second wait config
        public static ConfigEntry<int> gcSecondRandIntervalMax; //Second wait config
        public static ConfigEntry<int> gcRandIntervalACMin; //last wait config after code was sent
        public static ConfigEntry<int> gcRandIntervalACMax; //last wait config after code was sent
        public static ConfigEntry<int> gcMaxCodes; //Maximum amount of codes to send in one round

        public static void SetConfigSettings()
        {

            

            //Enable or Disable random intervals
            gcConfig.gcRandomIntervals = Plugin.instance.Config.Bind<bool>("General", "gcRandomIntervals", true, "Disable this to use the 'set' intervals");
            gcConfig.gcEnableTerminalSound = Plugin.instance.Config.Bind<bool>("General", "gcEnableTerminalSound", true, "This setting determines whether the terminal plays the 'alarm' sound when a ghost code is entered.");

            //Max Codes per round
            gcConfig.gcMaxCodes = Plugin.instance.Config.Bind<int>("General", "gcMaxCodes", 15, "This is the maximum amount of ghost codes that CAN be sent in one round, a random number of codes will be chosen with this value set as the maximum");

            //set interval
            gcConfig.gcFirstSetInterval = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcFirstRandInterval", 90, "This is the initial time it takes to start loading codes after landing the ship");
            gcConfig.gcSecondSetInterval = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcSecondRandInterval", 30, "This is the time it takes to load the code once it has been picked");
            gcConfig.gcSetIntervalAC = Plugin.instance.Config.Bind<int>("Set Interval Configurations", "gcSetIntervalAC", 180, "This is the time it waits to pick another code");
            
            //random interval
            gcConfig.gcFirstRandIntervalMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcFirstRandIntervalMin", 15, "This is the minimum time it should take to start loading codes after landing the ship");
            gcConfig.gcSecondRandIntervalMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcSecondRandIntervalMin", 5, "This is the minimum time it should take to load the code once it has been picked");
            gcConfig.gcRandIntervalACMin = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcRandIntervalACMin", 20, "This is the minimum time it should wait to pick another code");
            gcConfig.gcFirstRandIntervalMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcFirstRandIntervalMax", 120, "This is the maximum time it should take to start loading codes after landing the ship");
            gcConfig.gcSecondRandIntervalMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcSecondRandIntervalMax", 60, "This is the maximum time it should take to load the code once it has been picked");
            gcConfig.gcRandIntervalACMax = Plugin.instance.Config.Bind<int>("Random Interval Configurations", "gcRandIntervalACMax", 240, "This is the maximum time it should wait to pick another code");

            Plugin.GC.LogInfo("ghostCodes config initialized");
        }
    }
}
