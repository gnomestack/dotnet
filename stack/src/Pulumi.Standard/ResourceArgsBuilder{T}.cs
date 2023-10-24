namespace Pulumi;

public abstract class ResourceArgsBuilder<T> : PulumiBuilder<T>
    where T : ResourceArgs, new()
{
    protected ResourceArgsBuilder()
        : base()
    {
    }

    protected ResourceArgsBuilder(T value)
        : base(value)
    {
    }

    protected override T CreateInstance()
    {
        return new T();
    }
}