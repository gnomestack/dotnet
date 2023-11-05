using System.Management.Automation;

using GnomeStack.Diagnostics;

namespace GnomeStack.PowerShell;

public static class PsCmdletExtensions
{
    private static bool s_backwardsCompatibilityLayerRegistered;

    public static CommandInfo GetCommand(this PSCmdlet cmdlet, string command)
    {
        return cmdlet.InvokeCommand.GetCommand(command, CommandTypes.All);
    }

    public static CommandInfo GetCommand(this PSCmdlet cmdlet, string command, CommandTypes commandTypes)
    {
        return cmdlet.InvokeCommand.GetCommand(command, commandTypes);
    }

    public static CmdletInfo GetCmdlet(this PSCmdlet cmdlet, string command)
    {
        return cmdlet.InvokeCommand.GetCmdlet(command);
    }

    public static ActionPreference GetCommandPreference(this PSCmdlet cmdlet)
    {
        var globalPreference = cmdlet.GetGlobalVariable("CommandPreference")?.Value;
        if (globalPreference is null)
            return ActionPreference.Continue;

        if (globalPreference is ActionPreference globalActionPreference)
            return globalActionPreference;
        if (globalPreference is string globalActionPreferenceString &&
            Enum.TryParse<ActionPreference>(globalActionPreferenceString, out var actionPreferenceEnum))
            return actionPreferenceEnum;

        return ActionPreference.Continue;
    }

    public static PSVariable? GetVariable(this PSCmdlet cmdlet, string name)
    {
        return cmdlet.SessionState.PSVariable.Get(name);
    }

    public static PSVariable? GetGlobalVariable(this PSCmdlet cmdlet, string name)
        => GetVariable(cmdlet, $"Global:{name}");

    public static PSVariable? GetPrivateVariable(this PSCmdlet cmdlet, string name)
        => GetVariable(cmdlet, $"Private:{name}");

    public static PSVariable? GetScriptVariable(this PSCmdlet cmdlet, string name)
        => GetVariable(cmdlet, $"Script:{name}");

    public static void SetVariable(this PSCmdlet cmdlet, string name, object? value)
    {
        cmdlet.SessionState.PSVariable.Set(name, value);
    }

    public static void SetVariable(this PSCmdlet cmdlet, PSVariable variable)
    {
        cmdlet.SessionState.PSVariable.Set(variable);
    }

    public static Dictionary<string, object?> GetBoundParameters(
        this PSCmdlet cmdlet,
        Dictionary<string, object?> boundParameters)
        => cmdlet.MyInvocation.BoundParameters;

    public static bool IsDebug(this PSCmdlet cmdlet, bool checkPreference = true)
    {
        bool debug = false;
        if (cmdlet.MyInvocation.BoundParameters.TryGetValue("Debug", out var debugValue))
        {
            if (debugValue is bool debugBool)
                debug = debugBool;
            else if (debugValue is string debugString)
                debug = debugString.Equals("true", StringComparison.OrdinalIgnoreCase);

            return debug;
        }

        if (!checkPreference)
            return false;

        if (cmdlet.GetVariableValue("DebugPreference") is ActionPreference debugPreference)
            return debugPreference != ActionPreference.SilentlyContinue;

        return false;
    }

    public static bool IsVerbose(this PSCmdlet cmdlet, bool checkPreference = true)
    {
        bool debug = false;
        if (cmdlet.MyInvocation.BoundParameters.TryGetValue("Verbose", out var debugValue))
        {
            if (debugValue is bool debugBool)
                debug = debugBool;
            else if (debugValue is string debugString)
                debug = debugString.Equals("true", StringComparison.OrdinalIgnoreCase);

            return debug;
        }

        if (!checkPreference)
            return false;

        if (cmdlet.GetVariableValue("VerbosePreference") is ActionPreference debugPreference)
            return debugPreference != ActionPreference.SilentlyContinue;

        return false;
    }

    public static PSCmdlet WriteError(this PSCmdlet cmdlet, Exception exception)
    {
        var errorRecord = new ErrorRecord(exception, exception.GetType().Name, ErrorCategory.NotSpecified, null);
        cmdlet.WriteError(errorRecord);
        return cmdlet;
    }

    public static PSCmdlet WriteError(this PSCmdlet cmdlet, Exception exception, object? targetObject)
    {
        var errorRecord = new ErrorRecord(exception, exception.GetType().Name, ErrorCategory.NotSpecified, null);
        cmdlet.WriteError(errorRecord);
        return cmdlet;
    }

    public static PSCmdlet WriteError(
        this PSCmdlet cmdlet,
        Exception exception,
        string errorId,
        ErrorCategory errorCategory = ErrorCategory.NotSpecified,
        object? targetObject = null)
    {
        var errorRecord = new ErrorRecord(exception, errorId, errorCategory, null);
        cmdlet.WriteError(errorRecord);
        return cmdlet;
    }

    public static PSCmdlet ThrowTerminatingError(this PSCmdlet cmdlet, Exception exception)
    {
        var errorRecord = new ErrorRecord(exception, exception.GetType().Name, ErrorCategory.NotSpecified, null);
        cmdlet.ThrowTerminatingError(errorRecord);
        return cmdlet;
    }

    public static PSCmdlet ThrowTerminatingError(this PSCmdlet cmdlet, Exception exception, object? targetObject)
    {
        var errorRecord = new ErrorRecord(exception, exception.GetType().Name, ErrorCategory.NotSpecified, null);
        cmdlet.ThrowTerminatingError(errorRecord);
        return cmdlet;
    }

    public static PSCmdlet ThrowTerminatingError(
        this PSCmdlet cmdlet,
        Exception exception,
        string errorId,
        ErrorCategory errorCategory = ErrorCategory.NotSpecified,
        object? targetObject = null)
    {
        var errorRecord = new ErrorRecord(exception, errorId, errorCategory, null);
        cmdlet.ThrowTerminatingError(errorRecord);
        return cmdlet;
    }

    public static PSCmdlet WriteCommand(
        this PSCmdlet cmdlet,
        string command,
        PsArgs? args = null,
        ActionPreference? actionPreference = null)
    {
        if (actionPreference is null)
            actionPreference = cmdlet.GetCommandPreference();

        switch (actionPreference)
        {
            case ActionPreference.Continue:
                var (message, color) = CommandFormatter.FormatMessage(command, args);
                if (!color)
                {
                    cmdlet.Host.UI.WriteLine(message);
                }
                else
                {
                    cmdlet.Host.UI.WriteLine(ConsoleColor.Cyan, cmdlet.Host.UI.RawUI.BackgroundColor, message);
                }

                break;

            default:
                break;
        }

        return cmdlet;
    }
}