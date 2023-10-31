using System;
using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Secrets;

/// <summary>
/// A contract for masking values.
/// </summary>
public interface IMask
{
    /// <summary>
    /// Mask a value.
    /// </summary>
    /// <param name="value">The value to mask.</param>
    /// <returns>The masked value.</returns>
    [return: NotNullIfNotNull("value")]
    string? Mask(string? value);

    /// <summary>
    /// Mask a value.
    /// </summary>
    /// <param name="value">The value to mask.</param>
    /// <returns>The masked value.</returns>
    ReadOnlySpan<char> Mask(ReadOnlySpan<char> value);
}