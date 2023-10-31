using System.Diagnostics;

using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal class ShExecutor : ShellExecutor
{
    public ShExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("sh", (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"%ProgramFiles%\Git\usr\bin\sh.exe",
                @"%ChocolateyInstall%\msys2\usr\bin\sh.exe",
                @"%SystemDrive%\msys64\usr\bin\sh.exe",
                @"%SystemDrive%\msys\usr\bin\sh.exe",
            });
        });
    }

    public override string Shell => "sh";

    public override string Extension => ".sh";

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow("sh");
        var ps = new Ps(exe, info)
            .WithArgs(new[] { file });
        Debug.WriteLine($"{exe} -e {file}");

        return ps;
    }
}