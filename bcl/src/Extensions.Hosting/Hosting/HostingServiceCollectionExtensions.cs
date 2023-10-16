using GnomeStack.Extensions.Hosting;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class HostingServiceCollectionExtensions
{
    public static IServiceCollection AddAppInfo(this IServiceCollection services, Action<AppInfoOptions>? configure = null)
    {
        services.TryAddSingleton<IAppInfo>(serviceProvider =>
        {
            var options = new AppInfoOptions();
            configure?.Invoke(options);

            var info = new AppInfo(options, serviceProvider.GetRequiredService<IHostEnvironment>());
            var hbc = serviceProvider.GetService<HostBuilderContext>();
            if (hbc is not null)
                hbc.Properties["AppInfo"] = info;

            return info;
        });

        return services;
    }

    public static IServiceCollection AppAppPaths(this IServiceCollection services, Action<AppPathsOptions>? configure = null)
    {
        services.AddAppInfo();
        services.TryAddSingleton<IAppPaths>(serviceProvider =>
        {
            var options = new AppPathsOptions();
            configure?.Invoke(options);

            var paths = new AppPaths(options, serviceProvider.GetRequiredService<IAppInfo>());
            var hbc = serviceProvider.GetService<HostBuilderContext>();
            if (hbc is not null)
                hbc.Properties["AppPaths"] = paths;

            return paths;
        });

        return services;
    }
}