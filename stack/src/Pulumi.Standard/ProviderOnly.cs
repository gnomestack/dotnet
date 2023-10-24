
namespace Pulumi;

public struct ProviderOnly
{
    public ProviderOnly(ProviderResource provider)
    {
        this.Provider = provider;
    }

    public ProviderResource Provider { get; }

    public static implicit operator ProviderResource(ProviderOnly providerOnly)
    {
        return providerOnly.Provider;
    }

    public static implicit operator ProviderOnly(ProviderResource provider)
    {
        return new ProviderOnly(provider);
    }

    public static implicit operator CustomResourceOptions(ProviderOnly providerOnly)
    {
        return new CustomResourceOptions
        {
            Provider = providerOnly.Provider,
        };
    }

    public static implicit operator InvokeOptions(ProviderOnly providerOnly)
    {
        return new InvokeOptions
        {
            Provider = providerOnly.Provider,
        };
    }
}