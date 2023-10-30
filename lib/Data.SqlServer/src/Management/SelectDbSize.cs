using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectDbSize : SqlStatementBuilder
{
    public FileSize Size { get; set; } = FileSize.MegaBytes;

    public override Result<string, Exception> Build()
    {
        var sb = StringBuilderCache.Acquire();
        sb.Append("SELECT SUM(CAST(FILEPROPERTY(name, 'SpaceUsed') AS bigint) * 8192.)");
        int times = (int)this.Size;

        while (times > 0)
        {
            sb.Append(" / 1024 ");
            times--;
        }

        sb.Append(" AS size")
            .AppendLine();
        sb.Append("FROM sys.database_files WHERE type_desc = 'ROWS';");

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}