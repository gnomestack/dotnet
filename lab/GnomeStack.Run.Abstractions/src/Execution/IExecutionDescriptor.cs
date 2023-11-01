namespace GnomeStack.Run.Execution;

public interface IExecutionDescriptor
{
    string Id { get; }

    string Name { get; set; }

    string? Description { get; set; }

    IList<string> Deps { get; set; }

    EvaluateAsync<int>? Timeout { get; set; }

    EvaluateAsync<bool>? Force { get; set; }

    EvaluateAsync<bool>? Skip { get; set; }
}