using GnomeStack.Diagnostics;

namespace GnomeStack.Apt;

public class AptInstallCmd : AptCmd
{
    public PsArgs Packages { get; set; } = new();

    public bool OnlyUpgrade { get; set; }

    public bool InstallRecommends { get; set; }

    public bool InstallSuggests { get; set; }

    public bool Yes { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs { "install" };

        if (this.InstallRecommends)
            args.Add("--install-recommends");

        if (this.InstallSuggests)
            args.Add("--install-suggests");

        if (this.OnlyUpgrade)
            args.Add("--only-upgrade");

        if (this.Yes)
            args.Add("-y");


        args.Add(this.Packages);
        return args;
    }
}