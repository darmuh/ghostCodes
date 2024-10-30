using ghostCodes.Configs;
using System.Linq;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class InitPlugin
    {
        internal static void StartTheRound()
        {
            if (StartOfRound.Instance.currentLevel.name == "CompanyBuildingLevel" || StartOfRound.Instance.currentLevel.riskLevel == "Safe")
                return;

            GetPlayersAtStart();
            InitMode();
            BaseReset();
            DressGirl.InitDressGirl();
            StartPlugin();
        }

        private static void StartPlugin()
        {
            if (ShouldInitPlugin())
                StartOfRound.Instance.StartCoroutine(Coroutines.InitEnumerator(StartOfRound.Instance));
        }

        internal static void RestartPlugin()
        {
            if (ShouldInitPlugin())
                StartOfRound.Instance.StartCoroutine(Coroutines.RestartEnum(StartOfRound.Instance));
        }

        internal static void InitMode()
        {
            Plugin.MoreLogs("Initializing ghostCodes mode settings");
            SetupConfig.GhostCodesSettings.UpdateSettings(StartOfRound.Instance.currentLevel);
        }

        internal static void BaseReset()
        {
            Plugin.instance.CodeSent = false;
            RapidFire.startRapidFire = false;
            RapidFire.meltdown = false;
            Coroutines.rapidFireStart = false;
            Coroutines.codeLooperStarted = false;
            Plugin.instance.MaxSanity = 0f;
            Plugin.instance.GroupSanity = 0f;
            Plugin.instance.playersAtStart = 0;
            Plugin.instance.CodeCount = 0;
            Plugin.instance.RandCodeAmount = 0;
            ShipStuff.ResetLever();
            TerminalAdditions.RestoreCreds();
        }

        internal static void CodesInit()
        {
            Plugin.MoreLogs("Initializing GhostCode core variables");
            endAllCodes = false;
            

            TerminalAdditions.ResetBools();
            CodeStuff.GetUsableCodes();
            Plugin.instance.RandCodeAmount = NumberStuff.GetInt(SetupConfig.MinCodes.Value, SetupConfig.MaxCodes.Value);

            if (SetupConfig.SoloAssist.Value)
                InsanityStuff.InitsoloAssist();
        }

        internal static void GetPlayersAtStart()
        {
            Plugin.instance.playersAtStart = 0;
            Plugin.instance.players = StartOfRound.Instance.allPlayerScripts;

            for (int i = 0; i < StartOfRound.Instance.allPlayerScripts.Count(); i++)
            {
                if (StartOfRound.Instance.allPlayerScripts[i].isPlayerControlled)
                    Plugin.instance.playersAtStart++;
            }

            Plugin.MoreLogs($"Players at Start: {Plugin.instance.playersAtStart}");

            if (Plugin.instance.playersAtStart > 1)
                Plugin.MoreLogs("SoloAssist Buff disabled, more than one player detected.");
        }


    }
}
