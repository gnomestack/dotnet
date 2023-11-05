using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptAutoCleanCmd : AptCmd
{
    protected override PsArgs BuildPsArgs()
        => new() { "autoclean" };
}