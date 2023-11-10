using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerStop : DockerCmd
{
    public PsArgs Containers { get; set; } = new();

    public string? Signal { get; set; }

    public int? Time { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("stop");

        if (!this.Signal.IsNullOrWhiteSpace())
            args.Add("--signal", this.Signal);

        if (this.Time.HasValue)
            args.Add("--time", this.Time.Value.ToString());

        foreach (var c in this.Containers)
            args.Add(c);

        return args;
    }
}