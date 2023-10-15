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
using System.Text.RegularExpressions;

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that a string contains a given sub-string, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string expected to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        string expectedSubstring,
        [AllowNull] string actualString)
        => this.Contains(expectedSubstring, actualString, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a string contains a given sub-string, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string expected to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Contains(
        string expectedSubstring,
        [AllowNull] string actualString,
        StringComparison comparisonType)
    {
        if (expectedSubstring is null)
            throw new ArgumentNullException(nameof(expectedSubstring));

        if (actualString == null || actualString.IndexOf(expectedSubstring, comparisonType) < 0)
            throw new ContainsException(expectedSubstring, actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string does not contain a given sub-string, using the current culture.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string which is expected not to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the given string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        string expectedSubstring,
        [AllowNull] string actualString)
        => this.DoesNotContain(expectedSubstring, actualString, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a string does not contain a given sub-string, using the current culture.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string which is expected not to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the given string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotContain(
        string expectedSubstring,
        [AllowNull] string actualString,
        StringComparison comparisonType)
    {
        if (expectedSubstring is null)
            throw new ArgumentNullException(nameof(expectedSubstring));

        if (actualString?.IndexOf(expectedSubstring, comparisonType) >= 0)
            throw new DoesNotContainException(expectedSubstring, actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string starts with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartString">The string expected to be at the start of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        [AllowNull] string expectedStartString,
        [AllowNull] string actualString)
        => this.StartsWith(expectedStartString, actualString, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a string starts with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartString">The string expected to be at the start of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert StartsWith(
        [AllowNull] string expectedStartString,
        [AllowNull] string actualString,
        StringComparison comparisonType)
    {
        if (expectedStartString == null || actualString?.StartsWith(expectedStartString, comparisonType) != true)
            throw new StartsWithException(expectedStartString, actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string ends with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndString">The string expected to be at the end of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the string does not end with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        [AllowNull] string expectedEndString,
        [AllowNull] string actualString)
        => this.EndsWith(expectedEndString, actualString, StringComparison.CurrentCulture);

    /// <summary>
    /// Verifies that a string ends with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndString">The string expected to be at the end of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the string does not end with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert EndsWith(
        [AllowNull] string expectedEndString,
        [AllowNull] string actualString,
        StringComparison comparisonType)
    {
        if (expectedEndString == null || actualString?.EndsWith(expectedEndString, comparisonType) != true)
            throw new EndsWithException(expectedEndString, actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string matches a regular expression.
    /// </summary>
    /// <param name="expectedRegexPattern">The regex pattern expected to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="MatchesException">Thrown when the string does not match the regex pattern.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Matches(string expectedRegexPattern, [AllowNull] string actualString)
    {
        if (expectedRegexPattern is null)
            throw new ArgumentNullException(nameof(expectedRegexPattern));

        if (actualString == null || !Regex.IsMatch(actualString, expectedRegexPattern))
            throw new MatchesException(expectedRegexPattern, actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string matches a regular expression.
    /// </summary>
    /// <param name="expectedRegex">The regex expected to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="MatchesException">Thrown when the string does not match the regex.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Matches(Regex expectedRegex, [AllowNull] string actualString)
    {
        if (expectedRegex is null)
            throw new ArgumentNullException(nameof(expectedRegex));

        if (actualString == null || !expectedRegex.IsMatch(actualString))
            throw new MatchesException(expectedRegex.ToString(), actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string does not match a regular expression.
    /// </summary>
    /// <param name="expectedRegexPattern">The regex pattern expected not to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="DoesNotMatchException">Thrown when the string matches the regex pattern.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotMatch(string expectedRegexPattern, [AllowNull] string actualString)
    {
        if (expectedRegexPattern is null)
            throw new ArgumentNullException(nameof(expectedRegexPattern));

        if (actualString != null && Regex.IsMatch(actualString, expectedRegexPattern))
            throw new DoesNotMatchException(expectedRegexPattern, actualString);

        return this;
    }

    /// <summary>
    /// Verifies that a string does not match a regular expression.
    /// </summary>
    /// <param name="expectedRegex">The regex expected not to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="DoesNotMatchException">Thrown when the string matches the regex.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert DoesNotMatch(Regex expectedRegex, [AllowNull] string actualString)
    {
        if (expectedRegex is null)
            throw new ArgumentNullException(nameof(expectedRegex));

        if (actualString != null && expectedRegex.IsMatch(actualString))
            throw new DoesNotMatchException(expectedRegex.ToString(), actualString);

        return this;
    }

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual)
        => this.Equal(expected, actual, false, false, false);

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual,
        bool ignoreCase)
        => this.Equal(expected, actual, ignoreCase, false, false);

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual,
        bool ignoreCase,
        bool ignoreLineEndingDifferences)
        => this.Equal(expected, actual, ignoreCase, ignoreLineEndingDifferences, false);

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true" />, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences)
    {
        if (expected == null && actual == null)
            return this;

        if (expected == null || actual == null)
            throw new EqualException(expected, actual, -1, -1);

        return this.Equal(expected.AsSpan(), actual.AsSpan(), ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences);
    }
}