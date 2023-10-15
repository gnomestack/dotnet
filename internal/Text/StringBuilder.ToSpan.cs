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
    /// Returns a span of the characters in the string builder.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <returns>A span of characters.</returns>
    public static Span<char> AsSpan(this StringBuilder builder)
    {
        var set = new char[builder.Length];
        builder.CopyTo(
            0,
            set,
            0,
            set.Length);

        return new Span<char>(set);
    }

    /// <summary>
    /// Returns a span of the characters in the string builder.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <returns>A readonly span of characters.</returns>
    public static ReadOnlySpan<char> AsReadOnlySpan(this StringBuilder builder)
    {
        var set = new char[builder.Length];
        builder.CopyTo(
            0,
            set,
            0,
            set.Length);

        return new ReadOnlySpan<char>(set);
    }
}