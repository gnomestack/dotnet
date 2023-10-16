using System.Collections;
using System.Text.Json;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

public static class JsonHelpers
{
    public static void ConvertToJson(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "json");
        var value = arguments[0];
        if (value is null)
        {
            return;
        }

        var json = JsonSerializer.Serialize(value, new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        writer.WriteSafeString(json);
    }

    public static void RegisterJsonHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("json", ConvertToJson);

            return;
        }

        hb.RegisterHelper("json", ConvertToJson);
    }
}