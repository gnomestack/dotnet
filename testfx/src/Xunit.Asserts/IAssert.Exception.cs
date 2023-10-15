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
using System.Threading.Tasks;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    T Throws<T>(Action testCode)
        where T : Exception;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// Generally used to test property accessors.
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    T Throws<T>(Func<object> testCode)
        where T : Exception;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <param name="exceptionType">The type of the exception expected to be thrown.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    Exception Throws(Type exceptionType, Action testCode);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// Generally used to test property accessors.
    /// </summary>
    /// <param name="exceptionType">The type of the exception expected to be thrown.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    Exception Throws(Type exceptionType, Func<object> testCode);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
    /// derives from <see cref="ArgumentException"/> and has the given parameter name.
    /// </summary>
    /// <typeparam name="T">T represents an exception of type <see cref="ArgumentException" />.</typeparam>
    /// <param name="paramName">The parameter name that is expected to be in the exception.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    T Throws<T>(string paramName, Action testCode)
        where T : ArgumentException;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
    /// derives from <see cref="ArgumentException"/> and has the given parameter name.
    /// </summary>
    /// <typeparam name="T">T represents an exception of type <see cref="ArgumentException" />.</typeparam>
    /// <param name="paramName">The parameter name that is expected to be in the exception.</param>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    T Throws<T>(string paramName, Func<object> testCode)
        where T : ArgumentException;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the task to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    Task<T> ThrowsAsync<T>(Func<Task> testCode)
        where T : Exception;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the task to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    ValueTask<T> ThrowsAsync<T>(Func<ValueTask> testCode)
        where T : Exception;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
    /// derives from <see cref="ArgumentException"/> and has the given parameter name.
    /// </summary>
    /// <typeparam name="T">T represents an exception of type <see cref="ArgumentException" />.</typeparam>
    /// <param name="paramName">The parameter name that is expected to be in the exception.</param>
    /// <param name="testCode">A delegate to the task to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    Task<T> ThrowsAsync<T>(string paramName, Func<Task> testCode)
        where T : ArgumentException;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type), where the exception
    /// derives from <see cref="ArgumentException"/> and has the given parameter name.
    /// </summary>
    /// <typeparam name="T">T represents an exception of type <see cref="ArgumentException" />.</typeparam>
    /// <param name="paramName">The parameter name that is expected to be in the exception.</param>
    /// <param name="testCode">A delegate to the task to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    ValueTask<T> ThrowsAsync<T>(string paramName, Func<ValueTask> testCode)
        where T : ArgumentException;

    /// <summary>
    /// Verifies that the exact exception or a derived exception type is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    T ThrowsAny<T>(Action testCode)
        where T : Exception;

    /// <summary>
    /// Verifies that the exact exception or a derived exception type is thrown.
    /// Generally used to test property accessors.
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the code to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    T ThrowsAny<T>(Func<object> testCode)
        where T : Exception;

    /// <summary>
    /// Verifies that the exact exception or a derived exception type is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the exception expected to be thrown.</typeparam>
    /// <param name="testCode">A delegate to the task to be tested.</param>
    /// <returns>The exception that was thrown, when successful.</returns>
    /// <exception cref="ThrowsException">Thrown when an exception was not thrown, or when an exception of the incorrect type is thrown.</exception>
    Task<T> ThrowsAnyAsync<T>(Func<Task> testCode)
        where T : Exception;
}