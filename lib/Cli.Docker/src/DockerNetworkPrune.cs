using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerNetworkPrune : DockerCmd
{
    public string? Filter { get; set; }

    public bool Force { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("network", "prune");

        if (!this.Filter.IsNullOrWhiteSpace())
            args.Add("--filter", this.Filter);

        if (this.Force)
            args.Add("--force");

        return args;
    }
}