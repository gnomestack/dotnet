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
    ///    Converts the value of a <see cref="StringBuilder" /> to a <see cref="char" /> array.
    /// </summary>
    /// <param name="builder">The string builder.</param>
    /// <returns>An array with all the characters of the string builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
    public static char[] ToArray(this StringBuilder builder)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        var set = new char[builder.Length];
        builder.CopyTo(
            0,
            set,
            0,
            set.Length);
        return set;
    }
}