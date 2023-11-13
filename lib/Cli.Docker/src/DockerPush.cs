using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerPush : DockerCmd
{
    public string Name { get; set; } = string.Empty;

    public bool AllTags { get; set; }

    public bool DisableContentTrust { get; set; }

    public bool Quiet { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("push");

        if (this.AllTags)
            args.Add("--all-tags");

        if (this.DisableContentTrust)
            args.Add("--disable-content-trust");

        if (this.Quiet)
            args.Add("--quiet");

        args.Add(this.Name);

        return args;
    }
}