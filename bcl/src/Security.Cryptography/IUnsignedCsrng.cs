using System;

namespace GnomeStack.Security.Cryptography;

[CLSCompliant(false)]
public interface IUnsignedCsrng
{
    ushort NextUInt16();

    uint NextUInt32();

    long NextUInt64();
}