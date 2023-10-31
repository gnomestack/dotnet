namespace GnomeStack.Extensions.Secrets;

public interface ISecretRecord
{
    string Name { get; }

    string Value { get; set; }

    DateTime? ExpiresAt { get; set; }

    DateTime? CreatedAt { get; }

    DateTime? UpdatedAt { get; }

    IDictionary<string, string?> Tags { get; }
}