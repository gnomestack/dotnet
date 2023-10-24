using System.Diagnostics.CodeAnalysis;

namespace Pulumi;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1314:Type parameter names should begin with T")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class ProviderBuilder<P, A> : PulumiBuilder<A>
{
    protected ProviderBuilder()
    {
    }

    protected ProviderBuilder(string providerName)
    {
        this.ProviderName = providerName;
    }

    protected ProviderBuilder(string providerName, A instance)
        : base(instance)
    {
        this.ProviderName = providerName;
    }

    protected string? ProviderName { get; set; }

    protected virtual CustomResourceOptions? Options { get; set; }

    public virtual new P Build()
    {
        var instance = (P?)Activator.CreateInstance(
            this.Type,
            this.ProviderName,
            this.Instance,
            this.Options);

        if (instance is null)
            throw new InvalidOperationException("Unable to create instance of type {typeof(P)}");

        return instance;
    }
}