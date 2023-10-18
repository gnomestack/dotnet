using System.Runtime.InteropServices;
using System.Text;

namespace GnomeStack.Text;

public static class StringBuilderExtensions
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

    /// <summary>
    /// Appends a string with angle brackets.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append quoted by angle brackets.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithAngleBrackets(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('<')
            .Append(value)
            .Append('>');
    }

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

    /// <summary>
    /// Appends a string with brackets.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The to append and wrapped with square brackets.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithBrackets(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('[')
            .Append(value)
            .Append(']');
    }

    /// <summary>
    /// Appends a string with parens.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithParens(this StringBuilder stringBuilder, string value)
    {
        return stringBuilder
            .Append('(')
            .Append(value)
            .Append(')');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, byte value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, byte value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="times">The number of times to repeat the value.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, char value, int times)
    {
        return AppendWithQuotes(stringBuilder, value, times, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="times">The number of times to repeat the value.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, char value, int times, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value, times)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, char value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, char value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, StringBuilder value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, StringBuilder value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, bool value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, bool value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, decimal value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, decimal value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, double value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, double value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, short value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, short value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, int value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, int value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, long value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, long value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, object value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, object value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, ReadOnlySpan<char> value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <param name="quote">The quote character to use.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, ReadOnlySpan<char> value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append and then wrap the value with quotes.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, string value)
    {
        return AppendWithQuotes(stringBuilder, value, '"');
    }

    /// <summary>
    /// Appends the <paramref name="value"/> to the <paramref name="stringBuilder"/> with quotes.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    /// <param name="value">The value to append.</param>
    /// <param name="quote">The quote character used to wrap the value.</param>
    /// <returns>The string builder to chain.</returns>
    public static StringBuilder AppendWithQuotes(this StringBuilder stringBuilder, string value, char quote)
    {
        return stringBuilder
            .Append(quote)
            .Append(value)
            .Append(quote);
    }

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

#if NETLEGACY
    public static void CopyTo(this StringBuilder builder, int sourceIndex, Span<char> span, int count)
    {
        if (sourceIndex + count > builder.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Count must be less than or equal to the length of the string builder.");
        }

        if (count > span.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Count must be less than or equal to the length of the span.");
        }

        var set = new char[count];
        builder.CopyTo(
            sourceIndex,
            set,
            0,
            count);
        set.CopyTo(span);
    }
#endif

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