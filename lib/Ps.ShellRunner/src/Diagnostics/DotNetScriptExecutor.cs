namespace GnomeStack.Diagnostics;

internal class DotNetScriptExecutor : ShellExecutor
{
    public DotNetScriptExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("dotnet-script", (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"%USERPROFILE%\.dotnet\tools\dotnet-script.exe",
                @".\\dotnet-script.exe",
            });

            entry.Linux.AddRange(new HashSet<string>()
            {
                @"${HOME}/.dotnet/tools/dotnet-script",
                @"./dotnet-script",
            });
        });
    }

    public override string Shell => "dotnet-script";

    public override string Extension => ".csx";
}