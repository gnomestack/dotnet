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
    /// Appends a string with braces.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithBraces(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('{')
            .Append(value)
            .Append('}');
    }
}