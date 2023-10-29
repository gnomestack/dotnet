namespace GnomeStack.Text.DotEnv.Document;

public class EnvDocumentEntry
{
    private string? value;

    public string Value
    {
        get => this.value ??= new string(this.RawValue);
    }

    public char[] RawValue { get; set; } = Array.Empty<char>();

    public override string ToString()
    {
        return this.Value;
    }
}