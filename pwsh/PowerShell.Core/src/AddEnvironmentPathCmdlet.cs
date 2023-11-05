using System;
using System.Management.Automation;

using GnomeStack.Extras.Strings;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("add_env_path")]
[Cmdlet(VerbsCommon.Add, "EnvironmentPath")]
[OutputType(typeof(void))]
public class AddEnvironmentPathCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Path { get; set; } = Array.Empty<string>();

    [Alias("t")]
    [Parameter]
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Process;

    [Alias("p")]
    [Parameter]
    public SwitchParameter Prepend { get; set; }

    protected override void ProcessRecord()
    {
        if (!Env.IsWindows && this.Target != EnvironmentVariableTarget.Process)
        {
            this.WriteError(
                new PlatformNotSupportedException(
                    "Only EnvironmentVariableTarget.Process is supported on non-Windows platforms."));
            return;
        }

        foreach (var path in this.Path)
        {
            if (path.IsNullOrWhiteSpace())
                continue;

            Env.AddPath(path, this.Prepend, this.Target);
        }
    }
}