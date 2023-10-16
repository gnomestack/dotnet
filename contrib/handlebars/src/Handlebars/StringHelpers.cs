using System.Collections;

using GnomeStack.Extra.Strings;
using GnomeStack.Text;

using HandlebarsDotNet;

using Humanizer;

namespace GnomeStack.Handlebars.Helpers;

public static class StringHelpers
{
    public static string EncodeUri(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "encode-uri");
        var value = arguments.GetString(1, string.Empty);
        return System.Net.WebUtility.UrlEncode(value) ?? string.Empty;
    }

    public static string DecodeUri(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "encode-uri");
        var value = arguments.GetString(1, string.Empty);
        return System.Net.WebUtility.UrlDecode(value);
    }

    public static string Titleize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "titleize");
        var value = arguments.GetString(0);
        return value.Titleize();
    }

    public static string Camelize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "camelize");
        var value = arguments.GetString(0);
        return value.Camelize();
    }

    public static string Dasherize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "camelize");
        var value = arguments.GetString(0);
        return value.Dasherize();
    }

    public static string Kebaberize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "kebaberize");
        var value = arguments.GetString(0);
        return value.Kebaberize();
    }

    public static string Underscore(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "underscore");
        var value = arguments.GetString(0);
        return value.Underscore();
    }

    public static string Humanize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "humanize");
        var value = arguments.GetString(0);
        return value.Humanize();
    }

    public static string Truncate(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "truncate");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        return value.Truncate(length);
    }

    public static string TruncateWithEllipsis(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "truncate");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        return value.Truncate(length, "...");
    }

    public static string Pluralize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "pluralize");
        var value = arguments.GetString(0);
        return value.Pluralize();
    }

    public static string Singularize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "singularize");
        var value = arguments.GetString(0);
        return value.Singularize();
    }

    public static string Ordinalize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "ordinalize");
        var value = arguments.GetString(0);
        return value.Ordinalize();
    }

    public static string Capitalize(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "upperFirst");
        var value = arguments.GetString(0);
        return value[0].ToString().ToUpper() + value.Substring(1);
    }

    public static string ToLower(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "lower");
        var value = arguments.GetString(0);
        return value.ToLower();
    }

    public static string ToUpper(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "upper");
        var value = arguments.GetString(0);
        return value.ToUpper();
    }

    public static string[] Split(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "split");
        var value = arguments.GetString(0);
        var separator = arguments.GetString(1);
        return value.Split(separator.ToCharArray());
    }

    public static string Join(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "join");
        var value = arguments[0];
        if (value is not IEnumerable enumerable)
        {
            throw new HandlebarsException($"{{join}} helper must be called with an enumerable value");
        }

        var separator = arguments.GetString(1);
        return string.Join(separator, enumerable.Cast<object>());
    }

    public static bool IsString(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-string");
        var value = arguments[0];
        return value is string;
    }

    public static bool IsNullOrEmpty(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-empty");
        var value = arguments[0];
        if (value is null)
            return true;

        if (value is string str)
            return str.Length == 0;

        if (value is IEnumerable enumerable)
        {
            foreach (var n in enumerable)
#pragma warning disable S1751
                return false;
#pragma warning restore S1751

            return true;
        }

        return false;
    }

    public static bool IsNull(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-null");
        var value = arguments[0];
        return value is null;
    }

    public static bool IsNullOrWhiteSpace(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "is-whitespace");
        var value = arguments[0];
        if (value is null)
            return true;

        if (value is string str)
            return str.IsNullOrWhiteSpace();

        if (value is IEnumerable enumerable)
        {
            foreach (var n in enumerable)
#pragma warning disable S1751
                return false;
#pragma warning restore S1751

            return true;
        }

        return false;
    }

    public static object Concat(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "string-concat");
        var sb = StringBuilderCache.Acquire();
        foreach (var arg in arguments)
        {
            sb.Append(arg);
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }

    public static string Base64Encode(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "base64-encode");
        var value = arguments.GetString(0);
        return Convert.ToBase64String(Encodings.Utf8.GetBytes(value));
    }

    public static string Base64Decode(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "base64-decode");
        var value = arguments.GetString(0);
        return Encodings.Utf8.GetString(Convert.FromBase64String(value));
    }

    public static string PadLeft(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "pad-left");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        string padding = " ";
        if (arguments.Length > 0)
            padding = arguments.GetString(0, " ");

        return value.PadLeft(length, padding[0]);
    }

    public static string PadRight(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "pad-right");
        var value = arguments.GetString(0);
        var length = arguments.GetInt32(1);
        string padding = " ";
        if (arguments.Length > 0)
            padding = arguments.GetString(0, " ");

        return value.PadRight(length, padding[0]);
    }

    public static string Replace(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(3, "string-replace");
        var value = arguments.GetString(0);
        var oldValue = arguments.GetString(1);
        var newValue = arguments.GetString(2);
        return value.Replace(oldValue, newValue);
    }

    public static string Remove(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "string-remove");
        var value = arguments.GetString(0);
        var startIndex = arguments.GetInt32(1);
        var length = arguments.GetInt32(2, value.Length - startIndex);
        return value.Remove(startIndex, length);
    }

    public static string Prepend(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "prepend");
        var value = arguments.GetString(0);
        var prefix = arguments.GetString(1);
        return prefix + value;
    }

    public static string Append(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "append");
        var value = arguments.GetString(0);
        var suffix = arguments.GetString(1);
        return value + suffix;
    }

    public static string Trim(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "trim");
        var value = arguments.GetString(0);
        if (arguments.Length > 1)
        {
            var chars = arguments.GetString(0, " ").ToCharArray();
            return value.Trim(chars);
        }

        return value.Trim();
    }

    public static object TrimStart(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "trim-start");
        var value = arguments.GetString(0);

        if (arguments.Length > 1)
        {
            var chars = arguments.GetString(0, " ").ToCharArray();
            return value.TrimStart(chars);
        }

        return value.TrimStart();
    }

    public static string TrimEnd(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "trim-end");
        var value = arguments.GetString(0);

        if (arguments.Length > 1)
        {
            var chars = arguments.GetString(0, " ").ToCharArray();
            return value.TrimEnd(chars);
        }

        return value.TrimEnd();
    }

    public static bool StringEq(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "string-eq");
        var left = arguments.GetString(0);
        var right = arguments.GetString(1);

        var comparison = StringComparison.OrdinalIgnoreCase;
        if (arguments.Length > 2)
        {
            var comparisonString = arguments.GetString(2);
            if (Enum.TryParse<StringComparison>(comparisonString, true,  out var comparisonObj))
                comparison = comparisonObj;
        }

        return string.Equals(left, right, comparison);
    }

    public static string Format(
        IFormatProvider? provider,
        EncodedTextWriter writer,
        Context context,
        Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "format");
        var value = arguments[0];
        var format = arguments.GetString(1);
        var formatProvider = provider;

        // Attempt using a custom formatter from the format provider (if any)
        var customFormatter = formatProvider?.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
        var formattedValue = customFormatter?.Format(format, value, formatProvider);

        // Fallback to IFormattable
        formattedValue ??= (value as IFormattable)?.ToString(format, formatProvider);

        // Fallback to ToString
        formattedValue ??= value?.ToString();

        // Done
        return formattedValue ?? string.Empty;
    }

    public static bool StartsWith(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "starts-with");
        var value = arguments.GetString(0);
        var prefix = arguments.GetString(1);
        return value.StartsWith(prefix);
    }

    public static bool EndsWith(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "ends-with");
        var value = arguments.GetString(0);
        var suffix = arguments.GetString(1);
        return value.EndsWith(suffix);
    }

    public static void RegisterStringHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("titleize", Titleize);
            HandlebarsDotNet.Handlebars.RegisterHelper("camelize", Camelize);
            HandlebarsDotNet.Handlebars.RegisterHelper("dasherize", Dasherize);
            HandlebarsDotNet.Handlebars.RegisterHelper("kebaberize", Kebaberize);
            HandlebarsDotNet.Handlebars.RegisterHelper("underscore", Underscore);
            HandlebarsDotNet.Handlebars.RegisterHelper("humanize", Humanize);
            HandlebarsDotNet.Handlebars.RegisterHelper("truncate", Truncate);
            HandlebarsDotNet.Handlebars.RegisterHelper("truncateWithEllipsis", TruncateWithEllipsis);
            HandlebarsDotNet.Handlebars.RegisterHelper("pluralize", Pluralize);
            HandlebarsDotNet.Handlebars.RegisterHelper("singularize", Singularize);
            HandlebarsDotNet.Handlebars.RegisterHelper("ordinalize", Ordinalize);
            HandlebarsDotNet.Handlebars.RegisterHelper("capitalize", Capitalize);
            HandlebarsDotNet.Handlebars.RegisterHelper("lower", ToLower);
            HandlebarsDotNet.Handlebars.RegisterHelper("upper", ToUpper);
            HandlebarsDotNet.Handlebars.RegisterHelper("join", Join);
            HandlebarsDotNet.Handlebars.RegisterHelper("is-string", (c, a) => IsString(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("is-empty", (c, a) => IsNullOrEmpty(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("is-null", (c, a) => IsNull(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("is-whitespace", (c, a) => IsNullOrWhiteSpace(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("string-concat", Concat);
            HandlebarsDotNet.Handlebars.RegisterHelper("base64-encode", Base64Encode);
            HandlebarsDotNet.Handlebars.RegisterHelper("base64-decode", Base64Decode);
            HandlebarsDotNet.Handlebars.RegisterHelper("pad-left", PadLeft);
            HandlebarsDotNet.Handlebars.RegisterHelper("pad-right", PadRight);
            HandlebarsDotNet.Handlebars.RegisterHelper("string-replace", Replace);
            HandlebarsDotNet.Handlebars.RegisterHelper("string-remove", Remove);
            HandlebarsDotNet.Handlebars.RegisterHelper("prepend", Prepend);
            HandlebarsDotNet.Handlebars.RegisterHelper("append", Append);
            HandlebarsDotNet.Handlebars.RegisterHelper("trim", Trim);
            HandlebarsDotNet.Handlebars.RegisterHelper("trim-start", TrimStart);
            HandlebarsDotNet.Handlebars.RegisterHelper("trim-end", TrimEnd);
            HandlebarsDotNet.Handlebars.RegisterHelper("format", (w, c, a) => Format(HandlebarsDotNet.Handlebars.Configuration.FormatProvider, w, c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("starts-with", (c, a) => StartsWith(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("ends-with", (c, a) => EndsWith(c, a));
            HandlebarsDotNet.Handlebars.RegisterHelper("string-eq", (c, a) => StringEq(c, a));

            return;
        }

        hb.RegisterHelper("titleize", Titleize);
        hb.RegisterHelper("camelize", Camelize);
        hb.RegisterHelper("dasherize", Dasherize);
        hb.RegisterHelper("kebaberize", Kebaberize);
        hb.RegisterHelper("underscore", Underscore);
        hb.RegisterHelper("humanize", Humanize);
        hb.RegisterHelper("truncate", Truncate);
        hb.RegisterHelper("truncateWithEllipsis", TruncateWithEllipsis);
        hb.RegisterHelper("pluralize", Pluralize);
        hb.RegisterHelper("singularize", Singularize);
        hb.RegisterHelper("ordinalize", Ordinalize);
        hb.RegisterHelper("capitalize", Capitalize);
        hb.RegisterHelper("lower", ToLower);
        hb.RegisterHelper("upper", ToUpper);
        hb.RegisterHelper("join", Join);
        hb.RegisterHelper("is-string", (c, a) => IsString(c, a));
        hb.RegisterHelper("is-empty", (c, a) => IsNullOrEmpty(c, a));
        hb.RegisterHelper("is-null", (c, a) => IsNull(c, a));
        hb.RegisterHelper("is-whitespace", (c, a) => IsNullOrWhiteSpace(c, a));
        hb.RegisterHelper("string-concat", Concat);
        hb.RegisterHelper("base64-encode", Base64Encode);
        hb.RegisterHelper("base64-decode", Base64Decode);
        hb.RegisterHelper("pad-left", PadLeft);
        hb.RegisterHelper("pad-right", PadRight);
        hb.RegisterHelper("string-replace", Replace);
        hb.RegisterHelper("string-remove", Remove);
        hb.RegisterHelper("prepend", Prepend);
        hb.RegisterHelper("append", Append);
        hb.RegisterHelper("trim", Trim);
        hb.RegisterHelper("trim-start", TrimStart);
        hb.RegisterHelper("trim-end", TrimEnd);
        hb.RegisterHelper("format", (w, c, a) => Format(hb.Configuration.FormatProvider, w, c, a));
        hb.RegisterHelper("starts-with", (c, a) => StartsWith(c, a));
        hb.RegisterHelper("ends-with", (c, a) => EndsWith(c, a));
        hb.RegisterHelper("string-eq", (c, a) => StringEq(c, a));
    }
}