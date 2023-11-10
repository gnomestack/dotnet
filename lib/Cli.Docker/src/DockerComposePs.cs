using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerComposePs : DockerComposeServicesCmd
{
    public bool All { get; set; }

    public string? Filter { get; set; }

    public string? Format { get; set; }

    public bool Quiet { get; set; }

    public bool DisplayServices { get; set; }

    public PsArgs Status { get; set; } = new();

    protected override IReadOnlyCollection<string> Commands => new[] { "ps" };

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyComposeOptions(args);

        if (this.All)
            args.Add("--all");

        if (!this.Filter.IsNullOrWhiteSpace())
            args.Add("--filter", this.Filter);

        if (!this.Format.IsNullOrWhiteSpace())
            args.Add("--format", this.Format);

        if (this.Quiet)
            args.Add("--quiet");

        if (this.DisplayServices)
            args.Add("--services");

        foreach (var s in this.Services)
            args.Add(s);

        return args;
    }
}