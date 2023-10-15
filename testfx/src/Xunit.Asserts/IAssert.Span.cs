using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        Span<char> expectedSubSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        Span<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(
        Span<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(
        Span<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(
        ReadOnlySpan<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(
        ReadOnlySpan<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(
        Span<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(
        Span<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(
        ReadOnlySpan<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(
        ReadOnlySpan<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        Span<char> expectedStartSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        Span<char> expectedStartSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        Span<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        Span<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        Span<char> expectedEndSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        Span<char> expectedEndSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        Span<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        Span<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        Span<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);
}