using GnomeStack.Functional;

namespace GnomeStack.Extensions.Auditing;

public class AuditEvent
{
    public AuditEventId Id { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public TimeSpan Duration { get; set; }

    public string? OperationId { get; set; }

    public string? OperationParentId { get; set; }

    public string? OperationName { get; set; }

    public string? UserId { get; set; }

    public string? UserAccountId { get; set; }

    public string? ClientIp { get; set; }

    public string? ServiceId { get; set; }

    public string? HostName { get; set; }

    public string? HostIp { get; set; }

    public byte SeverityLevel { get; set; } = 0xff;

    public Error? Error { get; set; }
}