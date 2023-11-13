using System.Diagnostics;

namespace GnomeStack.Diagnostics;

public class PsActionCapture : IPsCapture
{
    private readonly Action<string?> action;

    public PsActionCapture(Action<string?> action)
    {
        this.action = action;
    }

    public void OnStart(Process process)
    {
        // do nothing
    }

    public void WriteLine(string line)
    {
        this.action.Invoke(line);
    }

    public void OnExit()
    {
        this.action.Invoke(null);
    }
}