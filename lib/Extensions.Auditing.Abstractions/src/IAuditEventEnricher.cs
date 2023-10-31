namespace GnomeStack.Extensions.Auditing;

public interface IAuditEventEnricher
{
    void Activate();

    void Enrich(AuditRecord record);
}