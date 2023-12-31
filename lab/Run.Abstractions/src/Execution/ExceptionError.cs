using GnomeStack.Functional;

namespace GnomeStack.Run.Execution;

public class ExceptionError : Error
{
    public ExceptionError(Exception exception)
        : base(
            exception.Message,
            exception.InnerException == null ? null : new ExceptionError(exception))
    {
        this.Exception = exception;
    }

    public Exception Exception { get; }
}