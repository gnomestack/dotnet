using System.Diagnostics;

using GnomeStack.Functional;
using GnomeStack.Identity.Actions.Events;
using GnomeStack.Identity.Store;
using GnomeStack.Text.Json;

using Microsoft.Extensions.Logging;

using OpenTelemetry.Trace;

namespace GnomeStack.Identity.Actions;

public class AuditedAction
{
    protected AuditedAction(string name, IdentityDbContext db, ILogger logger, IIdentityAuditEventLog auditLog, IJsonSerializer jsonSerializer)
    {
        this.Name = name;
        this.Db = db ?? throw new ArgumentNullException(nameof(db));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.AuditLog = auditLog ?? throw new ArgumentNullException(nameof(auditLog));
    }

    protected string Name { get; set; }

    protected IdentityDbContext Db { get; }

    protected ILogger Logger { get; }

    protected IIdentityAuditEventLog AuditLog { get; }

    protected IJsonSerializer JsonSerializer { get; }

    protected async Task<Error?> SaveChangesAsync(object data, string failMessage = "", CancellationToken cancellationToken = default)
    {
        try
        {
            await this.Db.SaveChangesAsync(cancellationToken)
                .NoCap();

            var activity = Activity.Current;
            var st = activity?.StartTimeUtc ?? DateTimeOffset.UtcNow;
            await this.AuditLog.TrackAsync(
                new IdentityEventRecord()
                {
                    Name = this.Name,
                    StartedAt = st,
                    Data = data,
                },
                cancellationToken).NoCap();

            System.Diagnostics.Activity.Current.SetStatus(Status.Ok);
        }
        catch (Exception e)
        {
            Activity.Current?.RecordException(e);
            Activity.Current?.SetStatus(ActivityStatusCode.Error, e.Message);
            this.Logger.LogError(e, failMessage);
            return new Error(failMessage);
        }

        return null;
    }
}