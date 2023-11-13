namespace GnomeStack.Run.Tasks;

public interface IRemoteTaskContext : ITaskContext
{
    public IReadOnlyCollection<Target> Targets { get; set; }
}