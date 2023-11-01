using System.Runtime.CompilerServices;

using GnomeStack.Functional;

namespace GnomeStack.Dex.Flows.Tasks;

public static class TaskCollectionExtensions
{
    public static ITaskBuilder AddTask(this ITaskCollection col, string id, RunTaskAsync run)
    {
        var dt = new DelegateAsyncTask(id, run);
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, RunTask run)
    {
        var dt = new DelegateTask(id, run);
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, IEnumerable<string> deps)
    {
        var dt = new DelegateAsyncTask(id, (_, _) => Task.FromResult(Result.Ok<object, Error>(Nil.Value)));
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, IEnumerable<string> deps, RunTaskAsync run)
    {
        var dt = new DelegateAsyncTask(id, run);
        dt.Deps = new List<string>(deps);
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, IEnumerable<string> deps, RunTask run)
    {
        var dt = new DelegateTask(id, run);
        dt.Deps = new List<string>(deps);
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, string name, RunTaskAsync run)
    {
        var dt = new DelegateAsyncTask(id, run)
        {
            Name = name,
        };
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, string name, RunTask run)
    {
        var dt = new DelegateTask(id, run)
        {
            Name = name,
        };
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, string name, IEnumerable<string> deps, RunTaskAsync run)
    {
        var dt = new DelegateAsyncTask(id, run)
        {
            Name = name,
            Deps = new List<string>(deps),
        };
        return col.Add(dt);
    }

    public static ITaskBuilder AddTask(this ITaskCollection col, string id, string name, IEnumerable<string> deps, RunTask run)
    {
        var dt = new DelegateTask(id, run)
        {
            Name = name,
            Deps = new List<string>(deps),
        };
        return col.Add(dt);
    }


    public static IShellTaskBuilder AddShellTask(this ITaskCollection col, IShellTask task)
    {
        col.Add(task);
        return new ShellTaskBuilder(task);
    }


    public static IShellTaskBuilder AddShellTask(this ITaskCollection col, string id, string script)
    {
        var dt = new ShellTask(id, script)
        {
            Name = id,
        };
        return col.AddShellTask(dt);
    }

    public static IShellTaskBuilder AddShellTask(this ITaskCollection col, string id, string shell, string script)
    {
        var dt = new ShellTask(id, script) { Name = id, Shell = shell };
        return col.AddShellTask(dt);
    }

    public static IShellTaskBuilder AddShellTask(this ITaskCollection col, string id, string shell, string script, IList<string> dependencies)
    {
        var dt = new ShellTask(id, script) { Name = id, Shell = shell };
        dt.Deps = dependencies;
        return col.AddShellTask(dt);
    }

    public static IShellTaskBuilder AddShellFileTask(this ITaskCollection col, string id, string file)
    {
        var dt = new ShellFileTask(id, file) { Name = id };
        return col.AddShellTask(dt);
    }

    public static IShellTaskBuilder AddShellFileTask(this ITaskCollection col, string id, string shell, string file)
    {
        var dt = new ShellFileTask(id, file) { Name = id, Shell = shell };
        return col.AddShellTask(dt);
    }

    public static IShellTaskBuilder AddShellFileTask(this ITaskCollection col, string id, string shell, string file, IList<string> deps)
    {
        var dt = new ShellFileTask(id, file) { Name = id, Shell = shell };
        dt.Deps = deps;
        return col.AddShellTask(dt);
    }
}