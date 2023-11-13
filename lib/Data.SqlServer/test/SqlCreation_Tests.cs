using System.Diagnostics.CodeAnalysis;

using GnomeStack.Data;
using GnomeStack.Data.SqlServer;
using GnomeStack.Data.SqlServer.Management;

using Microsoft.Data.SqlClient;

using Xunit.Abstractions;

namespace Tests;

[SuppressMessage("ReSharper", "UseAwaitUsing")]
public class SqlCreation_Tests : MssqlTestBase
{
    public SqlCreation_Tests(ITestOutputHelper writer)
        : base(writer)
    {
    }

    [IntegrationTest]
    public async Task CreateDatabaseAsync()
    {
        using var connection = new SqlConnection(this.ConnectString);
        await connection.ExecAsync(new MssqlCreateDatabase("king_bob"));

        var hasDbResult = await connection.ScalarAsResultAsync<int>(new MssqlSelectDbExists("king_bob"));
        Assert.False(hasDbResult.IsError);
        Assert.True(hasDbResult.Unwrap() == 1);

        await connection.ExecAsync(new MssqlDropDatabase("king_bob"));

        hasDbResult = await connection.ScalarAsResultAsync<int>(new MssqlSelectDbExists("king_bob"));
        Assert.False(hasDbResult.IsError);
        Assert.True(hasDbResult.Unwrap() == 0);
    }
}