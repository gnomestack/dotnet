namespace GnomeStack.Extras.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
    /// <summary>
    /// Clears the array of values.
    /// </summary>
    /// <param name="array">The one dimensional array to clear.</param>
    /// <param name="index">The zero-based position to start clearing values.</param>
    /// <param name="length">The number of elements to clear. If the length is less than 0, it is
    /// set to to the length of the array minus the index.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the length exceeds the length of the array.
    /// </exception>
    public static void Clear<T>(this T[] array, int index = 0, int length = -1)
    {
        if (length == -1)
            length = array.Length - index;

        if (length > array.Length - index)
        {
            throw new ArgumentOutOfRangeException(
                nameof(length),
                length,
                $"Length ({length}) + Index ({index}) must not exceed the length of the array ({array.Length}).");
        }

        Array.Clear(array, index, length);
    }
}