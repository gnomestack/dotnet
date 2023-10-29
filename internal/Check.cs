using System;

using System.Diagnostics.CodeAnalysis;

using GnomeStack.Extras.Strings;

namespace GnomeStack;

internal static class Check
{
    public static T NotNull<T>(T value, string name)
    {
        if (value is null)
            throw new ArgumentNullException(name);

        return value;
    }

    public static void Range(Func<bool> inRange, string name)
    {
        if (!inRange())
            throw new ArgumentOutOfRangeException(name);
    }

    public static void Range(Func<bool> inRange, string name, string? message)
    {
        if (!inRange())
            throw new ArgumentOutOfRangeException(name, message);
    }

    public static void Range(Func<bool> inRange, string name, string? message, object? value)
    {
        if (!inRange())
            throw new ArgumentOutOfRangeException(name, value, message);
    }

    public static T NotNullReference<T>(T value, string name, string? message)
    {
        if (value is null)
            throw new InvalidOperationException(message ?? $"{name} must not be null.");

        return value;
    }

    public static string NotNullOrEmpty(
        [NotNullIfNotNull("value")] string? value,
        string name,
        string? message = null)
    {
        if (value.IsNullOrEmpty())
            throw new ArgumentNullException(name, message);

        return value;
    }

    public static string NotNullOrWhiteSpace(
        [NotNullIfNotNull("value")] string? value,
        string name,
        string? message = null)
    {
        if (value.IsNullOrWhiteSpace())
            throw new ArgumentNullException(name, message);

        return value;
    }
}