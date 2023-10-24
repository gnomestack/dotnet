namespace GnomeStack;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class FluentBuilderAttribute : Attribute
{
    public FluentBuilderAttribute(string? name = null)
    {
        this.Name = name;
    }

    public string? Name { get; set; }
}