using System.Collections;

using GnomeStack.Collections.Generic;

namespace GnomeStack.Run.Tasks;

public class TaskCollection : ITaskCollection
{
    private OrderedDictionary<string, ITask> tasks = new(StringComparer.OrdinalIgnoreCase);

    public int Count => this.tasks.Count;

    public ITask? this[int index] => index < 0 || index >= this.tasks.Count ? null : this.tasks[index];

    public ITask? this[string id] => this.tasks.TryGetValue(id, out var v) ? v : null;

    public ITaskBuilder Add(ITask task)
    {
        this.tasks.Add(task.Id, task);
        return new TaskBuilder(task);
    }

    public void AddRange(IEnumerable<ITask> tasks)
    {
        foreach (var task in tasks)
        {
            this.tasks.Add(task.Id, task);
        }
    }

    public bool Contains(string id)
    {
        return this.tasks.TryGetValue(id, out var _);
    }

    public bool Contains(ITask task)
    {
        return this.tasks.ContainsValue(task);
    }

    public bool Contains(int index)
    {
        if (index < 0)
            return false;

        return this.tasks.Count > index;
    }

    public IEnumerator<ITask> GetEnumerator()
        => this.tasks.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    public ITask[] ToArray()
    {
        return this.tasks.Values.ToArray();
    }
}