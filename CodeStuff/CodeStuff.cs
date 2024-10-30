using ghostCodes.Configs;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace ghostCodes
{
    internal class CodeStuff
    {
        internal static List<TerminalAccessibleObject> myTerminalObjects = [];
        internal static List<TerminalAccessibleObject> filteredObjects = [];

        internal static void GetUsableCodes()
        {
            TerminalAccessibleObject[] array = Object.FindObjectsOfType<TerminalAccessibleObject>();

            if (array == null || array.Length <= 0)
                return;

            myTerminalObjects = array.ToList();
            Plugin.MoreLogs($"Initial Loaded objects count: {myTerminalObjects.Count}");
            string listContents = string.Join(", ", myTerminalObjects);
            Plugin.MoreLogs($"{listContents}");
            SortUsableCodes();
            Doors.UpdateCacheDoorsList();
        }

        internal static void SortUsableCodes()
        {
            filteredObjects.Clear();

            foreach (var obj in myTerminalObjects)
            {
                if (!(obj.gameObject.name.Contains("Landmine") && SetupConfig.IgnoreLandmines.Value) &&
                    !(obj.gameObject.name.Contains("TurretScript") && SetupConfig.IgnoreTurrets.Value) &&
                    !(obj.gameObject.name.Contains("BigDoor") && SetupConfig.IgnoreDoors.Value))
                {
                    filteredObjects.Add(obj);
                }
            }

            myTerminalObjects = filteredObjects;

            if (myTerminalObjects.Count < 0)
                return;

            Plugin.MoreLogs($"Final filtered myTerminalObjects({myTerminalObjects.Count}):");
            string listContents2 = string.Join(", ", myTerminalObjects);
            Plugin.MoreLogs($"{listContents2}");

        }
    }
}
