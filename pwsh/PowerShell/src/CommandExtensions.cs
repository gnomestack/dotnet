using System.Management.Automation.Runspaces;

namespace GnomeStack.PowerShell;

public static class CommandExtensions
{
    public static Command AddParameter(this Command command, string name, object? value)
    {
        command.Parameters.Add(name, value);
        return command;
    }

    public static Command AddParameter(this Command command, string name)
    {
        command.Parameters.Add(name);
        return command;
    }

    public static Command AddParameters(this Command command, params (string Name, object? Value)[] parameters)
    {
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter.Name, parameter.Value);

        return command;
    }

    public static Command AddParameters(this Command command, IEnumerable<KeyValuePair<string, object?>> parameters)
    {
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter.Key, parameter.Value);

        return command;
    }

    public static Command AddTo(this Command command, System.Management.Automation.PowerShell shell)
    {
        shell.Commands.AddCommand(command);
        return command;
    }
}