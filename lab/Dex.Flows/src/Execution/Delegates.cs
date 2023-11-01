using GnomeStack.Dex.Flows.Tasks;
using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows;

public delegate Result<T, Error> Evaluate<T>(ITaskContext state)
    where T : notnull;

public delegate Task<Result<T, Error>> EvaluateAsync<T>(ITaskContext state, CancellationToken? cancellationToken)
    where T : notnull;