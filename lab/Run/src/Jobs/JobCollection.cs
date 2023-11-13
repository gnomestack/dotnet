using System.Collections;
using System.Diagnostics.CodeAnalysis;

using GnomeStack.Collections.Generic;
using GnomeStack.Run.Tasks;

namespace GnomeStack.Run.Jobs;

[SuppressMessage("ReSharper", "ParameterHidesMember")]
public class JobCollection : IJobCollection
{
    private readonly OrderedDictionary<string, IJob> jobs =
        new(StringComparer.OrdinalIgnoreCase);

    public int Count => this.jobs.Count;

    public IJob? this[int index] => index < 0 || index >= this.jobs.Count ? null : this.jobs[index];

    public IJob? this[string id] => this.jobs.TryGetValue(id, out var v) ? v : null;

    public IJobBuilder Add(IJob job)
    {
        this.jobs.Add(job.Id, job);
        return new JobBuilder(job);
    }

    public void AddRange(IEnumerable<IJob> jobs)
    {
        foreach (var job in jobs)
        {
            this.jobs.Add(job.Id, job);
        }
    }

    public bool Contains(string id)
    {
        return this.jobs.TryGetValue(id, out var _);
    }

    public bool Contains(IJob job)
    {
        return this.jobs.ContainsValue(job);
    }

    public bool Contains(int index)
    {
        if (index < 0)
            return false;

        return this.jobs.Count > index;
    }

    public IEnumerator<IJob> GetEnumerator()
        => this.jobs.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    public void Remove(string id)
        => this.jobs.Remove(id);

    public void Remove(IJob task)
        => this.jobs.Remove(task.Id);

    public IJob[] ToArray()
    {
        return this.jobs.Values.ToArray();
    }
}