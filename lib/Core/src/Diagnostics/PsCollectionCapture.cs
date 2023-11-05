using System.Diagnostics;

namespace GnomeStack.Diagnostics;

public sealed class PsCollectionCapture : IPsCapture
{
    private readonly ICollection<string> lines;

    public PsCollectionCapture(ICollection<string> collection)
    {
        this.lines = collection;
    }

    public void OnStart(Process process)
    {
        // do nothing
    }

    public void WriteLine(string line)
    {
        this.lines.Add(line);
    }

    public void OnExit()
    {
        // do nothing
    }
}