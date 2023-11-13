using System;
using System.Collections;
using System.Management.Automation;

using GnomeStack.Extras.Strings;
using GnomeStack.Secrets;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("set_env")]
[Cmdlet(VerbsCommon.Set, "EnvironmentVariable")]
public class SetEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "NameValue")]
    public string Name { get; set; } = string.Empty;

    [Parameter(Position = 1, Mandatory = true, ParameterSetName = "NameValue")]
    public string Value { get; set; } = string.Empty;

    [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Values")]
    public IDictionary? VariableSet { get; set; }

    [Parameter(ParameterSetName = "Values")]
    [Parameter(ParameterSetName = "NameValue")]
    public SwitchParameter AsSecret { get; set; }

    [Parameter(ParameterSetName = "Values", Position = 2)]
    [Parameter(ParameterSetName = "NameValue", Position = 3)]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
        if (this.Name.IsNullOrWhiteSpace() && this.VariableSet is null)
        {
            this.ThrowTerminatingError(new PSArgumentNullException(nameof(this.Name), "The -Name or -VariableSet parameter must be set"));
        }

        if (this.Target == EnvironmentVariableTarget.Machine && !Env.IsPrivilegedProcess)
        {
            var ex = new UnauthorizedAccessException("You must be an administrator or root to remove machine environment variables.");
            this.ThrowTerminatingError(ex);
        }

        if (this.VariableSet != null)
        {
            foreach (var key in this.VariableSet.Keys)
            {
                if (key is not string name)
                    continue;

                var value = this.VariableSet[key] as string;
                if (value == null)
                    continue;

                if (this.AsSecret.ToBool())
                {
                    SecretMasker.Default.Add(value);
                }

                if (Env.IsWindows)
                {
                    Env.Set(name, value, this.Target);
                    return;
                }

                if (this.Target == EnvironmentVariableTarget.Process)
                {
                    Env.Set(name, value);
                }

                var ex = new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
                this.WriteError(ex, name);
            }

            return;
        }

        if (this.AsSecret.ToBool())
        {
            SecretMasker.Default.Add(this.Value);
        }

        if (Env.IsWindows)
        {
            Env.Set(this.Name, this.Value, this.Target);
            return;
        }

        if (this.Target == EnvironmentVariableTarget.Process)
        {
            Env.Set(this.Name, this.Value);
        }

        var ex2 = new PlatformNotSupportedException("Non-Windows platforms only support setting process environment variables.");
        this.WriteError(ex2, this.Name);
    }
}