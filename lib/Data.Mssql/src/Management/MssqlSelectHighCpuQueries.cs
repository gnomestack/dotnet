using GnomeStack.Functional;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlSelectHighCpuQueries : SqlStatementBuilder
{
    public int Top { get; set; } = 15;

    public override Result<(string, object?), Exception> Build()
    {
        var query = $"""
                    ---noinspection SqlNoDataSourceInspectionForFile
                    SELECT TOP {this.Top} query_stats.query_hash AS query_hash,
                        SUM(query_stats.total_worker_time) / SUM(query_stats.execution_count) AS avg_cpu_time,
                         MIN(query_stats.statement_text) AS statement_text
                    FROM
                        (SELECT QS.*,
                            SUBSTRING(ST.text, (QS.statement_start_offset/2) + 1,
                                ((CASE statement_end_offset
                                    WHEN -1 THEN DATALENGTH(ST.text)
                                    ELSE QS.statement_end_offset END
                                - QS.statement_start_offset)/2) + 1) AS statement_text
                    FROM sys.dm_exec_query_stats AS QS WITH (NOLOCK)
                        CROSS APPLY sys.dm_exec_sql_text(QS.sql_handle) AS ST
                        ) AS query_stats
                    GROUP BY query_stats.query_hash
                    ORDER BY 2 DESC;
                    """;
        return (query, null);
    }
}