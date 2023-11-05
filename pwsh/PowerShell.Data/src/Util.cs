using System.Collections;
using System.Management.Automation;

namespace GnomeStack.PowerShell.Data;

internal static class Util
{
    public static Dictionary<string, object?> ToDictionary(this PSObject psObject)
    {
        var map = new Dictionary<string, object?>();
        foreach (var property in psObject.Properties)
        {
            var name = property.Name;
            if (name is null)
                continue;
            var value = property.Value;
            if (value is PSObject psObject2)
            {
                map[name] = psObject2.BaseObject;
            }
            else
            {
                map[name] = value;
            }
        }

        return map;
    }

    public static Dictionary<string, object?> ToDictionary(this IDictionary dictionary)
    {
        var map = new Dictionary<string, object?>();
        foreach (var key in dictionary.Keys)
        {
            var name = key.ToString();
            if (name is null)
                continue;
            var value = dictionary[key];
            if (value is PSObject psObject)
            {
                map[name] = psObject.BaseObject;
            }
            else
            {
                map[name] = value;
            }
        }

        return map;
    }
}