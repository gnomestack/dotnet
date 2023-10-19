namespace GnomeStack.Extensions.Application;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class ApplicationAttribute : Attribute
{
    public ApplicationAttribute(string name, string? version = null)
    {
        this.Name = name;
        this.Version = version;
    }

    public string Name { get; }

    public string? Version { get; }
}