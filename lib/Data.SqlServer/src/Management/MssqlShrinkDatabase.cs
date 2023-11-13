using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlShrinkDatabase : SqlStatementBuilder
{
    public string Database { get; set; } = string.Empty;

    public int ShrinkPercentage { get; set; } = 10;

    public bool WithLowPriority { get; set; } = false;

    public override Result<(string, object?), Exception> Build()
    {
        if (!Validate.Identifier(this.Database.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.Database}");

        var sb = StringBuilderCache.Acquire();
        sb.Append("DBCC SHRINKDATABASE (")
            .Append(Quote.Identifier(this.Database.AsSpan()))
            .Append(",")
            .Append(this.ShrinkPercentage);

        if (this.WithLowPriority)
        {
            sb.Append(" WITH WAIT_AT_LOW_PRIORITY (ABORT_AFTER_WAIT = SELF);");
        }

        var sql = StringBuilderCache.GetStringAndRelease(sb);
        return (sql, null);
    }
}