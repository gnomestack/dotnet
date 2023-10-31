using GnomeStack.Functional;

namespace GnomeStack.Data;

public abstract class SqlStatementBuilder
{
    public abstract Result<string, Exception> Build();

    public override string ToString()
    {
        var r = this.Build();
        return r.IsError ? r.UnwrapError().Message : r.Unwrap();
    }
}