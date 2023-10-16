using System.Globalization;

using HandlebarsDotNet;

namespace GnomeStack.Handlebars.Helpers;

public static class DateTimeHelpers
{
    public static void FormatDate(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        arguments.RequireArgumentLength(1, "format-date");
        var date = arguments.GetDateTime(0);
        var format = arguments.Length > 1 ? arguments.GetString(1) : "yyyy-MM-dd";

        writer.WriteSafeString(date.ToString(format));
    }

    public static void Now(EncodedTextWriter writer, Context context, Arguments arguments)
    {
        switch (arguments.Length)
        {
            case 0:
                writer.WriteSafeString(DateTime.Now.ToString(CultureInfo.CurrentCulture));
                break;
            default:
                var format = arguments.GetString(0);
                var utcNow = false;
                if (arguments.Length > 1)
                    utcNow = arguments.GetBoolean(1);

                if (utcNow)
                {
                    writer.WriteSafeString(DateTime.UtcNow.ToString(format));
                    return;
                }

                writer.WriteSafeString(DateTime.Now.ToString(format));
                break;
        }
    }

    public static void RegisterDateTimeHelpers(this IHandlebars? hb)
    {
        if (hb is null)
        {
            HandlebarsDotNet.Handlebars.RegisterHelper("format-date", FormatDate);
            HandlebarsDotNet.Handlebars.RegisterHelper("now", Now);
            return;
        }

        hb.RegisterHelper("format-date", FormatDate);
        hb.RegisterHelper("now", Now);
    }
}