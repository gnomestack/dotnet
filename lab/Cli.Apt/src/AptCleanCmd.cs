using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptCleanCmd : AptCmd
{
    protected override PsArgs BuildPsArgs()
    {
        return new PsArgs() { "clean" };
    }
}