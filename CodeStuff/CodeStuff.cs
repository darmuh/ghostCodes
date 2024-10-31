using ghostCodes.Configs;
using System.Collections.Generic;

namespace ghostCodes
{
    internal class CodeStuff
    {
        internal static List<TerminalAccessibleObject> myTerminalObjects = [];
        internal static List<TerminalAccessibleObject> filteredObjects = [];

        internal static void NewUsableCode(TerminalAccessibleObject obj)
        {
            Plugin.Spam("NewUsableCode!");
            if (obj == null)
                return;

            if(myTerminalObjects.Contains(obj)) 
                return;

            if (obj.isBigDoor && SetupConfig.IgnoreDoors.Value)
                return;

            if (obj.gameObject.name.Contains("TurretScript") && SetupConfig.IgnoreTurrets.Value)
                return;

            if (obj.gameObject.name.Contains("Landmine") && SetupConfig.IgnoreLandmines.Value)
                return;

            Plugin.MoreLogs($"{obj.gameObject.name} added to myTerminalObjects listing");
                myTerminalObjects.Add(obj);

            Plugin.MoreLogs($"myTerminalObjects count: [ {myTerminalObjects.Count} ]");
        }

        internal static void RemoveUsableCode(TerminalAccessibleObject obj)
        {
            Plugin.Spam("RemoveUsableCode!");
            if (!myTerminalObjects.Contains(obj))
                return;

            Plugin.Spam("Removing item from myTerminalObjects");
            myTerminalObjects.Remove(obj);
            
            Plugin.Spam($"myTerminalObjects count: [ {myTerminalObjects.Count} ]");
        }
    }
}
