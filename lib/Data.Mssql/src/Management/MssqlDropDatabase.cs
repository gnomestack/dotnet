using GnomeStack.Functional;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlDropDatabase : SqlStatementBuilder
{
    public MssqlDropDatabase()
    {
    }

    public MssqlDropDatabase(string name)
    {
        this.Name = name;
    }

    public string Name { get; set; } = string.Empty;

    public override Result<(string, object?), Exception> Build()
    {
        if (!MssqlValidate.Identifier(this.Name.AsSpan()))
            return new InvalidDbIdentifierException($"Invalid database name {this.Name}");

        var sql = $"DROP DATABASE IF EXISTS {MssqlQuote.Identifier(this.Name.AsSpan())};";
        return (sql, null);
    }
}