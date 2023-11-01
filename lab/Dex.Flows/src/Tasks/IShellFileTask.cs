namespace GnomeStack.Dex.Flows.Tasks;

public interface IShellFileTask : IShellTask
{
    string File { get; set; }
}