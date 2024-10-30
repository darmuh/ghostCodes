using GameNetcodeStuff;
using ghostCodes.Configs;
using UnityEngine;

namespace ghostCodes
{
    internal class DressGirl : EnemyAI
    {
        internal static bool girlIsChasing = false;

        internal static void InitDressGirl()
        {
            Plugin.MoreLogs("Resetting DressGirl Variables...");
            girlIsChasing = false;
            Plugin.instance.DressGirl = null;
        }

        internal static void ChangeHauntedPlayer(PlayerControllerB player)
        {
            Plugin.instance.DressGirl.hauntingPlayer = player;
            Plugin.instance.DressGirl.ChangeOwnershipOfEnemy(Plugin.instance.DressGirl.hauntingPlayer.actualClientId);
            Plugin.instance.DressGirl.hauntingLocalPlayer = GameNetworkManager.Instance.localPlayerController == Plugin.instance.DressGirl.hauntingPlayer;
        }

        internal static void GGrapidFireChecks()
        {
            if (Plugin.instance.DressGirl == null || !SetupConfig.GhostCodesSettings.HauntingsMode)
                return;

            Plugin.MoreLogs("Checking for rapidFire ending events");
            DancingCheck();
            ShowerStuff.CheckForHauntedPlayerInShower();
        }


        private static void DancingCheck()
        {
            if (InteractionsConfig.EmoteStopChasing.Value > 1 || !girlIsChasing)
                return;

            if (!Bools.IsThisEffective(InteractionsConfig.EmoteStopChasing.Value))
            {
                Plugin.MoreLogs("Ghost girl doesn't want to see you dance!");
                return;
            }

            int playersEmoting = NumberStuff.GetNumberPlayersEmoting();
            int alivePlayers = NumberStuff.GetAlivePlayers();

            if (Bools.AreEnoughPlayersDancing(alivePlayers, playersEmoting, InteractionsConfig.EmoteStopChaseRequiredPlayers.Value))
            {
                Plugin.MoreLogs("Enough players emoting, stopping chase");
                StopGirlFromChasing();
                return;
            }

            Plugin.MoreLogs("You might wanna keep running...");

        }

        internal static void StopGirlFromChasing()
        {
            if (Plugin.instance.DressGirl == null || !Plugin.instance.DressGirl.hauntingLocalPlayer)
                return;

            Plugin.instance.DressGirl.StopChasing();
            Plugin.MoreLogs("initiating stop chase.");
        }

        internal static void GirlDisappear()
        {
            if (Plugin.instance.DressGirl == null || !Plugin.instance.DressGirl.hauntingLocalPlayer)
                return;

            Plugin.instance.DressGirl.DisappearDuringHaunt();
        }

        internal static void EndAllGirlStuff()
        {
            if (girlIsChasing)
            {
                StopGirlFromChasing();
            }
            if (Plugin.instance.DressGirl.staringInHaunt)
            {
                GirlDisappear();
            }

        }

        internal static void BreakersFix(DressGirlAI instance)
        {
            if (!SetupConfig.FixGhostGirlBreakers.Value || instance.currentBehaviourStateIndex == 1 || instance.timesChased != 1)
                return;

            if (Random.Range(0, 100) < SetupConfig.VanillaBreakersChance.Value)
            {
                instance.FlipLightsBreakerServerRpc();
                Plugin.GC.LogInfo("patched updated breaker chance for vanilla");
            }
            else
            {
                return;
            }
        }
    }
}
