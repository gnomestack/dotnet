using System.Data;

namespace GnomeStack.Data.Mssql.Management;

public static class GsMssqlConnectionExtensions
{
    public static void DropMssqlLogin(IDbConnection connection, string loginName)
    {
        var processIds = connection.Query<int>(new MssqlSelectLoginProcessIds(loginName));
        foreach (var processId in processIds)
        {
            connection.Exec($"KILL {processId}");
        }

        connection.Exec(new MssqlDropLogin(loginName));
    }

    public static async Task DropMssqlLoginAsync(this IDbConnection connection, string loginName)
    {
        var processIds = await connection.QueryAsync<int>(new MssqlSelectLoginProcessIds(loginName));
        foreach (var processId in processIds)
        {
            await connection.ExecAsync($"KILL {processId}");
        }

        await connection.ExecAsync(new MssqlDropLogin(loginName));
    }
}