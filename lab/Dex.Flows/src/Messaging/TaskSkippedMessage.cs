using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskSkippedMessage : TaskResultMessage
{
    public TaskSkippedMessage(ITaskResult result, string? reason)
        : base(result)
    {
        this.Reason = reason;
    }

    public string? Reason { get; }
}