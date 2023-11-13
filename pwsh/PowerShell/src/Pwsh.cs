using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace GnomeStack.PowerShell;

public static class Pwsh
{
    public static System.Management.Automation.PowerShell Create()
    {
        return System.Management.Automation.PowerShell.Create();
    }

    public static System.Management.Automation.PowerShell Create(Action<InitialSessionState> configure)
    {
        var state = InitialSessionState.CreateDefault();
        configure(state);
        return System.Management.Automation.PowerShell.Create(state);
    }

    public static System.Management.Automation.PowerShell Create(InitialSessionState state)
    {
        return System.Management.Automation.PowerShell.Create(state);
    }

    public static System.Management.Automation.PowerShell Create(RunspaceMode mode)
    {
        return System.Management.Automation.PowerShell.Create(mode);
    }

    public static Command CreateCommand(string command)
    {
        return new Command(command);
    }

    public static Command CreateCommand(Type command)
    {
        var attr = command.GetCustomAttribute<CmdletAttribute>();
        if (attr is null)
            throw new ArgumentException($"Type {command.FullName} is not a Cmdlet.", nameof(command));

        return new Command($"{attr.VerbName}-{attr.NounName}");
    }

    public static Command CreateCommand<T>()
        where T : Cmdlet
        => CreateCommand(typeof(T));
}