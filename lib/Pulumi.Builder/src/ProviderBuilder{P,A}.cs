using System.Diagnostics.CodeAnalysis;

using GnomeStack.Builder;

namespace Pulumi;

[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1314:Type parameter names should begin with T")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class ProviderBuilder<P, A> : Builder<A>
{
    protected ProviderBuilder()
    {
    }

    protected ProviderBuilder(string providerName)
    {
        this.ProviderName = providerName;
    }

    protected ProviderBuilder(string providerName, A instance)
    {
        this.ProviderName = providerName;
        this.Instance = instance;
    }

    protected string? ProviderName { get; set; }

    protected virtual CustomResourceOptions? Options { get; set; }

    public virtual P Build()
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