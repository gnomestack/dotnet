using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal class DenoJsExecutor : ShellExecutor
{
    public DenoJsExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("deno-js", (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"%USERPROFILE%\.deno\bin\deno.exe",
                @"%ChocolateyInstall%\lib\deno\tools\deno.exe",
            });

            entry.Linux.AddRange(new HashSet<string>()
            {
                @"${HOME}\.deno\bin\deno",
            });
        });
    }

    public override string Shell => "deno";

    public override string Extension => ".js";

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow("deno");
        var ps = new Ps(exe, info)
            .WithArgs(new[] { "run", "-A", "--unstable", file });

        return ps;
    }
}