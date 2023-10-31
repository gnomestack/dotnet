using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal class FsiExecutor : ShellExecutor
{
    public FsiExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("fsi", (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"%USERPROFILE%\.dotnet\dotnet.exe",
            });

            entry.Linux.AddRange(new HashSet<string>()
            {
                @"${HOME}/.dotnet/dotnet",
            });
        });
    }

    public override string Extension => ".fsx";

    public override string Shell => "fsi";

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow("fsi");
        var ps = new Ps(exe, info)
            .WithArgs(new[] { "fsi", file });

        return ps;
    }
}