namespace GnomeStack.Identity.Store;

public class IdentityEvent
{
    public long Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string? TraceId { get; set; }

    public string? CorrelationId { get; set; }

    public string? ParentId { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? Uid { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset EndedAt { get; set; }

    public string? Data { get; set; }
}