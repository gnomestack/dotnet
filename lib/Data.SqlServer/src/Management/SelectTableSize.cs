using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectTableSize : SqlStatementBuilder
{
    public FileSize Size { get; set; } = FileSize.MegaBytes;

    public override Result<string, Exception> Build()
    {
        var sb = StringBuilderCache.Acquire();
        int times = (int)this.Size;
        if (times == 0)
        {
            sb.Append(" * 1024");
        }

        while (times > 1)
        {
            sb.Append(" / 1024 ");
            times--;
        }

        var calc = StringBuilderCache.GetStringAndRelease(sb);
        return $"""
                    ---noinspection SqlNoDataSourceInspectionForFile
                    SELECT
                        s.Name AS schema_name,
                        t.Name AS table_name,
                        p.rows AS row_count,
                        (SUM(a.used_pages) * 8.0) {calc} AS used_space,
                        ((SUM(a.total_pages) - SUM(a.used_pages)) * 8.0) {calc} AS unused_space,
                        (SUM(a.total_pages) * 8.0) {calc} AS total_space
                    FROM sys.tables t
                        INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
                        INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                        INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
                        INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                    GROUP BY t.Name, s.Name, p.Rows
                    ORDER BY s.Name, t.Name
                    """;
    }
}