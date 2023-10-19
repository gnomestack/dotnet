namespace GnomeStack.Identity.Actions.Events;

public interface IIdentityAuditEventLog
{
    Task TrackAsync(IdentityEventRecord record, CancellationToken cancellationToken = default);
}