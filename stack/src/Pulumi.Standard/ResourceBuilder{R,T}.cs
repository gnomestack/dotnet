
using System.Diagnostics.CodeAnalysis;

namespace Pulumi;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
public abstract class ResourceBuilder<R, T> : ResourceArgsBuilder<T>
    where R : CustomResource
    where T : ResourceArgs, new()
{
    protected ResourceBuilder()
    {
    }

    protected ResourceBuilder(string resourceName)
    {
        this.ResourceName = resourceName;
    }

    protected ResourceBuilder(string resourceName, T value)
        : base(value)
    {
        this.ResourceName = resourceName;
    }

    protected virtual string? ResourceName { get; set; }

    protected virtual CustomResourceOptions? Options { get; set; }

    public virtual new R Build()
    {
        var instance = (R?)Activator.CreateInstance(typeof(R), this.ResourceName, this.Instance, this.Options);
        if (instance is null)
        {
            throw new InvalidOperationException($"Unable to create instance of type {typeof(R)}");
        }

        return instance;
    }
}