using System.Runtime.CompilerServices;

#pragma warning disable CS8601
namespace GnomeStack.Extras.Object;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ObjectExtensions
{
    /// <summary>
    /// Returns a string representation of the object or an empty string if the object is null.
    /// </summary>
    /// <param name="value">The object to call ToString.</param>
    /// <returns>The string represention of the object or an empty string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToSafeString(this object? value)
    {
        return value?.ToString() ?? string.Empty;
    }
}