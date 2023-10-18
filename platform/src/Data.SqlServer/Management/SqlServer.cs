using Dapper;

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
}