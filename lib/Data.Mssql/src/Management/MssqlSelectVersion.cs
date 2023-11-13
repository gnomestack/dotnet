using GnomeStack.Functional;

namespace GnomeStack.Data.Mssql.Management;

public class MssqlSelectVersion : SqlStatementBuilder
{
    public override Result<(string, object?), Exception> Build()
    {
        return ("SELECT @@VERSION;", null);
    }
}