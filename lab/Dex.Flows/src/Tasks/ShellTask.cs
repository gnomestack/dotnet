using System.Collections;

using GnomeStack.Diagnostics;
using GnomeStack.Functional;
using GnomeStack.Standard;

using RunShell = GnomeStack.Standard.Shell;

namespace GnomeStack.Dex.Flows.Tasks;

public class ShellTask : DexTask, IInlineShellTask
{
    public ShellTask(string id, string script)
        : base(id)
    {
        this.Script = script;
    }

    public string Script { get; set; }

    public string? WorkingDirectory { get; set; }

    public string? Shell { get; set; }

    public IDictionary<string, string?> Env { get; set; } = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

    public override async Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (this.Shell.IsNullOrWhiteSpace())
            {
                this.Shell = GnomeStack.Standard.Env.IsWindows ? "pwsh" : "bash";
            }

            var si = new PsStartInfo();
            if (!this.WorkingDirectory.IsNullOrWhiteSpace())
                si.WithCwd(this.WorkingDirectory);

            si.WithEnv(context.Env);

            var output = await RunShell.RunAsResultAsync(
                    this.Shell,
                    this.Script,
                    si,
                    cancellationToken)
                .NoCap();

            if (output.IsError)
                return output.UnwrapError();

            return output.Unwrap();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}