using GnomeStack.Extra.Strings;
using GnomeStack.Std;

using HandlebarsDotNet;

using Hb = HandlebarsDotNet.Handlebars;

namespace GnomeStack.Templates.Handlebars;

public static partial class Helpers
{
    [CLSCompliant(false)]
    public static void EnvHelpers(IHandlebars? hb)
    {
        HandlebarsHelper envHelper = (writer, context, arguments) =>
        {
            if (arguments.Length == 0)
                throw new InvalidOperationException("env helper requires at least one argument");

            var key = arguments[0].ToString();
            if (key.IsNullOrWhiteSpace())
                throw new InvalidOperationException("key must not be null or whitespace");

            var defaultValue = arguments.Length > 1 ? arguments[1].ToString() : string.Empty;
            if (Env.TryGet(NormalizeEnvKey(key), out var value))
            {
                writer.WriteSafeString(value);
                return;
            }

            writer.WriteSafeString(defaultValue);
        };

        HandlebarsReturnHelper envBoolHelper = (_, args) =>
        {
            if (args.Length == 0) throw new InvalidOperationException("conf helper requires at least one argument");

            var key = args[0].ToString();
            if (key.IsNullOrWhiteSpace())
                throw new InvalidOperationException("key must not be null or whitespace");
            if (Env.TryGet(NormalizeEnvKey(key), out var value))
            {
                if (value.EqualsIgnoreCase("true") || value == "1" ||
                    value.EqualsIgnoreCase("enabled") ||
                    value.EqualsIgnoreCase("yes") || value.EqualsIgnoreCase("y"))
                    return true;

                return false;
            }

            return false;
        };

        if (hb is null)
        {
            Hb.RegisterHelper("env", envHelper);
            Hb.RegisterHelper("env-bool", envBoolHelper);
        }
        else
        {
            hb.RegisterHelper("env", envHelper);
            hb.RegisterHelper("env-bool", envBoolHelper);
        }
    }

/*
    [CLSCompliant(false)]
    public static void ConfHelpers(ConfigurationManager config, IHandlebars? hb)
    {
        void ConfHelper(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            if (arguments.Length == 0) throw new InvalidOperationException("conf helper requires at least one argument");

            var key = arguments[0].ToString();
            if (key.IsNullOrWhiteSpace())
                throw new InvalidOperationException("key must not be null or whitespace");
            var defaultValue = arguments.Length > 1 ? arguments[1].ToString() : string.Empty;
            var section = config.GetSection(NormalizeConfigKey(key));
            if (section.Value == null)
            {
                writer.WriteSafeString(defaultValue);
                return;
            }

            writer.WriteSafeString(section.Value);
        }

        HandlebarsReturnHelper confBoolHelper = (_, args) =>
        {
            if (args.Length == 0) throw new InvalidOperationException("conf helper requires at least one argument");

            var key = args[0].ToString();
            if (key.IsNullOrWhiteSpace())
                throw new InvalidOperationException("key must not be null or whitespace");
            var section = config.GetSection(NormalizeConfigKey(key));

            if (section.Value is null)
                return false;

            return section.Value.AsBool(false);
        };

        Hb.RegisterHelper("conf-bool", (_, args) =>
        {
            if (args.Length == 0) throw new InvalidOperationException("conf helper requires at least one argument");

            var key = args[0].ToString();
            if (key.IsNullOrWhiteSpace())
                throw new InvalidOperationException("key must not be null or whitespace");

            var section = config.GetSection(NormalizeConfigKey(key));

            if (section.Value is null)
                return false;

            if (section.Value.EqualsIgnoreCase("true") || section.Value == "1" ||
                section.Value.EqualsIgnoreCase("enabled") ||
                section.Value.EqualsIgnoreCase("yes") || section.Value.EqualsIgnoreCase("y"))
                return true;

            return false;
        });

        if (hb is null)
        {
            Hb.RegisterHelper("conf", (HandlebarsHelper)ConfHelper);
            Hb.RegisterHelper("conf-bool", confBoolHelper);
        }
        else
        {
            hb.RegisterHelper("conf", (HandlebarsHelper)ConfHelper);
            hb.RegisterHelper("conf-bool", confBoolHelper);
        }
    }
    */

    private static string NormalizeEnvKey(string key)
    {
        var sb = GnomeStack.Text.StringBuilderCache.Acquire();
        foreach (var c in key)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
            }
            else if (c is '(' or ')' && Std.Env.IsWindows)
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

    private static string NormalizeConfigKey(string key)
    {
        var sb = GnomeStack.Text.StringBuilderCache.Acquire();
        foreach (var c in key)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
            }
            else if (c is '(' or ')' && Std.Env.IsWindows)
            {
                sb.Append(c);
            }
            else if (c is '-' or '.' or '_' or '/' or ':')
            {
                sb.Append(':');
            }
            else
            {
                throw new InvalidOperationException($"Invalid character '{c}' in configuration key '{key}'");
            }
        }

        return Text.StringBuilderCache.GetStringAndRelease(sb);
    }
}