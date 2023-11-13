using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptRemoveCmd : AptCmd
{
    public PsArgs Packages { get; set; } = new();

    public bool Yes { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs() { "remove" };
        if (this.Yes)
            args.Add("-y");

        args.Add(this.Packages);

        return args;
    }
}