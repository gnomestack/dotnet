using System;

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    // NOTE: there is an implicit conversion operator on Memory<T> to ReadOnlyMemory<T> - however, I have found that the compiler sometimes struggles
    // with identifying the proper methods to use, thus I have overloaded quite a few of the assertions in terms of supplying both
    // Memory and ReadOnlyMemory based methods

    // NOTE: we could consider StartsWith<T> and EndsWith<T> with both arguments as ReadOnlyMemory<T>, and use the Memory extension methods on Span to check difference
    // BUT: the current Exceptions for startswith and endswith are only built for string types, so those would need a change (or new non-string versions created).

    // NOTE: Memory and ReadonlyMemory, even when null, are coerced into empty arrays of the specified type when a value is grabbed. Thus some of the code below
    // for null scenarios looks odd, but is safe and correct.

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory) =>
        this.Contains((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.Contains((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory) =>
        this.Contains(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.Contains(expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.Contains((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType) =>
        this.Contains((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.Contains(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType)
    {
        return this.Contains(expectedSubMemory.Span, actualMemory.Span, comparisonType);
    }

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains<T>(
        Memory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T> =>
        this.Contains((ReadOnlyMemory<T>)expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains<T>(
        Memory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T> =>
        this.Contains((ReadOnlyMemory<T>)expectedSubMemory, actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T> =>
        this.Contains(expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

    /// <summary>
    /// Verifies that a Memory contains a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory.</exception>
    public IAssert Contains<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>
    {
        if (actualMemory.Span.IndexOf(expectedSubMemory.Span) < 0)
            throw new ContainsException(expectedSubMemory, actualMemory);

        return this;
    }

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory) =>
        this.DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory) =>
        this.DoesNotContain(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.DoesNotContain(expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        Memory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType) =>
        this.DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.DoesNotContain(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain(
        ReadOnlyMemory<char> expectedSubMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType)
    {
        return this.DoesNotContain(expectedSubMemory.Span, actualMemory.Span, comparisonType);
    }

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain<T>(
        Memory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T> =>
        this.DoesNotContain((ReadOnlyMemory<T>)expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain<T>(
        Memory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T> =>
        this.DoesNotContain((ReadOnlyMemory<T>)expectedSubMemory, actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T> =>
        this.DoesNotContain(expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

    /// <summary>
    /// Verifies that a Memory does not contain a given sub-Memory.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory.</exception>
    public IAssert DoesNotContain<T>(
        ReadOnlyMemory<T> expectedSubMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>
    {
        if (actualMemory.Span.IndexOf(expectedSubMemory.Span) > -1)
            throw new DoesNotContainException(expectedSubMemory, actualMemory);

        return this;
    }

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        Memory<char> expectedStartMemory,
        Memory<char> actualMemory) =>
        this.StartsWith((ReadOnlyMemory<char>)expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        Memory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.StartsWith((ReadOnlyMemory<char>)expectedStartMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        Memory<char> actualMemory) =>
        this.StartsWith(expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the default StringComparison.CurrentCulture comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.StartsWith(expectedStartMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        Memory<char> expectedStartMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.StartsWith((ReadOnlyMemory<char>)expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        Memory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType) =>
        this.StartsWith((ReadOnlyMemory<char>)expectedStartMemory, actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.StartsWith(expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory.</exception>
    public IAssert StartsWith(
        ReadOnlyMemory<char> expectedStartMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType)
    {
        return this.StartsWith(expectedStartMemory.Span, actualMemory.Span, comparisonType);
    }

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        Memory<char> expectedEndMemory,
        Memory<char> actualMemory) =>
        this.EndsWith((ReadOnlyMemory<char>)expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        Memory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.EndsWith((ReadOnlyMemory<char>)expectedEndMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        Memory<char> actualMemory) =>
        this.EndsWith(expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.EndsWith(expectedEndMemory, actualMemory, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        Memory<char> expectedEndMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.EndsWith((ReadOnlyMemory<char>)expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        Memory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType) =>
        this.EndsWith((ReadOnlyMemory<char>)expectedEndMemory, actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        Memory<char> actualMemory,
        StringComparison comparisonType) =>
        this.EndsWith(expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

    /// <summary>
    /// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory.</param>
    /// <param name="actualMemory">The Memory to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory.</exception>
    public IAssert EndsWith(
        ReadOnlyMemory<char> expectedEndMemory,
        ReadOnlyMemory<char> actualMemory,
        StringComparison comparisonType)
    {
        return this.EndsWith(expectedEndMemory.Span, actualMemory.Span, comparisonType);
    }

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            false,
            false,
            false);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            ignoreCase,
            false,
            false);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);

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
    public IAssert Equal(
        Memory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            actualMemory,
            false,
            false,
            false);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            actualMemory,
            ignoreCase,
            false,
            false);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            actualMemory,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);

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
    public IAssert Equal(
        Memory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences) =>
        this.Equal(
            (ReadOnlyMemory<char>)expectedMemory,
            actualMemory,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory) =>
        this.Equal(
            expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            false,
            false,
            false);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase) =>
        this.Equal(
            expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            ignoreCase,
            false,
            false);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences) =>
        this.Equal(
            expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);

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
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        Memory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences) =>
        this.Equal(
            expectedMemory,
            (ReadOnlyMemory<char>)actualMemory,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory)
    {
        return this.Equal(
            expectedMemory.Span,
            actualMemory.Span,
            false,
            false,
            false);
    }

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase)
    {
        return this.Equal(
            expectedMemory.Span,
            actualMemory.Span,
            ignoreCase,
            false,
            false);
    }

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences)
    {
        return this.Equal(
            expectedMemory.Span,
            actualMemory.Span,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);
    }

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
    public IAssert Equal(
        ReadOnlyMemory<char> expectedMemory,
        ReadOnlyMemory<char> actualMemory,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences)
    {
        return this.Equal(
            expectedMemory.Span,
            actualMemory.Span,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);
    }

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal<T>(
        Memory<T> expectedMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T> =>
        this.Equal((ReadOnlyMemory<T>)expectedMemory, (ReadOnlyMemory<T>)actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    public IAssert Equal<T>(
        Memory<T> expectedMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T> =>
        this.Equal((ReadOnlyMemory<T>)expectedMemory, actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    public IAssert Equal<T>(
        ReadOnlyMemory<T> expectedMemory,
        Memory<T> actualMemory)
        where T : IEquatable<T> =>
        this.Equal(expectedMemory, (ReadOnlyMemory<T>)actualMemory);

    /// <summary>
    /// Verifies that two Memory values are equivalent.
    /// </summary>
    /// <typeparam name="T">The object type from which the contiguous region of memory will be read.</typeparam>
    /// <param name="expectedMemory">The expected Memory value.</param>
    /// <param name="actualMemory">The actual Memory value.</param>
    /// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    public IAssert Equal<T>(
        ReadOnlyMemory<T> expectedMemory,
        ReadOnlyMemory<T> actualMemory)
        where T : IEquatable<T>
    {
        if (!expectedMemory.Span.SequenceEqual(actualMemory.Span))
            return this.Equal<object>(expectedMemory.Span.ToArray(), actualMemory.Span.ToArray());

        return this;
    }
}