using System.ComponentModel;

using GnomeStack.Extras.Strings;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

internal static class ArgumentExtensions
{
    public static void RequireArgumentLength(this Arguments arguments, int length, string name)
    {
        if (arguments.Length != length)
        {
            throw new HandlebarsException($"{{{{{name}}} helper must have {length} argument(s)");
        }
    }

    public static byte[] GetBytes(this Arguments arguments, int index)
        => GetBytes(arguments, index, Array.Empty<byte>());

    public static byte[] GetBytes(this Arguments arguments, int index, byte[] defaultValue)
    {
        var value = arguments[index];
        if (value == null)
        {
            return defaultValue;
        }

        if (value is byte[] bytes)
        {
            return bytes;
        }

        if (value is string str)
        {
            try
            {
                return Convert.FromBase64String(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        return defaultValue;
    }

    public static decimal GetDecimal(this Arguments arguments, int index)
        => GetDecimal(arguments, index, 0);

    public static decimal GetDecimal(this Arguments arguments, int index, decimal defaultValue)
    {
        var argument = arguments[index];
        return argument.AsDecimal(defaultValue);
    }

    public static double GetDouble(this Arguments arguments, int index)
        => GetDouble(arguments, index, 0);

    public static double GetDouble(this Arguments arguments, int index, double defaultValue)
    {
        var argument = arguments[index];
        return argument.AsDouble(defaultValue);
    }

    public static int GetInt16(this Arguments arguments, int index)
        => GetInt16(arguments, index, 0);

    public static short GetInt16(this Arguments arguments, int index, short defaultValue)
    {
        var argument = arguments[index];
        return argument.AsInt16(defaultValue);
    }

    public static int GetInt32(this Arguments arguments, int index)
        => GetInt32(arguments, index, 0);

    public static int GetInt32(this Arguments arguments, int index, int defaultValue)
    {
        var argument = arguments[index];
        return argument.AsInt32(defaultValue);
    }

    public static long GetInt64(this Arguments arguments, int index)
        => GetInt64(arguments, index, 0);

    public static long GetInt64(this Arguments arguments, int index, long defaultValue)
    {
        var argument = arguments[index];
        return argument.AsInt64(defaultValue);
    }

    public static string GetString(this Arguments arguments, int index)
        => GetString(arguments, index, string.Empty);

    public static string GetString(this Arguments arguments, int index, string defaultValue)
    {
        var value = arguments[index];
        return value.AsString(defaultValue);
    }

    public static DateTime GetDateTime(this Arguments arguments, int index)
        => GetDateTime(arguments, index, DateTime.Now);

    public static DateTime GetDateTime(this Arguments arguments, int index, DateTime defaultValue)
    {
        var value = arguments[index];
        return value.AsDateTime();
    }

    public static bool GetBoolean(this Arguments arguments, int index)
        => GetBoolean(arguments, index, false);

    public static bool GetBoolean(this Arguments arguments, int index, bool defaultValue)
    {
        var value = arguments[index];
        return value.AsBool(defaultValue);
    }
}