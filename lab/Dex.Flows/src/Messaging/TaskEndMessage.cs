using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskEndMessage : TaskResultMessage
{
    public TaskEndMessage(ITaskResult result)
        : base(result)
    {
    }
}