using System.Runtime.Versioning;
using System.Security;
using System.Text;

namespace GnomeStack.Diagnostics;

public class PsStartInfo
{
    private readonly List<IDisposable> disposables = new();

    private List<IPsCapture>? stdOutCaptures;

    private List<IPsCapture>? stdErrorCaptures;

    public PsArgs Args { get; set; } = new PsArgs();

    public string? Cwd { get; set; }

    public IDictionary<string, string?>? Env { get; set; }

    public Stdio StdOut { get; set; }

    public Stdio StdErr { get; set; }

    public Stdio StdIn { get; set; }

    public string? User { get; set; }

    public string? Verb { get; set; }

    [SupportedOSPlatform("windows")]
    [CLSCompliant(false)]
    public SecureString? Password { get; set; }

    [SupportedOSPlatform("windows")]
    public string? PasswordInClearText { get; set; }

    [SupportedOSPlatform("windows")]
    public string? Domain { get; set; }

    public bool LoadUserProfile { get; set; } = false;

    public bool CreateNoWindow { get; set; } = false;

    public bool UseShellExecute { get; set; } = false;

    protected internal IReadOnlyList<IPsCapture> StdOutCaptures
    {
        get
        {
            if (this.stdOutCaptures is null)
                return Array.Empty<IPsCapture>();

            return this.stdOutCaptures;
        }
    }

    protected internal IReadOnlyList<IPsCapture> StdErrorCaptures
    {
        get
        {
            if (this.stdErrorCaptures is null)
                return Array.Empty<IPsCapture>();

            return this.stdErrorCaptures;
        }
    }

    protected internal IReadOnlyList<IDisposable> Disposables
        => this.disposables;

    public PsStartInfo WithArgs(PsArgs args)
    {
        this.Args = args;
        return this;
    }

    public PsStartInfo WithCwd(string cwd)
    {
        this.Cwd = cwd;
        return this;
    }

    public PsStartInfo WithEnv(IDictionary<string, string?> env)
    {
        this.Env = env;
        return this;
    }

    public PsStartInfo SetEnv(string name, string value)
    {
        this.Env ??= new Dictionary<string, string?>();
        this.Env[name] = value;
        return this;
    }

    public PsStartInfo SetEnv(IEnumerable<KeyValuePair<string, string?>> values)
    {
        this.Env ??= new Dictionary<string, string?>();
        foreach (var kvp in values)
        {
            this.Env[kvp.Key] = kvp.Value;
        }

        return this;
    }

    public PsStartInfo WithDisposable(IDisposable disposable)
    {
        this.disposables.Add(disposable);
        return this;
    }

    public PsStartInfo WithDisposable(Action action)
    {
        this.disposables.Add(new DisposeAction(action));
        return this;
    }

    public PsStartInfo WithStdOut(Stdio stdio)
    {
        this.StdOut = stdio;
        return this;
    }

    public PsStartInfo WithStdErr(Stdio stdio)
    {
        this.StdErr = stdio;
        return this;
    }

    public PsStartInfo WithStdIn(Stdio stdio)
    {
        this.StdIn = stdio;
        return this;
    }

    public PsStartInfo WithStdio(Stdio stdio)
    {
        this.StdOut = stdio;
        this.StdErr = stdio;
        this.StdIn = stdio;
        return this;
    }

    public PsStartInfo WithVerb(string verb)
    {
        this.Verb = verb;
        return this;
    }

    public PsStartInfo AsWindowsAdmin()
    {
        this.Verb = "runas";
        return this;
    }

    public PsStartInfo AsSudo()
    {
        this.Verb = "sudo";
        return this;
    }

    [SupportedOSPlatform("windows")]
    public PsStartInfo WithUser(string user)
    {
        this.User = user;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public PsStartInfo WithPassword(string password)
    {
        this.PasswordInClearText = password;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public PsStartInfo WithDomain(string domain)
    {
        this.Domain = domain;
        return this;
    }

    public PsStartInfo Capture(ICollection<string> collection)
    {
        return this.Capture(new PsCollectionCapture(collection));
    }

    public PsStartInfo Capture(TextWriter writer, bool dispose = false)
    {
        return this.Capture(new PsTextWriterCapture(writer, dispose));
    }

    public PsStartInfo Capture(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        return this.Capture(new PsTextWriterCapture(stream, encoding, bufferSize, leaveOpen));
    }

    public PsStartInfo Capture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        return this.Capture(new PsTextWriterCapture(file, encoding, bufferSize));
    }

    public PsStartInfo Capture(IPsCapture capture)
    {
        this.StdOut = Stdio.Piped;
        this.stdOutCaptures ??= new List<IPsCapture>();
        this.stdOutCaptures.Add(capture);
        return this;
    }

    public PsStartInfo CaptureError(TextWriter writer, bool dispose = false)
    {
        return this.CaptureError(new PsTextWriterCapture(writer, dispose));
    }

    public PsStartInfo CaptureError(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        return this.CaptureError(new PsTextWriterCapture(stream, encoding, bufferSize, leaveOpen));
    }

    public PsStartInfo CaptureError(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        return this.CaptureError(new PsTextWriterCapture(file, encoding, bufferSize));
    }

    public PsStartInfo CaptureError(ICollection<string> collection)
    {
        return this.CaptureError(new PsCollectionCapture(collection));
    }

    public PsStartInfo CaptureError(IPsCapture capture)
    {
        this.StdErr = Stdio.Piped;
        this.stdErrorCaptures ??= new List<IPsCapture>();
        this.stdErrorCaptures.Add(capture);
        return this;
    }
}