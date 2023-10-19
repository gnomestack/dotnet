namespace GnomeStack.Identity.Actions.Events;

public class IdentityEventRecord
{
    public string Name { get; set; } = string.Empty;

    public string? TraceId { get; set; }

    public string? CorrelationId { get; set; }

    public string? ParentId { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? Uid { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset EndedAt { get; set; }

    public object? Data { get; set; }
}