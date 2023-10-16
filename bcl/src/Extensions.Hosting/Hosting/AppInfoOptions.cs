using System.Reflection;

namespace GnomeStack.Extensions.Hosting;

public class AppInfoOptions
{
    public string? Id { get; set; }

    public string? Version { get; set; }

    public string? InstanceName { get; set; }

    public Assembly? EntryAssembly { get; set; }

    public Dictionary<string, object?> Properties { get; } = new();
}