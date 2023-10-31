using GnomeStack.Builder;

namespace Pulumi;

public abstract class ResourceArgsBuilder<T> : Builder<T>
    where T : ResourceArgs, new()
{
    protected ResourceArgsBuilder()
        : base()
    {
    }

    protected ResourceArgsBuilder(T value)
    {
        this.Instance = value;
    }

    protected override T CreateInstance()
    {
        return new T();
    }
}