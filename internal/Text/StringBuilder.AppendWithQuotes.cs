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
}