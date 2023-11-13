using GnomeStack.Functional;

namespace GnomeStack.Data;

public abstract class SqlStatementBuilder : ISqlStatementBuilder
{
    public abstract Result<(string, object?), Exception> Build();

    public override string ToString()
    {
        var r = this.Build();
        return r.IsError ? r.UnwrapError().Message : r.Unwrap().Item1;
    }
}