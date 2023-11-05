using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Cmdlet(VerbsCommon.Set, "UserInteractive")]
[OutputType(typeof(void))]
public class SetUserInteractiveCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    public bool Interactive { get; set; }

    protected override void ProcessRecord()
    {
        ModuleState.ShellInteractive = this.Interactive;
        Env.UserInteractive = this.Interactive;
        Env.Set("POWERSHELL_INTERACTIVE", this.Interactive.ToString());
    }
}