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

    public override void SetSecretValue(string name, string secret)
    {
        // do nothing
    }

    public override ISecretRecord? GetSecret(string name)
        => null;

    public override string? GetSecretValue(string name)
        => null;

    public override void DeleteSecret(string name)
    {
        // do nothing
    }

    public override ISecretRecord CreateRecord(string name)
        => new NullSecretRecord(name);

    public class NullSecretRecord : SecretRecord
    {
        public NullSecretRecord(string name)
            : base(name)
        {
        }
    }
}