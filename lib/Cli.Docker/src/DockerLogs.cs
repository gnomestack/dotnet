using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerLogs : DockerCmd
{
    public string Container { get; set; } = string.Empty;

    public bool Details { get; set; }

    public bool Follow { get; set; }

    public string? Since { get; set; }

    public string? Tail { get; set; }

    public bool Timestamps { get; set; }

    public string? Until { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("logs");

        if (this.Details)
            args.Add("--details");

        if (this.Follow)
            args.Add("--follow");

        if (!this.Since.IsNullOrWhiteSpace())
            args.Add("--since", this.Since);

        if (!this.Tail.IsNullOrWhiteSpace())
            args.Add("--tail", this.Tail);

        if (this.Timestamps)
            args.Add("--timestamps");

        if (!this.Until.IsNullOrWhiteSpace())
            args.Add("--until", this.Until);

        args.Add(this.Container);

        return args;
    }
}