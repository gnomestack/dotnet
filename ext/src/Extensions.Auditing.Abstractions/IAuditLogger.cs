namespace GnomeStack.Extensions.Auditing;

public interface IAuditLogger
{
    void Log(AuditEvent auditEvent, object? properties = null);

    void Flush();

    Task FlushAsync();
}