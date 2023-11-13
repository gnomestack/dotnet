using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerStart : DockerCmd
{
    public bool Attach { get; set; }

    public string? DetachKeys { get; set; }

    public bool Interactive { get; set; }

    public PsArgs Containers { get; set; } = new();

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("start");

        if (this.Attach)
            args.Add("--attach");

        if (!this.DetachKeys.IsNullOrWhiteSpace())
            args.Add("--detach-keys", this.DetachKeys);

        if (this.Interactive)
            args.Add("--interactive");

        foreach (var c in this.Containers)
            args.Add(c);

        return args;
    }
}