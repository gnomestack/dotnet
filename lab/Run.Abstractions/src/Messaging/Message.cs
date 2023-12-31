using System.Diagnostics;

namespace GnomeStack.Run.Messaging;

public class Message
{
    public Message()
    {
        this.OperationId = Activity.Current?.Id;
        this.Timestamp = DateTimeOffset.UtcNow;
    }

    public string? OperationId { get; set; }

    public DateTimeOffset Timestamp { get; set; }
}