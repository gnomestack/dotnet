using System.Diagnostics;
using System.Reflection;

namespace GnomeStack.Diagnostics;

public static class ActivitySourceFactory
{
    public static ActivitySource Create(string name, string? version)
    {
        return new ActivitySource(name, version);
    }

    public static ActivitySource CreateFromAssembly<T>()
        => CreateFromAssembly(typeof(T).Assembly);

    public static ActivitySource CreateFromAssembly(Type type)
        => CreateFromAssembly(type.Assembly);

    public static ActivitySource CreateFromAssembly(Assembly assembly)
    {
        var attribute = assembly.GetCustomAttribute<ActivitySourceAttribute>();
        var name = assembly.GetName();
        if (attribute is not null)
        {
            return Create(attribute.Name, attribute.Version);
        }

        return Create(name.Name ?? "Unknown", name?.Version?.ToString() ?? "1.0.0");
    }

    public static ActivitySource CreateFromCallingAssembly()
        => CreateFromAssembly(Assembly.GetCallingAssembly());

    public static ActivitySource CreateFromEntryAssembly()
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly is not null)
            return CreateFromAssembly(assembly);

        return CreateFromCallingAssembly();
    }
}