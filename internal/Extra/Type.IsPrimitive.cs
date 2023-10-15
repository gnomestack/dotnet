namespace GnomeStack.Extra.Reflection;

#pragma warning disable SA1649
#if DFX_CORE
public
#else
internal
#endif
    static class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is primitive.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns><see langword="true" /> if the type is a primitive; otherwise, <see langword="false"/>.</returns>
    public static bool IsPrimitive(this Type type)
    {
        if (type == typeof(string))
            return true;

        return type is { IsValueType: true, IsPrimitive: true };
    }
}