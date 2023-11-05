using System.Management.Automation;

namespace GnomeStack.PowerShell.Data;

[Alias("set_default_db_connection_string", "set_default_db_cs")]
[Cmdlet(VerbsCommon.Set, "DefaultDbConnectionString")]
public class SetDefaultDbConnectionStringCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true, Position = 0, Mandatory = true)]
    public string ConnectionString { get; set; } = null!;

    protected override void ProcessRecord()
    {
        ProviderRegistry.DefaultConnectionString = this.ConnectionString;
    }
}