using GnomeStack.Fmt.Ansi;
using GnomeStack.Run.Runners;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Standard;

public static class Run
{
    public static ITaskCollection GlobalsTasks { get; } = new TaskCollection();

    public static ITaskBuilder Task(ITask task)
        => GlobalsTasks.Add(task);

    public static ITaskBuilder Task(string id, RunTaskAsync run)
        => GlobalsTasks.AddTask(id, run);

    public static ITaskBuilder Task(string id, RunTask run)
        => GlobalsTasks.AddTask(id, run);

    public static ITaskBuilder Task(string id, RunTaskAction run)
        => GlobalsTasks.AddTask(id, run);

    public static ITaskBuilder Task(string id, string name, RunTaskAsync run)
        => GlobalsTasks.AddTask(id, name, run);

    public static ITaskBuilder Task(string id, string name, RunTask run)
        => GlobalsTasks.AddTask(id, name, run);

    public static ITaskBuilder Task(string id, string name, RunTaskAction run)
        => GlobalsTasks.AddTask(id, name, run);

    public static ITaskBuilder Task(string id, IEnumerable<string> deps, RunTaskAsync run)
        => GlobalsTasks.AddTask(id, deps, run);

    public static ITaskBuilder Task(string id, IEnumerable<string> deps, RunTask run)
        => GlobalsTasks.AddTask(id, deps, run);

    public static ITaskBuilder Task(string id, IEnumerable<string> deps, RunTaskAction run)
        => GlobalsTasks.AddTask(id, deps, run);

    public static ITaskBuilder Task(string id, string name, IEnumerable<string> deps, RunTaskAsync run)
        => GlobalsTasks.AddTask(id, name, deps, run);

    public static ITaskBuilder Task(string id, string name, IEnumerable<string> deps, RunTask run)
        => GlobalsTasks.AddTask(id, name, deps, run);

    public static ITaskBuilder Task(string id, string name, IEnumerable<string> deps, RunTaskAction run)
        => GlobalsTasks.AddTask(id, name, deps, run);

    public static ITaskBuilder Task(string id, IEnumerable<string> deps)
        => GlobalsTasks.AddTask(id, deps);

    public static IShellTaskBuilder ShellTask(string id, string shell, string script)
        => GlobalsTasks.AddShellTask(id, shell, script);

    public static IShellTaskBuilder ShellTask(string id, string script)
        => GlobalsTasks.AddShellTask(id, script);

    public static IShellTaskBuilder ShellTask(string id, string shell, string script, IEnumerable<string> deps)
        => GlobalsTasks.AddShellTask(id, shell, script, deps);

    public static IShellTaskBuilder ShellFileTask(string id, string shell, string file)
        => GlobalsTasks.AddShellFileTask(id, shell, file);

    public static IShellTaskBuilder ShellFileTask(string id, string file)
        => GlobalsTasks.AddShellFileTask(id, file);

    public static IShellTaskBuilder ShellFileTask(string id, string shell, string file, IEnumerable<string> deps)
        => GlobalsTasks.AddShellFileTask(id, shell, file, deps);

    public static IShellTaskBuilder ShellFileTask(string id, string file, IEnumerable<string> deps)
        => GlobalsTasks.AddShellFileTask(id, file, deps);

    [CLSCompliant(false)]
    public static async Task<int> ParseAndRunAsync(
        string[] args,
        IAnsiWriter? writer = null,
        CancellationToken cancellationToken = default)
    {
        var runner = new ConsoleRunner();
        var result = await runner.RunAsync(args, GlobalsTasks, writer, cancellationToken).NoCap();
        return result;
    }
}