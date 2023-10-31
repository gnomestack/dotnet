using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal class CmdExecutor : ShellExecutor
{
    public override string Shell => "cmd";

    public override string Extension => ".cmd";

    protected override string GenerateScriptFile(string script, string extension)
    {
        script = $"""
                  @echo off
                  {script}
                  """;
        return base.GenerateScriptFile(script, extension);
    }

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow("cmd");
        var ps = new Ps(exe, info)
            .WithArgs(
                new[]
                {
                    "/D",
                    "/E:ON",
                    "/V:OFF",
                    "/S",
                    "/C", $"CALL \"{file}\"",
                });

        return ps;
    }
}