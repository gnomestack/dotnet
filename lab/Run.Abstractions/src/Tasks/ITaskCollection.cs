namespace GnomeStack.Run.Tasks;

public interface ITaskCollection : IEnumerable<ITask>
{
    int Count { get; }

    ITask? this[int index] { get; }

    ITask? this[string id] { get; }

    ITaskBuilder Add(ITask task);

    void AddRange(IEnumerable<ITask> tasks);

    bool Contains(string id);

    bool Contains(ITask task);

    ITask[] ToArray();
}