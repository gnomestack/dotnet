using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Standard;

/// <summary>
/// Provides methods for working with arrays that use the <see langword="ref"/> keyword
/// to act on the existing array reference.
/// </summary>
public static class ArrayRef
{
    /// <summary>
    /// Pushs the item to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first item to Push.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, T element1)
    {
        var copy = new T[array.Length + 1];
        Array.Copy(array, copy, array.Length);
        copy[^1] = element1;
        array = copy;
    }

    /// <summary>
    /// Pushs two items to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first item to Push.</param>
    /// <param name="element2">The second item to Push.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, T element1, T element2)
    {
        var copy = new T[array.Length + 2];
        Array.Copy(array, copy, array.Length);
        copy[^2] = element1;
        copy[^1] = element2;
        array = copy;
    }

    /// <summary>
    /// Pushs three items to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="element1">The first item to Push.</param>
    /// <param name="element2">The second item to Push.</param>
    /// <param name="element3">The third item to Push.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, T element1, T element2, T element3)
    {
        var copy = new T[array.Length + 3];
        Array.Copy(array, copy, array.Length);
        copy[^3] = element1;
        copy[^2] = element2;
        copy[^1] = element3;
        array = copy;
    }

    /// <summary>
    /// Pushs elements to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to Push at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, params T[] elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, copy, array.Length);
        Array.Copy(elements, 0, copy, array.Length, elements.Length);
        array = copy;
    }

    /// <summary>
    /// Pushs elements in a span to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to Push at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, Span<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, copy, array.Length);
        elements.CopyTo(copy.AsSpan(array.Length));
        array = copy;
    }

    /// <summary>
    /// Pushs elements in a readonly span to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to Push at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, ReadOnlySpan<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, copy, array.Length);
        elements.CopyTo(copy.AsSpan(array.Length));
        array = copy;
    }

    /// <summary>
    /// Pushs elements in an enumerable object to the end of the array and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="elements">The elements to Push at the end of the array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Push<T>(ref T[] array, IEnumerable<T> elements)
    {
        switch (elements)
        {
            case T[] array1:
                Push(ref array, array1.AsSpan());
                break;

            case List<T> list:
                {
                    var copy = new T[array.Length + list.Count];
                    Array.Copy(array, copy, array.Length);
                    list.CopyTo(copy, array.Length);
                    array = copy;
                }

                break;

            case IReadOnlyCollection<T> readOnlyCollection:
                {
                    var copy = new T[array.Length + readOnlyCollection.Count];
                    Array.Copy(array, copy, array.Length);
                    var j = array.Length;
                    foreach (var item in readOnlyCollection)
                        copy[j++] = item;

                    array = copy;
                }

                break;

            default:
#if !NETLEGACY
                if (elements.TryGetNonEnumeratedCount(out var count))
                {
                    var copy = new T[array.Length + count];
                    Array.Copy(array, copy, array.Length);
                    var j = array.Length;
                    foreach (var item in elements)
                        copy[j++] = item;

                    array = copy;
                    break;
                }
#endif
                Push(ref array, elements.ToArray().AsSpan());
                break;
        }
    }

    /// <summary>
    /// Increases the size of the of the one dimensional array by the amount given.
    /// </summary>
    /// <param name="array">The one dimensional array reference to resize.</param>
    /// <param name="amount">The additional amount to grow the array against the current length.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the amount added to the length is less than zero
    /// or exceeds the maximum length of an array.
    /// </exception>
    public static void Grow<T>([NotNull] ref T[]? array, int amount = 1)
    {
        var l = array?.Length ?? 0;
        var newSize = l + amount;
        if (newSize < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, $"Amount ({amount}) + Length ({l}) must not be negative");

#if NET6_0_OR_GREATER
        if (newSize > Array.MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                amount,
                $"Amount + Length must not exceed the max length of an array");
        }
#endif

        Array.Resize(ref array, l + amount);
    }

    /// <summary>
    /// Inserts two elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="element1">The first element to Push.</param>
    /// <param name="element2">The second element to Push.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Insert<T>(ref T[] array, int index, T element1, T element2)
    {
        var copy = new T[array.Length + 2];
        Array.Copy(array, 0, copy, 0, index);
        copy[index] = element1;
        copy[index + 1] = element2;
        Array.Copy(array, index, copy, index + 2, array.Length - index);
        array = copy;
    }

    /// <summary>
    /// Inserts three elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="element1">The first element to Push.</param>
    /// <param name="element2">The second element to Push.</param>
    /// <param name="element3">The third element to Push.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Insert<T>(ref T[] array, int index, T element1, T element2, T element3)
    {
        var copy = new T[array.Length + 3];
        Array.Copy(array, 0, copy, 0, index);
        copy[index] = element1;
        copy[index + 1] = element2;
        copy[index + 2] = element3;
        Array.Copy(array, index, copy, index + 3, array.Length - index);
        array = copy;
    }

    /// <summary>
    /// Inserts elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Insert<T>(ref T[] array, int index, params T[] elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, 0, index);
        Array.Copy(elements, 0, copy, index, elements.Length);
        Array.Copy(array, index, copy, index + elements.Length, array.Length - index);
        array = copy;
    }

    /// <summary>
    /// Inserts a span of elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Insert<T>(ref T[] array, int index, Span<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, 0, index);
        elements.CopyTo(copy.AsSpan(index));
        Array.Copy(array, index, copy, index + elements.Length, array.Length - index);
        array = copy;
    }

    /// <summary>
    /// Inserts a readonly span of elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Insert<T>(ref T[] array, int index, ReadOnlySpan<T> elements)
    {
        var copy = new T[array.Length + elements.Length];
        Array.Copy(array, 0, copy, 0, index);
        elements.CopyTo(copy.AsSpan(index));
        Array.Copy(array, index, copy, index + elements.Length, array.Length - index);
        array = copy;
    }

    /// <summary>
    /// Inserts an enumerable of elements into the array at the specified index and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <param name="index">The zero-based position to start the insertion.</param>
    /// <param name="elements">The elements to be inserted.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Insert<T>(ref T[] array, int index, IEnumerable<T> elements)
    {
        switch (elements)
        {
            case T[] array1:
                Insert(ref array, index, array1);
                break;

            case IList<T> list:
                {
                    var copy = new T[array.Length + list.Count];
                    Array.Copy(array, 0, copy, 0, index);
                    list.CopyTo(copy, index);
                    Array.Copy(array, index, copy, index + list.Count, array.Length - index);
                    array = copy;
                }

                break;

            case IReadOnlyCollection<T> readOnlyCollection:
                {
                    var copy = new T[array.Length + readOnlyCollection.Count];
                    Array.Copy(array, 0, copy, 0, index);
                    var j = index;
                    foreach (var item in readOnlyCollection)
                        copy[j++] = item;

                    Array.Copy(array, index, copy, index + readOnlyCollection.Count, array.Length - index);
                    array = copy;
                }

                break;

            default:
#if !NETLEGACY
                if (elements.TryGetNonEnumeratedCount(out var count))
                {
                    var copy = new T[array.Length + count];
                    Array.Copy(array, 0, copy, 0, index);
                    var j = index;
                    foreach (var item in elements)
                        copy[j++] = item;

                    Array.Copy(array, index, copy, index + count, array.Length - index);
                    array = copy;
                    break;
                }
#endif

                Insert(ref array, index, elements.ToArray());
                break;
        }
    }

    /// <summary>
    /// Pops the last item in the array and returns it.
    /// </summary>
    /// <param name="array">The array to resize.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>The last item in the array.</returns>
    /// <exception cref="ArgumentException">Throws when the length of the array is zero.</exception>
    public static T Pop<T>(ref T[] array)
    {
        if (array.Length == 0)
            throw new ArgumentException("Cannot pop the last item from an empty array.");

        var item = array[^1];
        var copy = new T[array.Length - 1];
        Array.Copy(array, copy, array.Length - 1);
        array = copy;
        return item;
    }

    /// <summary>
    /// Changes the size of the of the one dimensional array to the new size.
    /// </summary>
    /// <param name="array">The one dimensional array reference to resize.</param>
    /// <param name="newSize">The size of the new array.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Resize<T>([NotNull] ref T[]? array, int newSize)
    {
        Array.Resize(ref array, newSize);
    }

    /// <summary>
    /// Shifts the first item from the array and returns it and then resizes the array.
    /// </summary>
    /// <param name="array">The one dimensional array reference that will be resized.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns>Returns the last item in the array.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the array is empty.
    /// </exception>
    public static T Shift<T>(ref T[] array)
    {
        if (array.Length == 0)
            throw new ArgumentException("Cannot shift the first item from an empty array.");

        var item = array[0];
        var copy = new T[array.Length - 1];
        Array.Copy(array, 1, copy, 0, array.Length - 1);
        array = copy;
        return item;
    }

    /// <summary>
    /// Increases the size of the of the one dimensional array by the amount given.
    /// </summary>
    /// <param name="array">The one dimensional array reference to resize.</param>
    /// <param name="amount">The additional amount to grow the array against the current length.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Shrink<T>([NotNull] ref T[]? array, int amount = 1)
    {
        var l = array?.Length ?? 0;
        var newSize = l - amount;
        if (newSize < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, $"Amount ({amount} + Length ({l}) must not be less than zero.");

#if NET6_0_OR_GREATER
        if (newSize > Array.MaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                amount,
                $"Amount + Length must not exceed the max length of an array");
        }
#endif

        Resize(ref array, newSize);
    }
}