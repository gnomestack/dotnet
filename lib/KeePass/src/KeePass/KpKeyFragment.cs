using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass;

public abstract class KpKeyFragment
{
    private ShroudedBytes bytes = ShroudedBytes.Empty;

    public ReadOnlySpan<byte> Read()
    {
        return this.bytes.Read();
    }

    protected void SetData(ReadOnlySpan<byte> data)
    {
        this.bytes = new ShroudedBytes(data);
    }
}