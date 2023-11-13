# GnomeStack.Data

Provides extension methods for DataRecord and DataReader and includes Dapper
and adds additional extension methods for Dapper.

The extensions methods for DataRecord and DataReader are useful for

- Nullable types
- Getting values by name rather than by ordinal.

The extension methods add support Dapper for: 

- Retrying database operations using polly which is useful for handling
  transient errors, especially in Azure.
- Result<T, Exception>` objects rather than throwing exceptions. This is useful
  for handling errors in a functional way and forcing dealing with Exceptions
  rather than bubbling them up.
- Extensions that enable ISqlStatementBuilder to be used with Dapper
  which enables a kind of command object for creating SQL statements
  and optionally parameters.

Not all cases for for Dapper have been implemented for extension methods. They
will be implemented in time.  The Execute, ExecuteScalar, ExecuteReader, and `Query<T>` 
have extension methods.

## Usage Examples

### Using Result<T, Exception> with Dapper

```csharp
var r = await connection.ScalarAsResultAsync<int>("SELECT 1");
return r.IsOk ? r.Value : -1;
```


### Using SqlStatementBuilder with Dapper

```csharp
public class MssqlSelectDbExists : SqlStatementBuilder
{
    public MssqlSelectDbExists()
    {
    }

    public MssqlSelectDbExists(string databaseName)
    {
        this.DatabaseName = databaseName;
    }

    public string DatabaseName { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.Identifier(this.DatabaseName.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.DatabaseName}");

        var query = $"""
                     ---noinspection SqlNoDataSourceInspectionForFile
                     SELECT IIF(COUNT(*) > 0, 1, 0)
                        FROM sys.databases
                        WHERE name = '{this.DatabaseName}'
                     """;

        return (query, null);
    }
}

// elsewhere

var r = await connection.ScalarAsync<int>(new MssqlSelectDbExists("master"));
if (r == 1)
{
    Console.WriteLine("Database exists");
}
else
{
    Console.WriteLine("Database does not exist");
}
```

### Using RetryExecuteAsync with Dapper

```csharp
return await connection.RetryQueryAsync<UserDigest>(
    "SELECT name, date FROM users");
```

MIT License
