using GnomeStack.Functional;
using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal sealed class BashExecutor : ShellExecutor
{
    private const string DefaultShell = "bash";

    public BashExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate(DefaultShell, (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"%ProgramFiles%\Git\bin\bash.exe",
                @"%ProgramFiles%\Git\usr\bin\bash.exe",
                @"%ChocolateyInstall%\msys2\usr\bin\bash.exe",
                @"%SystemDrive%\msys64\usr\bin\bash.exe",
                @"%SystemDrive%\msys\usr\bin\bash.exe",
                @"%SystemRoot%\System32\bash.exe",
            });
        });
    }

    public override string Shell => DefaultShell;

    public override string Extension => ".sh";

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow(DefaultShell);
        if (Env.IsWindows)
        {
            file = file.Replace("\\", "/");
            if (exe.EndsWith("System32\\bash.exe", StringComparison.OrdinalIgnoreCase))
            {
                file = "/mnt/" + "c" + file.Substring(1).Replace(":", string.Empty);
            }
        }

        var ps = new Ps(exe, info)
            .WithArgs(new[] { "-noprofile", "--norc", "-e", "-o", "pipefail", "-c", file });
        return ps;
    }
}