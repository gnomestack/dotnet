using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlSelectAllocatedDbSize : SqlStatementBuilder
{
    public FileSize Size { get; set; } = FileSize.MegaBytes;

    public override Result<(string, object?), Exception> Build()
    {
        var sb = StringBuilderCache.Acquire();

        // gets kb
        sb.Append("SELECT (SUM(reserved_page_count) * 8.0) ");
        int times = (int)this.Size;

        // if bytes, multiply by 1024
        if (times < 1)
        {
            sb.Append(" * 1024 ");
        }

        // if bigger than kb, divide by 1024 for each size
        while (times > 1)
        {
            sb.Append(" / 1024 ");
            times--;
        }

        sb.Append(" AS size")
            .AppendLine();

        sb.Append("FROM sys.dm_db_partition_stats;");
        var sql = StringBuilderCache.GetStringAndRelease(sb);
        return (sql, null);
    }
}