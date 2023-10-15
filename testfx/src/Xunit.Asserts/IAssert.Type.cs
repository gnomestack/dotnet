using System;
using System.Reflection;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <typeparam name="T">The type the object should be.</typeparam>
    /// <param name="object">The object to be evaluated.</param>
    /// <returns>The object, casted to type T when successful.</returns>
    /// <exception cref="IsTypeException">Thrown when the object is not the given type.</exception>
    T IsType<T>(object @object);

    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <param name="expectedType">The type the object should be.</param>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsTypeException">Thrown when the object is not the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert IsType(Type expectedType, object @object);

    /// <summary>
    /// Verifies that an object is not exactly the given type.
    /// </summary>
    /// <typeparam name="T">The type the object should not be.</typeparam>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsNotTypeException">Thrown when the object is the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert IsNotType<T>(object @object);

    /// <summary>
    /// Verifies that an object is not exactly the given type.
    /// </summary>
    /// <param name="expectedType">The type the object should not be.</param>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsNotTypeException">Thrown when the object is the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert IsNotType(Type expectedType, object @object);

    /// <summary>
    /// Verifies that an object is of the given type or a derived type.
    /// </summary>
    /// <typeparam name="T">The type the object should be.</typeparam>
    /// <param name="object">The object to be evaluated.</param>
    /// <returns>The object is cast to type T when successful.</returns>
    /// <exception cref="IsAssignableFromException">Thrown when the object is not the given type.</exception>
    T IsAssignableFrom<T>(object @object);

    /// <summary>
    /// Verifies that an object is of the given type or a derived type.
    /// </summary>
    /// <param name="expectedType">The type the object should be.</param>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsAssignableFromException">Thrown when the object is not the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert IsAssignableFrom(Type expectedType, object @object);
}