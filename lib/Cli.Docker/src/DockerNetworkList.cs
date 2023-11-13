using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerNetworkList : DockerCmd
{
    public string? Filter { get; set; }

    public string? Format { get; set; }

    public bool NoTrunc { get; set; }

    public bool Quiet { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("network", "ls");

        if (!this.Filter.IsNullOrWhiteSpace())
            args.Add("--filter", this.Filter);

        if (!this.Format.IsNullOrWhiteSpace())
            args.Add("--format", this.Format);

        if (this.NoTrunc)
            args.Add("--no-trunc");

        if (this.Quiet)
            args.Add("--quiet");

        return args;
    }
}