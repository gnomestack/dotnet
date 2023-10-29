namespace GnomeStack.Extensions.Auditing;

public class NullAuditLogger : IAuditLogger
{
    public void Log(AuditEvent auditEvent, object? properties = null)
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