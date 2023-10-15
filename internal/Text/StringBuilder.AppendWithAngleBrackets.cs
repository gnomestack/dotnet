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
    /// Appends a string with angle brackets.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append quoted by angle brackets.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithAngles(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('<')
            .Append(value)
            .Append('>');
    }

    public static StringBuilder AppendWithAngleBrackets(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('<')
            .Append(value)
            .Append('>');
    }
}