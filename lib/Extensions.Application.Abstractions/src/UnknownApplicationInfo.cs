using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace GnomeStack.Extensions.Application;

public class UnknownApplicationEnvironment : IApplicationEnvironment
{
    public static IApplicationEnvironment Instance { get; } = new UnknownApplicationEnvironment();

    public string Name { get; } = "Unknown";

    public string Version { get; } = "0.0.0";

    public string Id { get; } = "Unknown";

    public string? InstanceName { get; } = null;

    public string EnvironmentName { get; } = "Unknown";

    public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

    public IFileProvider ContentRootFileProvider { get; } = new NullFileProvider();

    public string ContentRootPath { get; } = Directory.GetCurrentDirectory();

    public bool IsEnvironment(string environment)
    {
        return environment == "Unknown";
    }
}