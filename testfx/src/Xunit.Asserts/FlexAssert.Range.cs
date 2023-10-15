using System;
using System.Collections.Generic;

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that a value is within a given range.
    /// </summary>
    /// <typeparam name="T">The type of the value to be compared.</typeparam>
    /// <param name="actual">The actual value to be evaluated.</param>
    /// <param name="low">The (inclusive) low value of the range.</param>
    /// <param name="high">The (inclusive) high value of the range.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="InRangeException">Thrown when the value is not in the given range.</exception>
    public IAssert InRange<T>(T actual, T low, T high)
        where T : IComparable
    {
        return this.InRange(actual, low, high, GetComparer<T>());
    }

    /// <summary>
    /// Verifies that a value is within a given range, using a comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value to be compared.</typeparam>
    /// <param name="actual">The actual value to be evaluated.</param>
    /// <param name="low">The (inclusive) low value of the range.</param>
    /// <param name="high">The (inclusive) high value of the range.</param>
    /// <param name="comparer">The comparer used to evaluate the value's range.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="InRangeException">Thrown when the value is not in the given range.</exception>
    public IAssert InRange<T>(T actual, T low, T high, IComparer<T> comparer)
    {
        if (comparer is null)
            throw new ArgumentNullException(nameof(comparer));

        if (comparer.Compare(low, actual) > 0 || comparer.Compare(actual, high) > 0)
            throw new InRangeException(actual, low, high);

        return this;
    }

    /// <summary>
    /// Verifies that a value is not within a given range, using the default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value to be compared.</typeparam>
    /// <param name="actual">The actual value to be evaluated.</param>
    /// <param name="low">The (inclusive) low value of the range.</param>
    /// <param name="high">The (inclusive) high value of the range.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="NotInRangeException">Thrown when the value is in the given range.</exception>
    public IAssert NotInRange<T>(T actual, T low, T high)
        where T : IComparable
    {
        return this.NotInRange(actual, low, high, GetComparer<T>());
    }

    /// <summary>
    /// Verifies that a value is not within a given range, using a comparer.
    /// </summary>
    /// <typeparam name="T">The type of the value to be compared.</typeparam>
    /// <param name="actual">The actual value to be evaluated.</param>
    /// <param name="low">The (inclusive) low value of the range.</param>
    /// <param name="high">The (inclusive) high value of the range.</param>
    /// <param name="comparer">The comparer used to evaluate the value's range.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="NotInRangeException">Thrown when the value is in the given range.</exception>
    public IAssert NotInRange<T>(T actual, T low, T high, IComparer<T> comparer)
    {
        if (comparer is null)
            throw new ArgumentNullException(nameof(comparer));

        if (comparer.Compare(low, actual) <= 0 && comparer.Compare(actual, high) <= 0)
            throw new NotInRangeException(actual, low, high);

        return this;
    }
}