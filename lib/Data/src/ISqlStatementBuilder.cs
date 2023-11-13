using GnomeStack.Functional;

namespace GnomeStack.Data;

public interface ISqlStatementBuilder
{
    Result<(string, object?), Exception> Build();
}