namespace GnomeStack.Secrets;

/// <summary>
/// The contract for a secret masker.
/// </summary>
public interface ISecretMasker : IMask
{
    /// <summary>
    /// Add a secret to mask.
    /// </summary>
    /// <param name="secret">The secret to mask.</param>
    void Add(string? secret);

    /// <summary>
    /// Add a generator to create derivatives of secrets.
    /// </summary>
    /// <param name="generator">The derivative generator.</param>
    void AddDerivativeGenerator(Func<ReadOnlyMemory<char>, ReadOnlyMemory<char>> generator);
}