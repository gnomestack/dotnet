using System.Management.Automation;

namespace GnomeStack.PowerShell.Data;

[Alias("add_db_provider", "add_db_provider_factory")]
[Cmdlet(VerbsCommon.Add, "DbProvider")]
public class AddDbProviderCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string Name { get; set; } = null!;

    [Parameter(Position = 1, Mandatory = true)]
    public System.Data.Common.DbProviderFactory Factory { get; set; } = null!;

    protected override void ProcessRecord()
    {
        ProviderRegistry.Register(this.Name, this.Factory);
    }
}