using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerComposeUp : DockerComposeServicesCmd
{
    public bool AbortOnContainerExit { get; set; }

    public bool AlwaysRecreateDeps { get; set; }

    public PsArgs Attach { get; set; } = new();

    public bool Build { get; set; }

    public bool Detach { get; set; }

    public string? ExitCodeFrom { get; set; }

    public bool ForceRecreate { get; set; }

    public PsArgs NoAttach { get; set; } = new();

    public bool NoBuild { get; set; }

    public bool NoColor { get; set; }

    public bool NoDeps { get; set; }

    public bool NoLogPrefix { get; set; }

    public bool NoRecreate { get; set; }

    public bool NoStart { get; set; }

    public string? Pull { get; set; }

    public bool QuietPull { get; set; }

    public bool RemoveOrphans { get; set; }

    public bool RenewAnonVolumes { get; set; }

    public int? Scale { get; set; }

    public int? Timeout { get; set; }

    public bool Timestamps { get; set; }

    public bool Wait { get; set; }

    public int? WaitTimeout { get; set; }

    protected override IReadOnlyCollection<string> Commands => new[] { "up" };

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyComposeOptions(args);

        if (this.AbortOnContainerExit)
            args.Add("--abort-on-container-exit");

        if (this.AlwaysRecreateDeps)
            args.Add("--always-recreate-deps");

        foreach (var a in this.Attach)
            args.Add("--attach", a);

        if (this.Build)
            args.Add("--build");

        if (this.Detach)
            args.Add("--detach");

        if (!this.ExitCodeFrom.IsNullOrWhiteSpace())
            args.Add("--exit-code-from", this.ExitCodeFrom);

        if (this.ForceRecreate)
            args.Add("--force-recreate");

        foreach (var a in this.NoAttach)
            args.Add("--no-attach", a);

        if (this.NoBuild)
            args.Add("--no-build");

        if (this.NoColor)
            args.Add("--no-color");

        if (this.NoDeps)
            args.Add("--no-deps");

        if (this.NoLogPrefix)
            args.Add("--no-log-prefix");

        if (this.NoRecreate)
            args.Add("--no-recreate");

        if (this.NoStart)
            args.Add("--no-start");

        if (!this.Pull.IsNullOrWhiteSpace())
            args.Add("--pull", this.Pull);

        if (this.QuietPull)
            args.Add("--quiet-pull");

        if (this.RemoveOrphans)
            args.Add("--remove-orphans");

        if (this.RenewAnonVolumes)
            args.Add("--renew-anon-volumes");

        if (this.Scale.HasValue)
            args.Add("--scale", this.Scale.Value.ToString());

        if (this.Timeout.HasValue)
            args.Add("--timeout", this.Timeout.Value.ToString());

        if (this.Timestamps)
            args.Add("--timestamps");

        if (this.Wait)
            args.Add("--wait");

        if (this.WaitTimeout.HasValue)
            args.Add("--wait-timeout", this.WaitTimeout.Value.ToString());

        foreach (var s in this.Services)
            args.Add(s);

        return args;
    }
}