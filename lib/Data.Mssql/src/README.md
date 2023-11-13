# GnomeStack.Data.Mssql

Provides Management Sql statements for Sql Server like creating/dropping databases,
users, logins, or getting database sizes, info on resource usage, etc.  

The library is built on top of GnomeStack.Data which uses
dapper and provides extension methods for `IDbConnection` and Dapper that enables
classes or structs that implement `ISqlStatementBuilder` to be executed.

## Usage

```csharp
// this sample comes from tests. Please use a better password.
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

// this extension method gets all the process ids for the given login
// and kills them before dropping the login.
await connection.DropMssqlLoginAsync("king_bob_login");
await connection.ExecAsync(new MssqlDropConnections("king_bob_db"));
await connection.ExecAsync(new MssqlDropDatabase("king_bob_db"));
```










MIT License
