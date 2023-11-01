using GnomeStack.Functional;

namespace GnomeStack.Run.Execution;

public interface IExecutionResult
{
    ExecutionStatus Status { get; }

    Error? Error { get; }

    DateTimeOffset StartedAt { get; }

    DateTimeOffset EndedAt { get; }
}