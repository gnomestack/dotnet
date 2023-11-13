using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Messaging;

public class TasksSummaryMessage : Message
{
    public TasksSummaryMessage(IReadOnlyList<ITaskResult> taskResults)
    {
        this.TaskResultsResults = taskResults;
    }

    public IReadOnlyList<ITaskResult> TaskResultsResults { get; }
}