using System.Diagnostics.CodeAnalysis;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that two objects are not the same instance.
    /// </summary>
    /// <param name="expected">The expected object instance.</param>
    /// <param name="actual">The actual object instance.</param>
    /// <exception cref="NotSameException">Thrown when the objects are the same instance.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotSame([AllowNull] object expected, [AllowNull] object actual);

    /// <summary>
    /// Verifies that two objects are the same instance.
    /// </summary>
    /// <param name="expected">The expected object instance.</param>
    /// <param name="actual">The actual object instance.</param>
    /// <exception cref="SameException">Thrown when the objects are not the same instance.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Same([AllowNull] object expected, [AllowNull] object actual);
}