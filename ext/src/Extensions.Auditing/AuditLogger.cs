using System.Diagnostics;
using System.Net;

using GnomeStack.Extensions.DiagnosticSource;
using GnomeStack.Extensions.Logging;

using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Auditing;

public class AuditLogger : IAuditLogger
{
    private readonly AuditLoggerOptions options;

    private readonly IAuditLoggerChannel channel;

    private readonly ILogger logger;

    public AuditLogger(AuditLoggerOptions options, ILogger<AuditLogger> logger)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.channel = this.options.Channel;
        this.Init();
    }

    public virtual void Log(AuditEvent auditEvent, object? properties = null)
    {
        using var activity = this.CreateActivity();

        var record = new AuditRecord(auditEvent);
        if (properties is not null)
            record.Properties = properties;

        foreach (var enricher in this.options.Enrichers)
            enricher.Enrich(record);

        try
        {
            this.PublishAuditEvent(record);
        }
        catch (Exception ex)
        {
            this.LogInternalError(ex);
        }
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

    protected virtual void Init()
    {
        foreach (var enricher in this.options.Enrichers)
        {
            using var activity = AuditingActivity.Source.StartActivity("ActivateAuditEnricher", ActivityKind.Internal);
            activity?.SetTag("audit.enricher", enricher.GetType().FullName);
            try
            {
                enricher.Activate();
                activity?.SetStatus(ActivityStatusCode.Ok, "Audit enricher activated.");
                activity?.Stop();
            }
            catch (Exception ex)
            {
                activity?.RecordException(ex);
                activity?.Stop();
                activity?.SetStatus(ActivityStatusCode.Error, "Failed to activate audit enricher.");
                this.logger.Critical(ex, "Failed to activate audit {enricher}.", enricher.GetType().FullName);
            }
        }
    }

    private Activity? CreateActivity()
    {
        var activity = AuditingActivity.Source.StartActivity("LogAuditEvent", ActivityKind.Client);
        activity?.SetTag("audit.logger", this.GetType().FullName);
        return activity;
    }

    private void LogInternalError(Exception ex)
    {
        this.logger.Critical(ex, "Failed to publish audit event.");
        var activity = Activity.Current;
        if (activity is null)
            return;

        activity?.RecordException(ex);
    }
}