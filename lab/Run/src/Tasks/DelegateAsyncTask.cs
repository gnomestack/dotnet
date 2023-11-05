using GnomeStack.Functional;

namespace GnomeStack.Run.Tasks;

public class DelegateAsyncTask : BaseTask
{
    private readonly RunTaskAsync run;

    public DelegateAsyncTask(string id, RunTaskAsync run)
        : base(id)
    {
        this.run = run;
    }

    public override async Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var r = await this.run(context, cancellationToken).NoCap();
            return r;
        }
        catch (Exception e)
        {
            return Result.Error<object, Error>(e);
        }
    }
}