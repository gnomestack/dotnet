using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskMessage : Message
{
    public TaskMessage(ITask task)
    {
        this.Task = task;
    }

    public ITask Task { get; }
}