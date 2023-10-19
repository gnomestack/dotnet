using GnomeStack.Extra.Strings;
using GnomeStack.Standard;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

public static class EnvHelpers
{
    public static void RegisterEnvHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("env-get", GetEnvVariable);
            HandlebarsDotNet.Handlebars.RegisterHelper("env-get-bool", GetEnvVariableAsBool);
            HandlebarsDotNet.Handlebars.RegisterHelper("env-expand", ExpandEnvVar);
            HandlebarsDotNet.Handlebars.RegisterHelper("env-exists", GetEnvVariableExists);
            return;
        }

        hb.RegisterHelper("env-get", GetEnvVariable);
        hb.RegisterHelper("env-get-bool", GetEnvVariableAsBool);
        hb.RegisterHelper("env-expand", ExpandEnvVar);
        hb.RegisterHelper("env-exists", GetEnvVariableExists);
    }

    internal static void GetEnvVariable(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "env-value");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new HandlebarsException("env-get name must not be null or whitespace");

        var defaultValue = arguments.Length > 1 ? arguments[1].ToString() : string.Empty;
        if (Env.TryGet(NormalizeEnvKey(key), out var value))
        {
            writer.WriteSafeString(value);
            return;
        }

        writer.WriteSafeString(defaultValue);
    }

    internal static object GetEnvVariableExists(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "env-exists");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new HandlebarsException("ev-get-bool name must not be null or whitespace");

        if (Env.TryGet(NormalizeEnvKey(key), out var value) && !value.IsNullOrWhiteSpace())
        {
            return true;
        }

        return false;
    }

    internal static object GetEnvVariableAsBool(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "env-bool");

        var key = arguments[0].ToString();
        if (key.IsNullOrWhiteSpace())
            throw new HandlebarsException("ev-get-bool name must not be null or whitespace");

        object? defaultValue = false;
        if (arguments.Length > 0)
            defaultValue = arguments[0];

        if (Env.TryGet(NormalizeEnvKey(key), out var value))
        {
            return value.AsBool();
        }

        return defaultValue.AsBool();
    }

    internal  static void ExpandEnvVar(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "env-expand");

        var template = arguments[0].ToString();
        if (template.IsNullOrWhiteSpace())
            throw new HandlebarsException("ev-expand template must not be null or whitespace");

        writer.WriteSafeString(Env.Expand(template));
    }

    

    private static string NormalizeEnvKey(string key)
    {
        var sb = GnomeStack.Text.StringBuilderCache.Acquire();
        foreach (var c in key)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
            }
            else if (c is '(' or ')' && Env.IsWindows)
            {
                sb.Append(c);
            }
            else if (c is '-' or '.' or '_' or '/' or ':')
            {
                sb.Append('_');
            }
            else
            {
                throw new InvalidOperationException($"Invalid character '{c}' in configuration key '{key}'");
            }
        }

        return Text.StringBuilderCache.GetStringAndRelease(sb);
    }
}