using System.Diagnostics.CodeAnalysis;

using GnomeStack.Data;
using GnomeStack.Data.Mssql.Management;

#if NETLEGACY
// Microsoft.Data.SqlClient throws an exception during type init when using mono
// for the TDS parser on linux.
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

using Xunit.Abstractions;

namespace Tests;

[SuppressMessage("ReSharper", "UseAwaitUsing")]
public class MssqlSqlCreation_Tests : MssqlTestBase
{
    public MssqlSqlCreation_Tests(ITestOutputHelper writer)
        : base(writer)
    {
    }

    [IntegrationTest]
    public async Task CreateDatabaseAsync()
    {
        FlexAssert.Default.SkipWhen(
            this.SkipTest,
            "Skipping test on Windows CI in Github Actions as it can't find the container");
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

    [IntegrationTest]
    public async Task CreateLoginAsync()
    {
        FlexAssert.Default.SkipWhen(
            this.SkipTest,
            "Skipping test on Windows CI in Github Actions as it can't find the container");
        using var connection = new SqlConnection(this.ConnectString);
        await connection.ExecAsync(new MssqlCreateLogin("king_bob", "p@ssw0rd"));

        var hasLoginResult = await connection.ScalarAsResultAsync<int>(new MssqlSelectLoginExists("king_bob"));
        Assert.False(hasLoginResult.IsError);
        Assert.True(hasLoginResult.Unwrap() == 1);

        var drop = new MssqlDropLogin("king_bob");
        this.Writer.WriteLine(drop.ToString());
        await connection.ExecAsync(drop);
        hasLoginResult = await connection.ScalarAsResultAsync<int>(new MssqlSelectLoginExists("king_bob"));
        Assert.False(hasLoginResult.IsError);
        Assert.True(hasLoginResult.Unwrap() == 0);
    }

    [IntegrationTest]
    public async Task CreateUserWithRoleAsync()
    {
        FlexAssert.Default.SkipWhen(
            this.SkipTest,
            "Skipping test on Windows CI in Github Actions as it can't find the container");
        using var connection = new SqlConnection(this.ConnectString);
        await connection.ExecAsync(new MssqlCreateDatabase("king_bob_db"));
        await connection.ExecAsync(new MssqlCreateLogin("king_bob_login", "p@ssw0rd"));

        this.Writer.WriteLine("Creating cs 2 with database king_bob_db...");
        var dbcsBuilder = new SqlConnectionStringBuilder(this.ConnectString)
        {
            InitialCatalog = "king_bob_db",
        };
        var dbcs = dbcsBuilder.ToString();
        var conn2 = new SqlConnection(dbcs);

        this.Writer.WriteLine("Creating user...");
        await conn2.ExecAsync(new MssqlCreateUser("king_bob_user", "king_bob_login"));

        this.Writer.WriteLine("Adding user to role...");
        await conn2.ExecAsync(new MssqlAddRoleMember("db_owner", "king_bob_user"));

        var hasUserResult = await conn2.ScalarAsResultAsync<int>(new MssqlSelectUserExists("king_bob_user"));
        Assert.False(hasUserResult.IsError);
        Assert.True(hasUserResult.Unwrap() == 1);

        this.Writer.WriteLine("Creating cs 2 with user king_bob_login...");
        dbcsBuilder.UserID = "king_bob_login";
        dbcsBuilder.Password = "p@ssw0rd";
        dbcs = dbcsBuilder.ToString();
        var conn3 = new SqlConnection(dbcs);

        this.Writer.WriteLine("Testing connection as new user...");
        var selectResult = await conn3.ScalarAsResultAsync<int>("SELECT 1");
        Assert.False(selectResult.IsError);
        Assert.True(selectResult.Unwrap() == 1);

        conn3.Dispose();

        this.Writer.WriteLine("Removing user from role...");
        await conn2.ExecAsync(new MssqlDropRoleMember("db_owner", "king_bob_user"));

        this.Writer.WriteLine("Dropping user...");
        await conn2.ExecAsync(new MssqlDropUser("king_bob_user"));

        hasUserResult = await conn2.ScalarAsResultAsync<int>(new MssqlSelectUserExists("king_bob_user"));
        Assert.False(hasUserResult.IsError);
        conn2.Dispose();

        await connection.DropMssqlLoginAsync("king_bob_login");
        await connection.ExecAsync(new MssqlDropConnections("king_bob_db"));
        await connection.ExecAsync(new MssqlDropDatabase("king_bob_db"));
    }

    [IntegrationTest]
    public async Task CreateUserWithRoleUsingSpAsync()
    {
        FlexAssert.Default.SkipWhen(
            this.SkipTest,
            "Skipping test on Windows CI in Github Actions as it can't find the container");
        using var connection = new SqlConnection(this.ConnectString);
        await connection.ExecAsync(new MssqlCreateDatabase("king_bob2_db"));
        await connection.ExecAsync(new MssqlCreateLogin("king_bob2_login", "p@ssw0rd"));

        this.Writer.WriteLine("Creating cs 2 with database king_bob_db...");
        var dbcsBuilder = new SqlConnectionStringBuilder(this.ConnectString)
        {
            InitialCatalog = "king_bob2_db",
        };
        var dbcs = dbcsBuilder.ToString();
        var conn2 = new SqlConnection(dbcs);

        this.Writer.WriteLine("Creating user...");
        await conn2.ExecAsync(new MssqlCreateUser("king_bob2_user", "king_bob2_login"));

        this.Writer.WriteLine("Adding user to role...");
        await conn2.ExecAsync(new MssqlAddRoleMember("db_owner", "king_bob2_user")
        {
            UseStoredProc = true,
        });

        var hasUserResult = await conn2.ScalarAsResultAsync<int>(new MssqlSelectUserExists("king_bob2_user"));
        Assert.False(hasUserResult.IsError);
        Assert.True(hasUserResult.Unwrap() == 1);

        this.Writer.WriteLine("Creating cs 2 with user king_bob2_login...");
        dbcsBuilder.UserID = "king_bob2_login";
        dbcsBuilder.Password = "p@ssw0rd";
        dbcs = dbcsBuilder.ToString();
        var conn3 = new SqlConnection(dbcs);

        this.Writer.WriteLine("Testing connection as new user...");
        var selectResult = await conn3.ScalarAsResultAsync<int>("SELECT 1");
        Assert.False(selectResult.IsError);
        Assert.True(selectResult.Unwrap() == 1);

        conn3.Dispose();

        this.Writer.WriteLine("Removing user from role...");
        await conn2.ExecAsync(new MssqlDropRoleMember("db_owner", "king_bob2_user")
        {
            UseStoredProc = true,
        });

        this.Writer.WriteLine("Dropping user...");
        await conn2.ExecAsync(new MssqlDropUser("king_bob2_user"));

        hasUserResult = await conn2.ScalarAsResultAsync<int>(new MssqlSelectUserExists("king_bob2_user"));
        Assert.False(hasUserResult.IsError);
        conn2.Dispose();

        await connection.DropMssqlLoginAsync("king_bob2_login");
        await connection.ExecAsync(new MssqlDropConnections("king_bob2_db"));
        await connection.ExecAsync(new MssqlDropDatabase("king_bob2_db"));
    }
}