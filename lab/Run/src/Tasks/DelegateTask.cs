using GnomeStack.Functional;

namespace GnomeStack.Run.Tasks;

public class DelegateTask : BaseTask
{
    private readonly RunTask run;

    public DelegateTask(string id, RunTask run)
        : base(id)
    {
        this.run = run;
    }

    public override Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var r = this.run(context);
            return Task.FromResult(r);
        }
        catch (Exception e)
        {
            return Task.FromResult(Result.Error<object, Error>(e));
        }
    }
}