using GnomeStack.KeePass.Collections;
using GnomeStack.KeePass.Cryptography;
using GnomeStack.Security.Cryptography;

using static System.Buffers.Binary.BinaryPrimitives;

namespace GnomeStack.KeePass.Package;

#pragma warning disable SA1310
public static class KpHeaderExtensions
{
    internal const uint Signature1 = 0x9AA2D903;
    internal const uint Signature2 = 0xB54BFB67;
    internal const uint Version = 0x00040001;
    internal const uint Version4 = 0x00040000;
    internal const uint Version4_1 = 0x00040001;
    internal const uint Version3_1 = 0x00030001;
    internal const uint Version32 = 0x00040001;

    internal const uint Mask = 0xFFFF0000;

    public static void GenerateSafeDefaults(this KpHeader header)
    {
        if (header.AesRounds == 0)
            header.AesRounds = 6000;

        header.CipherId = KpAesCipher.IdValue.ToBytes();
        header.CompressionType = 1;
        header.CsrngType = (byte)2;
        var rng = new Csrng();
        var bytes = new byte[32];
        rng.GetBytes(bytes);

        header.CipherKeySeed = bytes;

        rng.GetBytes(bytes);
        header.AesKey = bytes;

        rng.GetBytes(bytes);
        header.CsrngKey = bytes;

        rng.GetBytes(bytes);
        header.HeaderByteMarks = bytes;

        bytes = new byte[16];
        rng.GetBytes(bytes);
        header.CipherIV = bytes;
    }

    public static void Write(this KpHeader header, Stream stream)
    {
        stream.Write(Signature1);
        stream.Write(Signature2);
        stream.Write(header.FormatVersion);

        stream.Write((byte)KpHeaderFields.CipherId, header.CipherId);
        stream.Write((byte)KpHeaderFields.CompressionType, (uint)header.CompressionType);
        stream.Write((byte)KpHeaderFields.CipherKeySeed, header.CipherKeySeed);

        if (header.FormatVersion < Version)
        {
            stream.Write((byte)KpHeaderFields.AesKeySeed, header.AesKey);
            stream.Write((byte)KpHeaderFields.AesIterations, (ulong)header.AesRounds);
        }
        else
        {
            stream.Write((byte)KpHeaderFields.DerivedKeyParams, header.DerivedKeyParams.ToBytes());
        }

        if (header.CipherIV.Length > 0)
            stream.Write((byte)KpHeaderFields.CipherIV, header.CipherIV);

        if (header.FormatVersion < Version4)
        {
            stream.Write((byte)KpHeaderFields.CsrngKey, header.CsrngKey);
            stream.Write((byte)KpHeaderFields.HeaderByteMark, header.HeaderByteMarks);
            stream.Write((byte)KpHeaderFields.CsrngType, (uint)header.CsrngType);
        }

        if (header.KpMap.Count > 0)
        {
            var bytes = header.KpMap.ToBytes();
            stream.Write((byte)KpHeaderFields.KpMap, bytes);
        }

        var r = (byte)'\r';
        var n = (byte)'\n';
        stream.Write(new[] { r, n, r, n });
    }

    public static void WriteRng(this KpHeader header, Stream stream)
    {
        stream.Write((byte)KpRngHeaderFields.RngId, (int)header.CsrngType);
        stream.Write((byte)KpRngHeaderFields.RngKey, header.CsrngKey);
    }

    public static void Read(this KpHeader header, Stream stream)
    {
        using var ms = new MemoryStream();
        var sig1 = stream.ReadUInt32();
        var sig2 = stream.ReadUInt32();
        if (sig1 != Signature1 || sig2 != Signature2)
            throw new InvalidDataException("Invalid signature");

        var version = stream.ReadUInt32();
        if ((version & Mask) != Version)
            throw new InvalidDataException("Invalid version");

        ms.Write(sig1);
        ms.Write(sig2);
        ms.Write(version);

        while (ReadNext(stream, ms, header))
        {
            // TODO: add logging
        }

        header.Hash = ms.ToSha256Hash();
    }

    private static bool ReadNext(Stream input, Stream output, KpHeader header)
    {
        int nextByte = input.ReadByte();
        if (nextByte == -1)
            throw new InvalidOperationException("Unexpected end of stream");

        output.Write(nextByte);
        var field = (KpHeaderFields)nextByte;
        var size = input.ReadUInt16();
        var data = Array.Empty<byte>();
        if (size > 0)
        {
            data = new byte[size];
            var c = input.Read(data, 0, size);
            if (c != size)
                throw new EndOfStreamException();
        }

        output.Write(size);
        output.Write(data);

        switch (field)
        {
            case KpHeaderFields.EndOfHeader:
                return false;

            case KpHeaderFields.CipherId:
                header.CipherId = data;
                break;

            case KpHeaderFields.CompressionType:
                header.CompressionType = (byte)ReadUInt32LittleEndian(data);
                break;

            case KpHeaderFields.CipherKeySeed:
                header.CipherKeySeed = data;
                break;

            // aes
            case KpHeaderFields.AesKeySeed:
                header.AesKey = data;
                break;

            // iterations
            case KpHeaderFields.AesIterations:
                header.AesRounds = (long)ReadUInt64LittleEndian(data);
                break;

            // nonce
            case KpHeaderFields.CipherIV:
                header.CipherIV = data;
                break;

            // random bytes
            case KpHeaderFields.CsrngKey:
                header.CsrngKey = data;
                break;

            // byte mark to notate end of header
            case KpHeaderFields.HeaderByteMark:
                header.HeaderByteMarks = data;
                break;

            // rng type
            case KpHeaderFields.CsrngType:
                header.CsrngType = (byte)ReadUInt32LittleEndian(data);
                break;

            case KpHeaderFields.DerivedKeyParams:
                header.DerivedKeyParams = DerivedKeyParams.FromBytes(data);
                break;

            case KpHeaderFields.KpMap:
                header.KpMap = KpMap.FromBytes(data);
                break;

            default:
                return false;
        }

        return true;
    }
}