using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class ShrinkDatabase : SqlStatementBuilder
{
    public string Database { get; set; } = string.Empty;

    public string ShrinkPercentage { get; set; } = string.Empty;

    public bool WithLowPriority { get; set; } = false;

    public static implicit operator string(ShrinkDatabase cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (Validate.Identifier(this.Database.AsSpan()))
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

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}