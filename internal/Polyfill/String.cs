using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

#if DFX_CORE
namespace GnomeStack.Extras.Strings;
#else
namespace System;
#endif

#if DFX_CORE
public
#else
internal
#endif
    static partial class StringExtensions
{
#if NETLEGACY
    /// <summary>
    /// Indicates whether this instance contains the value.
    /// </summary>
    /// <param name="source">The instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns><see langword="true" /> if contains the given value; otherwise, <see langword="false" />.</returns>
    public static bool Contains(this string? source, string value, StringComparison comparison)
    {
        if (source is null)
            return false;

        return value.IndexOf(value, comparison) > -1;
    }

    /// <summary>
    /// Splits a <see cref="string"/> into substrings using the separator.
    /// </summary>
    /// <param name="source">The string instance to split.</param>
    /// <param name="separator">The separator that is used to split the string.</param>
    /// <returns>The <see cref="T:string[]"/>.</returns>
    public static string[] Split(this string source, string separator)
    {
        return source.Split(separator.ToCharArray());
    }

    /// <summary>
    /// Splits a <see cref="string"/> into substrings using the separator.
    /// </summary>
    /// <param name="source">The string instance to split.</param>
    /// <param name="separator">The separator that is used to split the string.</param>
    /// <param name="options">The string split options.</param>
    /// <returns>The <see cref="T:string[]"/>.</returns>
    public static string[] Split(this string source, char separator, StringSplitOptions options)
    {
        return source.Split(new[] { separator }, options);
    }

    /// <summary>
    /// Splits a <see cref="string"/> into substrings using the separator.
    /// </summary>
    /// <param name="source">The string instance to split.</param>
    /// <param name="separator">The separator that is used to split the string.</param>
    /// <param name="options">The string split options.</param>
    /// <returns>The <see cref="T:string[]"/>.</returns>
    public static string[] Split(this string source, string separator, StringSplitOptions options)
    {
        return source.Split(separator.ToCharArray(), options);
    }
#endif

    /// <summary>
    /// Indicates whether or not the <see cref="string"/> value is null, empty, or white space.
    /// </summary>
    /// <param name="source">The source string.</param>
    /// <returns><see langword="true" /> if the <see cref="string"/>
    /// is null, empty, or white space; otherwise, <see langword="false" />.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? source)
        => string.IsNullOrWhiteSpace(source);

    /// <summary>
    /// Indicates whether or not the <see cref="string"/> value is null or empty.
    /// </summary>
    /// <param name="source">The <see cref="string"/> value.</param>
    /// <returns><see langword="true" /> if the <see cref="string"/> is null or empty; otherwise, <see langword="false" />.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? source)
        => string.IsNullOrEmpty(source);
}