using System;
using System.Diagnostics.CodeAnalysis;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Skips the current test. Used when determining whether a test should be skipped
    /// happens at runtime rather than at discovery time.
    /// </summary>
    /// <param name="reason">The message to indicate why the test was skipped.</param>
    /// <exception cref="SkipException">Thrown when the method is invoked.</exception>
    /// <exception cref="ArgumentNullException">Thrown the string is null or empty.</exception>
    [DoesNotReturn]
    void Skip(string reason);

    /// <summary>
    /// Will skip the current test unless <paramref name="condition"/> evaluates to <see langword="true" />.
    /// </summary>
    /// <param name="condition">When <see langword="true" />, the test will continue to run; when <see langword="false" />,
    /// the test will be skipped.</param>
    /// <param name="reason">The message to indicate why the test was skipped.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="SkipException">Thrown when the condition is not met.</exception>
    /// <exception cref="ArgumentNullException">Thrown the string is null or empty.</exception>
    IAssert SkipUnless([DoesNotReturnIf(false)] bool condition, string reason);

    /// <summary>
    /// Will skip the current test when <paramref name="condition"/> evaluates to <see langword="true" />.
    /// </summary>
    /// <param name="condition">When <see langword="true" />, the test will be skipped; when <see langword="false" />,
    /// the test will continue to run.</param>
    /// <param name="reason">The message to indicate why the test was skipped.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="SkipException">Thrown when the condition is met.</exception>
    /// <exception cref="ArgumentNullException">Thrown the string is null or empty.</exception>
    IAssert SkipWhen([DoesNotReturnIf(true)] bool condition, string reason);
}