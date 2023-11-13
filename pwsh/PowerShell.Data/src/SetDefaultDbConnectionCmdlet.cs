using System.Data.Common;
using System.Management.Automation;

namespace GnomeStack.PowerShell.Data;

[Alias("set_default_db_connection")]
[Cmdlet(VerbsCommon.Set, "DefaultDbConnection")]
public class SetDefaultDbConnectionCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true, Position = 0, Mandatory = true)]
    public DbConnection Connection { get; set; } = null!;

    protected override void ProcessRecord()
    {
        ProviderRegistry.DefaultConnection = this.Connection;
        this.WriteDebug("Default connection set");
    }
}