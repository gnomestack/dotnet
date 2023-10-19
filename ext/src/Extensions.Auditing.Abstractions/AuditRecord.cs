using GnomeStack.Functional;

namespace GnomeStack.Extensions.Auditing;

public class AuditRecord
{
    public AuditRecord()
    {
    }

    public AuditRecord(AuditEvent auditEvent)
    {
        this.OperationId = auditEvent.OperationId;
        this.OperationName = auditEvent.OperationName;
        this.OperationParentId = auditEvent.OperationParentId;
        this.Name = auditEvent.Id.Name;
        this.EventId = auditEvent.Id.Id;
        this.SeverityLevel = auditEvent.SeverityLevel;
        this.UserId = auditEvent.UserId;
        this.UserAccountId = auditEvent.UserAccountId;
        this.ClientIp = auditEvent.ClientIp;
        this.Timestamp = auditEvent.Timestamp;
        this.Duration = auditEvent.Duration;
        this.Error = auditEvent.Error;
        this.HostName = auditEvent.HostName;
        this.HostIp = auditEvent.HostIp;
        this.ServiceId = auditEvent.ServiceId;
    }

    public object? Properties { get; set; }

    public string? OperationId { get; set; }

    public string? OperationParentId { get; set; }

    public string? OperationName { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Name { get; set; }

    public int EventId { get; set; }

    public string? UserId { get; set; }

    public string? UserAccountId { get; set; }

    public string? HostName { get; set; }

    public string? HostIp { get; set; }

    public string? ClientIp { get; set; }

    public string? ServiceId { get; set; }

    public byte SeverityLevel { get; set; }

    public Error? Error { get; set; }
}