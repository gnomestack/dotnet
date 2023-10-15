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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Xunit.Sdk;

/// <summary>
/// A class that wraps <see cref="IEqualityComparer{T}"/> to create <see cref="IEqualityComparer"/>.
/// </summary>
/// <typeparam name="T">The type that is being compared.</typeparam>
/// <remarks><para>Copied from Xunit.</para></remarks>
internal class AssertEqualityComparerAdapter<T> : IEqualityComparer
{
    private readonly IEqualityComparer<T> innerComparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssertEqualityComparerAdapter{T}"/> class.
    /// </summary>
    /// <param name="innerComparer">The comparer that is being adapted.</param>
    public AssertEqualityComparerAdapter(IEqualityComparer<T> innerComparer)
    {
        this.innerComparer = innerComparer;
    }

    /// <inheritdoc/>
    public new bool Equals(object? x, object? y)
    {
        return this.innerComparer.Equals((T?)x!, (T?)y!);
    }

    /// <inheritdoc/>
    public int GetHashCode(object obj)
    {
        throw new NotImplementedException();
    }
}