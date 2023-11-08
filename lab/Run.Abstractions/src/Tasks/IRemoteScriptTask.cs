namespace GnomeStack.Run.Tasks;

public interface IRemoteScriptTask : IRemoteTask
{
    string Script { get; set; }
}