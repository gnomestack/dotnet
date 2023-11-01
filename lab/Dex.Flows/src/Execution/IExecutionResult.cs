using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows;

public interface IExecutionResult
{
    ExecutionStatus Status { get; }

    Error? Error { get; }

    DateTimeOffset StartedAt { get; }

    DateTimeOffset EndedAt { get; }
}