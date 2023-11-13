namespace GnomeStack.Run.Tasks;

public interface IRemoteTaskCollection
{
    IList<RemoteTarget> Targets { get; set; }
}