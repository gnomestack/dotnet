using System.Collections;
using System.Text.Json;

using GnomeStack.Standard;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

public static class JsonHelpers
{
    public static void RegisterJsonHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("json", ConvertToJson);
            return;
        }

        hb.RegisterHelper("json", ConvertToJson);
    }

    internal static void ConvertToJson(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "json");
        var value = arguments[0];
        if (value is null)
        {
            return;
        }

        var json = Json.Stringify(value);

        writer.WriteSafeString(json);
    }
}