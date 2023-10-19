namespace GnomeStack.Extensions.DiagnosticSource;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ActivitySourceAttribute : Attribute
{
    public ActivitySourceAttribute(string name, string? version = null)
    {
        this.Name = name;
        this.Version = version ?? "1.0.0";
    }

    public string Name { get; }

    public string Version { get; }
}