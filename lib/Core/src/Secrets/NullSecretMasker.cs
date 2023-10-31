using System;
using System.Diagnostics.CodeAnalysis;

namespace GnomeStack.Secrets;

public class NullSecretMasker : ISecretMasker
{
    public static ISecretMasker Default { get; } = new NullSecretMasker();

    public void Add(string? secret)
    {
        // noop
    }

    public void AddDerivativeGenerator(Func<ReadOnlyMemory<char>, ReadOnlyMemory<char>> generator)
    {
        // noop
    }

    public string? Mask([NotNullIfNotNull("value")] string? value)
    {
        return value;
    }

    public ReadOnlySpan<char> Mask(ReadOnlySpan<char> value)
    {
        return value;
    }
}