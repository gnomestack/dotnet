using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
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
    ///    Appends a string as a cli parameter to the end of the <see cref="StringBuilder" />.
    ///    This method is cross-platform and will escape the string as necessary.
    /// </summary>
    /// <param name="sb">The string builder.</param>
    /// <param name="argument">The argument to add and escape.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendCliArgument(this StringBuilder sb, string argument)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // based on the logic from http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp.
            // The method given there doesn't minimize the use of quotation. For that, I drew from
            // https://blogs.msdn.microsoft.com/twistylittlepassagesallalike/2011/04/23/everyone-quotes-command-line-arguments-the-wrong-way/

            // the essential encoding logic is:
            // (1) non-empty strings with no special characters require no encoding
            // (2) find each substring of 0-or-more \ followed by " and replace it by twice-as-many \, followed by \"
            // (3) check if argument ends on \ and if so, double the number of backslashes at the end
            // (4) add leading and trailing "
            if (!ContainsWindowsSpecialCharacter(argument))
            {
                sb.Append(argument);
                return sb;
            }

            sb.Append('"');

            var backSlashCount = 0;
            foreach (var ch in argument)
            {
                switch (ch)
                {
                    case '\\':
                        ++backSlashCount;
                        break;

                    case '"':
                        sb.Append('\\', repeatCount: (2 * backSlashCount) + 1);
                        backSlashCount = 0;
                        sb.Append(ch);
                        break;

                    default:
                        sb.Append('\\', repeatCount: backSlashCount);
                        backSlashCount = 0;
                        sb.Append(ch);
                        break;
                }
            }

            sb.Append('\\', repeatCount: 2 * backSlashCount)
                .Append('"');

            return sb;
        }

        if (!ContainsSpecialCharacter(argument))
        {
            sb.Append(argument);
            return sb;
        }

        sb.Append('"');
        foreach (var @char in argument)
        {
            switch (@char)
            {
                case '$':
                case '`':
                case '"':
                case '\\':
                    sb.Append('\\');
                    break;
            }

            sb.Append(@char);
        }

        sb.Append('"');

        return sb;
    }

    private static bool ContainsWindowsSpecialCharacter(string s)
    {
        if (s.Length == 0)
            return false;

        foreach (var c in s)
        {
            var isSpecial = c is ' ' or '"';
            if (isSpecial)
                return true;
        }

        return false;
    }

    private static bool ContainsSpecialCharacter(string s)
    {
        if (s.Length == 0)
            return false;

        foreach (var c in s)
        {
            var isSpecial = c switch
            {
                '\\' or '\'' or '"' => true,
                _ => char.IsWhiteSpace(c),
            };

            if (isSpecial)
                return true;
        }

        return false;
    }
}