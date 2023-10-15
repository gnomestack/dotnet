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

using System.Collections.Generic;
using System.Linq;

namespace Xunit.Sdk;

/// <summary>
/// Exception thrown when two values are unexpectedly not equal.
/// </summary>
public class EquivalentException : Xunit.Sdk.AssertActualExpectedException
{
    private readonly string? message;

    protected EquivalentException(string message)
        : base(null, null, null)
    {
        this.message = message;
    }

    protected EquivalentException(
        object? expected,
        object? actual,
        string messageSuffix,
        string? expectedTitle = null,
        string? actualTitle = null)
        : base(expected, actual, "Assert.Equivalent() Failure" + messageSuffix, expectedTitle, actualTitle)
    {
    }

    /// <inheritdoc/>
    public override string Message => this.message ?? base.Message;

    public static string FormatMemberNameList(
        IEnumerable<string> memberNames,
        string prefix)
    {
        return "[" + string.Join(", ", memberNames.Select(k => $"\"{prefix}{k}\"")) + "]";
    }

    /// <summary>
    /// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
    /// a circular reference was discovered.
    /// </summary>
    /// <param name="memberName">The name of the member that caused the circular reference.</param>
    /// <returns>An <see cref="EquivalentException" />.</returns>
    public static EquivalentException ForCircularReference(string memberName)
    {
        return new EquivalentException($"Assert.Equivalent() Failure: Circular reference found in '{memberName}'");
    }

    /// <summary>
    /// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
    /// that the list of available members does not match.
    /// </summary>
    /// <param name="expectedMemberNames">The expected member names.</param>
    /// <param name="actualMemberNames">The actual member names.</param>
    /// <param name="prefix">The prefix to be applied to the member names (may be an empty string for a
    /// top-level object, or a name in "member." format used as a prefix to show the member name list. </param>
    /// <returns>An <see cref="EquivalentException" />.</returns>
    public static EquivalentException ForMemberListMismatch(
        IEnumerable<string> expectedMemberNames,
        IEnumerable<string> actualMemberNames,
        string prefix)
    {
        return new EquivalentException(
            FormatMemberNameList(expectedMemberNames, prefix),
            FormatMemberNameList(actualMemberNames, prefix),
            ": Mismatched member list");
    }

    /// <summary>
    /// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
    /// that the fault comes from an individual value mismatch one of the members.
    /// </summary>
    /// <param name="expected">The expected member value.</param>
    /// <param name="actual">The actual member value.</param>
    /// <param name="memberName">The name of the mismatched member (may be an empty string for a
    /// top-level object).</param>
    /// <returns>An <see cref="EquivalentException" />.</returns>
    public static EquivalentException ForMemberValueMismatch(
        object? expected,
        object? actual,
        string memberName)
    {
        return new EquivalentException(
            expected,
            actual,
            memberName.Length == 0 ? string.Empty : $": Mismatched value on member '{memberName}'");
    }

    /// <summary>
    /// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
    /// a value was missing from the <paramref name="actual"/> collection.
    /// </summary>
    /// <param name="expected">The expected member value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="memberName">The name of the member that was being inspected (may be an empty
    /// string for a top-level collection).</param>
    /// <returns>An <see cref="EquivalentException" />.</returns>
    public static EquivalentException ForMissingCollectionValue(
        object? expected,
        IEnumerable<object?> actual,
        string memberName)
    {
        return new EquivalentException(
            expected,
            ArgumentFormatter.Format(actual),
            $": Collection value not found{(memberName.Length == 0 ? string.Empty : $" in member '{memberName}'")}",
            actualTitle: "In");
    }

    /// <summary>
    /// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
    /// that <paramref name="actual"/> contained one or more values that were not specified
    /// in <paramref name="expected"/>.
    /// </summary>
    /// <param name="expected">The expected values.</param>
    /// <param name="actual">The actual values.</param>
    /// <param name="actualLeftovers">The actual values that did not have matching expected values.</param>
    /// <param name="memberName">The name of the member that was being inspected (may be an empty
    /// string for a top-level collection).</param>
    /// <returns>An <see cref="EquivalentException" />.</returns>
    public static EquivalentException ForExtraCollectionValue(
        IEnumerable<object?> expected,
        IEnumerable<object?> actual,
        IEnumerable<object?> actualLeftovers,
        string memberName)
    {
        return new EquivalentException(
            ArgumentFormatter.Format(expected),
            $"{ArgumentFormatter.Format(actualLeftovers)} left over from {ArgumentFormatter.Format(actual)}",
            $": Extra values found{(memberName.Length == 0 ? string.Empty : $" in member '{memberName}'")}");
    }
}