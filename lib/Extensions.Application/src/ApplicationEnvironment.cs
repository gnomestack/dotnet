using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.FileProviders;

namespace GnomeStack.Extensions.Application;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
public class ApplicationEnvironment : IApplicationEnvironment
{
    private readonly ConcurrentDictionary<string, object?> map = new();

    public ApplicationEnvironment(ApplicationEnvironmentOptions? options = null, MicrosoftHostEnvironment? environment = null)
    {
        options ??= new ApplicationEnvironmentOptions();
        var name = options.Name ?? environment?.ApplicationName;
        var entryAssembly = options.EntryAssembly ?? Assembly.GetEntryAssembly();
        Debug.WriteLineIf(entryAssembly is null, "EntryAssembly is null");
        var assembly = entryAssembly ?? Assembly.GetCallingAssembly();
        var entryAssemblyName = assembly.GetName();

        this.Version = options.Version ?? entryAssemblyName.Version?.ToString() ?? "0.0.0";
        name ??= entryAssemblyName.Name;
        this["EntryAssemblyName"] = entryAssemblyName.Name;
        this["EntryAssemblyVersion"] = entryAssemblyName.Version?.ToString();

        // is the entry assembly a test host?
        if (entryAssemblyName?.Name?.StartsWith("testhost.", StringComparison.OrdinalIgnoreCase) == true)
            this["TestHost"] = true;

        this.Name = name ?? "Unknown";

        if (options.UseEnvironmentVariables)
        {
            this.EnvironmentName = GetEnvironmentName();
        }
        else
        {
            this.EnvironmentName = options.EnvironmentName ?? environment?.EnvironmentName
                ?? GetEnvironmentName();
        }

        this.Id = options.Id ?? $"{this.Name}@{this.Version}-{this.EnvironmentName}";
        this.InstanceName = options.InstanceName ?? $"{this.Id}-{Environment.MachineName}";

        foreach (var kvp in options.Properties)
            this.map[kvp.Key] = kvp.Value;

        this.ContentRootPath = options.ContentRootPath ?? environment?.ContentRootPath ?? Directory.GetCurrentDirectory();
        this.ContentRootFileProvider = options.ContentRootFileProvider ?? environment?.ContentRootFileProvider ?? new NullFileProvider();
    }

    public static IApplicationEnvironment Current { get; set; } = new UnknownApplicationEnvironment();

    public virtual string Name { get; }

    public virtual string Version { get; }

    public virtual string Id { get; }

    public virtual string InstanceName { get; }

    public virtual string EnvironmentName { get;  }

    public virtual string ContentRootPath { get; }

    public virtual IFileProvider ContentRootFileProvider { get; }

    public IDictionary<string, object?> Properties => this.map;

    public virtual object? this[string key]
    {
        get => this.map.TryGetValue(key, out var value) ? value : null;
        set
        {
            if (value == null)
                this.map.TryRemove(key, out var _);
            else
                this.map[key] = value;
        }
    }

    public static string GetEnvironmentName()
    {
        return Environment.GetEnvironmentVariable("APP_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("GNOMESTACK_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? "Production";
    }

    public static string GetAssemblyName()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        Debug.WriteLineIf(entryAssembly is null, "EntryAssembly is null");
        var assembly = entryAssembly ?? Assembly.GetCallingAssembly();
        var entryAssemblyName = assembly.GetName();
        return entryAssemblyName?.Name ?? "Unknown";
    }

    public virtual bool IsEnvironment(string environment)
        => string.Equals(this.EnvironmentName, environment, StringComparison.OrdinalIgnoreCase);
}