using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptAutoRemoveCmd : AptCmd
{
    protected override PsArgs BuildPsArgs()
        => new() { "autoremove" };
}