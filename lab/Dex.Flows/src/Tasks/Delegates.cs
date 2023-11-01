using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Tasks;

public delegate Task<Result<object, Error>> RunTaskAsync(ITaskContext state, CancellationToken? cancellationToken);

public delegate Result<object, Error> RunTask(ITaskContext state);