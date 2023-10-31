using Azure.Core;

using GnomeStack.Azure.Identity;
using GnomeStack.Extensions.Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class AzureIdentityServiceCollectionExtensions
{
    public static IServiceCollection AddGsAzureIdentity(this IServiceCollection services, Action<AzureIdentityOptions>? configure)
    {
        var options = new AzureIdentityOptions();
        configure?.Invoke(options);
        services.TryAddSingleton<TokenCredential>(s =>
        {
            if (options.TokenCredential is not null)
            {
                return options.TokenCredential;
            }

            if (options.TokenCredentialFactory is not null)
            {
                return options.TokenCredentialFactory(s);
            }

            var configuration = s.GetRequiredService<IConfiguration>();
            return new GsConfigTokenCredential(configuration);
        });

        return services;
    }
}