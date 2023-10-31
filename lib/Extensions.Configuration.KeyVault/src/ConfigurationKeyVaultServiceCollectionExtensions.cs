using Azure.Core;

using GnomeStack.Extensions.Configuration.KeyVault;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationKeyVaultServiceCollectionExtensions
{
    public static IConfigurationBuilder AddGsAzureKeyVault(
        this IConfigurationBuilder builder,
        Uri vaultUri,
        TokenCredential credential,
        string? prefix = null,
        string delimiter = "-")
    {
        builder.AddAzureKeyVault(vaultUri, credential, new GsKeyVaultSecretManager(prefix, delimiter));
        return builder;
    }
}