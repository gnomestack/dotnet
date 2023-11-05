using System.Management.Automation;

using Dapper;

namespace GnomeStack.PowerShell.Data;

[Cmdlet(VerbsCommon.New, "DbDynamicParameter")]
public class NewDbDynamicParameterCmdlet : PSCmdlet
{
    protected override void BeginProcessing()
    {
        this.WriteObject(new DynamicParameters());
    }
}