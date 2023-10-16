using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace GnomeStack.Platform.App;

public class AppInfo : IAppInfo
{
    private readonly ConcurrentDictionary<string, object?> map = new();

    public AppInfo(AppInfoOptions options, IHostEnvironment? environment)
    {
        this.Name = environment?.ApplicationName ?? "Unknown";
        var entryAssembly = options.EntryAssembly ?? Assembly.GetEntryAssembly();
        Debug.WriteLineIf(entryAssembly is null, "EntryAssembly is null");
        var assembly = entryAssembly ?? Assembly.GetCallingAssembly();
        var entryAssemblyName = assembly.GetName();

        this.Version = options.Version ?? entryAssemblyName.Version?.ToString() ?? "0.0.0";
        if (entryAssemblyName is not null)
        {
            var name = entryAssemblyName.Name;
            this["EntryAssemblyName"] = entryAssemblyName.Name;
            this["EntryAssemblyVersion"] = entryAssemblyName.Version?.ToString();
            if (name?.StartsWith("testhost.", StringComparison.OrdinalIgnoreCase) == true)
                this["TestHost"] = true;

            this.Name = environment?.ApplicationName ?? name ?? "Unknown";
        }

        this.EnvironmentName = environment?.EnvironmentName
            ?? GetEnvironmentName();
        this.Id = options.Id ?? $"{this.Name}@{this.Version}-{this.EnvironmentName}";
        this.InstanceName = options.InstanceName ??
            $"{this.Name}@{this.Version}-{this.EnvironmentName}-{Environment.MachineName}";

        foreach (var kvp in options.Properties)
            this.map[kvp.Key] = kvp.Value;

        this.ContentRootPath = environment?.ContentRootPath ?? Directory.GetCurrentDirectory();
        this.ContentRootFileProvider = environment?.ContentRootFileProvider ?? new PhysicalFileProvider(this.ContentRootPath);
    }

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

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => this.map.GetEnumerator();

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => this.map.GetEnumerator();
}