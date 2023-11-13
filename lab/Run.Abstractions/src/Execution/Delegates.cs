using GnomeStack.Functional;

namespace GnomeStack.Run.Execution;

public delegate Result<T, Error> Evaluate<T>(IExecutionContext context)
    where T : notnull;

public delegate Task<Result<T, Error>> EvaluateAsync<T>(IExecutionContext context, CancellationToken? cancellationToken)
    where T : notnull;