namespace GnomeStack.Run.Jobs;

public interface IJobCollection : IEnumerable<IJob>
{
    int Count { get; }

    IJob? this[int index] { get; }

    IJob? this[string id] { get; }

    IJobBuilder Add(IJob job);

    void AddRange(IEnumerable<IJob> tasks);

    bool Contains(string id);

    bool Contains(IJob task);

    IJob[] ToArray();
}