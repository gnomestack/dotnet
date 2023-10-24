using GnomeStack.KeePass.Collections;
using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Package;

public class KpHeader
{
    public byte[] CipherId { get; set; } = Array.Empty<byte>();

    public byte[] CipherIV { get; set; } = Array.Empty<byte>();

    public byte[] CipherKeySeed { get; set; } = Array.Empty<byte>();

    public byte CompressionType { get; set; }

    [CLSCompliant(false)]
    public uint FormatVersion { get; set; } = KpHeaderExtensions.Version4_1;

    public byte[] AesKey { get; set; } = Array.Empty<byte>();

    public long AesRounds { get; set; }

    public byte CsrngType { get; set; }

    public byte[] CsrngKey { get; set; } = Array.Empty<byte>();

    public byte[] HeaderByteMarks { get; set; } = Array.Empty<byte>();

    public byte[] Hash { get; set; } = Array.Empty<byte>();

    public DerivedKeyParams DerivedKeyParams { get; set; } = DerivedKeyParams.Empty;

    public KpMap KpMap { get; set; } = new();
}