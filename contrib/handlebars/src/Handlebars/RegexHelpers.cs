using System.Text.RegularExpressions;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

public static class RegexHelpers
{
    public static bool IsMatch(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "is-match");
        var input = arguments.GetString(0, string.Empty);
        var pattern = arguments.GetString(1, string.Empty);
        string? options = null;
        if (arguments.Length > 2)
        {
            options = arguments.GetString(2, string.Empty);
        }

        return MatchInternal(true, input, pattern, options) != null;
    }

    public static object? Match(Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(2, "match");
        var input = arguments.GetString(0, string.Empty);
        var pattern = arguments.GetString(1, string.Empty);
        string? options = null;
        if (arguments.Length > 2)
        {
            options = arguments.GetString(2, string.Empty);
        }

        return MatchInternal(false, input, pattern, options);
    }

    public static void RegisterRegexHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("is-match", (context, arguments) => IsMatch(context, arguments));
            HandlebarsDotNet.Handlebars.RegisterHelper("match", Match);
            return;
        }

        hb.RegisterHelper("is-match", (context, arguments) => IsMatch(context, arguments));
        hb.RegisterHelper("match", Match);
    }

    private static object? MatchInternal(bool isBlockHelper, string value, string regexPattern, string? options = null)
    {
        Regex regex;
        if (!string.IsNullOrWhiteSpace(options))
        {
            RegexOptions regexOptions = RegexOptions.None;
            foreach (char ch in options.Distinct())
            {
                switch (ch)
                {
                    case 'i':
                        regexOptions |= RegexOptions.IgnoreCase;
                        break;

                    case 'm':
                        regexOptions |= RegexOptions.Multiline;
                        break;

                    case 'n':
                        regexOptions |= RegexOptions.ExplicitCapture;
                        break;

                    case 'c':
                        regexOptions |= RegexOptions.Compiled;
                        break;

                    case 's':
                        regexOptions |= RegexOptions.Singleline;
                        break;

                    case 'x':
                        regexOptions |= RegexOptions.IgnorePatternWhitespace;
                        break;

                    case 'r':
                        regexOptions |= RegexOptions.RightToLeft;
                        break;

                    case 'e':
                        regexOptions |= RegexOptions.ECMAScript;
                        break;

                    case 'C':
                        regexOptions |= RegexOptions.CultureInvariant;
                        break;
                }
            }

            regex = new Regex(regexPattern, regexOptions);
        }
        else
        {
            regex = new Regex(regexPattern);
        }

        var namedGroups = GetNamedGroups(regex, value);
        if (isBlockHelper && namedGroups.Any())
        {
            return namedGroups;
        }

        var match = regex.Match(value);
        if (match.Success)
        {
            return match.Value;
        }

        return null;
    }

    private static Dictionary<string, string> GetNamedGroups(Regex regex, string input)
    {
        var namedGroupsDictionary = new Dictionary<string, string>();

        GroupCollection groups = regex.Match(input).Groups;
        foreach (string groupName in regex.GetGroupNames())
        {
            if (groups[groupName].Captures.Count > 0)
            {
                namedGroupsDictionary.Add(groupName, groups[groupName].Value);
            }
        }

        return namedGroupsDictionary;
    }
}