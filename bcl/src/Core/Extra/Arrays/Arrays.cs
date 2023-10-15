using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GnomeStack.Extra.Arrays;

public static partial class ArrayExtensions
{
    public static T At<T>(this T[] array, int index)
        => array[index];

    /// <summary>
    /// Concatenates the two arrays into a single new array.
    /// </summary>
    /// <param name="array1">The first array to concatenate.</param>
    /// <param name="array2">The second array to concatenate.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new array.</returns>
    public static T[] Concat<T>(this T[] array1, T[] array2)
    {
        var result = new T[array1.Length + array2.Length];
        Array.Copy(array1, result, array1.Length);
        Array.Copy(array2, 0, result, array1.Length, array2.Length);
        return result;
    }

    /// <summary>
    /// Concatenates the two arrays into a single new array.
    /// </summary>
    /// <param name="array1">The first array to concatenate.</param>
    /// <param name="array2">The second array to concatenate.</param>
    /// <param name="array3">The third array to concatenate.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new array.</returns>
    public static T[] Concat<T>(this T[] array1, T[] array2, T[] array3)
    {
        var result = new T[array1.Length + array2.Length + array3.Length];
        Array.Copy(array1, result, array1.Length);
        Array.Copy(array2, 0, result, array1.Length, array2.Length);
        Array.Copy(array3, 0, result, array1.Length + array2.Length, array3.Length);
        return result;
    }

    /// <summary>
    /// Concatenates multiple arrays into a single new array.
    /// </summary>
    /// <param name="array1">The first array to concatinate.</param>
    /// <param name="arrays">The array of arrays to concatenate.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new array.</returns>
    #pragma warning disable S2368 // Prefer jagged arrays over multidimensional
    public static T[] Concat<T>(this T[] array1, params T[][] arrays)
    {
        var result = new T[arrays.Sum(a => a.Length) + array1.Length];
        var offset = 0;
        Array.Copy(array1, result, array1.Length);
        offset += array1.Length;

        foreach (var array in arrays)
        {
            Array.Copy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array)
        => Array.Sort(array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, IComparer<T>? comparer)
        => Array.Sort(array, comparer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, Comparer<T> comparer)
        => Array.Sort(array, comparer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, Comparison<T> comparison)
        => Array.Sort(array, comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, int index)
        => Array.Sort(array, index, array.Length - index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, int index, int length, IComparer<T> comparer)
        => Array.Sort(array, index, length, comparer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, int index, int length, Comparer<T> comparer)
        => Array.Sort(array, index, length, comparer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(this T[] array, int index, int length, Comparison<T> comparison)
        => Array.Sort(array, index, length, new ArrayComparer<T>(comparison));

    private sealed class ArrayComparer<T> : Comparer<T>
    {
        private readonly Comparison<T> comparison;

        public ArrayComparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        public override int Compare(T? x, T? y)
            => this.comparison(x!, y!);
    }
}