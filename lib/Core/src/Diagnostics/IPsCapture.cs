using System.Diagnostics;

namespace GnomeStack.Diagnostics;

public interface IPsCapture
{
    void OnStart(Process process);

    void WriteLine(string line);

    void OnExit();
}