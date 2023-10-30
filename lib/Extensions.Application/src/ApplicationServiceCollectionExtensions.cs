using System.Diagnostics;

using GnomeStack.Extensions.Application;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddGsApplicationPaths(this IServiceCollection services, Action<ApplicationPathsOptions>? configure)
    {
        var options = new ApplicationPathsOptions();
        configure?.Invoke(options);
        services.TryAddSingleton<IApplicationPaths>(s =>
        {
            if (ApplicationPaths.Current is not UnknownApplicationPaths)
                return ApplicationPaths.Current;

            var appInfo = s.GetService<IApplicationEnvironment>();
            var paths = new ApplicationPaths(options, appInfo);
            ApplicationPaths.Current = paths;
            return paths;
        });

        return services;
    }

    public static IServiceCollection AddGsApplicationEnvironment(
        this IServiceCollection services,
        Action<ApplicationEnvironmentOptions>? configure,
        Func<IServiceProvider, MicrosoftHostEnvironment>? configureMicrosoftHostEnvironment)
    {
        var options = new ApplicationEnvironmentOptions();
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

        services.TryAddSingleton<IApplicationEnvironment>(s =>
        {
            if (ApplicationEnvironment.Current is not UnknownApplicationEnvironment)
                return ApplicationEnvironment.Current;

            var environment = configureMicrosoftHostEnvironment(s);
            var env = new ApplicationEnvironment(options, environment);
            ApplicationEnvironment.Current = env;
            return env;
        });

        return services;
    }
}