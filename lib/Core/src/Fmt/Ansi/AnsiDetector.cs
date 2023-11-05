using System.Diagnostics;

using GnomeStack.Extras.Strings;

using GnomeStack.Standard;

namespace GnomeStack.Fmt.Ansi;

public static class AnsiDetector
{
    public static AnsiSettings Detect()
    {
        var settings = new AnsiSettings();
        settings.Mode = DetectMode();
        if (settings.Mode == AnsiMode.TwentyFourBit)
            settings.Links = true;

        return settings;
    }

    public static bool EnableVirtualTerminalProcessing(bool stdError = false)
    {
        var handle = GetStdPipeHandle(stdError);
        return InternalEnableVirtualTerminalProcessing(handle);
    }

    public static bool IsTermVariableAnsiCompatible(params string[] tests)
    {
        var set = new List<string>()
        {
            "^xterm",
            "^rxvt",
            "^cygwin",
            "ansi",
            "linux",
            "konsole",
            "tmux",
            "alacritty",
            "^vt100",
            "^vt220",
            "^vt220",
            "^vt320",
            "^screen",
        };

        set.AddRange(tests);
        var term = Environment.GetEnvironmentVariable("TERM");
        if (string.IsNullOrEmpty(term))
            return false;

        foreach (string match in set)
        {
            if (match[0] is '^')
            {
                if (match.Substring(1).StartsWith(term))
                    return true;

                continue;
            }

            if (match.Contains(term, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public static AnsiMode DetectMode()
    {
        if (Env.TryGet("GNOMESTACK_TERM", out var term))
        {
            switch (term)
            {
                case "none":
                case "no-color":
                case "nocolor":
                case "0":
                    return AnsiMode.None;
                case "3bit":
                    return AnsiMode.ThreeBit;
                case "4bit":
                    return AnsiMode.FourBit;
                case "8bit":
                    return AnsiMode.EightBit;
                case "color":
                case "24bit":
                    return AnsiMode.TwentyFourBit;
                default:
                    return AnsiMode.FourBit;
            }
        }

        if (Env.IsWindows)
        {
            var conEmu = Environment.GetEnvironmentVariable("ConEmuANSI");
            if (!string.IsNullOrEmpty(conEmu) && conEmu.Equals("On", StringComparison.OrdinalIgnoreCase))
                return AnsiMode.TwentyFourBit;

            try
            {
                var build = Environment.OSVersion.Version;
                if (build is { Major: > 9, Build: >= 18262 })
                {
                    var result = EnableVirtualTerminalProcessing();
                    if (result)
                    {
                        EnableVirtualTerminalProcessing(true);
                    }

                    return AnsiMode.TwentyFourBit;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        if (IsTermVariableAnsiCompatible())
            return AnsiMode.TwentyFourBit;

        return AnsiMode.FourBit;
    }

    private static bool InternalEnableVirtualTerminalProcessing(IntPtr handle)
    {
        if (!Interop.Kernel32.GetConsoleMode(handle, out var mode))
        {
            return false;
        }

        mode |= Interop.Kernel32.ENABLE_VIRTUAL_TERMINAL_PROCESSING;

        return Interop.Kernel32.SetConsoleMode(handle, mode);
    }

    private static (bool, int) GetConsoleMode(IntPtr handle)
    {
        if (!Interop.Kernel32.GetConsoleMode(handle, out var mode))
        {
            return (false, mode);
        }

        return (true, mode);
    }

    private static IntPtr GetStdPipeHandle(bool isError)
    {
        return Interop.Kernel32.GetStdHandle(isError
            ? Interop.Kernel32.STD_ERROR_HANDLE
            : Interop.Kernel32.STD_OUTPUT_HANDLE);
    }
}