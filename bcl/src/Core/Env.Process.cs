using System;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace GnomeStack;

public static partial class Env
{
    private static readonly Lazy<string[]> argv = new Lazy<string[]>(Environment.GetCommandLineArgs);

    private static readonly Lazy<Process> process = new Lazy<Process>(Process.GetCurrentProcess);

    private static readonly Lazy<int> getProcessId = new(() =>
    {
#if NETLEGACY
        return process.Value.Id;
#else
        return Environment.ProcessId;
#endif
    });

    public static bool Is64BitProcess => Environment.Is64BitProcess;

    [UnsupportedOSPlatform("browser")]
    public static string? ProcessPath => Argv.FirstOrDefault();

    [UnsupportedOSPlatform("browser")]
    public static int ProcessId
    {
        get
        {
#if !NETLEGACY
            if (OperatingSystem.IsBrowser())
                throw new PlatformNotSupportedException("Browser does not support ProcessId.");
#endif

            return getProcessId.Value;
        }
    }

    public static IReadOnlyList<string> Argv => argv.Value;

    public static string User => Environment.UserName;

    public static string UserDomain => Environment.UserDomainName;

    public static string Host => Environment.MachineName;

    public static string NewLine => Environment.NewLine;

    public static string CurrentDirectory
    {
        get => Environment.CurrentDirectory;
        set => Environment.CurrentDirectory = value;
    }

    public static bool UserInteractive => Environment.UserInteractive;

    public static string Home => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
}