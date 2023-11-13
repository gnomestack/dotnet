using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerPull : DockerCmd
{
    public string Image { get; set; } = string.Empty;

    public bool AllTags { get; set; }

    public bool DisableContentTrust { get; set; }

    public string? Platform { get; set; }

    public bool Quiet { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("pull");

        if (this.AllTags)
            args.Add("--all-tags");

        if (this.DisableContentTrust)
            args.Add("--disable-content-trust");

        if (!this.Platform.IsNullOrWhiteSpace())
            args.Add("--platform", this.Platform);

        if (this.Quiet)
            args.Add("--quiet");

        args.Add(this.Image);

        return args;
    }
}