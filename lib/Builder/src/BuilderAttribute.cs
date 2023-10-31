namespace GnomeStack.Builder;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]

public sealed class BuilderAttribute : Attribute
{
    public BuilderAttribute(Type? type = null, BuilderOptions options = BuilderOptions.Default)
    {
        this.Type = type;
        this.Options = options;
    }

    public Type? Type { get; }

    public BuilderOptions Options { get; }
}