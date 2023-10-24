using System.Diagnostics;

namespace GnomeStack.Extensions.Auditing;

public class AuditLogger : IAuditLogger
{
    private readonly AuditLoggerOptions options;

    private readonly IAuditLoggerChannel channel;

    public AuditLogger(AuditLoggerOptions options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.channel = this.options.Channel;
        this.Init();
    }

    public virtual void Log(AuditEvent auditEvent, object? properties = null)
    {
        var record = new AuditRecord(auditEvent);
        if (properties is not null)
            record.Properties = properties;

        foreach (var enricher in this.options.Enrichers)
            enricher.Enrich(record);

        this.PublishAuditEvent(record);
    }

    public void Flush()
    {
        this.channel.Flush();
    }

    public Task FlushAsync()
    {
        return this.channel.FlushAsync();
    }

    protected void PublishAuditEvent(AuditRecord record)
    {
        this.channel.Send(record);
    }

    protected void Init()
    {
        foreach (var enricher in this.options.Enrichers)
            enricher.Activate();
    }
}