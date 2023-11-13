using GnomeStack.Functional;

namespace GnomeStack.Run.Tasks;

public class ActionTask : BaseTask
{
    private readonly RunTaskAction action;

    public ActionTask(string id, RunTaskAction action)
        : base(id)
    {
        this.action = action;
    }

    public override Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            this.action.Invoke(context);
            return Task.FromResult<Result<object, Error>>(Nil.Value);
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result.Error<object>(Error.Convert(ex)));
        }
    }
}