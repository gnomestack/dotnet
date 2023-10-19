using System;
using System.Collections.Generic;
using System.Text;

namespace GnomeStack.Extensions.Application;

public interface IApplicationInfo
{
    string Name { get; }

    string Version { get; }

    string Id { get; }

    string? InstanceName { get; }

    string EnvironmentName { get; }

    IDictionary<string, object?> Properties { get; }

    bool IsEnvironment(string environment);
}