namespace GnomeStack.Diagnostics;

internal class PythonExecutor : ShellExecutor
{
    public PythonExecutor()
    {
        PsPathRegistry.Default.RegisterOrUpdate("python", (entry) =>
        {
            entry.Windows.AddRange(new HashSet<string>()
            {
                @"%SystemDrive%\Windows\py.exe",
                @"%USERPROFILE%\AppData\Local\Programs\Python\Launcher\py.exe",
                @"%ProgramFiles%\Python310\python.exe",
                @"%ProgramFiles%\Python39\python.exe",
                @"%ProgramFiles%\Python38\python.exe",
                @"%SystemDrive%\Python310\python.exe",
                @"%SystemDrive%\Python39\python.exe",
                @"%SystemDrive%\Python38\python.exe",
                @"%USERPROFILE%\AppData\Local\Programs\Python\Python310\python.exe",
                @"%USERPROFILE%\AppData\Local\Programs\Python\Python39\python.exe",
                @"%USERPROFILE%\AppData\Local\Programs\Python\Python38\python.exe",
            });

            entry.Linux.AddRange(new HashSet<string>()
            {
                "/usr/bin/python3",
            });
        });
    }

    public override string Shell => "python";

    public override string Extension => ".py";
}