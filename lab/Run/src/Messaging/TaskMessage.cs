using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskMessage : Message
{
    public TaskMessage(ITask task)
    {
        this.Task = task;
    }

    public ITask Task { get; }
}