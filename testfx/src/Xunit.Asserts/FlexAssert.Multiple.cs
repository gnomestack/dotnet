using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

using Xunit.Sdk;

namespace Xunit;

public partial class FlexAssert
{
    /// <summary>
    /// Runs multiple checks, collecting the exceptions from each one, and then bundles all failures
    /// up into a single assertion failure.
    /// </summary>
    /// <param name="checks">The individual assertions to run, as actions.</param>
    /// <returns>An instance of <see cref="IAssert" />.</returns>
    /// <exception cref="MultipleException">Thrown when there are one or more checks that fail.</exception>
    public IAssert Multiple(params Action[] checks)
    {
        if (checks == null || checks.Length == 0)
            return this;

        var exceptions = new List<Exception>();

        foreach (var check in checks)
        {
            try
            {
                check();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Count == 0)
            return this;

        if (exceptions.Count == 1)
            ExceptionDispatchInfo.Capture(exceptions[0]).Throw();

        throw new MultipleException(exceptions);
    }
}