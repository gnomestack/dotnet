using GnomeStack.Text.Serialization;

namespace Models;

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