using static UnityEngine.Object;

namespace ghostCodes
{
    internal class ShowerStuff
    {
        internal static void CheckForHauntedPlayerInShower()
        {
            if (!DressGirl.girlIsChasing) //config
                return;

            if(!IsShowerEffective())
            {
                Plugin.MoreLogs("The shower is not washing the ghostGirl's mark off!!!");
                return;
            }

            ShowerTrigger shower = FindObjectOfType<ShowerTrigger>();
            if (shower == null)
                return;

            if (shower.playersInShower.Count != 0 && shower.playersInShower.Contains(Plugin.instance.DressGirl.hauntingPlayer) && shower.showerOn)
            {
                Plugin.MoreLogs("haunted player taking a shower detected, stopping chase");
                DressGirl.StopGirlFromChasing();
                return;
            }
        }

        private static bool IsShowerEffective()
        {
            return Bools.IsThisEffective(gcConfig.ggShowerStopChasingChance.Value);
        }
    }
}
