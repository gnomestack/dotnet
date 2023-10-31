using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal class DenoExecutor : ShellExecutor
{
    public DenoExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("deno", (entry) =>
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

    public override string Extension => ".ts";

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow("deno");
        var ps = new Ps(exe, info)
            .WithArgs(new[] { "run", "-A", "--unstable", file });

        return ps;
    }
}