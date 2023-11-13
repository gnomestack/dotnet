using System.Collections;

namespace GnomeStack.Extensions.Secrets;

public sealed class NullSecretVault : SecretVault
{
    internal NullSecretVault()
    {
    }

    public static SecretVault Instance { get; } = new NullSecretVault();

    public override bool SupportsSynchronous => true;

    public override string Name => "null-vault";

    public override string Kind => "null-vault";

    public override IEnumerable<string> ListNames()
    {
        return Array.Empty<string>();
    }

    public override void SetSecret(ISecretRecord secret)
    {
        // do nothing
    }

    public override void SetSecretValue(string path, string secret)
    {
        // do nothing
    }

    public override ISecretRecord? GetSecret(string path)
        => null;

    public override string? GetSecretValue(string path)
        => null;

    public override void DeleteSecret(string path)
    {
        // do nothing
    }

    public override ISecretRecord CreateRecord(string path)
        => new NullSecretRecord(path);

    public class NullSecretRecord : SecretRecord
    {
        public NullSecretRecord(string name)
            : base(name)
        {
        }
    }
}