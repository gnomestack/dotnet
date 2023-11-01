using System.Collections;

namespace GnomeStack.Run.Tasks;

public interface IShellTask : ITask
{
    string? Shell { get; set; }

    string? WorkingDirectory { get; set; }

    IDictionary<string, string?> Env { get; set; }
}