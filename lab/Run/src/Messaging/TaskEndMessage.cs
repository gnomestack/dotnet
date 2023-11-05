using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskEndMessage : TaskResultMessage
{
    public TaskEndMessage(ITaskResult result)
        : base(result)
    {
    }
}