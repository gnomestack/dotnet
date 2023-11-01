using GnomeStack.Dex.Flows.Tasks;
using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Jobs;

public delegate Task<Result<object, Error>> RunJobAsync(IJobContext state, CancellationToken? cancellationToken);

public delegate Result<object, Error> RunJob(IJobContext state);