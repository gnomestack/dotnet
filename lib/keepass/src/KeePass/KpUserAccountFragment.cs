using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass;

public class KpUserAccountFragment : KpKeyFragment
{
    public const string Bin = "ProtectedUserKey.bin";

    public KpUserAccountFragment(string? keyLocation = null)
    {
        this.GenerateKey(keyLocation);
    }

    private static ReadOnlySpan<byte> Entropy => new byte[]
    {
        0xDE, 0x13, 0x5B, 0x5F,
        0x18, 0xA3, 0x46, 0x70,
        0xB2, 0x57, 0x24, 0x29,
        0x69, 0x88, 0x98, 0xE6,
    };

    public void GenerateKey(string? keyLocation)
    {
        var filePath = string.IsNullOrWhiteSpace(keyLocation) ? GetKeyFilePath() : keyLocation;
        filePath = Path.GetFullPath(filePath);
        var dir = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var key = ReadOnlySpan<byte>.Empty;
        if (File.Exists(filePath))
            key = this.GetKey(filePath);

        if (key.IsEmpty)
            key = this.SetKey(filePath);

        this.SetData(key);
    }

    private static string GetKeyFilePath()
    {
        var roaming = Environment.GetEnvironmentVariable("APPDATA");
        if (string.IsNullOrWhiteSpace(roaming))
        {
            roaming = Environment.GetEnvironmentVariable("HOME");
        }

        if (string.IsNullOrWhiteSpace(roaming))
            throw new InvalidProgramException("Could not determine home directory");

        roaming = System.IO.Path.Combine(roaming, "KeePass", Bin);

        return roaming;
    }

    private ReadOnlySpan<byte> GetKey(string filepath)
    {
        byte[] bytes = File.ReadAllBytes(filepath);
        return Dpapi.Unprotect(bytes, Entropy);
    }

    private ReadOnlySpan<byte> SetKey(string filepath)
    {
        var key = KpRng.Default.NextBytes(64);

        var bytes = Dpapi.Protect(key, Entropy);
        using var fw = File.OpenWrite(filepath);
        fw.Write(bytes);

        return key;
    }
}