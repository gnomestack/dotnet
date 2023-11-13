using System.Data.Common;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Data;

[Alias("new_db_connection", "new_db_conn")]
[Cmdlet(VerbsCommon.New, "DbConnection")]
public class NewDbConnectionCmdlet : PSCmdlet
{
    private const string ProviderSet = "Provider";
    private const string ProviderNameSet = "ProviderName";

    [Parameter(
        ParameterSetName = ProviderNameSet,
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true)]
    public string? ProviderName { get; set; }

    [Parameter(
        ParameterSetName = ProviderSet,
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true)]
    public DbProviderFactory? Provider { get; set; }

    [Parameter(ParameterSetName = ProviderNameSet, Position = 1)]
    [Parameter(ParameterSetName = ProviderSet, Position = 1)]
    public string? ConnectionString { get; set; }

    protected override void ProcessRecord()
    {
        if (this.ProviderName.IsNullOrWhiteSpace())
            this.ProviderName = ProviderRegistry.DefaultProviderName;

        if (this.Provider is null)
        {
            this.Provider = ProviderRegistry.Get(this.ProviderName);
            if (this.Provider is null)
            {
                var msg = $"The provider {this.ProviderName} was not found";
                this.WriteError(new InvalidOperationException(msg));
                return;
            }
        }

        var conn = this.Provider.CreateConnection();
        if (!this.ConnectionString.IsNullOrWhiteSpace())
            conn!.ConnectionString = this.ConnectionString;

        if (conn is null)
        {
            var msg = $"The provider {this.Provider.GetType().FullName} was unable to create a DbConnection";
            this.WriteError(new InvalidOperationException(msg));
            return;
        }

        this.WriteObject(conn);
    }
}