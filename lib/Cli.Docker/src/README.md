# GnomeStack.Cli.Docker

Provides classes that implement PsCommand and represent the Docker CLI. 
The library does not yet fully implement the Docker CLI. It is a work in progress.

However, any missing commands can be added by creating a class that implements
PsCommand and using the Ps.Exec or Ps.Capture methods. For this library,
the commands may implement `DockerCmd` as a base class.

## Usage Examples

```csharp
// capture will capture the output of the command
// as part of the result in the StdOut and StdError properties.
var r = Ps.Capture(new DockerContainerList());

if (r.ExitCode != 0)
   throw new ProcessException("Failed to list containers.");

foreach (var line in r.StdOut)
   writer.WriteLine(line);

// executes without capturing the output.
Ps.Exec(new DockerNetworkList()
    {
        Format = "{{.Name}}",
    });

// Ps can be used without objects that inherit from PsCommand.
// This is useful for commands that are not yet implemented.
Ps.Exec("docker", PsArgs.From("network", "list", "--format", "{{.Name}}"));

// in dotnet 8
Ps.Exec("docker", ["network", "list", "--format", "{{.Name}}"]);
```

Sample implementation of a command

```csharp
public class DockerStart : DockerCmd
{
    public bool Attach { get; set; }

    public string? DetachKeys { get; set; }

    public bool Interactive { get; set; }

    public PsArgs Containers { get; set; } = new();

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("start");

        if (this.Attach)
            args.Add("--attach");

        if (!this.DetachKeys.IsNullOrWhiteSpace())
            args.Add("--detach-keys", this.DetachKeys);

        if (this.Interactive)
            args.Add("--interactive");

        foreach (var c in this.Containers)
            args.Add(c);

        return args;
    }
}
```

MIT License
