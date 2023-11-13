using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectTableStats : SqlStatementBuilder
{
    public override Result<(string, object?), Exception> Build()
    {
        var sql = """
                    ---noinspection SqlNoDataSourceInspectionForFile
                    SELECT DISTINCT
                        OBJECT_NAME(s.[object_id]) AS table_name,
                        c.name AS column_name,
                        s.name AS stats_name,
                        s.stats_id,
                        sc.stats_column_id,
                        s.auto_created,
                        s.user_created,
                        s.no_recompute,
                        s.[object_id],
                        sc.column_id,
                        STATS_DATE(s.[object_id], s.stats_id) AS last_updated
                    FROM sys.stats s 
                        JOIN sys.stats_columns sc 
                            ON sc.[object_id] = s.[object_id] 
                            AND sc.stats_id = s.stats_id
                        JOIN sys.columns c 
                            ON c.[object_id] = sc.[object_id]
                            AND c.column_id = sc.column_id
                        JOIN sys.partitions par 
                            ON par.[object_id] = s.[object_id]
                        JOIN sys.objects obj 
                            ON par.[object_id] = obj.[object_id]
                        WHERE OBJECTPROPERTY(s.OBJECT_ID,'IsUserTable') = 1
                            AND (s.auto_created = 1 OR s.user_created = 1);
                    """;
        return (sql, null);
    }
}