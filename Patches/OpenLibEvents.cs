using ghostCodes.Compatibility;
using ghostCodes.Configs;
using static OpenLib.Common.StartGame;

namespace ghostCodes.Patches
{
    public class OpenLibEvents
    {
        public static OpenLib.Events.Events.CustomEvent GhostCodesReset = new();
        public static OpenLib.Events.Events.CustomEvent ApparatusPull = new();

        internal static void Subscribers()
        {
            OpenLib.Events.EventManager.TerminalAwake.AddListener(OnTerminalAwake);
            OpenLib.Events.EventManager.GameNetworkManagerStart.AddListener(OnGameStart);
            OpenLib.Events.EventManager.StartOfRoundAwake.AddListener(StartOfRoundAwake);
            GhostCodesReset.AddListener(InitPlugin.BaseReset);
            ApparatusPull.AddListener(AppPull);
        }

        internal static void StartOfRoundAwake()
        {
            StartOfRound.Instance.StartNewRoundEvent.AddListener(InitPlugin.StartTheRound);
            NetObject.SpawnNetworkHandler();
        }

        internal static void AppPull()
        {
            if (!SetupConfig.GhostCodesSettings.HauntingsMode && !Bools.endAllCodes)
                Bools.endAllCodes = true;
        }

        internal static void GetInvoke()
        {
            if (Bools.appPullInvoked)
                return;

            Bools.appPullInvoked = true;
            Plugin.Spam("Apparatus pull detected");
            ApparatusPull.Invoke();
        }

        internal static void OnTerminalAwake(Terminal instance)
        {
            Plugin.instance.Terminal = instance;
        }

        internal static void OnGameStart()
        {
            if (SoftCompatibility("me.loaforc.facilitymeltdown", ref Plugin.instance.FacilityMeltdown))
            {
                Plugin.MoreLogs("Facility Meltdown mod detected!");
            }
            if (SoftCompatibility("com.github.zehsteam.ToilHead", ref Plugin.instance.ToilHead))
            {
                Plugin.MoreLogs("ToilHeads mod detected!");
            }

            NetObject.Init();
            SoundSystem.LoadCustomSounds();
            LethalConfigCheck();
        }

        private static void LethalConfigCheck()
        {
            Plugin.Spam("LethalConfigCheck");
            if (OpenLib.Plugin.instance.LethalConfig)
            {
                Plugin.MoreLogs("Lethalconfig stuff enabled!");
                LethalConfigStuff.AddLoadCodeButton();
                LethalConfigStuff.QueueAndLoad(Configs.ModConfig.configFiles);
            }
        }
    }
}
