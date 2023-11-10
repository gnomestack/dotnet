using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerInspect : DockerCmd
{
    public string? Format { get; set; }

    public PsArgs Containers { get; set; } = new();

    public bool Size { get; set; }

    public string? Type { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("inspect");

        if (!this.Format.IsNullOrWhiteSpace())
            args.Add("--format", this.Format);

        if (this.Size)
            args.Add("--size");

        if (!this.Type.IsNullOrWhiteSpace())
            args.Add("--type", this.Type);

        foreach (var c in this.Containers)
            args.Add(c);

        return args;
    }
}