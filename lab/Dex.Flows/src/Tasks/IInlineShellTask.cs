namespace GnomeStack.Dex.Flows.Tasks;

public interface IInlineShellTask : IShellTask
{
    string Script { get; set; }
}