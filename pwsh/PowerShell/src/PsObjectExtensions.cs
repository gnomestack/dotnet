using System.Management.Automation;

namespace GnomeStack.PowerShell;

public static class PsObjectExtensions
{
    public static IEnumerable<string> GetPropertyNames(this PSObject psObject)
    {
        foreach (var prop in psObject.Properties)
        {
            yield return prop.Name;
        }
    }

    public static IEnumerable<string> GetPropertyValues(this PSObject psObject)
    {
        foreach (var prop in psObject.Properties)
        {
            yield return prop.Value.ToString() ?? string.Empty;
        }
    }
}