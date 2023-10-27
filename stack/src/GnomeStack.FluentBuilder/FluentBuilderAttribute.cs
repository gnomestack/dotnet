namespace GnomeStack.FluentBuilder;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]

public sealed class FluentBuilderAttribute : Attribute
{
    public FluentBuilderAttribute(Type? type = null, FluentOptions options = FluentOptions.Default)
    {
        this.Type = type;
        this.Options = options;
    }

    public Type? Type { get; }

    public FluentOptions Options { get; }
}