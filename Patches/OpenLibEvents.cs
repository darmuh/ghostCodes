using ghostCodes.Compatibility;
using ghostCodes.Configs;
using static OpenLib.Common.StartGame;
using static ghostCodes.Interactions.DeathNote;

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

            OpenLib.TerminalUpdatePatch.usePatch = true;
            OpenLib.Events.EventManager.TerminalKeyPressed.AddListener(OnTerminalKeyPress);
        }

        internal static void StartOfRoundAwake()
        {
            StartOfRound.Instance.StartNewRoundEvent.AddListener(InitPlugin.StartTheRound);
            NetObject.SpawnNetworkHandler();
        }

        internal static void OnTerminalKeyPress()
        {
            if (!Plugin.instance.Terminal.terminalInUse)
                return;

            if (inDeathNote)
                HandleInput();
        }

        internal static void GetInvoke() //needed for patch
        {
            if (Bools.appPullInvoked)
                return;

            Bools.appPullInvoked = true;
            Plugin.Spam("Apparatus pull detected");
            ApparatusPull.Invoke();
        }

        internal static void AppPull()
        {
            if (!SetupConfig.GhostCodesSettings.HauntingsMode && !Bools.endAllCodes)
                Bools.endAllCodes = true;
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
            if (SoftCompatibility("darmuh.TerminalStuff", ref Plugin.instance.TerminalStuff))
                Plugin.MoreLogs("TerminalStuf compatibility added!");

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
