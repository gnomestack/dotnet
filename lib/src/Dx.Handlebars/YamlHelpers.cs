using GnomeStack.Standard;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

public static class YamlHelpers
{
    public static void RegisterJsonHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("yaml", ConvertToYaml);
            return;
        }

        hb.RegisterHelper("yaml", ConvertToYaml);
    }

    private static void ConvertToYaml(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "yaml");
        var value = arguments[0];
        if (value is null)
        {
            return;
        }

        var json = Yaml.Stringify(value);

        writer.WriteSafeString(json);
    }
}