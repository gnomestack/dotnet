using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace GnomeStack.PowerShell;

public static class InitialSessionStateExtensions
{
    public static InitialSessionState AddCommand(this InitialSessionState state, string command)
    {
        state.Commands.Add(new SessionStateCmdletEntry(command, null, null));
        return state;
    }

    public static InitialSessionState AddCommand(this InitialSessionState state, Type command)
    {
        var attr = command.GetCustomAttribute<CmdletAttribute>();
        if (attr is null)
            throw new ArgumentException($"Type {command.FullName} is not a Cmdlet.", nameof(command));

        state.Commands.Add(new SessionStateCmdletEntry($"{attr.VerbName}-{attr.NounName}", command, null));
        return state;
    }

    public static InitialSessionState AddCommand<T>(this InitialSessionState state)
        where T : Cmdlet
        => state.AddCommand(typeof(T));

    public static InitialSessionState AddCommand(this InitialSessionState state, string command, Type implementation)
    {
        state.Commands.Add(new SessionStateCmdletEntry(command, implementation, null));
        return state;
    }

    public static InitialSessionState AddVariableRange(
        this InitialSessionState state,
        IEnumerable<KeyValuePair<string, object?>> variables)
    {
        foreach (var variable in variables)
            state.Variables.Add(new SessionStateVariableEntry(variable.Key, variable.Value, null));

        return state;
    }

    public static InitialSessionState AddVariable(this InitialSessionState state, string name, object? value)
    {
        state.Variables.Add(new SessionStateVariableEntry(name, value, null));
        return state;
    }

    public static InitialSessionState AddVariable(this InitialSessionState state, string name, object value, string? description)
    {
        state.Variables.Add(new SessionStateVariableEntry(name, value, description));
        return state;
    }

    public static InitialSessionState AddVariable(this InitialSessionState state, string name, object value, ScopedItemOptions options)
    {
        state.Variables.Add(new SessionStateVariableEntry(name, value, null, options));
        return state;
    }

    public static InitialSessionState AddVariable(this InitialSessionState state, string name, object value, string? description, ScopedItemOptions options)
    {
        state.Variables.Add(new SessionStateVariableEntry(name, value, description, options));
        return state;
    }

    public static InitialSessionState AddAlias(this InitialSessionState state, string name, string command)
    {
        state.Commands.Add(new SessionStateAliasEntry(name, command));
        return state;
    }
}