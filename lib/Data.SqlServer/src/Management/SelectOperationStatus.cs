using GnomeStack.Functional;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectOperationStatus : SqlStatementBuilder
{
    public string? DatabaseName { get; set; }

    public List<string> OperationNames { get; set; } = new();

    public static implicit operator string(SelectOperationStatus cmd)
        => cmd.Build().Unwrap();

    public override Result<string, Exception> Build()
    {
        if (!this.DatabaseName.IsNullOrWhiteSpace() && !Validate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        foreach (var operationName in this.OperationNames)
        {
            if (!Validate.Identifier(operationName.AsSpan()))
                return new InvalidDbIdentifierException($"Invalid operation name {operationName}");
        }

        var query = """
                    ---noinspection SqlNoDataSourceInspectionForFile
                    SELECT 
                            state, 
                            major_resource_id, 
                            operation, 
                            percent_complete, 
                            start_time, 
                            last_modify_time, 
                            error_code, 
                            error_message
                        FROM sys.dm_operation_status
                    """;

        if (!this.DatabaseName.IsNullOrWhiteSpace() && this.OperationNames.Count == 0)
            return query;

        var sb = StringBuilderCache.Acquire();
        if (this.OperationNames.Count > 0)
        {
            var i = 0;
            sb.Append("    WHERE (");
            foreach (var operationName in this.OperationNames)
            {
                if (i > 0)
                    sb.Append(" OR ");

                sb.Append("operation = '")
                    .Append(operationName)
                    .Append('\'');

                i++;
            }

            sb.Append(")");
        }

        if (!this.DatabaseName.IsNullOrWhiteSpace())
        {
            if (this.OperationNames.Count > 0)
            {
                sb.AppendLine()
                    .Append("        AND");
            }

            sb.Append(" major_resource_id = '")
                .Append(this.DatabaseName)
                .Append('\'');
        }

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}