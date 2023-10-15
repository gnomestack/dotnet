using System.Collections.Generic;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that a set is a proper subset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSuperset">The expected superset.</param>
    /// <param name="actual">The set expected to be a proper subset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a proper subset of the expected set.</exception>
    IAssert ProperSubset<T>(ISet<T> expectedSuperset, ISet<T>? actual);

    /// <summary>
    /// Verifies that a set is a proper superset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSubset">The expected subset.</param>
    /// <param name="actual">The set expected to be a proper superset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a proper superset of the expected set.</exception>
    IAssert ProperSuperset<T>(ISet<T> expectedSubset, ISet<T>? actual);

    /// <summary>
    /// Verifies that a set is a subset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSuperset">The expected superset.</param>
    /// <param name="actual">The set expected to be a subset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a subset of the expected set.</exception>
    IAssert Subset<T>(ISet<T> expectedSuperset, ISet<T>? actual);

    /// <summary>
    /// Verifies that a set is a superset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSubset">The expected subset.</param>
    /// <param name="actual">The set expected to be a superset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a superset of the expected set.</exception>
    IAssert Superset<T>(ISet<T> expectedSubset, ISet<T>? actual);
}