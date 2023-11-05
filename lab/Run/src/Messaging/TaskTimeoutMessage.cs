using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskTimeoutMessage : TaskResultMessage
{
    public TaskTimeoutMessage(ITaskResult result, int timeout)
        : base(result)
    {
        this.Timeout = timeout;
    }

    public int Timeout { get; }
}