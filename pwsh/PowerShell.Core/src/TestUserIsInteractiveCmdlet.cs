using System;
using System.Linq;
using System.Management.Automation;

using GnomeStack.Extras.Strings;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("is_user_interactive")]
[Cmdlet(VerbsDiagnostic.Test, "UserIsInteractive")]
[OutputType(typeof(bool))]
public class TestUserIsInteractiveCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        if (ModuleState.ShellInteractive.HasValue)
        {
            this.WriteObject(ModuleState.ShellInteractive.Value);
            return;
        }

        var pwshInteractive = Env.Get("POWERSHELL_INTERACTIVE");
        if (pwshInteractive is not null && !pwshInteractive.Equals("true", StringComparison.OrdinalIgnoreCase) && pwshInteractive != "1")
        {
            ModuleState.ShellInteractive = false;
            Env.UserInteractive = false;
            this.WriteObject(Env.UserInteractive);
            Env.Set("POWERSHELL_INTERACTIVE", ModuleState.ShellInteractive.Value.ToString());
            return;
        }

        ModuleState.ShellInteractive = false;
        if (!Env.UserInteractive)
        {
            ModuleState.ShellInteractive = false;
        }

        Env.Set("POWERSHELL_INTERACTIVE", ModuleState.ShellInteractive.Value.ToString());

        this.WriteObject(ModuleState.ShellInteractive);
    }
}