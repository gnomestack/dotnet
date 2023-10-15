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

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        Span<char> expectedSubSpan,
        Span<char> actualSpan) =>
        this.Contains(
            (ReadOnlySpan<char>)expectedSubSpan,
            (ReadOnlySpan<char>)actualSpan,
            StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        Span<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.Contains((ReadOnlySpan<char>)expectedSubSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan) =>
        this.Contains((ReadOnlySpan<char>)expectedSubSpan, actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType) =>
        this.Contains((ReadOnlySpan<char>)expectedSubSpan, actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan) =>
        this.Contains(expectedSubSpan, (ReadOnlySpan<char>)actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.Contains(expectedSubSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan)
    {
        if (actualSpan.IndexOf(expectedSubSpan, StringComparison.CurrentCulture) < 0)
            throw new ContainsException(expectedSubSpan.ToString(), actualSpan.ToString());

        return this;
    }

    /// <summary>
    /// Verifies that a span contains a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType)
    {
        if (actualSpan.IndexOf(expectedSubSpan, comparisonType) < 0)
            throw new ContainsException(expectedSubSpan.ToString(), actualSpan.ToString());

        return this;
    }

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains<T>(
        Span<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T> =>
        this.Contains((ReadOnlySpan<T>)expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains<T>(
        Span<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T> =>
        this.Contains((ReadOnlySpan<T>)expectedSubSpan, actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains<T>(
        ReadOnlySpan<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T> =>
        this.Contains(expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

    /// <summary>
    /// Verifies that a span contains a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains<T>(
        ReadOnlySpan<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T>
    {
        if (actualSpan.IndexOf(expectedSubSpan) < 0)
            throw new ContainsException(expectedSubSpan.ToArray(), actualSpan.ToArray());

        return this;
    }

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        Span<char> actualSpan) =>
        this.DoesNotContain(
            (ReadOnlySpan<char>)expectedSubSpan,
            (ReadOnlySpan<char>)actualSpan,
            StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.DoesNotContain((ReadOnlySpan<char>)expectedSubSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan) =>
        this.DoesNotContain(
            (ReadOnlySpan<char>)expectedSubSpan,
            actualSpan,
            StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        Span<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType) =>
        this.DoesNotContain(
            (ReadOnlySpan<char>)expectedSubSpan,
            actualSpan,
            comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.DoesNotContain(expectedSubSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        Span<char> actualSpan) =>
        this.DoesNotContain(expectedSubSpan, (ReadOnlySpan<char>)actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan)
    {
        if (actualSpan.IndexOf(expectedSubSpan, StringComparison.CurrentCulture) > -1)
            throw new DoesNotContainException(expectedSubSpan.ToString(), actualSpan.ToString());

        return this;
    }

    /// <summary>
    /// Verifies that a span does not contain a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        ReadOnlySpan<char> expectedSubSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType)
    {
        if (actualSpan.IndexOf(expectedSubSpan, comparisonType) > -1)
            throw new DoesNotContainException(expectedSubSpan.ToString(), actualSpan.ToString());

        return this;
    }

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain<T>(
        Span<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T> =>
        this.DoesNotContain((ReadOnlySpan<T>)expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain<T>(
        Span<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T> =>
        this.DoesNotContain((ReadOnlySpan<T>)expectedSubSpan, actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain<T>(
        ReadOnlySpan<T> expectedSubSpan,
        Span<T> actualSpan)
        where T : IEquatable<T> =>
        this.DoesNotContain(expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

    /// <summary>
    /// Verifies that a span does not contain a given sub-span.
    /// </summary>
    /// <typeparam name="T">T is an IEquatable object.</typeparam>
    /// <param name="expectedSubSpan">The sub-span expected not to be in the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain<T>(
        ReadOnlySpan<T> expectedSubSpan,
        ReadOnlySpan<T> actualSpan)
        where T : IEquatable<T>
    {
        if (actualSpan.IndexOf(expectedSubSpan) > -1)
            throw new DoesNotContainException(expectedSubSpan.ToArray(), actualSpan.ToArray());

        return this;
    }

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        Span<char> expectedStartSpan,
        Span<char> actualSpan) =>
        this.StartsWith(
            (ReadOnlySpan<char>)expectedStartSpan,
            (ReadOnlySpan<char>)actualSpan,
            StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        Span<char> expectedStartSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.StartsWith((ReadOnlySpan<char>)expectedStartSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        Span<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan) =>
        this.StartsWith(
            (ReadOnlySpan<char>)expectedStartSpan,
            actualSpan,
            StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        Span<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType) =>
        this.StartsWith((ReadOnlySpan<char>)expectedStartSpan, actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        Span<char> actualSpan) =>
        this.StartsWith(expectedStartSpan, (ReadOnlySpan<char>)actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.StartsWith(expectedStartSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan) =>
        this.StartsWith(expectedStartSpan, actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span starts with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartSpan">The sub-span expected to be at the start of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="StartsWithException">Thrown when the span does not start with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        ReadOnlySpan<char> expectedStartSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType)
    {
        if (!actualSpan.StartsWith(expectedStartSpan, comparisonType))
            throw new StartsWithException(expectedStartSpan.ToString(), actualSpan.ToString());

        return this;
    }

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        Span<char> expectedEndSpan,
        Span<char> actualSpan) =>
        this.EndsWith(
            (ReadOnlySpan<char>)expectedEndSpan,
            (ReadOnlySpan<char>)actualSpan,
            StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        Span<char> expectedEndSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.EndsWith((ReadOnlySpan<char>)expectedEndSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        Span<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan) =>
        this.EndsWith((ReadOnlySpan<char>)expectedEndSpan, actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        Span<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType) =>
        this.EndsWith((ReadOnlySpan<char>)expectedEndSpan, actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        Span<char> actualSpan) =>
        this.EndsWith(expectedEndSpan, (ReadOnlySpan<char>)actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        Span<char> actualSpan,
        StringComparison comparisonType) =>
        this.EndsWith(expectedEndSpan, (ReadOnlySpan<char>)actualSpan, comparisonType);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan)
        => this.EndsWith(expectedEndSpan, actualSpan, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a span ends with a given sub-span, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndSpan">The sub-span expected to be at the end of the span.</param>
    /// <param name="actualSpan">The span to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="EndsWithException">Thrown when the span does not end with the expected subspan.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        ReadOnlySpan<char> expectedEndSpan,
        ReadOnlySpan<char> actualSpan,
        StringComparison comparisonType)
    {
        if (!actualSpan.EndsWith(expectedEndSpan, comparisonType))
            throw new EndsWithException(expectedEndSpan.ToString(), actualSpan.ToString());

        return this;
    }

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            false,
            false,
            false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            ignoreCase,
            false,
            false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);

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
    public IAssert Equal(
        Span<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            actualSpan,
            false,
            false,
            false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            actualSpan,
            ignoreCase,
            false,
            false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            actualSpan,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);

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
    public IAssert Equal(
        Span<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences) =>
        this.Equal(
            (ReadOnlySpan<char>)expectedSpan,
            actualSpan,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        Span<char> actualSpan) =>
        this.Equal(
            expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            false,
            false,
            false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase) =>
        this.Equal(
            expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            ignoreCase,
            false,
            false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences) =>
        this.Equal(
            expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            ignoreCase,
            ignoreLineEndingDifferences,
            false);

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
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        Span<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences) =>
        this.Equal(
            expectedSpan,
            (ReadOnlySpan<char>)actualSpan,
            ignoreCase,
            ignoreLineEndingDifferences,
            ignoreWhiteSpaceDifferences);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan)
        => this.Equal(expectedSpan, actualSpan, false, false, false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase)
        => this.Equal(expectedSpan, actualSpan, ignoreCase, false, false);

    /// <summary>
    /// Verifies that two spans are equivalent.
    /// </summary>
    /// <param name="expectedSpan">The expected span value.</param>
    /// <param name="actualSpan">The actual span value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences)
        => this.Equal(expectedSpan, actualSpan, ignoreCase, ignoreLineEndingDifferences, false);

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
    public IAssert Equal(
        ReadOnlySpan<char> expectedSpan,
        ReadOnlySpan<char> actualSpan,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences)
    {
        // Walk the string, keeping separate indices since we can skip variable amounts of
        // data based on ignoreLineEndingDifferences and ignoreWhiteSpaceDifferences.
        var expectedIndex = 0;
        var actualIndex = 0;
        var expectedLength = expectedSpan.Length;
        var actualLength = actualSpan.Length;

        while (expectedIndex < expectedLength && actualIndex < actualLength)
        {
            var expectedChar = expectedSpan[expectedIndex];
            var actualChar = actualSpan[actualIndex];

            if (ignoreLineEndingDifferences && Util.IsLineEnding(expectedChar) && Util.IsLineEnding(actualChar))
            {
                expectedIndex = Util.SkipLineEnding(expectedSpan, expectedIndex);
                actualIndex = Util.SkipLineEnding(actualSpan, actualIndex);
            }
            else if (ignoreWhiteSpaceDifferences && Util.IsWhiteSpace(expectedChar) && Util.IsWhiteSpace(actualChar))
            {
                expectedIndex = Util.SkipWhitespace(expectedSpan, expectedIndex);
                actualIndex = Util.SkipWhitespace(actualSpan, actualIndex);
            }
            else
            {
                if (ignoreCase)
                {
                    expectedChar = char.ToUpperInvariant(expectedChar);
                    actualChar = char.ToUpperInvariant(actualChar);
                }

                if (expectedChar != actualChar)
                {
                    break;
                }

                expectedIndex++;
                actualIndex++;
            }
        }

        if (expectedIndex < expectedLength || actualIndex < actualLength)
            throw new EqualException(expectedSpan.ToString(), actualSpan.ToString(), expectedIndex, actualIndex);

        return this;
    }
}