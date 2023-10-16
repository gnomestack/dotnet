using GnomeStack.Extra.Strings;

namespace GnomeStack.Handlebars.Helpers;

internal static class TypeConverter
{
    public static bool AsBool(this object? value)
        => AsBool(value, false);

    public static bool AsBool(this object? value, bool defaultValue)
    {
        if (value == null)
            return defaultValue;

        if (value is bool b)
            return b;

        if (value is string stringValue)
        {
            if (stringValue == "1" ||
                stringValue.EqualsIgnoreCase("true") ||
                stringValue.EqualsIgnoreCase("yes") ||
                stringValue.EqualsIgnoreCase("y") ||
                stringValue.EqualsIgnoreCase("on"))
                return true;

            if (stringValue == "0" ||
                stringValue.EqualsIgnoreCase("false") ||
                stringValue.EqualsIgnoreCase("no") ||
                stringValue.EqualsIgnoreCase("n") ||
                stringValue.EqualsIgnoreCase("off"))
                return false;

            return defaultValue;
        }

        if (value is int i)
        {
            return i switch
            {
                0 => false,
                1 => true,
                _ => defaultValue,
            };
        }

        if (value is long l)
        {
            return l switch
            {
                0 => false,
                1 => true,
                _ => defaultValue,
            };
        }

        if (value is short s)
        {
            return s switch
            {
                0 => false,
                1 => true,
                _ => defaultValue,
            };
        }

        if (value is double d)
        {
            return d switch
            {
                0 => false,
                1 => true,
                _ => defaultValue,
            };
        }

        if (value is decimal m)
        {
            return m switch
            {
                0 => false,
                1 => true,
                _ => defaultValue,
            };
        }

        if (value is byte bit)
        {
            return bit switch
            {
                0 => false,
                1 => true,
                _ => defaultValue,
            };
        }

        return defaultValue;
    }

    public static Guid AsGuid(this object? value)
        => AsGuid(value, Guid.Empty);

    public static Guid AsGuid(this object? value, Guid defaultValue)
    {
        if (value == null)
            return defaultValue;

        if (value is Guid guid)
            return guid;

        if (value is string stringValue && Guid.TryParse(stringValue, out var result))
            return result;

        return defaultValue;
    }

    public static DateTime AsDateTime(this object? value)
        => AsDateTime(value, DateTime.MinValue);

    public static DateTime AsDateTime(this object? value, DateTime defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (value is DateTime dateTime)
        {
            return dateTime;
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }

        if (value is string stringValue && DateTime.TryParse(stringValue, out dateTime))
        {
            return dateTime;
        }

        return defaultValue;
    }

    public static decimal AsDecimal(this object? value)
        => AsDecimal(value, 0);

    public static decimal AsDecimal(this object? value, decimal defaultValue)
    {
        if (value == null)
            return defaultValue;

        if (value is decimal d)
            return d;

        if (decimal.TryParse(value.ToString(), out var result))
            return result;

        return defaultValue;
    }

    public static double AsDouble(this object? value)
        => AsDouble(value, 0);

    public static double AsDouble(this object? value, double defaultValue)
    {
        if (value == null)
            return defaultValue;

        if (value is double d)
            return d;

        if (double.TryParse(value.ToString(), out d))
            return d;

        return defaultValue;
    }

    public static short AsInt16(this object? value, short defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (value is short i)
            return i;

        if (short.TryParse(value.ToString(), out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static int AsInt32(this object? value, int defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (value is int i)
            return i;

        if (int.TryParse(value.ToString(), out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static long AsInt64(this object? value, long defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (value is long i)
            return i;

        if (long.TryParse(value.ToString(), out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static string AsString(this object? value)
        => AsString(value, string.Empty);

    public static string AsString(this object? value, string defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (value is string s)
            return s;

        return value.ToString() ?? defaultValue;
    }

    public static TimeSpan AsTimeSpan(this object? value)
        => AsTimeSpan(value, TimeSpan.Zero);

    public static TimeSpan AsTimeSpan(this object? value, TimeSpan defaultValue)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (value is TimeSpan timeSpan)
        {
            return timeSpan;
        }

        if (value is string stringValue && TimeSpan.TryParse(stringValue, out timeSpan))
        {
            return timeSpan;
        }

        return defaultValue;
    }
}