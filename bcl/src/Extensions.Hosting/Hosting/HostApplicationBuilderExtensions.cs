using System.Reflection;

using GnomeStack.Extensions.Hosting;

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class HostApplicationBuilderExtensions
{
    public static HostApplicationBuilder UseAppInfo(this HostApplicationBuilder app, Action<AppInfoOptions>? configure = null)
    {
        app.Services.AddAppInfo(configure);
        return app;
    }

    public static HostApplicationBuilder UseAppPaths(this HostApplicationBuilder app, Action<AppPathsOptions>? configure = null)
    {
        app.Services.AppAppPaths(configure);
        return app;
    }

    public static HostApplicationBuilderSettings ApplyDefaults(this HostApplicationBuilderSettings settings)
    {
        settings.EnvironmentName ??= Environment.GetEnvironmentVariable("APP_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("GnomeStack_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? "Production";

        settings.ApplicationName ??= Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
        return settings;
    }
}