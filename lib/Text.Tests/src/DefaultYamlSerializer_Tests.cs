using GnomeStack.Extras.Strings;
using GnomeStack.Standard;
using GnomeStack.Text.Yaml;

using Models;

namespace Tests;

public class DefaultYamlSerializer_Tests
{
    [UnitTest]
    public void Serialize()
    {
        var cat = new Cat();
        var yaml = new DefaultYamlSerializer();
        var result = yaml.Serialize(cat);
        if (Env.IsWindows)
            Assert.Equal("cat_name: Floof\r\ncat_age: 21\r\ncat_is_alive: false\r\n", result);
        else
            Assert.Equal("cat_name: Floof\ncat_age: 21\ncat_is_alive: false\n", result);
    }

    [UnitTest]
    public void SerializeWithStd()
    {
        var result = Yaml.Stringify(new Cat());
        if (Env.IsWindows)
            Assert.Equal("cat_name: Floof\r\ncat_age: 21\r\ncat_is_alive: false\r\n", result);
        else
            Assert.Equal("cat_name: Floof\ncat_age: 21\ncat_is_alive: false\n", result);
    }

    [UnitTest]
    public void SerializeNull()
    {
        Cat? cat = null;
        var yaml = new DefaultYamlSerializer();
        var result = yaml.Serialize(cat);
        Assert.Equal("null", result);
    }

    [UnitTest]
    public void SerializeTextStyles()
    {
        var textStyles = new TextStyles();
        var yaml = new DefaultYamlSerializer();
        var result = yaml.Serialize(textStyles);
        var r = """
single: 'single'
double: "double"
literal: |-
  literal
plain: 'line1

  line2'
folded: >-
  line1

  line2

""";
        Assert.Equal(r, result);
    }

    [UnitTest]
    public void Deserialize()
    {
        var yaml = new DefaultYamlSerializer();
        var result = yaml.Deserialize<Cat>("cat_name: Floof2\ncat_age: 22\ncat_is_alive: true\n");
        Assert.Equal("Floof2", result.Name);
        Assert.Equal(22, result.Age);
        Assert.True(result.IsAlive);
    }

    [UnitTest]
    public void DeserializeStream(IAssert assert)
    {
        var yaml = new DefaultYamlSerializer();
        var result = yaml.Deserialize<Cat>("cat_name: Floof3\ncat_age: 25\ncat_is_alive: true\n".AsStream());
        assert.NotNull(result);
        Assert.Equal("Floof3", result.Name);
        Assert.Equal(25, result.Age);
        Assert.True(result.IsAlive);
    }

    [UnitTest]
    public void DeserializeWithStd(IAssert assert)
    {
        var result = Yaml.Parse<Cat>("cat_name: Floof2\ncat_age: 22\ncat_is_alive: true\n");
        assert.NotNull(result);
        Assert.Equal("Floof2", result.Name);
        Assert.Equal(22, result.Age);
        Assert.True(result.IsAlive);
    }
}