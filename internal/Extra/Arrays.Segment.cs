namespace GnomeStack.Extras.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
    /// <summary>
    /// Creates a new <see cref="ArraySegment{T}"/> from the given array.
    /// </summary>
    /// <param name="array">The one dimensional array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new <see cref="ArraySegment{T}"/>.</returns>
    public static ArraySegment<T> Segment<T>(this T[] array)
        => Segment(array, 0, array.Length);

    /// <summary>
    /// Creates a new <see cref="ArraySegment{T}"/> from the given array.
    /// </summary>
    /// <param name="array">The one dimensional array.</param>
    /// <param name="start">The zero-based position that will be the start of the segment.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new <see cref="ArraySegment{T}"/>.</returns>
    public static ArraySegment<T> Segment<T>(this T[] array, int start)
        => Segment(array, start, array.Length - start);

    /// <summary>
    /// Creates a new <see cref="ArraySegment{T}"/> from the given array.
    /// </summary>
    /// <param name="array">The one dimensional array.</param>
    /// <param name="start">The zero-based position that will be the start of the segment.</param>
    /// <param name="length">The number of elements from the array to include in the segment.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>A new <see cref="ArraySegment{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="array"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="start"/> or <paramref name="length"/> is less than zero.
    /// Thrown when the <paramref name="start"/> plus <paramref name="length"/> is greater than
    /// <paramref name="array"/>'s length.
    /// </exception>
    public static ArraySegment<T> Segment<T>(this T[] array, int start, int length)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (start < 0)
            throw new ArgumentOutOfRangeException(nameof(start));

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        if ((start + length) > array.Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        return new ArraySegment<T>(array, start, length);
    }
}