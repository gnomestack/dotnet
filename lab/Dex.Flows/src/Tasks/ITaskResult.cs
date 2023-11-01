namespace GnomeStack.Dex.Flows.Tasks;

public interface ITaskResult : IExecutionResult
{
    ITask Task { get; }
}