using GnomeStack.Secrets;
using GnomeStack.Text.Serialization;

namespace GnomeStack.Extensions.Secrets;

public class SecretRequirement
{
    [Serialization("url")]
    public string Url { get; set; } = string.Empty;

    [Serialization("env")]
    public string? EnvironmentVariableName { get; set; } = null;

    [Serialization("default")]
    public string? Default { get; set; } = null;

    [Serialization("generate")]
    public bool Generate { get; set; } = false;

    [Serialization("required")]
    public bool Required { get; set; } = false;

    [Serialization("length")]
    public int Length { get; set; } = 16;

    [Serialization("upper")]
    public bool Upper { get; set; } = true;

    [Serialization("lower")]
    public bool Lower { get; set; } = true;

    [Serialization("number")]
    public bool Number { get; set; } = true;

    [Serialization("special")]
    public string? Special { get; set; } = SecretCharacterSets.SpecialSafe;
}