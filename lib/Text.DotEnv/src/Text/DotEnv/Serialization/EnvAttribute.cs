namespace GnomeStack.Text.DotEnv.Serialization;

[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Property,
    AllowMultiple = false,
    Inherited = true)]
public class EnvAttribute : Attribute
{
    public EnvAttribute()
    {
    }

    public string? Name { get; set; }

    public int Order { get; set; } = int.MaxValue;

    public bool IsSecret { get; set; } = true;
}