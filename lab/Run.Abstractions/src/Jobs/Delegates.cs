using GnomeStack.Functional;

namespace GnomeStack.Run.Jobs;

public delegate Task<Result<object, Error>> RunJobAsync(IJobContext context, CancellationToken? cancellationToken);

public delegate Result<object, Error> RunJob(IJobContext context);

public delegate void RunJobAction(IJobContext context);