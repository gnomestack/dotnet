using System.Text;

using Dapper;

using GnomeStack.Text;

using Microsoft.Data.SqlClient;

namespace GnomeStack.Data.SqlServer.Management;

public static class SqlServer
{
    public static void CreateLogin(this SqlConnection connection, string loginName, string password)
    {
         connection.Execute($@"
            IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = '{loginName}')
            BEGIN
                CREATE LOGIN [{loginName}] WITH PASSWORD = '{password}';
            END");
    }

    public static string SelectVersion(this SqlConnection connection)
    {
        return connection.RetryQuerySingle<string>("SELECT @@VERSION");
    }

    public static bool CreateDatabase(this SqlConnection connection, CreateDatabaseOptions options)
    {
        if (options.Name.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("Database name is required", nameof(options));
        }

        bool? isAzure = null;

        if (!options.CopyFromDatabase.IsNullOrWhiteSpace() && !Utils.IsValidIdentifer(options.CopyFromDatabase))
        {
            throw new ArgumentException("Invalid database name", nameof(options));
        }

        var sb = GnomeStack.Text.StringBuilderCache.Acquire();
        sb.Append("CREATE DATABASE [")
        .Append(options.Name)
        .Append(']');

        if (!options.Collation.IsNullOrWhiteSpace())
        {
            sb.Append(" COLLATE ")
            .Append(options.Collation);
        }

        if (!options.DataFile.IsNullOrWhiteSpace())
        {
            isAzure = connection.SelectVersion().Contains("Azure", StringComparison.OrdinalIgnoreCase);
            if (isAzure is true)
                throw new NotSupportedException("Azure SQL does not support specifying data file locations");

            sb.Append(" ON PRIMARY (NAME = N'")
                .Append(options.Name)
                .Append("', FILENAME = N'")
                .Append(options.DataFile)
                .Append("', SIZE = 8192KB, FILEGROWTH = 65536KB)");

            if (!options.LogFile.IsNullOrWhiteSpace())
            {
                sb.Append(" LOG ON (NAME = N'")
                .Append(options.Name)
                .Append("_log', FILENAME = N'")
                .Append(options.LogFile)
                .Append("', SIZE = 8192KB, FILEGROWTH = 65536KB)");
            }
        }

        if (!options.CopyFromDatabase.IsNullOrWhiteSpace())
        {
            sb.Append(" AS COPY OF ")
            .Append(options.CopyFromDatabase);
        }

        var query = StringBuilderCache.GetStringAndRelease(sb);
        connection.Execute(query);
        return true;
    }
}