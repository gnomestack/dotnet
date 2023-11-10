using GnomeStack.Diagnostics;
using GnomeStack.Standard;

namespace GnomeStack.Cli.Docker;

public abstract class DockerComposeCmd : PsCommand
{
    public string? Ansi { get; set; }

    public bool Compatibility { get; set; }

    public PsArgs File { get; set; } = new();

    public PsArgs EnvFile { get; set; } = new();

    public int? Parallel { get; set; }

    public string? ProjectName { get; set; }

    public PsArgs Profile { get; set; } = new();

    public string? ProjectDirectory { get; set; }

    public string? Progress { get; set; }

    public bool DryRun { get; set; }

    protected abstract IReadOnlyCollection<string> Commands { get; }

    protected override string GetExecutablePath()
        => Ps.Which("docker") ?? throw new NotFoundOnPathException("docker not found on path");

    protected virtual void ApplyComposeOptions(PsArgs args)
    {
        args.Add("compose");

        if (!this.Ansi.IsNullOrWhiteSpace())
            args.Add("--ansi", this.Ansi);

        if (this.Compatibility)
            args.Add("--compatibility");

        if (this.DryRun)
            args.Add("--dry-run");

        if (this.Parallel.HasValue)
            args.Add("--parallel", this.Parallel.Value.ToString());

        foreach (var f in this.File)
            args.Add("--file", f);

        foreach (var f in this.EnvFile)
            args.Add("--env-file", f);

        foreach (var p in this.Profile)
            args.Add("--profile", p);

        if (!this.ProjectName.IsNullOrWhiteSpace())
            args.Add("--project-name", this.ProjectName);

        if (!this.ProjectDirectory.IsNullOrWhiteSpace())
            args.Add("--project-directory", this.ProjectDirectory);

        if (!this.Progress.IsNullOrWhiteSpace())
            args.Add("--progress", this.Progress);

        if (this.Commands.Count > 0)
            args.AddRange(this.Commands);
    }
}