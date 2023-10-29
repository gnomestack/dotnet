using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnomeStack.Text.DotEnv;

public class DotEnvSettings
{
    public bool Override { get; set; }

    public bool Debug { get; set; }

    public string? Path { get; set; }

    public bool Expand { get; set; }

    public Func<string, string?> GetVariable { get; set; } = Environment.GetEnvironmentVariable;

    public IReadOnlyList<string> Files { get; set; } = Array.Empty<string>();

    public Encoding Encoding { get; set; } = Encoding.UTF8;
}