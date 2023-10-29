namespace GnomeStack.Extensions.Auditing;

public interface IAuditLoggerChannel
{
    void Send(AuditRecord auditRecord);

    void Flush();

    Task FlushAsync();
}