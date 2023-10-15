namespace GnomeStack.Text.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SerializationAttribute : Attribute
{
    public string? Name { get; set; }

    public int Order { get; set; }
}