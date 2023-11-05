using System;
using System.IO;
using System.Management.Automation;
using System.Security;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("unset_env")]
[Cmdlet(VerbsCommon.Remove, "EnvironmentVariable")]
public class RemoveEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
        if (Env.IsWindows)
        {
            Env.Remove(this.Name, this.Target);
            return;
        }

        if (!Env.IsWindows && this.Target != EnvironmentVariableTarget.Process)
        {
            this.WriteError(
                new PlatformNotSupportedException(
                    "Only EnvironmentVariableTarget.Process is supported on non-Windows platforms."));
            return;
        }

        if (this.Target == EnvironmentVariableTarget.Machine && !Env.IsPrivilegedProcess)
        {
            this.WriteError(
                new SecurityException("You must be running as an administrator to modify the machine environment path."));
            return;
        }

        Env.Remove(this.Name);
    }
}