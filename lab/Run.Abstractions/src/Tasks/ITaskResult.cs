using GnomeStack.Run.Execution;

namespace GnomeStack.Run.Tasks;

public interface ITaskResult : IExecutionResult
{
    ITask Task { get; }
}