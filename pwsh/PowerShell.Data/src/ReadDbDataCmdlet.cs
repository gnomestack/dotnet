using System.Collections;
using System.Data;
using System.Data.Common;
using System.Management.Automation;
using System.Text.RegularExpressions;

using Dapper;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Data;

[Alias("read_db_data")]
[OutputType(typeof(PSObject), typeof(List<PSObject>), typeof(List<List<PSObject>>))]
[Cmdlet(VerbsCommunications.Read, "DbData")]
public class ReadDbDataCmdlet : PSCmdlet
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
    public object? Parameters { get; set; }

    [Parameter(ParameterSetName = ConnectionSet)]
    [Parameter(ParameterSetName = TransactionSet)]
    public int? Timeout { get; set; }

    [Parameter(ParameterSetName = ConnectionSet)]
    [Parameter(ParameterSetName = TransactionSet)]
    public CommandType Type { get; set; } = CommandType.Text;

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

        object? parameters = this.Parameters;
        if (this.Parameters is Hashtable hashtable)
        {
            parameters = hashtable.ToDictionary();
        }

        if (this.Parameters is PSObject psObj)
        {
            parameters = psObj.ToDictionary();
        }

        if (parameters is IReadOnlyCollection<object?> values)
        {
            int i = 0;
            Regex.Replace(this.Sql, "\\?", m =>
            {
                var replacement = $"@{i}";
                i++;
                return replacement;
            });

            if (i != values.Count)
            {
                var msg = "The number of '?' placeholders in the query does not match the number of parameters";
                this.WriteError(new InvalidOperationException(msg));
                return;
            }

            var map = new Dictionary<string, object?>();
            i = 0;
            foreach (var value in values)
            {
                if (value is PSObject psObject)
                {
                    map[i.ToString()] = psObject.BaseObject;
                }
                else
                {
                    map[i.ToString()] = value;
                }

                i++;
            }

            parameters = map;
        }

        if (parameters is not IDictionary<string, object?> and not DynamicParameters)
        {
            const string msg = "The parameters must be a hashtable, PSObject," +
                               " Dictionary<string, object?> or DynamicParameters";
            this.WriteError(new InvalidOperationException(msg));
            return;
        }

        using var dr = conn.ExecuteReader(
            this.Sql,
            parameters,
            this.Transaction,
            this.Timeout,
            this.Type);
        var list = new List<PSObject>();
        while (dr.Read())
        {
            var record = new PSObject();
            for (var i = 0; i < dr.FieldCount; i++)
            {
                var name = dr.GetName(i);
                var value = dr.GetValue(i);
                record.Properties.Add(new PSNoteProperty(name, value));
            }

            list.Add(record);
        }

        var nextResultSet = dr.NextResult();
        if (!nextResultSet)
        {
            foreach (var item in list)
            {
                this.WriteObject(item);
            }

            return;
        }

        var sets = new List<List<PSObject>> { list };
        while (nextResultSet)
        {
            var nextList = new List<PSObject>();
            while (dr.Read())
            {
                var record = new PSObject();
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName(i);
                    var value = dr.GetValue(i);
                    record.Properties.Add(new PSNoteProperty(name, value));
                }

                nextList.Add(record);
            }

            sets.Add(nextList);
            nextResultSet = dr.NextResult();
        }

        this.WriteObject(sets);
    }
}