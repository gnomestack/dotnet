using GnomeStack.Extensions.Application;

namespace GnomeStack.Extensions.Auditing;

public class AuditLoggerOptions
{
    public AuditLoggerOptions(IApplicationEnvironment app)
    {
        this.App = app;
    }

    public IApplicationEnvironment App { get; set; }

    public IAuditLoggerChannel Channel { get; set; } = NullAuditLoggerChannel.Instance;

    public List<IAuditEventEnricher> Enrichers { get; } = new();
}