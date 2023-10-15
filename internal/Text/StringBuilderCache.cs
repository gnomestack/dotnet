// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GnomeStack.Text;

/// <summary>Provide a cached reusable instance of <see cref="StringBuilder"/> per thread.</summary>
// exclude as the source was copied for the dotnet runtime.
[ExcludeFromCodeCoverage]
#if DFX_CORE
public
#else
internal
#endif
    static class StringBuilderCache
{
    // The value 360 was chosen in discussion with performance experts as a compromise between using
    // as little memory per thread as possible and still covering a large part of short-lived
    // StringBuilder creations on the startup path of VS designers.
    internal const int MaxBuilderSize = 360;

    private const int DefaultCapacity = 16; // == StringBuilder.DefaultCapacity

    // WARNING: We allow diagnostic tools to directly inspect this member (s_cachedInstance).
    // See https://github.com/dotnet/corert/blob/master/Documentation/design-docs/diagnostics/diagnostics-tools-contract.md for more details.
    // Please do not change the type, the name, or the semantic usage of this member without understanding the implication for tools.
    // Get in touch with the diagnostics team if you have questions.
    [ThreadStatic]
    private static StringBuilder? s_cachedInstance;

    /// <summary>Get a StringBuilder for the specified capacity.</summary>
    /// <remarks><para>If a StringBuilder of an appropriate size is cached, it will be returned and the cache emptied.</para></remarks>
    /// <param name="capacity">The capacity of the new string builder.</param>
    /// <returns>A <see cref="StringBuilder"/> from the cache.</returns>
    public static StringBuilder Acquire(int capacity = DefaultCapacity)
    {
        if (capacity <= MaxBuilderSize)
        {
            StringBuilder? sb = s_cachedInstance;

            // Avoid string builder block fragmentation by getting a new StringBuilder
            // when the requested size is larger than the current capacity
            if (sb != null && capacity <= sb.Capacity)
            {
                s_cachedInstance = null;
                sb.Clear();
                return sb;
            }
        }

        return new StringBuilder(capacity);
    }

    /// <summary>
    /// <summary>Place the specified builder in the cache if it is not too big.</summary>
    /// </summary>
    /// <param name="sb">The string builder.</param>
    public static void Release(StringBuilder sb)
    {
        if (sb.Capacity <= MaxBuilderSize)
        {
            s_cachedInstance = sb;
        }
    }

    /// <summary>
    /// Calls <see cref="StringBuilder.ToString()"/> on the <see cref="StringBuilder"/>. Release it to the cache. Return the resulting string.
    /// </summary>
    /// <param name="sb">The string builder.</param>
    /// <returns>A new string from the <see cref="StringBuilder"/>.</returns>
    public static string GetStringAndRelease(StringBuilder sb)
    {
        string result = sb.ToString();
        Release(sb);
        return result;
    }
}