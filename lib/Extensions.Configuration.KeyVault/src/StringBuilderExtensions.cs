using System.Text;

namespace GnomeStack.Extensions.Configuration.KeyVault;

public static class StringBuilderExtensions
{
    public static StringBuilder Append(
        this StringBuilder builder,
        ReadOnlySpan<char> input)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        foreach (var t in input)
            builder.Append(t);

        return builder;
    }
}