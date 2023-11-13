using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectTableIndexFragmentation : SqlStatementBuilder
{
    public bool Detailed { get; set; } = false;

    public override Result<(string, object?), Exception> Build()
    {
        var set = this.Detailed ? "DETAILED" : "SAMPLED";
        var query = $"""
                    ---noinspection SqlNoDataSourceInspectionForFile
                    SELECT 
                        OBJECT_SCHEMA_NAME(ips.object_id) AS schema_name,
                        OBJECT_NAME(ips.object_id) AS object_name,
                        t.name AS table_name,
                        s.name AS schema_name,
                        i.name AS index_name,
                        i.type_desc AS index_type,
                        ips.avg_fragmentation_in_percent,
                        ips.avg_page_space_used_in_percent,
                        ips.page_count,
                        ips.alloc_unit_type_desc
                    FROM sys.dm_db_index_physical_stats(DB_ID(), default, default, default, '{set}') AS ips
                    INNER JOIN sys.indexes AS i
                        ON ips.object_id = i.object_id
                        AND ips.index_id = i.index_id
                    INNER JOIN sys.tables t 
                        ON t.object_id = ips.object_id
                    INNER JOIN sys.schemas s 
                        ON t.schema_id = s.schema_id
                    ORDER BY page_count DESC;
                    """;
        return (query, null);
    }
}