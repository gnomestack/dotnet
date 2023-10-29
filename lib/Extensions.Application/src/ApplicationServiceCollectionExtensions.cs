using System.Diagnostics;

using GnomeStack.Extensions.Application;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationPaths(this IServiceCollection services, Action<ApplicationPathsOptions>? configure)
    {
        var options = new ApplicationPathsOptions();
        configure?.Invoke(options);
        services.TryAddSingleton<IApplicationPaths>(s =>
        {
            var appInfo = s.GetService<IApplicationInfo>();
            return new ApplicationPaths(options, appInfo);
        });

        return services;
    }

    public static IServiceCollection AddApplicationInfo(
        this IServiceCollection services,
        Action<ApplicationInfoOptions>? configure,
        Func<IServiceProvider, MicrosoftHostEnvironment>? configureMicrosoftHostEnvironment)
    {
        var options = new ApplicationInfoOptions();
        configure?.Invoke(options);

        if (configureMicrosoftHostEnvironment is null)
        {
            configureMicrosoftHostEnvironment = s =>
            {
                var microsoftHostEnvironment = new MicrosoftHostEnvironment();
                try
                {
                    var iHostingEnvironmentType =
                        Type.GetType("Microsoft.Extensions.Hosting.IHostingEnvironment", false, true);
                    if (iHostingEnvironmentType is not null)
                    {
                        var iHostingEnvironment = s.GetService(iHostingEnvironmentType);
                        if (iHostingEnvironment is not null)
                        {
                            var applicationName = iHostingEnvironmentType.GetProperty("ApplicationName")
                                ?.GetValue(iHostingEnvironment) as string;
                            var environmentName = iHostingEnvironmentType.GetProperty("EnvironmentName")
                                ?.GetValue(iHostingEnvironment) as string;
                            var contentRootPath = iHostingEnvironmentType.GetProperty("ContentRootPath")
                                ?.GetValue(iHostingEnvironment) as string;
                            var contentRootFileProvider =
                                iHostingEnvironmentType.GetProperty("ContentRootFileProvider")
                                    ?.GetValue(iHostingEnvironment) as IFileProvider;

                            microsoftHostEnvironment.EnvironmentName ??= environmentName;
                            microsoftHostEnvironment.ApplicationName ??= applicationName;
                            microsoftHostEnvironment.ContentRootPath ??= contentRootPath;
                            microsoftHostEnvironment.ContentRootFileProvider ??= contentRootFileProvider;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                return microsoftHostEnvironment;
            };
        }

        services.TryAddSingleton<IApplicationInfo>(s =>
        {
            var environment = configureMicrosoftHostEnvironment(s);
            return new ApplicationInfo(options, environment);
        });

        return services;
    }
}