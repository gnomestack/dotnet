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
    /// Copies the characters from a specified segment of this instance to a specified segment of a destination <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <param name="span">The span to copy values into.</param>
    /// <param name="count">The number of characters to copy.</param>
    public static void CopyTo(this StringBuilder builder, Span<char> span, int count)
    {
        builder.CopyTo(0, span, count);
    }

    /// <summary>
    /// Copies the characters from a specified segment of this instance to a specified segment of a destination <see cref="Span{T}"/>.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <param name="span">The span to copy all the characters into.</param>
    public static void CopyTo(this StringBuilder builder, Span<char> span)
    {
        builder.CopyTo(0, span, span.Length);
    }
}