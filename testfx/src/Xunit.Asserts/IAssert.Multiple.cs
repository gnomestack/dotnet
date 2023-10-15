using System;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Runs multiple checks, collecting the exceptions from each one, and then bundles all failures
    /// up into a single assertion failure.
    /// </summary>
    /// <param name="checks">The individual assertions to run, as actions.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="MultipleException">Thrown when there are one or more checks that fail.</exception>
    IAssert Multiple(params Action[] checks);
}