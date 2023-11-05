using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Management.Automation;

using Dapper;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Data;

[Cmdlet(VerbsCommunications.Write, "DbData")]
public class WriteDbDataCmdlet : PSCmdlet
{
    private const string ConnectionSet = "Connection";
    private const string TransactionSet = "Transaction";

    [Parameter(ParameterSetName = ConnectionSet, ValueFromPipeline = true)]
    public DbConnection? Connection { get; set; }

    [Parameter(ParameterSetName = TransactionSet, ValueFromPipeline = true)]
    public DbTransaction? Transaction { get; set; }

    [Parameter(ParameterSetName = ConnectionSet, Position = 0)]
    [Parameter(ParameterSetName = TransactionSet, Position = 0)]
    public string Sql { get; set; } = null!;

    [Parameter(ParameterSetName = ConnectionSet, Position = 1)]
    [Parameter(ParameterSetName = TransactionSet, Position = 1)]
    public object? Data { get; set; }

    [Parameter(ParameterSetName = ConnectionSet)]
    [Parameter(ParameterSetName = TransactionSet)]
    public int? Timeout { get; set; }

    [Parameter(ParameterSetName = ConnectionSet)]
    [Parameter(ParameterSetName = TransactionSet)]
    public CommandType Type { get; set; } = CommandType.Text;

    [Parameter(ParameterSetName = ConnectionSet)]
    [Parameter(ParameterSetName = TransactionSet)]
    public SwitchParameter PassThru { get; set; }

    protected override void ProcessRecord()
    {
        var conn = this.Connection;
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
            }
        }

        if (conn is null)
        {
            const string msg = "No connection was provided and no default connection was found";
            this.WriteError(new InvalidOperationException(msg));
            return;
        }

        if (this.Data is null)
        {
            this.WriteWarning("No data to write");
        }

        List<Dictionary<string, object?>> data = new List<Dictionary<string, object?>>();
        if (this.Data is IReadOnlyCollection<PSObject> psObjects)
        {
            foreach (var psObject in psObjects)
            {
                data.Add(psObject.ToDictionary());
            }
        }

        if (this.Data is IReadOnlyList<Dictionary<string, object?>> dictionaries)
        {
            data.AddRange(dictionaries);
        }

        if (this.Data is IReadOnlyList<Hashtable> hashtables)
        {
            foreach (var hashtable in hashtables)
            {
                data.Add(hashtable.ToDictionary());
            }
        }

        if (this.Data is IReadOnlyList<OrderedDictionary> orderedDictionaries)
        {
            foreach (var orderedDictionary in orderedDictionaries)
            {
                data.Add(orderedDictionary.ToDictionary());
            }
        }

        if (this.Data is PSObject psObject1)
        {
            data.Add(psObject1.ToDictionary());
        }

        if (this.Data is IDictionary hashtable1)
        {
            data.Add(hashtable1.ToDictionary());
        }

        if (this.Data is IDictionary<string, object?> lookup)
        {
            var map = new Dictionary<string, object?>();
            foreach (var kvp in lookup)
                map[kvp.Key] = kvp.Value;

            data.Add(map);
        }

        foreach (var row in data)
        {
            var result = conn.Execute(this.Sql, row, this.Transaction, this.Timeout, this.Type);
            if (this.PassThru.ToBool())
                this.WriteObject(result);
        }
    }
}