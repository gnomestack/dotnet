using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerNetworkRemove : DockerCmd
{
    public PsArgs Networks { get; set; } = new();

    public bool Force { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("network", "rm");

        if (this.Force)
            args.Add("--force");

        foreach (var n in this.Networks)
            args.Add(n);

        return args;
    }
}