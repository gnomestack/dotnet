namespace GnomeStack.Data.SqlServer.Management;

public static class SqlFragments
{
    public static string DbCreationState =>
        """
        SELECT state
        FROM sys.dm_operation_status
        WHERE major_resource_id = @db AND (operation = 'CREATE DATABASE' OR operation = 'TERMINATE CONTINUOUS DATABASE COPY')
        ORDER BY last_modify_time DESC
        """;

    public static string AzOperationStatus =>
        """
        SELECT operation
            FROM sys.dm_operation_status
            WHERE major_resource_id = @db
                AND resource_type_desc = 'Database' AND (state = 0 OR state = 1);
        """;

    public static string DbsSizeInMb =>
        """
        SELECT (SUM(reserved_page_count) * 8192) / 1024 / 1024 AS DbSizeInMB
        FROM sys.dm_db_partition_stats;
        """;

    public static string LoginExists =>
        """
        IF (EXISTS (SELECT name  FROM master.sys.sql_logins WHERE name = @login))
            SELECT 1
        ELSE
            SELECT 0;
        """;

    public static string DbExists =>
        """
        IF(EXISTS(SELECT name  FROM master.sys.databases d WHERE name = @dbName))
            SELECT 1
        ELSE
            SELECT 0;
        """;

    public static string CreateSqlServerUser =>
        """
        CREATE USER [{0}] FOR LOGIN [{1}];
        """;

    public static string AddUserToRole =>
        """
        ALTER ROLE {0} ADD MEMBER [{1}];
        """;

    public static string Shrinking =>
        """
        DBCC SHRINKDATABASE ({0}, 10);
        """;

    public static string AzDbInfo =>
        """
        SELECT d.name, slo.edition, slo.service_objective, slo.elastic_pool_name 
        FROM sys.databases d 
        JOIN sys.database_service_objectives slo  
        ON d.database_id = slo.database_id
        """;

    public static string ChangePoolSize =>
        """
        ALTER DATABASE [{0}] MODIFY ( SERVICE_OBJECTIVE = '{1}' );";
        """;

    public static string ChangePoolEdition =>
        """
        ALTER DATABASE [{0}] MODIFY ( EDITION = '{1}' );";
        """;

    public static string ChangePoolName =>
        """
        ALTER DATABASE [{0}] MODIFY ( ELASTIC_POOL = [{1}] );";
        """;

    public static string CreatePool =>
        """
        CREATE ELASTIC POOL [{0}] ( SERVICE_OBJECTIVE = '{1}' )";
        """;

    // default -2
    public static string ListElasticPoolNames =>
        """
        SELECT temp.elastic_pool_name
            FROM
                (
                    SELECT elastic_pool_name, MAX(end_time) max_end
                    FROM sys.elastic_pool_resource_stats
                    GROUP BY elastic_pool_name
                ) temp
            WHERE temp.max_end > DATEADD(hour, {0}, GETUTCDATE());
        """;

    public static string ListElasticPoolStats =>
        """
        SELECT 
            poolEndTime.PoolName, 
            poolEndTime.EndTime,
            epStats2.elastic_pool_storage_limit_mb,
            epStats2.elastic_pool_dtu_limit
            FROM
            (
                SELECT 
                    epStats.elastic_pool_name PoolName, 
                    MAX(end_time) EndTime FROM sys.elastic_pool_resource_stats epStats
                GROUP BY epStats.elastic_pool_name
            ) poolEndTime
            JOIN sys.elastic_pool_resource_stats epStats2 
                ON (epStats2.elastic_pool_name = poolEndTime.PoolName 
                AND epStats2.end_time = poolEndTime.EndTime)
            WHERE poolEndTime.EndTime > DATEADD(hour, -2, GETUTCDATE());
        """;
}