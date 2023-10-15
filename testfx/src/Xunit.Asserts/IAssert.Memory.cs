using System;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains<T>(
        Memory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains<T>(
        Memory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    IAssert Contains<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain<T>(
        Memory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain<T>(
        Memory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    IAssert DoesNotContain<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        Memory<char> expectedStartMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        Memory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default StringComparison.CurrentCulture comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        Memory<char> expectedStartMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        Memory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        Memory<char> expectedEndMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        Memory<char> expectedEndMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        Memory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal<T>(
        Memory<T> expectedMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    IAssert Equal<T>(
        Memory<T> expectedMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    IAssert Equal<T>(
        ReadOnlyMemory<T> expectedMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T>;

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    IAssert Equal<T>(
        ReadOnlyMemory<T> expectedMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>;
}