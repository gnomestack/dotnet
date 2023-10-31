namespace GnomeStack.Extras.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
    /// <summary>
    /// Creates a slice of the given array as a <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="array">The one dimensional array to slice from.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A <see cref="Span{T}"/>.</returns>
    public static Span<T> Slice<T>(this T[] array)
        => Slice(array, 0, array.Length);

    /// <summary>
    /// Creates a slice of the given array as a <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="array">The one dimensional array to slice from.</param>
    /// <param name="start">The zero-based position to start the slice.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A <see cref="Span{T}"/>.</returns>
    public static Span<T> Slice<T>(this T[] array, int start)
        => Slice(array, start, array.Length - start);

    /// <summary>
    /// Creates a slice of the given array as a <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="array">The one dimensional array to slice from.</param>
    /// <param name="start">The zero-based position to start the slice.</param>
    /// <param name="length">The number of elements to include in the slice.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A <see cref="Span{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="array"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="start"/> or <paramref name="length"/> is less than zero.
    /// Thrown when the <paramref name="start"/> plus <paramref name="length"/> is greater than
    /// <paramref name="array"/>'s length.
    /// </exception>
    public static Span<T> Slice<T>(this T[] array, int start, int length)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (start < 0)
            throw new ArgumentOutOfRangeException(nameof(start));

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        if ((start + length) > array.Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        return array.AsSpan(start, length);
    }
}