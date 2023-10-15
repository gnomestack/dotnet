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

public partial class FlexAssert : IAssert
{
    /// <summary>
    /// Verifies that an expression is ok.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="userMessage">The message to be shown when the condition is false.</param>
    /// <exception cref="TrueException">Thrown when the condition is false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert Ok(bool condition, string? userMessage = null)
    {
        return this.True((bool?)condition, userMessage);
    }

    /// <summary>
    /// Verifies that an expression is true.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <exception cref="TrueException">Thrown when the condition is false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert True([DoesNotReturnIf(parameterValue: false)] bool condition)
    {
        return this.True((bool?)condition, null);
    }

    /// <summary>
    /// Verifies that an expression is true.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="userMessage">The message to be shown when the condition is false.</param>
    /// <exception cref="TrueException">Thrown when the condition is false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert True([DoesNotReturnIf(parameterValue: false)] bool condition, string? userMessage)
    {
        return this.True((bool?)condition, userMessage);
    }

    /// <summary>
    /// Verifies that an expression is true.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="userMessage">The message to be shown when the condition is false.</param>
    /// <exception cref="TrueException">Thrown when the condition is false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert True([DoesNotReturnIf(parameterValue: false)] bool? condition, string? userMessage)
    {
        return !condition.HasValue || !condition.GetValueOrDefault() ?
            throw new TrueException(userMessage, condition)
            : this;
    }

    /// <summary>
    /// Verifies that the condition is false.
    /// </summary>
    /// <param name="condition">The condition to be tested.</param>
    /// <exception cref="FalseException">Thrown if the condition is not false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert False([DoesNotReturnIf(parameterValue: true)] bool condition)
    {
        return this.False((bool?)condition, null);
    }

    /// <summary>
    /// Verifies that the condition is false.
    /// </summary>
    /// <param name="condition">The condition to be tested.</param>
    /// <exception cref="FalseException">Thrown if the condition is not false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert False([DoesNotReturnIf(parameterValue: true)] bool? condition)
    {
        return this.False(condition, null);
    }

    /// <summary>
    /// Verifies that the condition is false.
    /// </summary>
    /// <param name="condition">The condition to be tested.</param>
    /// <param name="userMessage">The message to show when the condition is not false.</param>
    /// <exception cref="FalseException">Thrown if the condition is not false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert False([DoesNotReturnIf(parameterValue: true)] bool condition, string? userMessage)
    {
        return this.False((bool?)condition, userMessage);
    }

    /// <summary>
    /// Verifies that the condition is false.
    /// </summary>
    /// <param name="condition">The condition to be tested.</param>
    /// <param name="userMessage">The message to show when the condition is not false.</param>
    /// <exception cref="FalseException">Thrown if the condition is not false.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert False([DoesNotReturnIf(parameterValue: true)] bool? condition, string? userMessage)
    {
        return !condition.HasValue || condition.GetValueOrDefault() ?
            throw new FalseException(userMessage, condition)
            : this;
    }
}