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
    /// Append a space to the StringBuilder.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendSpace(this StringBuilder stringBuilder)
    {
        return stringBuilder.Append(' ');
    }

    /// <summary>
    /// Append a space to the StringBuilder.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="count">The number of spaces to add.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendSpace(this StringBuilder stringBuilder, int count)
    {
        return stringBuilder.Append(' ', count);
    }

    /// <summary>
    /// Append a space to the StringBuilder.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="count">The number of tabs to add.</param>
    /// <param name="tabSize">The tab size.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendSpace(this StringBuilder stringBuilder, int count, int tabSize)
    {
        return stringBuilder.Append(' ', count * tabSize);
    }
}