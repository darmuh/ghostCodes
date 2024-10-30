using ghostCodes.Configs;
using static UnityEngine.Object;

namespace ghostCodes
{
    internal class ShowerStuff
    {
        internal static void CheckForHauntedPlayerInShower()
        {
            if (!DressGirl.girlIsChasing && InteractionsConfig.ShowerStopChasing.Value < 1)
                return;

            ShowerTrigger shower = FindObjectOfType<ShowerTrigger>();
            if (shower == null)
                return;

            if (!IsShowerEffective())
            {
                Plugin.MoreLogs("The shower is not washing the ghostGirl's mark off!!!");
                return;
            }

            if (shower.playersInShower.Count != 0 && shower.playersInShower.Contains(Plugin.instance.DressGirl.hauntingPlayer) && shower.showerOn)
            {
                Plugin.MoreLogs("haunted player taking a shower detected, stopping chase");
                DressGirl.StopGirlFromChasing();
                return;
            }
        }

        private static bool IsShowerEffective()
        {
            return Bools.IsThisEffective(InteractionsConfig.ShowerStopChasing.Value);
        }
    }
}
