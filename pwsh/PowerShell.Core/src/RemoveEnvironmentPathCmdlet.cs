using System;
using System.Management.Automation;
using System.Security;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Cmdlet(VerbsCommon.Remove, "EnvironmentPath")]
[OutputType(typeof(void))]
public class RemoveEnvironmentPathCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Path { get; set; } = Array.Empty<string>();

    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    protected override void ProcessRecord()
    {
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

        foreach (var path in this.Path)
        {
            if (string.IsNullOrWhiteSpace(path))
                continue;

            Env.RemovePath(path, this.Target);
        }
    }
}