using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskStartMessage : TaskMessage
{
    public TaskStartMessage(ITask task)
        : base(task)
    {
    }
}