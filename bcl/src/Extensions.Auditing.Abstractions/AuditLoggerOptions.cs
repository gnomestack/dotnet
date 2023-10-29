using GnomeStack.Extensions.Application;

namespace GnomeStack.Extensions.Auditing;

public class AuditLoggerOptions
{
    public AuditLoggerOptions(IApplicationInfo app)
    {
        this.App = app;
    }

    public IApplicationInfo App { get; set; }

    public IAuditLoggerChannel Channel { get; set; } = NullAuditLoggerChannel.Instance;

    public List<IAuditEventEnricher> Enrichers { get; } = new();
}