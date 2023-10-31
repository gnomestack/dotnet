using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Extras.Strings;

public partial class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToSafeString(this string? value)
    {
        return value ?? string.Empty;
    }

    /// <summary>
    /// Converts the span of characters to a <see cref="string"/> for
    /// all targeted .net frameworks.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <returns>A new string from the span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string AsString(this ReadOnlySpan<char> source)
    {
#if NETLEGACY
        return new string(source.ToArray());
#else
        return source.ToString();
#endif
    }

    /// <summary>
    /// Determines whether the end of the span matches the specified value when compared using the
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="value">The sequence to compare to the end of the source span.</param>
    /// <returns><see langword="true" /> when the span ends with the given value; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWithIgnoreCase(this ReadOnlySpan<char> source, ReadOnlySpan<char> value)
        => source.EndsWith(value, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether the end of the string matches the specified value when compared using the
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="value">The sequence to compare to the end of the source string.</param>
    /// <returns><see langword="true" /> when the string ends with the given value; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWithIgnoreCase(this string? source, string value)
    {
        if (source is null)
            return false;

        return source.EndsWith(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether the start of the span matches the specified value when compared using the
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="value">The sequence to compare to the start of the source span.</param>
    /// <returns><see langword="true" /> when the span starts with the given value; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWithIgnoreCase(this ReadOnlySpan<char> source, ReadOnlySpan<char> value)
        => source.StartsWith(value, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether the start of the string matches the specified value when compared using the
    /// <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="value">The sequence to compare to the start of the source string.</param>
    /// <returns><see langword="true" /> when the string starts with the given value; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWithIgnoreCase(this string? source, string value)
    {
        if (source is null)
            return false;

        return source.StartsWith(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Indicates whether a specified value occurs within a read-only character span
    /// using the <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="value">The value to seek within the source span.</param>
    /// <returns><see langword="true" /> when the span contains the given value; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsIgnoreCase(this ReadOnlySpan<char> source, ReadOnlySpan<char> value)
        => source.Contains(value, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Indicates whether a specified value occurs within a string
    /// using the <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="value">The value to seek within the source string.</param>
    /// <returns><see langword="true" /> when the string contains the given value; otherwise, <see langword="false"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsIgnoreCase(this string? source, string value)
    {
        if (source is null)
            return false;

        return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) > -1;
    }

    /// <summary>
    /// Indicates whether this span is equal to the given value using
    /// the <see cref="StringComparison.OrdinalIgnoreCase"/> comparison.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="value">The value to test for equality.</param>
    /// <returns><see langword="true" /> when the span equals the value; otherwise, <see langword="false" />.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsIgnoreCase(this ReadOnlySpan<char> source, ReadOnlySpan<char> value)
    {
        return source.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether the value on the left is equal to value
    /// right using <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// by default.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="right">The value to compare.</param>
    /// <param name="comparison">The string comparison type.</param>
    /// <returns><see langword="true" /> if the value is to the value on the right; otherwise, <see langword="false" />.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEqualTo(
        this string? source,
        string? right,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        if (ReferenceEquals(source, right))
            return true;
        return source?.Equals(right, comparison) == true;
    }

    /// <summary>
    /// Determines whether the value on the left is not equal to the value on the
    /// right using <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// by default.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <param name="right">The value to compare.</param>
    /// <param name="comparison">The string comparison type.</param>
    /// <returns><see langword="true" /> if the value is to the value on the right; otherwise, <see langword="false" />.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEqualTo(
        this string? source,
        string? right,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        if (ReferenceEquals(source, right))
            return false;

        return source?.Equals(right, comparison) == false;
    }

    public static string ScreamingSnakeCase(this string value)
    {
        var builder = StringBuilderCache.Acquire();
        var previous = char.MinValue;
        foreach (var c in value)
        {
            if (char.IsUpper(c) && builder.Length > 0 && previous != '_')
            {
                builder.Append('_');
            }

            if (c is '-' or ' ' or '_')
            {
                builder.Append('_');
                previous = '_';
                continue;
            }

            if (!char.IsLetterOrDigit(c))
                continue;

            builder.Append(char.ToUpperInvariant(c));
            previous = c;
        }

        return StringBuilderCache.GetStringAndRelease(builder);
    }

    public static string Hyphenate(this string value)
    {
        var builder = StringBuilderCache.Acquire();
        var previous = char.MinValue;
        foreach (var c in value)
        {
            if (char.IsUpper(c) && builder.Length > 0 && previous != '-')
            {
                builder.Append('-');
            }

            if (c is '_' or '-' or ' ')
            {
                builder.Append('-');
                previous = '-';
                continue;
            }

            if (!char.IsLetterOrDigit(c))
                continue;

            builder.Append(char.ToLowerInvariant(c));
            previous = c;
        }

        return StringBuilderCache.GetStringAndRelease(builder);
    }

    public static string Underscore(this string value)
    {
        var builder = StringBuilderCache.Acquire();
        var previous = char.MinValue;
        foreach (var c in value)
        {
            if (char.IsUpper(c) && builder.Length > 0 && previous != '_')
            {
                builder.Append('_');
            }

            if (c is '-' or ' ' or '_')
            {
                builder.Append('_');
                previous = '_';
                continue;
            }

            if (!char.IsLetterOrDigit(c))
                continue;

            builder.Append(char.ToLowerInvariant(c));
            previous = c;
        }

        return StringBuilderCache.GetStringAndRelease(builder);
    }

    public static Stream AsStream(this string value, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return new MemoryStream(encoding.GetBytes(value));
    }

    internal static bool IsHexString(this string value)
    {
        return value.Length >= 2 && value[0] == '0' && (value[1] == 'x' || value[1] == 'X');
    }
}