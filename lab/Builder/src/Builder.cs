using System;
using System.Linq;

using GnomeStack.Extras.Object;

namespace GnomeStack.Builder;

public abstract class Builder<T>
{
    private Lazy<T> init;

    protected Builder()
    {
        this.init = new(this.CreateInstance);
    }

    protected T Instance
    {
        get => this.init.Value;
        set => this.init = new(() => value);
    }

    protected Type Type { get; } = typeof(T);

    public virtual T Build(bool createNew = false)
    {
        var value = createNew ? this.Instance.DeepCopy() : this.Instance;
        return this.OnAfterBuild(value);
    }

    protected virtual T OnAfterBuild(T value)
    {
        return value;
    }

    protected virtual T CreateInstance()
    {
        return Activator.CreateInstance<T>()!;
    }
}