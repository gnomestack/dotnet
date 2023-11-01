using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskTimeoutMessage : TaskResultMessage
{
    public TaskTimeoutMessage(ITaskResult result, int timeout)
        : base(result)
    {
        this.Timeout = timeout;
    }

    public int Timeout { get; }
}