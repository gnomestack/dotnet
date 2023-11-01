using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TaskResultMessage : TaskMessage
{
    public TaskResultMessage(ITaskResult result)
        : base(result.Task)
    {
        this.TaskResult = result;
    }

    public ITaskResult TaskResult { get; }
}