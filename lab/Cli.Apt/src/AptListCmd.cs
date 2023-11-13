using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptListCmd : AptCmd
{
    public PsArgs Query { get; set; } = new();

    protected override string GetExecutablePath()
        => "/usr/bin/apt";

    protected override PsArgs BuildPsArgs()
    {
        return new PsArgs { "list", "-qq", this.Query };
    }
}