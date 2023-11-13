using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskSkippedMessage : TaskResultMessage
{
    public TaskSkippedMessage(ITaskResult result, string? reason)
        : base(result)
    {
        this.Reason = reason;
    }

    public string? Reason { get; }
}