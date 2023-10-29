using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using GnomeStack.Collections.Generic;
using GnomeStack.Extra.Object;
using GnomeStack.Extra.Strings;
using GnomeStack.Text.Serialization;

namespace GnomeStack.Diagnostics;

public partial class PsArgs
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public static PsArgs Splat(Splattable obj)
    {
        if (obj is IPsArgsBuilder builder)
            return builder.BuildPsArgs();

        var splat = new PsArgs();
        var options = obj.BuildSplatOptions();
        var extraArgs = new PsArgs();
        var separateArgs = new PsArgs();
        var orderedArgs = new PsArgs();

        if (options.Command.Count > 0)
            splat.AddRange(options.Command);

        if (options.Arguments.Count > 0)
            orderedArgs = new PsArgs(new string[options.Arguments.Count]);

        var assign = options.Assignment != " ";
        foreach (var prop in obj.GetType().GetProperties())
        {
            var name = prop.Name;
            var value = prop.GetValue(obj);

            if (name == options.ExtraArgumentsName)
            {
                if (value is IEnumerable enumerable)
                {
                    foreach (var n in enumerable)
                    {
                        extraArgs.Add(n.ToSafeString());
                    }
                }
                else
                {
                    extraArgs.Add(value.ToSafeString());
                }
            }

            if (name == options.SeparateArgumentsName)
            {
                if (value is IEnumerable enumerable)
                {
                    foreach (var n in enumerable)
                    {
                        separateArgs.Add(n.ToSafeString());
                    }
                }
                else
                {
                    separateArgs.Add(value.ToSafeString());
                }
            }

            var argIndex = options.Arguments.IndexOf(name, StringComparison.OrdinalIgnoreCase);
            if (argIndex > -1)
            {
                switch (value)
                {
                    case string str:
                        orderedArgs[argIndex] = str;
                        break;

                    case IEnumerable _:
                        {
                            throw new NotSupportedException(
                                "Objects of IEnumerable are not supported for positional arguments");
                        }

                    default:
                        orderedArgs[argIndex] = value.ToSafeString();
                        break;
                }

                orderedArgs[argIndex] = value.ToSafeString();
                continue;
            }

            if (options.Included.Count > 0 && !options.Included.Contains(name, StringComparison.OrdinalIgnoreCase))
                continue;

            if (options.Excluded.Count > 0 && options.Excluded.Contains(name, StringComparison.OrdinalIgnoreCase))
                continue;

            if (options.Aliases.TryGetValue(name, out var alias))
            {
                name = alias;
            }
            else
            {
                var prefix = options!.ShortFlag && name.Length == 1 ? "-" : options.Prefix;
                name = options.PreserveCase ? $"{prefix}{name}" : $"{prefix}{name.Hyphenate()}";
            }

            var attr = prop.GetCustomAttribute<FormatAttribute>();

            switch (value)
            {
                case bool bit:
                    {
                        if (bit)
                        {
                            splat.Add(name);
                        }
                    }

                    break;

                case string str:
                    {
                        if (assign)
                            splat.Add($"{name}{options.Assignment}{str}");
                        else
                            splat.Add(name, str);
                    }

                    break;

                case IEnumerable<string> enumerable:
                    {
                        foreach (var n in enumerable)
                        {
                            if (assign)
                                splat.Add($"{name}{options.Assignment}{n}");
                            else
                                splat.Add(name, n);
                        }
                    }

                    break;

                default:
                    {
                        var formatted = attr != null ? string.Format(attr.Format, value) : value.ToSafeString();

                        if (assign)
                            splat.Add($"{name}{options.Assignment}{formatted}");
                        else
                            splat.Add(name, formatted);
                    }

                    break;
            }
        }

        splat.AddRange(extraArgs);

        if (separateArgs.Count > 0)
        {
            splat.Add(options.SeparateArgumentsPrefix);
            splat.AddRange(separateArgs);
        }

        if (orderedArgs.Count > 0)
        {
            if (options.AppendArguments)
            {
                foreach (var n in orderedArgs)
                {
                    if (string.IsNullOrWhiteSpace(n))
                        continue;

                    splat.Add(n);
                }
            }
            else
            {
                var filtered = orderedArgs.Where(o => !string.IsNullOrWhiteSpace(o));
                splat.InsertRange(0, filtered);
            }
        }

        return splat;
    }
}