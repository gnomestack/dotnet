using GnomeStack;
using GnomeStack.Functional;

using static GnomeStack.Standard.Option;

namespace Tests;

public class Option_Tests
{
    [UnitTest]
    public void SomeAndDeconstruct()
    {
        var optional = Some(1);
        var (value, some) = optional;
        Assert.True(some);
        Assert.Equal(1, value);
    }

    [UnitTest]
    public void NoneAndDeconstruct()
    {
        var optional = None<int>();
        var (value, some) = optional;
        Assert.False(some);
        Assert.Equal(0, value);
    }

    [UnitTest]
    public void SomeAndDeconstructWithOut()
    {
        var optional = Some(1);
        optional.Deconstruct(out var value, out var some);
        Assert.True(some);
        Assert.Equal(1, value);
    }

    [UnitTest]
    public void NoneAndDeconstructWithOut()
    {
        var optional = None<int>();
        optional.Deconstruct(out var value, out var some);
        Assert.False(some);
        Assert.Equal(0, value);
    }

    [UnitTest]
    public void Unwrap()
    {
        var optional = Some(1);
        Assert.Equal(1, optional.Unwrap());
    }

    [UnitTest]
    public void UnwrapWithNoneMustThrow()
    {
        var optional = None<int>();
        Assert.Throws<OptionException>(() => optional.Unwrap());
    }

    [UnitTest]
    public void UnwrapOrWithSome()
    {
        var optional = Some(1);
        Assert.Equal(1, optional.UnwrapOr(2));
    }

    [UnitTest]
    public void UnwrapOrWithNone()
    {
        var optional = None<int>();
        Assert.Equal(2, optional.UnwrapOr(2));
    }

    [UnitTest]
    public void UnwrapOrWithSomeAndFactory()
    {
        var optional = Some(1);
        Assert.Equal(1, optional.UnwrapOr(() => 2));
    }

    [UnitTest]
    public void UnwrapOrWithNoneAndFactory()
    {
        var optional = None<int>();
        Assert.Equal(2, optional.UnwrapOr(() => 2));
    }

    [UnitTest]
    public void OrWithSome()
    {
        var optional = Some(1);
        Assert.Equal(1, optional.Or(2));
    }

    [UnitTest]
    public void IsNoneCheck()
    {
        var none = None<int>();
        Assert.True(IsNone(none));
        Assert.False(IsNone(Some(1)));
        Assert.False(IsNone(1));
        Assert.False(IsNone("1"));
        Assert.False(IsNone(new object()));
        Assert.False(IsNone(new object[] { }));
        Assert.False(IsNone(new List<int>()));
        Assert.True(IsNone(null));
        Assert.True(IsNone(Nil.Value));
        Assert.True(IsNone(DBNull.Value));
        Assert.True(IsNone(ValueTuple.Create()));
        Assert.True(IsNone(None()));
        Assert.True(none.Equals(None()));
        Assert.True(none.Equals(Nil.Value));
    }
}