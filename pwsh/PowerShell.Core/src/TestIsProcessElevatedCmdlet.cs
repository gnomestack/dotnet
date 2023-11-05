using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("is_admin", "Test-UserIsAdministrator")]
[Cmdlet(VerbsDiagnostic.Test, "UserIsAdmin")]
[OutputType(typeof(bool))]
public class TestIsProcessElevatedCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(Env.IsPrivilegedProcess);
    }
}