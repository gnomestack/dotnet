using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Xunit;

internal static class Util
{
    public const string TestDiscoverer = "Xunit.Sdk.GnomeStackTestDiscoverer";

    public const string AssemblyName = "GnomeStack.Xunit.Attributes";

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? value)
        => string.IsNullOrWhiteSpace(value);

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
        => string.IsNullOrEmpty(value);

#if NETSTANDARD2_0 || NETFRAMEWORK
    public static bool Contains(this string? instance, string value, StringComparison comparison)
    {
        if (instance is null)
            return false;

        return value.IndexOf(value, comparison) > -1;
    }
#endif

    public static void Add<TKey, TValue>(
        this IDictionary<TKey, List<TValue>> dictionary,
        TKey key,
        TValue value)
    {
        dictionary.GetOrAdd(key).Add(value);
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key)
        where TValue : new()
    {
        return dictionary.GetOrAdd<TKey, TValue>(key, () => new TValue());
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValue)
    {
        if (dictionary.TryGetValue(key, out var result))
            return result;

        result = newValue();
        dictionary[key] = result;

        return result;
    }
}