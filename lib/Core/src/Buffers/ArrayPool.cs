using System.Buffers;
using System.Runtime.CompilerServices;

namespace GnomeStack.Buffers;

/// <summary>
/// Provides an ArrayPool that can be imported with a `using static` directive.
/// </summary>
public static class ArrayPool
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArrayPool<T> Create<T>()
    {
        return ArrayPool<T>.Create();
    }

    /// <summary>
    /// Retrieves a one dimensional array from the shared pool.
    /// </summary>
    /// <param name="minimumLength">The minimum length of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The rented one dimensional array from the shared poo.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <paramref name="minimumLength"/> is less than zero.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Rent<T>(int minimumLength)
    {
        return ArrayPool<T>.Shared.Rent(minimumLength);
    }

    /// <summary>
    /// Returns a one dimensional rented array returned from <see cref="Rent{T}"/> method to the
    /// shared pool.
    /// </summary>
    /// <param name="array">A rented array to return to the shared pool.</param>
    /// <param name="clearArray">
    /// Instructs the pool to clear the contents of the array.
    /// When the contents of an array are not cleared, the contents are available
    /// when the array is rented again.
    /// </param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the array is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Return<T>(T[] array, bool clearArray = false)
    {
        ArrayPool<T>.Shared.Return(array, clearArray);
    }
}