using GnomeStack.Extras.Arrays;

namespace Tests;

public static class ArrayExtensions_Tests
{
    [UnitTest]
    public static void Clear(IAssert assert)
    {
        var array = new[] { 1, 2, 3 };
        assert.Equal(3, array.Length);
        assert.Equal(1, array[0]);
        assert.Equal(2, array[1]);
        assert.Equal(3, array[2]);

        array.Clear();
        assert.Equal(3, array.Length);
        assert.Equal(0, array[0]);
        assert.Equal(0, array[1]);
        assert.Equal(0, array[2]);
    }

    [UnitTest]
    public static void Sort(IAssert assert)
    {
        var array = new[] { 3, 2, 1 };
        assert.Equal(3, array.Length);
        assert.Equal(3, array[0]);
        assert.Equal(2, array[1]);
        assert.Equal(1, array[2]);

        array.Sort();
        assert.Equal(3, array.Length);
        assert.Equal(1, array[0]);
        assert.Equal(2, array[1]);
        assert.Equal(3, array[2]);
    }

    [UnitTest]
    public static void SortWithComparison(IAssert assert)
    {
        var array = new[] { 1, 2, 3 };
        assert.Equal(3, array.Length);
        assert.Equal(1, array[0]);
        assert.Equal(2, array[1]);
        assert.Equal(3, array[2]);

        array.Sort((x, y) => y.CompareTo(x));
        assert.Equal(3, array.Length);
        assert.Equal(3, array[0]);
        assert.Equal(2, array[1]);
        assert.Equal(1, array[2]);
    }

    [UnitTest]
    public static void Concat()
    {
        var array1 = new[] { 1 };
        var array2 = new[] { 2 };
        var array3 = new[] { 3 };
        var array4 = new[] { 4 };

        var array = array1.Concat(array2, array3, array4);
        Assert.Equal(4, array.Length);
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(4, array[3]);

        array = array1.Concat(array2);
        Assert.Equal(2, array.Length);
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);

        array = array1.Concat(array2, array3);
        Assert.Equal(3, array.Length);
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(3, array[2]);
    }

    [UnitTest]
    public static void Verify_Slice(IAssert assert)
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var slice = array.Slice(1, 3);

        assert.Equal(3, slice.Length);
        assert.Equal(2, slice[0]);
        assert.Equal(3, slice[1]);

        slice = array.Slice(1);
        assert.Equal(4, slice.Length);
        assert.Equal(2, slice[0]);
        assert.Equal(5, slice[3]);
    }

    [UnitTest]
    public static void Verify_Segment(IAssert assert)
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var segment = array.Segment(1, 3);

        assert.Equal(1, segment.Offset);
        assert.Equal(3, segment.Count);
        assert.Equal(2, segment.First());
        assert.Equal(4, segment.Last());

        segment = array.Segment(1);
        assert.Equal(1, segment.Offset);
        assert.Equal(4, segment.Count);
        assert.Equal(2, segment.First());
        assert.Equal(5, segment.Last());
    }

    [UnitTest]
    public static void Verify_Swap(IAssert assert)
    {
        var array = new[] { 1, 2, 3 };
        array.Swap(0, 2);

        assert.Equal(3, array.Length);
        assert.Equal(3, array[0]);
        assert.Equal(2, array[1]);
        assert.Equal(1, array[2]);
    }

    [UnitTest]
    public static void Verify_EqualTo(IAssert assert)
    {
        var array1 = new[] { 1, 2, 3 };
        var array2 = new[] { 1, 2, 3 };
        var array3 = new[] { 1, 2, 3, 4 };
        var array4 = new[] { 4, 2, 4 };

        assert.True(array1.EqualTo(array1));
        assert.True(array1.EqualTo(array2));
        assert.False(array1.EqualTo(array3));
        assert.False(array1.EqualTo(array4));

        assert.True(array1.EqualTo(array1, (a, b) => a.CompareTo(b)));
        assert.True(array1.EqualTo(array2, (a, b) => a.CompareTo(b)));
        assert.False(array1.EqualTo(array3, (a, b) => a.CompareTo(b)));

        // treat 4 as a wildcard where it matches any value
        var eq = new IntEqualityComparer();
        assert.True(array1.EqualTo(array1, eq));
        assert.True(array1.EqualTo(array2, eq));
        assert.False(array1.EqualTo(array3, eq));
        assert.True(array1.EqualTo(array4, eq));

        // treat 4 as a wildcard where it matches any value
        var cp = new IntComparer();
        assert.True(array1.EqualTo(array1, cp));
        assert.True(array1.EqualTo(array2, cp));
        assert.False(array1.EqualTo(array3, cp));
        assert.True(array1.EqualTo(array4, cp));
    }

    private class IntComparer : Comparer<int>
    {
        public override int Compare(int x, int y)
        {
            return x == y || x == 4 || y == 4 ? 0 : 1;
        }
    }

    private class IntEqualityComparer : IEqualityComparer<int>
    {
        public bool Equals(int x, int y)
        {
            return x == y || x == 4 || y == 4;
        }

        public int GetHashCode(int obj)
        {
            throw new NotImplementedException();
        }
    }
}