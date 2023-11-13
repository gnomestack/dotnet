using GnomeStack.Extensions.Secrets;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class SecretServiceCollectionExtensions
{
    public static IServiceCollection AddSecrets<T>(this IServiceCollection services, Func<IServiceProvider, T> secretVaultFactory)
        where T : ISecretVault
    {
        services.TryAddSingleton(typeof(ISecretVault), (s) => secretVaultFactory(s));
        services.TryAddSingleton(secretVaultFactory);
        return services;
    }

    public static IServiceCollection AddSecrets(this IServiceCollection services, SecretVaultOptions options)
    {
        services.TryAddSingleton(typeof(ISecretVault), options.SecretVaultType);
        services.TryAddSingleton(options.SecretVaultType, options.SecretVaultType);
        services.TryAddSingleton(options);
        return services;
    }
}