using System.Security.Cryptography;

using GnomeStack.Security.Cryptography;

namespace GnomeStack.KeePass.Cryptography;

public static class KpRng
{
    private static readonly List<Type?> s_engines = new() { null, null, typeof(Salsa20Rng), typeof(ChaCha20) };

    public static IKpStreamCipherRng Default { get; } = Create(4);

    public static IKpStreamCipherRng Create(int id)
    {
        if (id < 0 || id >= s_engines.Count)
            throw new ArgumentOutOfRangeException(nameof(id));

        var pw = new byte[32];
        var salt = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(pw);
        rng.GetBytes(salt);
        using var pbdk2 = new GnomeStackRfc2898DeriveBytes(pw, salt, 100001, HashAlgorithmName.SHA256);
        var key = pbdk2.GetBytes(32);

        var type = s_engines[id];
        if (type == null)
            throw new InvalidOperationException($"No random byte generator engine registered for id {id}");
        return (IKpStreamCipherRng)Activator.CreateInstance(type, args: new object[] { key })!;
    }
}