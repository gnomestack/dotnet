# GnomeStack.Text.DotEnv

Provides a dotenv `.env` serializer for .NET that supports
comments, blank lines, json, yaml and environment variable expansion.

Json and Yaml escapes are only supported for parsing. Backticks \` may
be used for quoting to handle both single and double quotes in a value.

Serialization/Deserialization currently only supports for `EnvDocument` 
and dictionaries including an `OrderedDictionary<string, string>`.

## Usage

### Loading

```csharp
using GnomeStack.Std;

// elsewhere
DotEnv.Load(new DotEnvLoadOptions()
        {
            Content = """
                      name="John Doe"
                      age="21"
                      """,
        });

Console.WriteLine(Env.Get("name"));
```

### Std

```csharp
using GnomeStack.Std;

// elsewhere 
var envText = DotEnv.Stringify(new OrderedDictionary<string, string> {
    { "NAME", "John Doe" },
    { "AGE", "42" }
});
Console.WriteLine(envText);
// => 
// NAME="John Doe"
// AGE="42"
```

### Deserialize

```csharp
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
    typeof(Dictionary<string, string>));

assert.NotNull(values);
assert.IsType<Dictionary<string, string>>(values);

var env = (Dictionary<string, string>)values;
assert.Equal(4, env.Count);
assert.Equal("hello_world", env["TEST"]);
assert.Equal("X232dwe)()_+!@", env["PW"]);
var r = "1\r\n2\r\n3\r\n  4";
if (!Env.IsWindows)
    r = "1\n2\n3\n  4";
assert.Equal(r, env["MULTI"]);
assert.Equal("Hello, world", env["HW"]);
```