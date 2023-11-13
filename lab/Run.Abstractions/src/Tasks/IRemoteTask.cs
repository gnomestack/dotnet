namespace GnomeStack.Run.Tasks;

public interface IRemoteTask : ITask
{
    IList<RemoteTarget> Targets { get; set; }
}