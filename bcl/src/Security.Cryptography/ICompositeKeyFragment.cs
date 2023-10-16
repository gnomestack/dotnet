using System;

namespace GnomeStack.Security.Cryptography
{
    public interface ICompositeKeyFragment : IDisposable
    {
        ReadOnlySpan<byte> ToReadOnlySpan();
    }
}