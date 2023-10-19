using System.Diagnostics;

using GnomeStack.Extensions.Logging;
using GnomeStack.Identity.Store;
using GnomeStack.Standard;
using GnomeStack.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GnomeStack.Identity.Actions.Events;

public class IdentityAuditEventLog
{
    private readonly DbContext db;
    private readonly ILogger<IdentityAuditEventLog> logger;
    private readonly IJsonSerializer jsonSerializer;

    public IdentityAuditEventLog(IdentityDbContext context, ILogger<IdentityAuditEventLog> logger, IJsonSerializer jsonSerializer)
    {
        this.db = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
    }

    public async Task TrackAsync(IdentityEventRecord record)
    {
        var json = record.Data is null ? "{}" : await this.jsonSerializer.SerializeAsync(record.Data);
        var @event = new IdentityEvent
        {
            Name = record.Name,
            StartedAt = record.StartedAt,
            EndedAt = record.EndedAt,
            TraceId = record.TraceId ?? Activity.Current?.Id,
            ParentId = record.ParentId ?? Activity.Current?.ParentId,
            CorrelationId = record.CorrelationId ?? Activity.Current?.GetBaggageItem("correlation_id"),
            Data = json,
        };

        this.db.Add(@event);
        try
        {
            await this.db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            this.logger.Critical(ex, "Failed to save identity audit event {@event}", @event);
        }
    }
}