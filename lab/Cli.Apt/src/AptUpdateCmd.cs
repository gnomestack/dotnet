using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptUpdateCmd : AptCmd
{
    protected override PsArgs BuildPsArgs()
        => new() { "update" };
}