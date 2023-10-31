# GnomeStack.Text.Json

Provides a simplified interface to serialize and deserialize
JSON and enables common attributes for serialization and provides
extension methods.

## Usage

### Std

```csharp
using GnomeStack.Std;

// elsewhere 
var yaml = Json.Stringify(new { Name = "John Doe", Age = 42 });
Console.WriteLine(yaml);
// => "{\n  \"name\": \"John Doe\",\n  \"age\": 42\n}\n"
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
var json = Json.Stringify(cat);
Console.WriteLine(json);
// => "{\n  \"name\": \"John Doe\",\n  \"age\": 42\n}\n"
```

### Extension Methods

```csharp
using GnomeStack.Extras.Yaml;
// elsewhere
var json = new Person { Name = "John Doe", Age = 42 }.ToJson();
// => "Name: John Doe\nAge: 42\n"
var person = json.FromJson<Person>();
```

MIT License
System.Text.Json is licensed under the MIT license.