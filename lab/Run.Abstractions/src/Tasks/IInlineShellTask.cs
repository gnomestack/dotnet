namespace GnomeStack.Run.Tasks;

public interface IInlineShellTask : IShellTask
{
    string Script { get; set; }
}