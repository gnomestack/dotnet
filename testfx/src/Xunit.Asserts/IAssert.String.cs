using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that a string contains a given sub-string, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string expected to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        string expectedSubstring,
        [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string contains a given sub-string, using the given comparison type.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string expected to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains(
        string expectedSubstring,
        [AllowNull] string actualString,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a string does not contain a given sub-string, using the current culture.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string which is expected not to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the given string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        string expectedSubstring,
        [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string does not contain a given sub-string, using the current culture.
    /// </summary>
    /// <param name="expectedSubstring">The sub-string which is expected not to be in the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the given string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain(
        string expectedSubstring,
        [AllowNull] string actualString,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a string starts with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartString">The string expected to be at the start of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        [AllowNull] string expectedStartString,
        [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string starts with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedStartString">The string expected to be at the start of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the string does not start with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert StartsWith(
        [AllowNull] string expectedStartString,
        [AllowNull] string actualString,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a string ends with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndString">The string expected to be at the end of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the string does not end with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        [AllowNull] string expectedEndString,
        [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string ends with a given string, using the given comparison type.
    /// </summary>
    /// <param name="expectedEndString">The string expected to be at the end of the string.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <param name="comparisonType">The type of string comparison to perform.</param>
    /// <exception cref="ContainsException">Thrown when the string does not end with the expected string.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert EndsWith(
        [AllowNull] string expectedEndString,
        [AllowNull] string actualString,
        StringComparison comparisonType);

    /// <summary>
    /// Verifies that a string matches a regular expression.
    /// </summary>
    /// <param name="expectedRegexPattern">The regex pattern expected to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="MatchesException">Thrown when the string does not match the regex pattern.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Matches(string expectedRegexPattern, [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string matches a regular expression.
    /// </summary>
    /// <param name="expectedRegex">The regex expected to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="MatchesException">Thrown when the string does not match the regex.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Matches(Regex expectedRegex, [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string does not match a regular expression.
    /// </summary>
    /// <param name="expectedRegexPattern">The regex pattern expected not to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="DoesNotMatchException">Thrown when the string matches the regex pattern.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotMatch(string expectedRegexPattern, [AllowNull] string actualString);

    /// <summary>
    /// Verifies that a string does not match a regular expression.
    /// </summary>
    /// <param name="expectedRegex">The regex expected not to match.</param>
    /// <param name="actualString">The string to be inspected.</param>
    /// <exception cref="DoesNotMatchException">Thrown when the string matches the regex.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotMatch(Regex expectedRegex, [AllowNull] string actualString);

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual);

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual,
        bool ignoreCase);

    /// <summary>
    /// Verifies that two strings are equivalent.
    /// </summary>
    /// <param name="expected">The expected string value.</param>
    /// <param name="actual">The actual string value.</param>
    /// <param name="ignoreCase">If set to <see langword="true" />, ignores cases differences. The invariant culture is used.</param>
    /// <param name="ignoreLineEndingDifferences">If set to <see langword="true" />, treats \r\n, \r, and \n as equivalent.</param>
    /// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual,
        bool ignoreCase,
        bool ignoreLineEndingDifferences);

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
    IAssert Equal(
        [AllowNull] string expected,
        [AllowNull] string actual,
        bool ignoreCase,
        bool ignoreLineEndingDifferences,
        bool ignoreWhiteSpaceDifferences);
}