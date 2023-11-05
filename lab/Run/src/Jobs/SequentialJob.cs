using GnomeStack.Functional;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Jobs;

public class SequentialJob : BaseJob
{
    private readonly TaskCollection tasks = new TaskCollection();

    public SequentialJob(string id)
        : base(id)
    {
    }

    public override ITaskCollection Tasks => this.tasks;

    public override Task<Result<object, Error>> RunAsync(IJobContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}