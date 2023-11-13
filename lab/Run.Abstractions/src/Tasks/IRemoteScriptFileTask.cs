namespace GnomeStack.Run.Tasks;

public interface IRemoteScriptFileTask : IRemoteTask
{
    string File { get; set; }
}