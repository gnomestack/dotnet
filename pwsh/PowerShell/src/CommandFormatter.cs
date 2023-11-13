using GnomeStack.Diagnostics;
using GnomeStack.Secrets;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell;

public static class CommandFormatter
{
    public static Func<string, PsArgs?, (string, bool)>? MessageFormatter { get; set; }

    public static (string, bool) FormatMessage(string command, PsArgs? args = null)
    {
        if (MessageFormatter is not null)
        {
            return MessageFormatter(command, args);
        }

        args ??= new PsArgs();
        var message = $"{command} {args}";
        message = SecretMasker.Default.Mask(message);

        if (Env.Get("TF_BUILD")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
        {
            return ($"##[command]{message}", false);
        }
        else if (Env.Get("GITHUB_ACTIONS")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true)
        {
            return ($"::notice::{message}", false);
        }
        else
        {
            return (message, true);
        }
    }
}