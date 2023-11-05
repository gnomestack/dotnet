using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskCancellationMessage : TaskResultMessage
{
    public TaskCancellationMessage(ITaskResult result, string? reason)
        : base(result)
    {
        this.Reason = reason;
    }

    public string? Reason { get; }
}