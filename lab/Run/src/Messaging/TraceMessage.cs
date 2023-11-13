namespace GnomeStack.Run.Messaging;

public class TraceMessage : Message
{
    public TraceMessage(FormattableString message, IReadOnlyDictionary<string, object?>? data = null)
    {
        this.Message = message;
        this.Data = data ?? new Dictionary<string, object?>();
    }

    public FormattableString Message { get; }

    public IReadOnlyDictionary<string, object?> Data { get; }
}