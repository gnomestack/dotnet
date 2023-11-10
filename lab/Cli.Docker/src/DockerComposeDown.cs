using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerComposeDown : DockerComposeServicesCmd
{
    public bool RemoveOrphans { get; set; }

    public string? Rmi { get; set; }

    public int? Timeout { get; set; }

    public bool Volumes { get; set; }

    protected override IReadOnlyCollection<string> Commands => new[] { "down" };

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyComposeOptions(args);
        if (this.RemoveOrphans)
            args.Add("--remove-orphans");

        if (!this.Rmi.IsNullOrWhiteSpace())
            args.Add("--rmi", this.Rmi);

        if (this.Timeout.HasValue)
            args.Add("--timeout", this.Timeout.Value.ToString());

        if (this.Volumes)
            args.Add("--volumes");

        foreach (var s in this.Services)
            args.Add(s);

        return args;
    }
}