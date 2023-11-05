using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectHighCumulativeCpuQueries : SqlStatementBuilder
{
    public int Top { get; set; } = 15;

    public static implicit operator string(SelectHighCumulativeCpuQueries cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        var query = $"""
                     ---noinspection SqlNoDataSourceInspectionForFile
                     SELECT
                         highest_cpu_queries.plan_handle,
                         highest_cpu_queries.total_worker_time,
                         q.dbid,
                         q.objectid,
                         q.number,
                         q.encrypted,
                         q.[text]
                     FROM
                         (SELECT TOP {this.Top}
                                qs.plan_handle,
                                qs.total_worker_time
                             FROM
                                 sys.dm_exec_query_stats AS qs WITH (NOLOCK)
                             ORDER BY qs.total_worker_time desc
                         ) AS highest_cpu_queries
                     CROSS APPLY sys.dm_exec_sql_text(plan_handle) AS q
                     ORDER BY highest_cpu_queries.total_worker_time DESC;
                     """;

        return query;
    }
}