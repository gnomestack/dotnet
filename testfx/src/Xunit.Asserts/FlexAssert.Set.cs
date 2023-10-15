using System;
using System.Collections.Generic;

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Verifies that a set is a proper subset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSuperset">The expected superset.</param>
    /// <param name="actual">The set expected to be a proper subset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a proper subset of the expected set.</exception>
    public IAssert ProperSubset<T>(ISet<T> expectedSuperset, ISet<T>? actual)
    {
        if (expectedSuperset is null)
            throw new ArgumentNullException(nameof(expectedSuperset));

        if (actual?.IsProperSubsetOf(expectedSuperset) != true)
            throw new ProperSubsetException(expectedSuperset, actual);

        return this;
    }

    /// <summary>
    /// Verifies that a set is a proper superset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSubset">The expected subset.</param>
    /// <param name="actual">The set expected to be a proper superset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a proper superset of the expected set.</exception>
    public IAssert ProperSuperset<T>(ISet<T> expectedSubset, ISet<T>? actual)
    {
        if (expectedSubset is null)
            throw new ArgumentNullException(nameof(expectedSubset));

        if (actual?.IsProperSupersetOf(expectedSubset) != true)
            throw new ProperSupersetException(expectedSubset, actual);

        return this;
    }

    /// <summary>
    /// Verifies that a set is a subset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSuperset">The expected superset.</param>
    /// <param name="actual">The set expected to be a subset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a subset of the expected set.</exception>
    public IAssert Subset<T>(ISet<T> expectedSuperset, ISet<T>? actual)
    {
        if (expectedSuperset is null)
            throw new ArgumentNullException(nameof(expectedSuperset));

        if (actual?.IsSubsetOf(expectedSuperset) != true)
            throw new SubsetException(expectedSuperset, actual);

        return this;
    }

    /// <summary>
    /// Verifies that a set is a superset of another set.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expectedSubset">The expected subset.</param>
    /// <param name="actual">The set expected to be a superset.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="ContainsException">Thrown when the actual set is not a superset of the expected set.</exception>
    public IAssert Superset<T>(ISet<T> expectedSubset, ISet<T>? actual)
    {
        if (expectedSubset is null)
            throw new ArgumentNullException(nameof(expectedSubset));

        if (actual?.IsSupersetOf(expectedSubset) != true)
            throw new SupersetException(expectedSubset, actual);

        return this;
    }
}