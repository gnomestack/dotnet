using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class MssqlSelectVersion : SqlStatementBuilder
{
    public override Result<(string, object?), Exception> Build()
    {
        return ("SELECT @@VERSION;", null);
    }
}