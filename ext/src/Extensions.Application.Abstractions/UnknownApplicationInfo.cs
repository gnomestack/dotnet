using System;
using System.Collections.Generic;
using System.Text;

namespace GnomeStack.Extensions.Application;

public class UnknownApplicationInfo : IApplicationInfo
{
    public static IApplicationInfo Instance { get; } = new UnknownApplicationInfo();

    public string Name { get; } = "Unknown";

    public string Version { get; } = "0.0.0";

    public string Id { get; } = "Unknown";

    public string? InstanceName { get; }

    public string EnvironmentName { get; } = "Unknown";

    public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

    public bool IsEnvironment(string environment)
    {
        return environment == "Unknown";
    }
}