namespace GnomeStack.Extensions.Auditing;

public class NullAuditLoggerChannel : IAuditLoggerChannel
{
    public static NullAuditLoggerChannel Instance { get; } = new();

    public void Send(AuditRecord auditRecord)
    {
        // Do nothing
    }

    public void Flush()
    {
        // Do nothing
    }

    public Task FlushAsync()
    {
        return Task.CompletedTask;
    }
}