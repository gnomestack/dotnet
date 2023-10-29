using System.Reflection;
using System.Text.Json.Serialization.Metadata;

using GnomeStack.Extras.Strings;
using GnomeStack.Text.Serialization;

namespace GnomeStack.Text.Json;

public static class Modifiers
{
    public static void CustomAttributes(JsonTypeInfo info)
    {
        foreach (var prop in info.Properties)
        {
            var attrProvider = prop.AttributeProvider;
            if (attrProvider is null)
                continue;

            var ignoreAttrs = attrProvider.GetCustomAttributes(typeof(IgnoreAttribute), true)
                .Cast<IgnoreAttribute>()
                .ToArray();

            if (ignoreAttrs.Length > 0)
            {
                prop.ShouldSerialize = (_, _) => false;
                continue;
            }

            var serializeAttrs = attrProvider.GetCustomAttributes(typeof(SerializationAttribute), true)
                .Cast<SerializationAttribute>()
                .ToArray();

            if (serializeAttrs.Length > 0)
            {
                var attr = serializeAttrs[0];
                if (!attr.Name.IsNullOrWhiteSpace())
                    prop.Name = attr.Name;

                if (prop.Order > 0)
                    prop.Order = attr.Order;
            }
        }
    }
}