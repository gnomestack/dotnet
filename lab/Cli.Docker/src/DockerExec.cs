using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerExec : DockerCmd
{
    public string Command { get; set; } = string.Empty;

    public PsArgs Args { get; set; } = new();

    public bool Detach { get; set; }

    public Dictionary<string, string>? Env { get; set; }

    public PsArgs? EnvFile { get; set; }

    public bool Interactive { get; set; }

    public bool Privileged { get; set; }

    public bool Tty { get; set; }

    public string? User { get; set; }

    public string? WorkDir { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("exec");

        if (this.Detach)
            args.Add("--detach");

        if (this.Interactive)
            args.Add("--interactive");

        if (this.Privileged)
            args.Add("--privileged");

        if (this.Tty)
            args.Add("--tty");

        if (!this.User.IsNullOrWhiteSpace())
            args.Add("--user", this.User);

        if (!this.WorkDir.IsNullOrWhiteSpace())
            args.Add("--workdir", this.WorkDir);

        if (this.Env?.Count > 0)
        {
            foreach (var kvp in this.Env)
            {
                args.Add("--env", $"{kvp.Key}='{kvp.Value}'");
            }
        }

        if (this.EnvFile?.Count > 0)
        {
            foreach (var f in this.EnvFile)
            {
                args.Add("--env-file", f);
            }
        }

        args.Add(this.Command);
        args.AddRange(this.Args);

        return args;
    }
}