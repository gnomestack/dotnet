namespace GnomeStack.Dex.Flows.Messaging;

public class UnhandledExceptionMessage : Message
{
    public UnhandledExceptionMessage(Exception exception)
    {
        this.Exception = exception;
    }

    public Exception Exception { get; }
}