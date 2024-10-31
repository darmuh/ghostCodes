using System.Collections.Generic;
using static ghostCodes.CodeStuff;

namespace ghostCodes
{
    internal class Mines
    {
        internal static void HandleMineBoom()
        {
            if (myTerminalObjects.Count == 0)
                return;

            List<TerminalAccessibleObject> mines = myTerminalObjects.FindAll(m => m.gameObject.GetComponent<Landmine>() != null && !m.gameObject.GetComponent<Landmine>().hasExploded);

            if (mines.Count > 0)
            {
                int thismine = NumberStuff.Rand.Next(mines.Count);
                Landmine landmine = mines[thismine].gameObject.GetComponent<Landmine>();
                if (landmine.hasExploded)
                    return;

                landmine.ExplodeMineClientRpc();
                landmine.ExplodeMineServerRpc();
                Plugin.MoreLogs("WHAT THE FUUUUUU-");
            }
            else
                Plugin.Spam("No landmines to go kaboom");
        }
    }
}
