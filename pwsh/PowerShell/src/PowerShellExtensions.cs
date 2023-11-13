namespace GnomeStack.PowerShell;

public static class PowerShellExtensions
{
    public static System.Management.Automation.PowerShell AddParameterSet(this System.Management.Automation.PowerShell shell, params (string, object?)[] parameters)
    {
        foreach (var parameter in parameters)
            shell.AddParameter(parameter.Item1, parameter.Item2);

        return shell;
    }

    public static System.Management.Automation.PowerShell AddParameterSet(this System.Management.Automation.PowerShell shell, IEnumerable<KeyValuePair<string, object?>> parameters)
    {
        foreach (var parameter in parameters)
            shell.AddParameter(parameter.Key, parameter.Value);

        return shell;
    }

    public static T InvokeAsSingle<T>(this System.Management.Automation.PowerShell shell)
    {
        var result = shell.Invoke<T>();
        if (result is null || result.Count == 0)
            return default!;

        return result[0];
    }
}