using GnomeStack.Collections.Generic;

namespace Tests;

public static class OrderedDictionary_Tests
{
    [UnitTest]
    public static void Ctor(IAssert assert)
    {
        var dict = new OrderedDictionary<string, object?>();
        assert.Equal(0, dict.Count);
    }

    [UnitTest]
    public static void CtorWithComparer(IAssert assert)
    {
        var dict = new OrderedDictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        assert.Equal(0, dict.Count);
    }

    [UnitTest]
    public static void CtorWithValues(IAssert assert)
    {
        var dict = new OrderedDictionary<string, object>(new[] { new KeyValuePair<string, object>("a", 1), new KeyValuePair<string, object>("b", 2) });
        assert.Equal(2, dict.Count);
        assert.Equal(1, dict["a"]);
        assert.Equal(2, dict["b"]);

        var keys = dict.Keys.ToList();
        assert.Equal(2, keys.Count);
        assert.Equal("a", keys[0]);
        assert.Equal("b", keys[1]);
    }

    [UnitTest]
    public static void CtorWithValuesAndComparer(IAssert assert)
    {
        var dict = new OrderedDictionary<string, object>(new[] { new KeyValuePair<string, object>("a", 1), new KeyValuePair<string, object>("b", 2) }, StringComparer.OrdinalIgnoreCase);
        assert.Equal(2, dict.Count);
        assert.Equal(1, dict["A"]);
        assert.Equal(2, dict["B"]);

        var keys = dict.Keys.ToList();
        assert.Equal(2, keys.Count);
        assert.Equal("a", keys[0]);
        assert.Equal("b", keys[1]);
    }

    [UnitTest]
    public static void Indexers(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        dict.Add("Bob", 1);
        dict.Add("Alice", 2);

        assert.Equal(0, dict["Test"]);
        assert.Equal(0, dict[0]);
        assert.Equal(1, dict["Bob"]);
        assert.Equal(1, dict[1]);
        assert.Equal(2, dict["Alice"]);
        assert.Equal(2, dict[2]);

        dict["Test"] = 3;
        assert.Equal(3, dict["Test"]);
        assert.Equal(3, dict[0]);

        dict[0] = 4;
        assert.Equal(4, dict["Test"]);
        assert.Equal(4, dict[0]);
    }

    [UnitTest]
    public static void ContainsKey(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>
        {
            { "Test", 0 },
            { "Bob", 1 },
            { "Alice", 2 },
        };

        assert.True(dict.ContainsKey("Test"));
        assert.True(dict.ContainsKey("Bob"));
        assert.True(dict.ContainsKey("Alice"));
        assert.False(dict.ContainsKey("NotHere"));
        assert.False(dict.ContainsKey("NotThere"));
        assert.False(dict.ContainsKey("Alic"));
    }

    [UnitTest]
    public static void ContainsValue(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        dict.Add("Bob", 1);
        dict.Add("Alice", 2);

        assert.True(dict.ContainsValue(0));
        assert.True(dict.ContainsValue(1));
        assert.True(dict.ContainsValue(2));
        assert.False(dict.ContainsValue(3));
        assert.False(dict.ContainsValue(4));
    }

    [UnitTest]
    public static void Keys(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        var keys = dict.Keys.ToArray();
        assert.Collection(
            keys,
            o => assert.Equal("Bob", o),
            o => assert.Equal("Alice", o),
            o => assert.Equal("Test", o));
    }

    [UnitTest]
    public static void Values(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        var keys = dict.Values.ToArray();
        assert.Collection(
            keys,
            o => assert.Equal(1, o),
            o => assert.Equal(2, o),
            o => assert.Equal(0, o));
    }

    [UnitTest]
    public static void Add(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        dict.Add("Bob", 1);
        dict.Add("Alice", 2);

        assert.Equal(3, dict.Count);
        assert.Equal(0, dict["Test"]);
        assert.Equal(1, dict["Bob"]);
        assert.Equal(2, dict["Alice"]);
    }

    [UnitTest]
    public static void AddDuplicateKey(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        assert.Throws<ArgumentException>(() => dict.Add("Test", 1));
    }

    [UnitTest]
    public static void Remove(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        assert.True(dict.Remove("Test"));
        assert.False(dict.Remove("Test"));
        assert.Equal(2, dict.Count);

        assert.True(dict.Remove("Bob"));
        assert.False(dict.Remove("Bob"));
        assert.Equal(1, dict.Count);
    }

    [UnitTest]
    public static void Clear(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        dict.Clear();
        assert.Equal(0, dict.Count);

        dict.Add("Test7", 0);
        dict.Add("4", 1);

        assert.Equal(2, dict.Count);
        assert.Equal(0, dict["Test7"]);
        assert.Equal(1, dict["4"]);
    }

    [UnitTest]
    public static void IndexOf(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        assert.Equal(0, dict.IndexOf("Bob"));
        assert.Equal(1, dict.IndexOf("Alice"));
        assert.Equal(2, dict.IndexOf("Test"));
    }

    [UnitTest]
    public static void RemoveAt(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        dict.RemoveAt(1);
        assert.Equal(2, dict.Count);

        assert.Throws<ArgumentOutOfRangeException>(() => dict.RemoveAt(10));
    }

    [UnitTest]
    public static void Insert(IAssert assert)
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        dict.Insert(3, "Test2", 3);

        assert.Equal(4, dict.Count);
        assert.Equal(3, dict["Test2"]);
        assert.Equal(3, dict.IndexOf("Test2"));

        dict.Insert(0, "Test3", 4);
        assert.Equal(5, dict.Count);
        assert.Equal(4, dict["Test3"]);
        assert.Equal(0, dict.IndexOf("Test3"));
        assert.Equal(1, dict.IndexOf("Bob"));
    }
}