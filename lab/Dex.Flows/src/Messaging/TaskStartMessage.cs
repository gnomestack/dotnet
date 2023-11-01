using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskStartMessage : TaskMessage
{
    public TaskStartMessage(ITask task)
        : base(task)
    {
    }
}