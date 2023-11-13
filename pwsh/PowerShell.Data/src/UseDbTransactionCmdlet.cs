using System.Data;
using System.Data.Common;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Data;

[Alias("use_db_transaction")]
[OutputType(typeof(void))]
[Cmdlet(VerbsOther.Use, "DbTransaction")]
public class UseDbTransactionCmdlet : PSCmdlet
{
    [Parameter(ValueFromPipeline = true)]
    public DbConnection? Connection { get; set; }

    [Parameter]
    public DbTransaction? Transaction { get; set; }

    [Parameter(Position = 0, Mandatory = true)]
    public ScriptBlock Action { get; set; } = null!;

    protected override void ProcessRecord()
    {
        var conn = this.Connection;
        var connectionCreated = false;
        if (conn is null)
        {
            if (this.Transaction is not null)
                conn = this.Transaction.Connection;

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
                connectionCreated = true;
            }
        }

        if (conn is null)
        {
            const string msg = "No connection was provided and no default connection was found";
            this.WriteError(new InvalidOperationException(msg));
            return;
        }

        var tx = this.Transaction;
        if (tx is null)
        {
            tx = conn.BeginTransaction();
            this.WriteVerbose($"Created transaction {tx.GetHashCode()}");
        }

        var variables = new List<PSVariable>
        {
            new PSVariable("_", conn),
            new PSVariable("Connection", conn),
            new PSVariable("conn", conn),
            new PSVariable("Transaction", tx),
            new PSVariable("tx", tx),
        };

        try
        {
            this.Action.InvokeWithContext(
                new Dictionary<string, ScriptBlock>(),
                variables);

            tx.Commit();
        }
        catch (Exception ex)
        {
            this.WriteError(ex);
            tx.Rollback();
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }
        finally
        {
            tx.Dispose();
            if (connectionCreated)
                conn.Dispose();
        }
    }
}