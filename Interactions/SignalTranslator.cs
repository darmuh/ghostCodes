using ghostCodes.Configs;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ghostCodes
{
    internal class SignalTranslator
    {
        internal static string lastmessage;
        internal static string OddSignalMessage(List<string> messages, out string message)
        {
            if (messages.Count == 0)
            {
                message = "ERROR";
                return message;
            }

            if (InteractionsConfig.OnlyUniqueMessages.boolValue && messages.Count > 1 && lastmessage != string.Empty && messages.Contains(lastmessage))
                messages.Remove(lastmessage);

            int rand = Random.Range(0, messages.Count - 1);
            message = messages[rand];
            lastmessage = messages[rand];
            return message;
        }

        internal static void MessWithSignalTranslator()
        {
            if (InteractionsConfig.SignalTranslator.Value < 1)
                return;

            if (InteractionsConfig.SignalTranslator.Value < NumberStuff.GetInt(0, 100))
                return;

            List<string> messages = [.. InteractionsConfig.AllSignalMessages.stringValue.Split(',')];

            OddSignalMessage(messages, out string message);
            HUDManager.Instance.UseSignalTranslatorServerRpc(message);
        }
    }
}
