using static ghostCodes.CodeStuff;

namespace ghostCodes
{
    internal class Mines
    {
        internal static void HandleMineAction(int randomObjectNum)
        {
            if (myTerminalObjects.Count == 0)
                return;

            if (myTerminalObjects[randomObjectNum].gameObject.name.Contains("Landmine"))
            {
                Landmine landmine = myTerminalObjects[randomObjectNum].gameObject.GetComponent<Landmine>();
                landmine.ExplodeMineClientRpc();
                landmine.ExplodeMineServerRpc();
                SignalTranslator.MessWithSignalTranslator();
                myTerminalObjects.Remove(myTerminalObjects[randomObjectNum]);
                Plugin.MoreLogs("WHAT THE FUUUUUU-");
            }
            else
            {
                myTerminalObjects[randomObjectNum].CallFunctionFromTerminal();
            }
        }
    }
}
