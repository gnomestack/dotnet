using System;
using System.Reflection;

using Xunit.Sdk;

// ReSharper disable UseStringInterpolation
// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <typeparam name="T">The type the object should be.</typeparam>
    /// <param name="object">The object to be evaluated.</param>
    /// <returns>The object is cast to type T when successful.</returns>
    /// <exception cref="IsTypeException">Thrown when the object is not the given type.</exception>
    public T IsType<T>(object @object)
    {
        this.IsType(typeof(T), @object);
        return (T)@object;
    }

    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <param name="expectedType">The type the object should be.</param>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsTypeException">Thrown when the object is not the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert IsType(Type expectedType, object @object)
    {
        if (expectedType is null)
            throw new ArgumentNullException(nameof(expectedType));

        if (@object == null)
            throw new IsTypeException(expectedType.FullName, null);

        Type actualType = @object.GetType();
        if (expectedType != actualType)
        {
            string expectedTypeName = expectedType.FullName ?? string.Empty;
            string actualTypeName = actualType.FullName ?? string.Empty;

            if (expectedTypeName == actualTypeName)
            {
                expectedTypeName += string.Format(" ({0})", expectedType.GetTypeInfo().Assembly.GetName().FullName);
                actualTypeName += string.Format(" ({0})", actualType.GetTypeInfo().Assembly.GetName().FullName);
            }

            throw new IsTypeException(expectedTypeName, actualTypeName);
        }

        return this;
    }

    /// <summary>
    /// Verifies that an object is not exactly the given type.
    /// </summary>
    /// <typeparam name="T">The type the object should not be.</typeparam>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsNotTypeException">Thrown when the object is the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert IsNotType<T>(object @object)
    {
        return this.IsNotType(typeof(T), @object);
    }

    /// <summary>
    /// Verifies that an object is not exactly the given type.
    /// </summary>
    /// <param name="expectedType">The type the object should not be.</param>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsNotTypeException">Thrown when the object is the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert IsNotType(Type expectedType, object @object)
    {
        if (expectedType is null)
            throw new ArgumentNullException(nameof(expectedType));

        // ReSharper disable once CheckForReferenceEqualityInstead.1
        if (@object != null && expectedType.Equals(@object.GetType()))
            throw new IsNotTypeException(expectedType, @object);

        return this;
    }

    /// <summary>
    /// Verifies that an object is of the given type or a derived type.
    /// </summary>
    /// <typeparam name="T">The type the object should be.</typeparam>
    /// <param name="object">The object to be evaluated.</param>
    /// <returns>The object is cast to type T when successful.</returns>
    /// <exception cref="IsAssignableFromException">Thrown when the object is not the given type.</exception>
    public T IsAssignableFrom<T>(object @object)
    {
        this.IsAssignableFrom(typeof(T), @object);
        return (T)@object;
    }

    /// <summary>
    /// Verifies that an object is of the given type or a derived type.
    /// </summary>
    /// <param name="expectedType">The type the object should be.</param>
    /// <param name="object">The object to be evaluated.</param>
    /// <exception cref="IsAssignableFromException">Thrown when the object is not the given type.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    public IAssert IsAssignableFrom(Type expectedType, object @object)
    {
        if (expectedType is null)
            throw new ArgumentNullException(nameof(expectedType));

        if (@object == null || !expectedType.GetTypeInfo().IsAssignableFrom(@object.GetType().GetTypeInfo()))
            throw new IsAssignableFromException(expectedType, @object);

        return this;
    }
}