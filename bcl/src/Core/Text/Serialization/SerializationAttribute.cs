namespace GnomeStack.Text.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SerializationAttribute : Attribute
{
    public SerializationAttribute(string? name = null)
    {
        this.Name = name;
    }

    public string? Name { get; set; }

    public int Order { get; set; }

    public StringStyle StringStyle { get; set; } = StringStyle.Default;

    public string Format { get; set; } = string.Empty;
}