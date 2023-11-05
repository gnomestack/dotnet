namespace GnomeStack.Run.Tasks;

public interface IShellFileTask : IShellTask
{
    string File { get; set; }
}