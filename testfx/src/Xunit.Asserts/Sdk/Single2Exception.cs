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
using System.Globalization;

namespace Xunit.Sdk;

public class Single2Exception : XunitException
{
    public Single2Exception(string errorMessage)
        : base(errorMessage)
    {
    }

    /// <summary>
    /// Creates an instance of <see cref="SingleException"/> for when the collection didn't contain any of the expected value.
    /// </summary>
    /// <param name="expected">The expected argument.</param>
    /// <returns>An a single exception.</returns>
    public static Exception Empty([AllowNull] string? expected) =>
        new Single2Exception(
            string.Format(
                CultureInfo.CurrentCulture,
                "The collection was expected to contain a single element{0}, but it {1}",
                expected == null ? string.Empty : " matching " + expected,
                expected == null ? "was empty." : "contained no matching elements."));

    /// <summary>
    /// Creates an instance of <see cref="SingleException"/> for when the collection had too many of the expected items.
    /// </summary>
    /// <param name="count">The expected count.</param>
    /// <param name="expected">The expected argument.</param>
    /// <returns>An a single exception.</returns>
    public static Exception MoreThanOne(int count, [AllowNull] string? expected) =>
        new Single2Exception(
            string.Format(
                CultureInfo.CurrentCulture,
                "The collection was expected to contain a single element{0}, but it contained {1}{2} elements.",
                expected == null ? string.Empty : " matching " + expected,
                count,
                expected == null ? string.Empty : " matching"));
}