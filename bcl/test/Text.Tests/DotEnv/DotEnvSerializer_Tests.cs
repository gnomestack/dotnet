using GnomeStack.Collections.Generic;
using GnomeStack.Standard;
using GnomeStack.Text.DotEnv;
using GnomeStack.Text.DotEnv.Document;

namespace Tests;

// ReSharper disable once InconsistentNaming
public class DotEnvSerializer_Tests
{
    [UnitTest]
    public void Verify_Simple(IAssert assert)
    {
        var values = DotEnvSerializer.Deserialize(
            """
TEST="Hello World"
""",
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal("Hello World", env["TEST"]);
    }

    [UnitTest]
    public void Verify_Multiple(IAssert assert)
    {
        var envContent = """
TEST=hello_world
NUMBER=1
PW=X232dwe)()_+!@
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("1", env["NUMBER"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
    }

    [UnitTest]
    public void Verify_EmptyLinesAreIgnored(IAssert assert)
    {
        var envContent = """
TEST=hello_world

PW=X232dwe)()_+!@

""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
    }

    [UnitTest]
    public void Verify_ExpandBashVariable(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "world");
        var envContent = """
TEST=hello_$WORD
TEST2="Hello ${WORD}"
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("Hello world", env["TEST2"]);
    }

    [UnitTest]
    public void Verify_ExpandWithDefault(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "world");
        var envContent = """
TEST=hello_$WORD
TEST2="Hello ${WORD2:-world2}"
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("Hello world2", env["TEST2"]);
    }

    [UnitTest]
    public void Verify_ExpandWithCustomVariables(IAssert assert)
    {
        var envContent = """
PW=X232dwe)()_+!@
TEST=hello_world
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
    }

    [UnitTest]
    public void Verify_KeyCanSkipWhiteSpace(IAssert assert)
    {
        var envContent = """
  TEST  =hello_world
  PW   =X232dwe)()_+!@
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
    }

    [UnitTest]
    public void Verify_OneMultilineValue(IAssert assert)
    {
        var envContent = """
TEST="1
2
3"
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        var r = "1\r\n2\r\n3";
        if (!Env.IsWindows)
            r = "1\n2\n3";
        assert.Equal(r, env["TEST"]);
    }

    [UnitTest]
    public void Verify_MultipleMultilineValues(IAssert assert)
    {
        var envContent = """
TEST="1
2
3"
PW='1
2
4'
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        var r1 = "1\r\n2\r\n3";
        if (!Env.IsWindows)
            r1 = "1\n2\n3";

        assert.Equal(r1, env["TEST"]);

        var r2 = "1\r\n2\r\n4";
        if (!Env.IsWindows)
            r2 = "1\n2\n4";

        assert.Equal(r2, env["PW"]);
    }

    [UnitTest]
    public void Verify_ValuesCanSkipWhiteSpace(IAssert assert)
    {
        var envContent = """
TEST=  hello_world  
PW=  X232dwe)()_+!@  
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
    }

    [UnitTest]
    public void Verify_CommentsAreIgnored(IAssert assert)
    {
        var envContent = """
# This is a comment
TEST=hello_world
  # this is a comment too
PW=X232dwe)()_+!@
## this is a comment woo hoo
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
    }

    [UnitTest]
    public void Verify_DeserializeDictionary(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "world");
        var envContent = """
# This is a comment
TEST=hello_world
  # this is a comment too
PW=X232dwe)()_+!@
## this is a comment woo hoo
MULTI="1
2
3
  4"
HW="Hello, ${WORD}"
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(OrderedDictionary<string, string>));

        assert.NotNull(values);
        assert.IsType<OrderedDictionary<string, string>>(values);

        var env = (OrderedDictionary<string, string>)values;
        assert.Equal(4, env.Count);
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);
        var r = "1\r\n2\r\n3\r\n  4";
        if (!Env.IsWindows)
            r = "1\n2\n3\n  4";
        assert.Equal(r, env["MULTI"]);
        assert.Equal("Hello, world", env["HW"]);
    }

    [UnitTest]
    public void Verify_Json(IAssert assert)
    {
        var envContent = """
JSON={
    ""test"": ""hello world"",
    ""test2"": ""hello world2""
}
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>),
            new DotEnvSerializerOptions { AllowJson = true });

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(1, env.Count);
        assert.Equal(
            """
{
    ""test"": ""hello world"",
    ""test2"": ""hello world2""
}
""",
            env["JSON"]);
    }

    [UnitTest]
    public void Verify_Yaml(IAssert assert)
    {
        var envContent = """
YAML=---
test: hello world
test2: hello world2
---
TEST=hello_world
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(Dictionary<string, string>),
            new DotEnvSerializerOptions { AllowYaml = true });

        assert.NotNull(values);
        assert.IsType<Dictionary<string, string>>(values);

        var env = (Dictionary<string, string>)values;
        assert.Equal(2, env.Count);
        assert.Equal(
            """
test: hello world
test2: hello world2

""",
            env["YAML"]);
        assert.Equal("hello_world", env["TEST"]);
    }

    [UnitTest]
    public void Verify_DeserializeEnvDocument(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "world");
        var envContent = """
# This is a comment
TEST=hello_world
  # this is a comment too
PW=X232dwe)()_+!@
## this is a comment woo hoo
MULTI="1
2
3
  4"
HW="Hello, ${WORD}"
""";
        var values = DotEnvSerializer.Deserialize(
            envContent,
            typeof(EnvDocument));

        assert.NotNull(values);
        assert.IsType<EnvDocument>(values);

        var env = (EnvDocument)values;
        assert.Equal("hello_world", env["TEST"]);
        assert.Equal("X232dwe)()_+!@", env["PW"]);

        var r = "1\r\n2\r\n3\r\n  4";
        if (!Env.IsWindows)
            r = "1\n2\n3\n  4";
        assert.Equal(r, env["MULTI"]);
        assert.Equal("Hello, world", env["HW"]);
    }

    [UnitTest]
    public void Serialize()
    {
        var dictionary = new OrderedDictionary<string, string>()
        {
            ["name"] = "John Doe",
            ["age"] = "21",
        };

        var result = DotEnvSerializer.Serialize(dictionary);
        var expected = $$"""
                         name="John Doe"
                         age="21"
                         """;
        if (!Env.IsWindows)
            expected = expected.Replace("\r\n", "\n");
        Assert.Equal(expected, result);
    }

    [UnitTest]
    public void SerializeEnvDocument()
    {
        var doc = new EnvDocument();
        doc.Add("name", "John Doe");
        doc.AddEmptyLine();
        doc.AddComment("Test");
        doc.Add("age", "21");

        var result = DotEnvSerializer.Serialize(doc);
        var expected = $$"""
                         name="John Doe"
                         
                         # Test
                         age="21"
                         """;
        if (!Env.IsWindows)
            result = result.Replace("\r\n", "\n");

        Assert.Equal(expected, result);
    }

    [UnitTest]
    public void Load()
    {
        Env.Remove("age");
        Env.Remove("name");
        Assert.False(Env.Has("name"));
        var options = new DotEnvLoadOptions()
        {
            Content = """
                      name="John Doe"
                      age="21"
                      """,
        };

        DotEnv.Load(options);
        Assert.True(Env.Has("name"));
        Assert.True(Env.Has("age"));
        Assert.Equal("21", Env.GetRequired("age"));
        Assert.Equal("John Doe", Env.GetRequired("name"));
    }
}