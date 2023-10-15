/*
Copyright (c) .NET Foundation and Contributors
All Rights Reserved

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Xunit.Sdk;

// ReSharper disable All
namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that two objects are equal, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal<T>([AllowNull] T expected, [AllowNull] T actual)
    {
        return this.Equal(expected, actual, new AssertEqualityComparer<T?>());
    }

    /// <summary>
    /// Verifies that two arrays of unmanaged type T are equal, using Span&lt;T&gt;.SequenceEqual.
    /// </summary>
    /// <typeparam name="T">The type of items whose arrays are to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="EqualException">Thrown when the arrays are not equal.</exception>
    /// <remarks>
    ///     <para>
    ///     If Span&lt;T&gt;.SequenceEqual fails, a call to Assert.Equal(object, object) is made,
    ///     to provide a more meaningful error message.
    ///     </para>
    /// </remarks>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal<T>([AllowNull] T[] expected, [AllowNull] T[] actual)
        where T : unmanaged, IEquatable<T>
    {
        if (expected == null && actual == null)
            return this;

        // Call into Equal<object> so we get proper formatting of the sequence
        if (expected == null || actual == null || !expected.AsSpan().SequenceEqual(actual))
            return this.Equal<object>(expected, actual);

        return this;
    }

    /// <summary>
    /// Verifies that two objects are equal, using a custom equatable comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="comparer">The comparer used to compare the two objects.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal<T>([AllowNull] T expected, [AllowNull] T actual, IEqualityComparer<T?> comparer)
    {
        var expectedAsIEnum = expected as IEnumerable;
        var actualAsIEnum = actual as IEnumerable;

        // If both are IEnumerable (or null), see if we got an AssertEqualityComparer<T>, so that
        // we can invoke it to get the mismatched index.
        if (((expectedAsIEnum != null && (actual == null || actualAsIEnum != null)) ||
             (actualAsIEnum != null && expected == null)) && comparer is AssertEqualityComparer<T> aec
                                                          && !aec.Equals(expected, actual, out var mismatchedIndex))
        {
            if (mismatchedIndex.HasValue)
                throw Equal2Exception.FromEnumerable(expectedAsIEnum, actualAsIEnum, mismatchedIndex.Value);
            else
                throw new EqualException(expected, actual);
        }

        if (!comparer.Equals(expected, actual))
            throw new EqualException(expected, actual);
        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="DateTime"/> values are equal, within the precision
    /// given by <paramref name="precision"/>.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The allowed difference in time where the two dates are considered equal.</param>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(DateTime expected, DateTime actual, TimeSpan precision)
    {
        var difference = (expected - actual).Duration();
        if (difference > precision)
        {
            throw new EqualException(
                string.Format(CultureInfo.CurrentCulture, "{0} ", expected),
                string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} difference {1} is larger than {2}",
                    actual,
                    difference.ToString(),
                    precision.ToString()));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="decimal"/> values are equal, within the number of decimal
    /// places given by <paramref name="precision"/>. The values are rounded before comparison.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-28).</param>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(decimal expected, decimal actual, int precision)
    {
        var expectedRounded = Math.Round(expected, precision);
        var actualRounded = Math.Round(actual, precision);

        if (expectedRounded != actualRounded)
        {
            throw new EqualException(
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="double"/> values are equal, within the number of decimal
    /// places given by <paramref name="precision"/>. The values are rounded before comparison.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-15).</param>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(double expected, double actual, int precision)
    {
        var expectedRounded = Math.Round(expected, precision);
        var actualRounded = Math.Round(actual, precision);

        if (!Equals(expectedRounded, actualRounded))
        {
            throw new EqualException(
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="double"/> values are equal, within the number of decimal
    /// places given by <paramref name="precision"/>. The values are rounded before comparison.
    /// The rounding method to use is given by <paramref name="rounding" />.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-15).</param>
    /// <param name="rounding">Rounding method to use to process a number that is midway between two numbers.</param>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(double expected, double actual, int precision, MidpointRounding rounding)
    {
        var expectedRounded = Math.Round(expected, precision, rounding);
        var actualRounded = Math.Round(actual, precision, rounding);

        if (!Equals(expectedRounded, actualRounded))
        {
            throw new EqualException(
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="double"/> values are equal, within the tolerance given by
    /// <paramref name="tolerance"/> (positive or negative).
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="tolerance">The allowed difference between values.</param>
    /// <exception cref="ArgumentException">Thrown when supplied tolerance is invalid.</exception>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(double expected, double actual, double tolerance)
    {
        if (double.IsNaN(tolerance) || double.IsNegativeInfinity(tolerance) || tolerance < 0.0)
            throw new ArgumentException("Tolerance must be greater than or equal to zero", nameof(tolerance));

        if (!(double.Equals(expected, actual) || Math.Abs(expected - actual) <= tolerance))
        {
            throw new EqualException(
                string.Format(CultureInfo.CurrentCulture, "{0:G17}", expected),
                string.Format(CultureInfo.CurrentCulture, "{0:G17}", actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="float"/> values are equal, within the tolerance given by
    /// <paramref name="tolerance"/> (positive or negative).
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="tolerance">The allowed difference between values.</param>
    /// <exception cref="ArgumentException">Thrown when supplied tolerance is invalid.</exception>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(float expected, float actual, float tolerance)
    {
        if (float.IsNaN(tolerance) || float.IsNegativeInfinity(tolerance) || tolerance < 0.0)
            throw new ArgumentException("Tolerance must be greater than or equal to zero", nameof(tolerance));

        if (!(float.Equals(expected, actual) || Math.Abs(expected - actual) <= tolerance))
        {
            throw new EqualException(
                string.Format(CultureInfo.CurrentCulture, "{0:G9}", expected),
                string.Format(CultureInfo.CurrentCulture, "{0:G9}", actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two arrays of unmanaged type T are not equal, using Span&lt;T&gt;.SequenceEqual.
    /// </summary>
    /// <typeparam name="T">The type of items whose arrays are to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="NotEqualException">Thrown when the arrays are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert NotEqual<T>([AllowNull] T[] expected, [AllowNull] T[] actual)
        where T : unmanaged, IEquatable<T>
    {
        // Call into NotEqual<object> so we get proper formatting of the sequence
        if (expected == null && actual == null)
            return this.NotEqual<object>(expected, actual);
        if (expected == null || actual == null)
            return this;
        if (expected.AsSpan().SequenceEqual(actual))
            this.NotEqual<object>(expected, actual);

        return this;
    }

    /// <summary>
    /// Verifies that two objects are not equal, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert NotEqual<T>([AllowNull] T expected, [AllowNull] T actual)
    {
        return this.NotEqual(expected, actual, new AssertEqualityComparer<T?>());
    }

    /// <summary>
    /// Verifies that two objects are not equal, using a custom equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="comparer">The comparer used to examine the objects.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert NotEqual<T>([AllowNull] T expected, [AllowNull] T actual, IEqualityComparer<T?> comparer)
    {
        if (comparer is null)
            throw new ArgumentNullException(nameof(comparer));

        if (comparer.Equals(expected, actual))
            throw new NotEqualException(ArgumentFormatter.Format(expected), ArgumentFormatter.Format(actual));

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="decimal"/> values are not equal, within the number of decimal
    /// places given by <paramref name="precision"/>.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-28).</param>
    /// <exception cref="EqualException">Thrown when the values are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert NotEqual(decimal expected, decimal actual, int precision)
    {
        var expectedRounded = Math.Round(expected, precision);
        var actualRounded = Math.Round(actual, precision);

        if (expectedRounded == actualRounded)
        {
            throw new NotEqualException(
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two <see cref="double"/> values are not equal, within the number of decimal
    /// places given by <paramref name="precision"/>.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-15).</param>
    /// <exception cref="EqualException">Thrown when the values are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert NotEqual(double expected, double actual, int precision)
    {
        var expectedRounded = Math.Round(expected, precision);
        var actualRounded = Math.Round(actual, precision);

        if (Equals(expectedRounded, actualRounded))
        {
            throw new NotEqualException(
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
                string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual));
        }

        return this;
    }

    /// <summary>
    /// Verifies that two objects are strictly equal, using the type's default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StrictEqual<T>([AllowNull] T expected, [AllowNull] T actual) =>
        this.Equal(expected, actual, EqualityComparer<T?>.Default);

    /// <summary>
    /// Verifies that two objects are strictly not equal, using the type's default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert NotStrictEqual<T>([AllowNull] T expected, [AllowNull] T actual) =>
        this.NotEqual(expected, actual, EqualityComparer<T?>.Default);
}