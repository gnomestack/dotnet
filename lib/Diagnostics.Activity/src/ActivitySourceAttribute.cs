namespace GnomeStack.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class ActivitySourceAttribute : Attribute
{
    public ActivitySourceAttribute(string name, string? version = null)
    {
        this.Name = name;
        this.Version = version ?? "1.0.0";
    }

    public string Name { get; }

    public string Version { get; }
}