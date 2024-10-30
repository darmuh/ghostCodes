using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace ghostCodes
{
    internal class SignalTranslator
    {
        internal static string lastmessage;
        internal static string OddSignalMessage(List<string> messages, out string message)
        {
            if(messages.Count == 0)
            {
                message = "ERROR";
                return message;
            }

            if(gcConfig.onlyUniqueMessages.Value && messages.Count > 1 && lastmessage != string.Empty && messages.Contains(lastmessage))
                messages.Remove(lastmessage);

            int rand = Random.Range(0, messages.Count - 1);
            message = messages[rand];
            lastmessage = messages[rand];
            return message;
        }

        internal static void MessWithSignalTranslator()
        {
            if (!gcConfig.canSendMessages.Value)
                return;

            if (gcConfig.onlyGGSendMessages.Value && Plugin.instance.DressGirl == null)
                return;

            List<string> messages = gcConfig.signalMessages.Value.Split(',').ToList();

            OddSignalMessage(messages, out string message);
            HUDManager.Instance.UseSignalTranslatorServerRpc(message);
        }
    }
}
