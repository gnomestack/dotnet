using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectAzDbTierInfo : SqlStatementBuilder
{
    public override Result<string, Exception> Build()
    {
        return """
               --- noinspection SqlNoDataSourceInspectionForFile
               SELECT d.name, slo.edition, slo.service_objective, slo.elastic_pool_name
               FROM sys.databases d
               JOIN sys.database_service_objectives slo
               ON d.database_id = slo.database_id
               """;
    }
}