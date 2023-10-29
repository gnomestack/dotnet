# GnomeStack.Text.Yaml

Provides a simplified interface to serialize and deserialize
YAML documents and enables common attributes for serialization,
keeps the YamlDotNet attributes intact, and provides extension methods.

## Usage

### Std

```csharp
using GnomeStack.Std;

// elsewhere 
var yaml = Yaml.Stringify(new { Name = "John Doe", Age = 42 });
Console.WriteLine(yaml);
// => "Name: John Doe\nAge: 42\n"
```

### Attributes

```csharp
public class Cat
{
    [Serialization("cat_name")]
    public string Name { get; set; } = "Floof";

    [Serialization("cat_age")]
    public int Age { get; set; } = 21;

    [Serialization("cat_is_alive")]
    public bool IsAlive { get; set; }

    [Ignore]
    public string Ignored { get; set; } = "ignored";
}

var cat = new Cat { IsAlive = true };
var yaml = Yaml.Stringify(cat);
Console.WriteLine(yaml);
// => "cat_name: Floof\ncat_age: 21\ncat_is_alive: true\n"
```

### Extension Methods

```csharp
using GnomeStack.Extras.Yaml;
// elsewhere
var yaml = new { Name = "John Doe", Age = 42 }.ToYaml();
// => "Name: John Doe\nAge: 42\n"
```

MIT License
YamlDotNet is licensed under the MIT license.