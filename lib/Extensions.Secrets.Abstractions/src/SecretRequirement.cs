namespace GnomeStack.Extensions.Secrets;

public class SecretRequirement
{
    public string Url { get; set; } = string.Empty;

    public string? Default { get; set; } = null;

    public bool Generate { get; set; } = false;

    public bool Required { get; set; } = false;

    public int Length { get; set; } = 16;

    public bool Upper { get; set; } = true;

    public bool Lower { get; set; } = true;

    public bool Number { get; set; } = true;

    public bool Special { get; set; } = true;
}