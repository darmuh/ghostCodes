
using UnityEngine;

namespace ghostCodes
{
    internal class Misc
    {
        internal static void LogTime()
        {
            string clockTime = HUDManager.Instance.clockNumber.text;
            string logTime = clockTime.Replace("\n", "").Replace("\r", "");
            Plugin.MoreLogs($"TIME: {logTime}");
        }

        internal static Color32 ParseColorFromString(string colorString)
        {
            // Split the string by commas
            string[] colorComponents = colorString.Split(',');

            // Check if the string contains 4 components
            if (colorComponents.Length != 4)
            {
                Plugin.GC.LogWarning("Invalid color config setting. Returning white color.");
                return Color.white;
            }

            byte[] colorValues = new byte[4];

            // Parse each component as byte
            for (int i = 0; i < 4; i++)
            {
                if (!byte.TryParse(colorComponents[i].Trim(), out colorValues[i]))
                {
                    Plugin.GC.LogWarning("Color config format failed. Returning white color.");
                    return Color.white;
                }
            }

            // Create and return Color32
            return new Color32(colorValues[0], colorValues[1], colorValues[2], colorValues[3]);
        }
    }
}
