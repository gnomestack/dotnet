using System.Collections;
using System.Management.Automation;

namespace GnomeStack.PowerShell;

public static class PsHashtable
{
    public static object? Convert(object? input)
    {
        if (input is null or string)
        {
            return input;
        }

        if (input is PSObject psObject)
        {
            var result = new Hashtable();
            foreach (var property in psObject.Properties)
            {
                result.Add(property.Name, Convert(property.Value));
            }

            return result;
        }

        if (input is IEnumerable enumerable)
        {
            var result = new List<object?>();
            foreach (var next in enumerable)
            {
                result.Add(Convert(next));
            }

            return result.ToArray();
        }

        return input;
    }
}