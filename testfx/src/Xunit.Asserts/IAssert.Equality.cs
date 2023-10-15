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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that two objects are equal, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal<T>(T expected, T actual);

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
    IAssert Equal<T>([AllowNull] T[] expected, [AllowNull] T[] actual)
        where T : unmanaged, IEquatable<T>;

    /// <summary>
    /// Verifies that two objects are equal, using a custom equatable comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="comparer">The comparer used to compare the two objects.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal<T>([AllowNull] T expected, [AllowNull] T actual, IEqualityComparer<T?> comparer);

    /// <summary>
    /// Verifies that two <see cref="DateTime"/> values are equal, within the precision
    /// given by <paramref name="precision"/>.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The allowed difference in time where the two dates are considered equal.</param>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(DateTime expected, DateTime actual, TimeSpan precision);

    /// <summary>
    /// Verifies that two <see cref="decimal"/> values are equal, within the number of decimal
    /// places given by <paramref name="precision"/>. The values are rounded before comparison.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-28).</param>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(decimal expected, decimal actual, int precision);

    /// <summary>
    /// Verifies that two <see cref="double"/> values are equal, within the number of decimal
    /// places given by <paramref name="precision"/>. The values are rounded before comparison.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-15).</param>
    /// <exception cref="EqualException">Thrown when the values are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(double expected, double actual, int precision);

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
    IAssert Equal(double expected, double actual, int precision, MidpointRounding rounding);

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
    IAssert Equal(double expected, double actual, double tolerance);

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
    IAssert Equal(float expected, float actual, float tolerance);

    /// <summary>
    /// Verifies that two arrays of unmanaged type T are not equal, using Span&lt;T&gt;.SequenceEqual.
    /// </summary>
    /// <typeparam name="T">The type of items whose arrays are to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="NotEqualException">Thrown when the arrays are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual<T>([AllowNull] T[] expected, [AllowNull] T[] actual)
        where T : unmanaged, IEquatable<T>;

    /// <summary>
    /// Verifies that two objects are not equal, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual<T>([AllowNull] T expected, [AllowNull] T actual);

    /// <summary>
    /// Verifies that two objects are not equal, using a custom equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="comparer">The comparer used to examine the objects.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual<T>([AllowNull] T expected, [AllowNull] T actual, IEqualityComparer<T?> comparer);

    /// <summary>
    /// Verifies that two <see cref="decimal"/> values are not equal, within the number of decimal
    /// places given by <paramref name="precision"/>.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-28).</param>
    /// <exception cref="EqualException">Thrown when the values are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual(decimal expected, decimal actual, int precision);

    /// <summary>
    /// Verifies that two <see cref="double"/> values are not equal, within the number of decimal
    /// places given by <paramref name="precision"/>.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="precision">The number of decimal places (valid values: 0-15).</param>
    /// <exception cref="EqualException">Thrown when the values are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual(double expected, double actual, int precision);

    /// <summary>
    /// Verifies that two objects are strictly equal, using the type's default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StrictEqual<T>([AllowNull] T expected, [AllowNull] T actual);

    /// <summary>
    /// Verifies that two objects are strictly not equal, using the type's default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotStrictEqual<T>([AllowNull] T expected, [AllowNull] T actual);
}