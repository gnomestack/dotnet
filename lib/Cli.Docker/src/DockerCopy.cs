using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerCopy : DockerCmd
{
    public string Source { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public bool Archive { get; set; }

    public bool FollowLink { get; set; }

    public bool Quiet { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("cp");

        if (this.Archive)
            args.Add("--archive");

        if (this.FollowLink)
            args.Add("--follow-link");

        if (this.Quiet)
            args.Add("--quiet");

        args.Add(this.Source);
        args.Add(this.Destination);

        return args;
    }
}