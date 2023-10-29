namespace GnomeStack.Extras.Arrays;

#if DFX_CORE
public
#else
internal
#endif
    static partial class ArrayExtensions
{
     /// <summary>
    /// Compares two arrays for equality.
    /// </summary>
    /// <param name="left">The left side of the compare.</param>
    /// <param name="right">The right side of the compare.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns><c>True</c> when both objects are equal; otherwise, <c>false</c>.</returns>
    public static bool EqualTo<T>(this T[] left, T[] right)
    {
        return EqualTo(left, right, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Compares two arrays for equality using the <paramref name="comparer"/>.
    /// </summary>
    /// <param name="left">The left side of the compare.</param>
    /// <param name="right">The right side of the compare.</param>
    /// <param name="comparer">The comparer implementation to use.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns><c>True</c> when both objects are equal; otherwise, <c>false</c>.</returns>
    public static bool EqualTo<T>(this T[] left, T[] right, IComparer<T> comparer)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left == null || right == null)
            return false;

        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            var lValue = left[i];
            var rValue = right[i];

            if (comparer.Compare(lValue, rValue) == 0)
                continue;

            return false;
        }

        return true;
    }

    /// <summary>
    /// Compares two arrays for equality using the <paramref name="comparer"/>.
    /// </summary>
    /// <param name="left">The left side of the compare.</param>
    /// <param name="right">The right side of the compare.</param>
    /// <param name="comparer">The comparison delegate to use.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns><c>True</c> when both objects are equal; otherwise, <c>false</c>.</returns>
    public static bool EqualTo<T>(this T[]? left, T[]? right, Comparison<T> comparer)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left == null || right == null)
            return false;

        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            var lValue = left[i];
            var rValue = right[i];

            if (comparer(lValue, rValue) == 0)
                continue;

            return false;
        }

        return true;
    }

    /// <summary>
    /// Compares two arrays for equality using the <paramref name="comparer"/>.
    /// </summary>
    /// <param name="left">The left side of the compare.</param>
    /// <param name="right">The right side of the compare.</param>
    /// <param name="comparer">The equality comparer implementation to use.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <returns><c>True</c> when both objects are equal; otherwise, <c>false</c>.</returns>
    public static bool EqualTo<T>(this T[]? left, T[]? right, IEqualityComparer<T> comparer)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left == null || right == null)
            return false;

        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            var lValue = left[i];
            var rValue = right[i];

            if (comparer.Equals(lValue, rValue))
                continue;

            return false;
        }

        return true;
    }
}