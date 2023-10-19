using System.Diagnostics;
using System.Reflection;

namespace GnomeStack.Extensions.DiagnosticSource;

public static class ActivitySourceFactory
{
    public static ActivitySource Create(string name, string? version = null)
    {
        var source = new ActivitySource(name, version ?? "1.0.0");
        return source;
    }

    public static ActivitySource Create(Assembly assembly)
    {
        var attr = assembly.GetCustomAttribute<ActivitySourceAttribute>();
        if (attr is not null)
        {
            return new ActivitySource(attr.Name, attr.Version);
        }

        return new ActivitySource(
            assembly.GetName().Name ?? "Unknown",
            assembly.GetName().Version?.ToString() ?? "1.0.0");
    }

    public static ActivitySource Create(Type type)
    {
        var attr = type.GetCustomAttribute<ActivitySourceAttribute>();
        if (attr is not null)
            return new ActivitySource(attr.Name, attr.Version);

        return Create(type.Assembly);
    }

    public static ActivitySource Create<T>()
        => Create(typeof(T));
}