using System.Diagnostics.CodeAnalysis;

using GnomeStack.Extras.Strings;

namespace GnomeStack.Secrets;

/// <summary>
/// The default implementation of <see cref="ISecretMasker"/> which can
/// mask secrets in strings and spans have been added to it.
/// </summary>
public class SecretMasker : ISecretMasker
{
    public static ISecretMasker Default { get; } = new SecretMasker();

    protected List<ReadOnlyMemory<char>> Secrets { get; } = new();

    protected List<Func<ReadOnlyMemory<char>, ReadOnlyMemory<char>>> Generators { get; } = new();

    /// <summary>
    /// Add a secret to mask.
    /// </summary>
    /// <param name="secret">The secret to mask.</param>
    public void Add(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
            return;

        var memory = secret.AsMemory();

        // don't exit method as there may be new generators.
        if (!this.Secrets.Contains(memory))
            this.Secrets.Add(memory);

        foreach (var generator in this.Generators)
        {
            var next = generator(memory);

            if (!this.Secrets.Contains(next))
                this.Secrets.Add(next);
        }
    }

    /// <summary>
    /// Add a generator to create derivatives of secrets.
    /// </summary>
    /// <param name="generator">The derivative generator.</param>
    public void AddDerivativeGenerator(Func<ReadOnlyMemory<char>, ReadOnlyMemory<char>> generator)
    {
        this.Generators.Add(generator);
    }

    /// <summary>
    /// Mask a value.
    /// </summary>
    /// <param name="value">The value to mask.</param>
    /// <returns>The masked value.</returns>
    public ReadOnlySpan<char> Mask(ReadOnlySpan<char> value)
    {
        if (this.Secrets.Count == 0 || value.IsEmpty || value.IsWhiteSpace())
            return value;

        return value.SearchAndReplace(this.Secrets, "**********".AsSpan(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Mask a value.
    /// </summary>
    /// <param name="value">The value to mask.</param>
    /// <returns>The masked value.</returns>
    [return: NotNullIfNotNull("value")]
    public string? Mask(string? value)
    {
        if (value is null || string.IsNullOrWhiteSpace(value))
            return value;

        if (this.Secrets.Count == 0)
            return value;

        return value.AsSpan()
            .SearchAndReplace(
                this.Secrets,
                "**********".AsSpan(),
                StringComparison.OrdinalIgnoreCase)
            .AsString();
    }
}