using GnomeStack.Extras.Strings;
using GnomeStack.Standard;
using GnomeStack.Text.Json;

using Models;

namespace Tests;

public class DefaultJsonSerializer_Tests
{
    [UnitTest]
    public void Serialize()
    {
        var cat = new Cat();
        var json = new DefaultJsonSerializer();
        var result = json.Serialize(cat);
        var expected = $$"""
{
  "cat_name": "Floof",
  "cat_age": 21,
  "cat_is_alive": false
}
""";
        if (!Env.IsWindows)
        {
            expected = expected.Replace("\r\n", "\n");
        }

        Assert.Equal(expected, result);
    }

    [UnitTest]
    public void SerializeWithStd()
    {
        var result = Json.Stringify(new Cat());
        var expected = $$"""
                         {
                           "cat_name": "Floof",
                           "cat_age": 21,
                           "cat_is_alive": false
                         }
                         """;
        if (!Env.IsWindows)
        {
            expected = expected.Replace("\r\n", "\n");
        }

        Assert.Equal(expected, result);
    }

    [UnitTest]
    public void SerializeNull()
    {
        Cat? cat = null;
        var json = new DefaultJsonSerializer();
        var result = json.Serialize(cat);
        Assert.Equal("null", result);
    }

    [UnitTest]
    public void SerializeTextStyles()
    {
        var textStyles = new TextStyles();
        var json = new DefaultJsonSerializer();
        var result = json.Serialize(textStyles);
        var expected = $$"""
                         {
                           "single": "single",
                           "double": "double",
                           "literal": "literal",
                           "plain": "line1\nline2",
                           "folded": "line1\nline2"
                         }
                         """;

        if (!Env.IsWindows)
            expected = expected.Replace("\r\n", "\n");

        Assert.Equal(expected, result);
    }

    [UnitTest]
    public void Deserialize()
    {
        var json = new DefaultJsonSerializer();
        var text = $$"""
                     {
                       "cat_name": "Floof2",
                       "cat_age": 27,
                       "cat_is_alive": false
                     }
                     """;
        var cat = json.Deserialize<Cat>(text);
        Assert.Equal("Floof2", cat.Name);
        Assert.Equal(27, cat.Age);
        Assert.False(cat.IsAlive);
    }

    [UnitTest]
    public void DeserializeWithStd()
    {
        var text = $$"""
                     {
                       "cat_name": "Floof2",
                       "cat_age": 27,
                       "cat_is_alive": false
                     }
                     """;
        var cat = Json.Parse<Cat>(text);
        Assert.Equal("Floof2", cat.Name);
        Assert.Equal(27, cat.Age);
        Assert.False(cat.IsAlive);
    }

    [UnitTest]
    public void DeserializeStream(IAssert assert)
    {
        var json = new DefaultJsonSerializer();
        var text = $$"""
                     {
                       "cat_name": "Floof3",
                       "cat_age": 25,
                       "cat_is_alive": true
                     }
                     """;
        var cat = json.Deserialize<Cat>(text.AsStream());
        assert.NotNull(cat);
        Assert.Equal("Floof3", cat.Name);
        Assert.Equal(25, cat.Age);
        Assert.True(cat.IsAlive);
    }
}