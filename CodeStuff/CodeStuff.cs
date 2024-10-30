using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;
using static ghostCodes.NumberStuff;

namespace ghostCodes
{
    internal class CodeStuff
    {
        internal static List<TerminalAccessibleObject> myTerminalObjects = new List<TerminalAccessibleObject>();
        internal static List<TerminalAccessibleObject> filteredObjects = new List<TerminalAccessibleObject>();

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
            
        }

        internal static void SortUsableCodes()
        {
            filteredObjects.Clear();

            foreach (var obj in myTerminalObjects)
            {
                if (!(obj.gameObject.name.Contains("Landmine") && ModConfig.gcIgnoreLandmines.Value) &&
                    !(obj.gameObject.name.Contains("TurretScript") && ModConfig.gcIgnoreTurrets.Value) &&
                    !(obj.gameObject.name.Contains("BigDoor") && ModConfig.gcIgnoreDoors.Value))
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

        internal static void GetRandomCodeAmount()
        {
            Plugin.instance.randGC = GetInt(ModConfig.gcMinCodes.Value, ModConfig.gcMaxCodes.Value);
        }
    }
}
