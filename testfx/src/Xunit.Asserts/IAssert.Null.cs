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

using System.Diagnostics.CodeAnalysis;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that an object reference is null.
    /// </summary>
    /// <param name="object">The object to be inspected.</param>
    /// <exception cref="NullException">Thrown when the object reference is not null.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Null(object? @object);

    /// <summary>
    /// Verifies that an object reference is not null.
    /// </summary>
    /// <param name="object">The object to be validated.</param>
    /// <exception cref="NotNullException">Thrown when the object is not null.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotNull([NotNull] object? @object);
}