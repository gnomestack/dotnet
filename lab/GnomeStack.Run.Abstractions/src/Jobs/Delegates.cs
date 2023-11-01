using GnomeStack.Functional;

namespace GnomeStack.Run.Jobs;

public delegate Task<Result<object, Error>> RunJobAsync(IJobContext state, CancellationToken? cancellationToken);

public delegate Result<object, Error> RunJob(IJobContext state);