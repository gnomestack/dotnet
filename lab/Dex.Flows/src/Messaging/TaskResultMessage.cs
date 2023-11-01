using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TaskResultMessage : TaskMessage
{
    public TaskResultMessage(ITaskResult result)
        : base(result.Task)
    {
        this.TaskResult = result;
    }

    public ITaskResult TaskResult { get; }
}