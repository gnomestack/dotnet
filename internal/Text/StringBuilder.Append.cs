using System.Buffers;
using System.IO;
using System.Text;

namespace GnomeStack.Text;

#if DFX_CORE
public
#else
internal
#endif
   static partial class StringBuilderExtensions
{
    /// <summary>
    /// Appends the <paramref name="input"/> to the <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <param name="input">The span to append.</param>
    /// <returns>The string builder to chain.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="builder"/> is null.
    /// </exception>
    public static StringBuilder Append(
        this StringBuilder builder,
        Span<char> input)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        foreach (var t in input)
            builder.Append(t);

        return builder;
    }

    /// <summary>
    /// Appends the <paramref name="input"/> to the <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <param name="input">The span to append.</param>
    /// <returns>The string builder to chain.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="builder"/> is null.
    /// </exception>
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

    /// <summary>
    /// Appends the <paramref name="input"/> to the <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <param name="input">The span to append.</param>
    /// <returns>The string builder to chain.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="builder"/> is null.
    /// </exception>
    public static StringBuilder Append(
        this StringBuilder builder,
        Memory<char> input)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        foreach (var t in input.Span)
            builder.Append(t);

        return builder;
    }

    /// <summary>
    /// Appends the <paramref name="input"/> to the <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <param name="input">The span to append.</param>
    /// <returns>The string builder to chain.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="builder"/> is null.
    /// </exception>
    public static StringBuilder Append(
        this StringBuilder builder,
        ReadOnlyMemory<char> input)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        foreach (var t in input.Span)
            builder.Append(t);

        return builder;
    }
}