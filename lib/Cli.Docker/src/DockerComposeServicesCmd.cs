using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public abstract class DockerComposeServicesCmd : DockerComposeCmd
{
    public PsArgs Services { get; set; } = new();
}