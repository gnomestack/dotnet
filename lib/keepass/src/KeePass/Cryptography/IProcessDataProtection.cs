namespace GnomeStack.KeePass.Cryptography;

public interface IProcessDataProtection
{
    ReadOnlySpan<byte> Protect(ReadOnlySpan<byte> userData, ReadOnlySpan<byte> optionalEntropy, bool isLocalMachine = false);

    ReadOnlySpan<byte> Unprotect(ReadOnlySpan<byte> userData, ReadOnlySpan<byte> optionalEntropy, bool isLocalMachine = false);
}