using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptSearchCmd : AptCmd
{
    public string Query { get; set; } = string.Empty;

    protected override string GetExecutablePath()
        => "/usr/bin/apt";

    protected override PsArgs BuildPsArgs()
        => new PsArgs()
        {
            "search",
            "-qq",
            this.Query,
        };
}