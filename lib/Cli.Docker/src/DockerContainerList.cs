using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerContainerList : DockerCmd
{
    public bool All { get; set; }

    public string? Filter { get; set; }

    public string? Format { get; set; }

    public int? Last { get; set; }

    public bool Latest { get; set; }

    public bool NoTrunc { get; set; }

    public bool Quiet { get; set; }

    public bool Size { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("container", "list");

        if (this.All)
            args.Add("--all");

        if (!this.Filter.IsNullOrWhiteSpace())
            args.Add("--filter", this.Filter);

        if (!this.Format.IsNullOrWhiteSpace())
            args.Add("--format", this.Format);

        if (this.Last.HasValue)
            args.Add("--last", this.Last.Value.ToString());

        if (this.Latest)
            args.Add("--latest");

        if (this.NoTrunc)
            args.Add("--no-trunc");

        if (this.Quiet)
            args.Add("--quiet");

        if (this.Size)
            args.Add("--size");

        return args;
    }
}