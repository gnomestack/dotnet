using System;
using System.Collections.Generic;
using System.Linq;

namespace Xunit.Sdk;

/// <summary>
/// Exception thrown when multiple assertions failed via <see cref="IAssert.Multiple"/>.
/// </summary>
public class MultipleException : XunitException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultipleException"/> class.
    /// </summary>
    /// <param name="innerExceptions">The innerExceptions from failed check actions.</param>
    public MultipleException(IEnumerable<Exception> innerExceptions)
        : base("Multiple failures were encountered:")
    {
        if (innerExceptions == null)
            throw new ArgumentNullException(nameof(innerExceptions));

        this.InnerExceptions = innerExceptions.ToList();
    }

    /// <summary>
    /// Gets the list of inner exceptions that were thrown.
    /// </summary>
    public IReadOnlyCollection<Exception> InnerExceptions { get; }

    /// <inheritdoc/>
    public override string? StackTrace => "Inner stack traces:";
}