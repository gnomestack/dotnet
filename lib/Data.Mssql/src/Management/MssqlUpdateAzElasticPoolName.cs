using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlUpdateAzElasticPoolName : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public string ElasticPoolName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        if (!MssqlValidate.Identifier(this.ElasticPoolName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid elastic pool name {this.ElasticPoolName}");

        var sb = StringBuilderCache.Acquire();
        sb.Append("ALTER DATABASE ")
            .Append(MssqlQuote.Identifier(this.DatabaseName.AsSpan()))
            .Append(" MODIFY (SERVICE_OBJECTIVE = ELASTIC_POOL (NAME = ")
            .Append(MssqlQuote.Identifier(this.ElasticPoolName.AsSpan()))
            .Append("));");
        var sql = StringBuilderCache.GetStringAndRelease(sb);
        return (sql, null);
    }
}