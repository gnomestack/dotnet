using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectElasticPoolNames : SqlStatementBuilder
{
    public int Hours { get; set; } = 2;

    public static implicit operator string(SelectElasticPoolNames cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        var sb = StringBuilderCache.Acquire();
        sb.Append(
                """
                ---noinspection SqlNoDataSourceInspectionForFile
                SELECT temp.elastic_pool_name
                    FROM
                        (
                            SELECT 
                                elastic_pool_name, 
                                MAX(end_time) max_end
                            FROM sys.elastic_pool_resource_stats WITH (NOLOCK)
                            GROUP BY elastic_pool_name
                        ) temp
                    WHERE temp.max_end > DATEADD(hour,
                """)
            .Append(this.Hours)
            .AppendLine(", GETUTCDATE());");
        return StringBuilderCache.GetStringAndRelease(sb);
    }
}