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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Xunit.Sdk;

/// <summary>
/// Default implementation of <see cref="IComparer{T}"/> used by the xUnit.net range assertions.
/// </summary>
/// <typeparam name="T">The type that is being compared.</typeparam>
public class AssertComparer<T> : IComparer<T>
    where T : IComparable
{
    /// <inheritdoc/>
    public int Compare(T? x, T? y)
    {
        // Null?
        if (x == null && y == null)
            return 0;
        if (x == null)
            return -1;
        if (y == null)
            return 1;

        // Same type?
        if (x.GetType() != y.GetType())
            return -1;

        // Implements IComparable<T>?
        if (x is IComparable<T> comparable1)
            return comparable1.CompareTo(y);

        // Implements IComparable
        return x.CompareTo(y);
    }
}