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
using System.Diagnostics.CodeAnalysis;

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Skips the current test. Used when determining whether a test should be skipped
    /// happens at runtime rather than at discovery time.
    /// </summary>
    /// <param name="reason">The message to indicate why the test was skipped.</param>
    /// <exception cref="SkipException">Thrown when the method is invoked.</exception>
    /// <exception cref="ArgumentNullException">Thrown the string is null or empty.</exception>
    [DoesNotReturn]
    public void Skip(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentNullException(nameof(reason));

        throw new SkipException(reason);
    }

    /// <summary>
    /// Will skip the current test unless <paramref name="condition"/> evaluates to <see langword="true" />.
    /// </summary>
    /// <param name="condition">When <see langword="true" />, the test will continue to run; when <see langword="false" />,
    /// the test will be skipped.</param>
    /// <param name="reason">The message to indicate why the test was skipped.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="SkipException">Thrown when the condition is not met.</exception>
    /// <exception cref="ArgumentNullException">Thrown the string is null or empty.</exception>
    public IAssert SkipUnless([DoesNotReturnIf(false)] bool condition, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentNullException(nameof(reason));

        if (!condition)
            throw new SkipException(reason);

        return this;
    }

    /// <summary>
    /// Will skip the current test when <paramref name="condition"/> evaluates to <see langword="true" />.
    /// </summary>
    /// <param name="condition">When <see langword="true" />, the test will be skipped; when <see langword="false" />,
    /// the test will continue to run.</param>
    /// <param name="reason">The message to indicate why the test was skipped.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="SkipException">Thrown when the condition is met.</exception>
    /// <exception cref="ArgumentNullException">Thrown the string is null or empty.</exception>
    public IAssert SkipWhen([DoesNotReturnIf(true)] bool condition, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentNullException(nameof(reason));

        if (condition)
            throw new SkipException(reason);

        return this;
    }
}