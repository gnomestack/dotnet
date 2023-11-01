namespace GnomeStack.Dex.Flows;

public interface IExecutionState
{
    string Id { get; }

    string Name { get; }

    string? Description { get; }

    IReadOnlyList<string> Deps { get; }

    int? Timeout { get; }

    bool Force { get; }

    bool Skip { get; }

    ExecutionStatus Status { get; }

    IDictionary<string, object?> Outputs { get; }
}