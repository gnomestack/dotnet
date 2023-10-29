namespace GnomeStack.Extensions.Auditing;

public class AuditLoggerChannelOptions
{
    public TimeSpan FlushSpan { get; set; } = TimeSpan.FromSeconds(5);
}