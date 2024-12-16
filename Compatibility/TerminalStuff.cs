using TerminalStuff;

namespace ghostCodes.Compatibility
{
    internal class TerminalStuff
    {
        internal static void StopShortCuts(bool stop)
        {
            if (!Plugin.instance.TerminalStuff)
                return;

            ShortcutBindings.stopForAnyReason = stop;
        }
    }
}
