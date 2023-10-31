using GnomeStack.Data.SqlServer.Management;
using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class UpdateElasticPoolName : SqlStatementBuilder
{
    public string DatabaseName { get; set; } = string.Empty;

    public string ElasticPoolName { get; set; } = string.Empty;

    public static implicit operator string(UpdateElasticPoolName cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!Validate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        if (!Validate.Identifier(this.ElasticPoolName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid elastic pool name {this.ElasticPoolName}");

        var sb = StringBuilderCache.Acquire();
        sb.Append("ALTER DATABASE ")
            .Append(Quote.Identifier(this.DatabaseName.AsSpan()))
            .Append(" MODIFY (SERVICE_OBJECTIVE = ELASTIC_POOL (NAME = ")
            .Append(Quote.Identifier(this.ElasticPoolName.AsSpan()))
            .Append("));");
        return StringBuilderCache.GetStringAndRelease(sb);
    }
}