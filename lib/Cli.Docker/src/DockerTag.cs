using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerTag : DockerCmd
{
    public string Source { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("tag");

        args.Add(this.Source);
        args.Add(this.Target);
        return args;
    }
}