using UnityEngine;
using System.Linq;
using GameNetcodeStuff;

namespace ghostCodes
{
    internal class DressGirl : EnemyAI
    {
        internal static bool performingAction = false;
        internal static bool girlIsChasing = false;

        internal static void InitDressGirl()
        {
            Plugin.MoreLogs("Resetting DressGirl Variables...");
            performingAction = false;
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
            if (Plugin.instance.DressGirl == null || !gcConfig.ghostGirlEnhanced.Value)
                return;

            Plugin.MoreLogs("Checking for rapidFire ending events");
            DancingCheck();
            ShowerStuff.CheckForHauntedPlayerInShower();
        }


        private static void DancingCheck()
        {
            if (!gcConfig.ggEmoteCheck.Value || !girlIsChasing)
                return;

            if(!Bools.IsThisEffective(gcConfig.ggEmoteStopChasingChance.Value))
            {
                Plugin.MoreLogs("Ghost girl doesn't want to see you dance!");
                return;
            }

            int playersEmoting = NumberStuff.GetNumberPlayersEmoting();
            int alivePlayers = NumberStuff.GetAlivePlayers();

            if(Bools.AreEnoughPlayersDancing(alivePlayers, playersEmoting, gcConfig.ggEmoteStopChasePlayers.Value))
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
        }
        
        internal static void GirlDisappear()
        {
            if (Plugin.instance.DressGirl == null || !Plugin.instance.DressGirl.hauntingLocalPlayer)
                return;

            Plugin.instance.DressGirl.DisappearDuringHaunt();
        }

        internal static void EndAllGirlStuff()
        {
            if(girlIsChasing)
            {
                StopGirlFromChasing();
            }
            if(Plugin.instance.DressGirl.staringInHaunt)
            {
                GirlDisappear();
            }

        }

        internal static void BreakersFix(DressGirlAI instance)
        {
            if (instance.currentBehaviourStateIndex == 1)
                return;

            instance.SwitchToBehaviourStateOnLocalClient(1);
            instance.staringInHaunt = false;
            instance.disappearingFromStare = false;
            instance.disappearByVanishing = false;
            instance.choseDisappearingPosition = false;
            instance.agent.speed = 5.25f;
            instance.creatureAnimator.SetBool("Walk", true);
            instance.timesChased++;
            if (instance.timesChased != 1 && Random.Range(0, 100) < gcConfig.ggVanillaBreakerChance.Value)
            {
                instance.FlipLightsBreakerServerRpc();
                Plugin.GC.LogInfo("patched updated breaker chance for vanilla");
            }
            else
            {
                instance.MessWithLightsServerRpc();
            }

            instance.chaseTimer = 20f;
            instance.timer = 0f;
            instance.SetMovingTowardsTargetPlayer(instance.hauntingPlayer);
            instance.moveTowardsDestination = true;
        }
    }
}
