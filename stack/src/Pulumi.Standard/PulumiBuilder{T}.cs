
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using FastMember;

namespace Pulumi;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1314:Type parameter names should begin with T")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PulumiBuilder<T>
{
    private readonly Lazy<ObjectAccessor> accessor = new(() => ObjectAccessor.Create(typeof(T)));

    protected PulumiBuilder()
    {
        #pragma warning disable S1699 // Possible null reference argument.
        this.Instance = this.CreateInstance();
        this.Type = typeof(T);
    }

    protected PulumiBuilder(T instance)
    {
        this.Instance = instance;
        this.Type = typeof(T);
    }

    protected T Instance { get; set; }

    protected Type Type { get; }

    protected ObjectAccessor Accessor => this.accessor.Value;

    public virtual T Build()
    {
        return this.Instance;
    }

    protected void Set<P>(Expression<Func<T, P>> expression, P value)
    {
        var member = (MemberExpression)expression.Body;
        var param = Expression.Parameter(typeof(string), "value");
        var set = Expression.Lambda<Action<T, P>>(
            Expression.Assign(member, param), expression.Parameters[0], param);

        set.Compile()(this.Instance, value);
    }

    protected void Set<P>(string name, P value)
    {
        this.Accessor[name] = value;
    }

    protected object? Get(string name)
    {
        return this.Accessor[name];
    }

    protected P Get<P>(string name)
    {
        return (P)this.Accessor[name];
    }

    protected P Get<P>(Expression<Func<T, P>> expression)
    {
        return expression.Compile()(this.Instance);
    }

    protected virtual T CreateInstance()
    {
        return Activator.CreateInstance<T>();
    }
}