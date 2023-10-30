using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace GnomeStack.Extensions.Application;

public interface IApplicationEnvironment
{
    string Name { get; }

    string Version { get; }

    string Id { get; }

    string? InstanceName { get; }

    string EnvironmentName { get; }

    IFileProvider ContentRootFileProvider { get; }

    string ContentRootPath { get; }

    IDictionary<string, object?> Properties { get; }

    bool IsEnvironment(string environment);
}