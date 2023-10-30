using GnomeStack.Functional;

namespace GnomeStack.Data.SqlServer.Management;

public class SelectVersion : SqlStatementBuilder
{
    public override Result<string, Exception> Build()
    {
        return "SELECT @@VERSION;";
    }
}