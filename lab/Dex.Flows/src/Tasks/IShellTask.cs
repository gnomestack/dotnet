using System.Collections;

namespace GnomeStack.Dex.Flows.Tasks;

public interface IShellTask : ITask
{
    string? Shell { get; set; }

    string? WorkingDirectory { get; set; }

    IDictionary<string, string?> Env { get; set; }
}