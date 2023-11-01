using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskCancellationMessage : TaskResultMessage
{
    public TaskCancellationMessage(ITaskResult result, string? reason)
        : base(result)
    {
        this.Reason = reason;
    }

    public string? Reason { get; }
}