using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography;

public class ChaCha20Csrng : Csrng
{
    private readonly ChaCha20 algo;

    private readonly ICryptoTransform transform;

    public ChaCha20Csrng(byte[] key, byte[] iv)
    {
        this.algo = ChaCha20.Create();
        this.algo.Key = key;
        this.algo.IV = iv;
        this.algo.SkipXor = true;
        this.algo.Rounds = ChaChaRound.Twenty;
        this.transform = this.algo.CreateEncryptor();
    }

    public override void GetBytes(byte[] data)
    {
        this.transform.TransformBlock(data, 0, data.Length, data, 0);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        this.transform.Dispose();
        this.algo.Dispose();
        base.Dispose(disposing);
    }
}