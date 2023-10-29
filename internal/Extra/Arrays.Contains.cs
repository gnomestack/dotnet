using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace GnomeStack.Extras.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
    /// <summary>
    /// Determines whether the array contains the specified item.
    /// </summary>
    /// <param name="array">The array to check.</param>
    /// <param name="item">The item.</param>
    /// <typeparam name="T">The element type.</typeparam>
    /// <returns>Returns <see langword="true" /> when the item is found in the array; otherwise, <see langword="false" />.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this T[] array, T item)
        => Array.IndexOf(array, item) != -1;

    public static bool Contains<T>(this T[] array, T item, IEqualityComparer<T> comparer)
    {
        if (comparer == null)
            return Contains(array, item);

        for (var i = 0; i < array.Length; i++)
        {
            if (comparer.Equals(array[i], item))
                return true;
        }

        return false;
    }

    public static bool Contains(this string[] array, string item, StringComparer comparer)
    {
        if (comparer == null)
            return Contains(array, item);

        for (var i = 0; i < array.Length; i++)
        {
            if (comparer.Equals(array[i], item))
                return true;
        }

        return false;
    }
}