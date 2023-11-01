using GnomeStack.Dex.Flows.Tasks;

namespace GnomeStack.Dex.Flows.Messaging;

public class TasksSummaryMessage : Message
{
    public TasksSummaryMessage(IReadOnlyList<ITaskResult> taskResults)
    {
        this.TaskResultsResults = taskResults;
    }

    public IReadOnlyList<ITaskResult> TaskResultsResults { get; }
}