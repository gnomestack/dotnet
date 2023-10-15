namespace GnomeStack.Text.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FormatAttribute : Attribute
{
    public FormatAttribute(string format)
    {
        this.Format = format;
    }

    public string Format { get; set; }
}