using System.Data.Common;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Data;

[Alias("use_db_connection", "use_db_conn")]
[OutputType(typeof(void))]
[Cmdlet(VerbsOther.Use, "DbConnection")]
public class UseDbConnectionCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true)]
    public DbConnection? Connection { get; set; }

    [Parameter(Position = 0, Mandatory = true)]
    public ScriptBlock Action { get; set; } = null!;

    protected override void ProcessRecord()
    {
        var conn = this.Connection;
        var created = false;
        if (conn is null)
        {
            if (conn is null && ProviderRegistry.DefaultConnection is not null)
                conn = ProviderRegistry.DefaultConnection;

            if (conn is null && !ProviderRegistry.DefaultConnectionString.IsNullOrWhiteSpace())
            {
                var factory = ProviderRegistry.Get(ProviderRegistry.DefaultProviderName);
                if (factory is null)
                {
                    var msg = $"The provider {ProviderRegistry.DefaultProviderName} was not found";
                    this.WriteError(new InvalidOperationException(msg));
                    return;
                }

                conn = factory.CreateConnection();
                if (conn is null)
                {
                    var msg = $"The provider {factory.GetType().FullName} was unable to create a DbConnection";
                    this.WriteError(new InvalidOperationException(msg));
                    return;
                }

                conn.ConnectionString = ProviderRegistry.DefaultConnectionString;
                created = true;
            }
        }

        if (conn is null)
        {
            const string msg = "No connection was provided and no default connection was found";
            this.WriteError(new InvalidOperationException(msg));
            return;
        }

        var variables = new List<PSVariable>
        {
            new PSVariable("_", conn),
            new PSVariable("Connection", conn),
            new PSVariable("Conn", conn),
        };

        try
        {
            this.Action.InvokeWithContext(
                new Dictionary<string, ScriptBlock>(),
                variables);
        }
        finally
        {
            if (created)
                conn.Dispose();
        }

        base.ProcessRecord();
    }
}