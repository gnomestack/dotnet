using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectResourceStats : SqlStatementBuilder
{
    public int? Hours { get; set; } = null;

    public int? Days { get; set; } = null;

    public string? DatabaseName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!this.DatabaseName.IsNullOrWhiteSpace() && !MssqlValidate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        var query = """
                    ---noinspection SqlNoDataSourceInspectionForFile
                    SELECT 
                        rs.database_name,
                        rs.sku,
                        storage_mb = MAX(rs.Storage_in_megabytes),
                        avg_cpu_percent = AVG(rs.avg_cpu_percent),
                        max_cpu_percent = MAX(rs.avg_cpu_percent),
                        avg_io_percent = AVG(rs.avg_data_io_percent),
                        max_io_percent = MAX(rs.avg_data_io_percent),
                        avg_log_write_percent = AVG(rs.avg_log_write_percent),
                        max_log_write_percent = MAX(rs.avg_log_write_percent),
                        avg_requests_percent = AVG(rs.max_worker_percent),
                        max_requests_percent = MAX(rs.max_worker_percent),
                        avg_sessions_percent = AVG(rs.max_session_percent),
                        max_sessions_percent = MAX(rs.max_session_percent)
                    FROM sys.resource_stats AS rs
                    """;

        var sb = StringBuilderCache.Acquire();
        sb.AppendLine(query)
            .Append("    WHERE ");
        if (!this.DatabaseName.IsNullOrWhiteSpace())
        {
            sb.Append(" rs.database_name = '")
                .Append(this.DatabaseName)
                .Append("\' AND ");
        }

        if (!this.Days.HasValue && !this.Hours.HasValue)
        {
            this.Days = 1;
        }

        if (this.Days.HasValue)
        {
            sb.Append(" rs.start_time > DATEADD(day, -")
                .Append(this.Days)
                .Append(", GETDATE())");
        }
        else if (this.Hours.HasValue)
        {
            sb.Append(" rs.start_time > DATEADD(hour, -")
                .Append(this.Hours)
                .Append(", GETDATE())");
        }

        sb.AppendLine()
            .Append("        GROUP BY rs.database_name, rs.sku;");

        var sql = StringBuilderCache.GetStringAndRelease(sb);
        return (sql, null);
    }
}