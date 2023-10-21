using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

using GnomeStack.KeePass.Package;

using static System.Buffers.Binary.BinaryPrimitives;

namespace GnomeStack.KeePass;

internal static class Utils
{
    public static UTF8Encoding Utf8NoBom { get; } = new(false, false);

    public static ReadOnlySpan<byte> GetBytesAsSpan(ReadOnlySpan<char> chars)
    {
        return GetBytesAsSpan(Utf8NoBom, chars);
    }

    public static ReadOnlySpan<byte> GetBytesAsSpan(this Encoding encoding, ReadOnlySpan<char> chars)
    {
#if NETLEGACY
        var rental = ArrayPool<char>.Shared.Rent(chars.Length);
        chars.CopyTo(rental);
        var ret = encoding.GetBytes(rental);
        ArrayPool<char>.Shared.Return(rental, true);
        return ret;
#else
        var l = Utils.Utf8NoBom.GetByteCount(chars);
        var span = new Span<byte>(new byte[l]);
        Utils.Utf8NoBom.GetBytes(chars, span);
        return span;
#endif
    }

    public static int Read(this Stream stream, byte[] buffer)
    {
        return stream.Read(buffer, 0, buffer.Length);
    }

    public static void ReadOrThrow(this Stream stream, byte[] buffer)
    {
        var read = stream.Read(buffer);
        if (read != buffer.Length)
            throw new EndOfStreamException();
    }

    public static ushort ReadUInt16(this Stream stream)
    {
        const int l = 2;
        var bytes = ArrayPool<byte>.Shared.Rent(l);
        var c = stream.Read(bytes, 0, l);
        if (c != l)
            throw new EndOfStreamException();
        var ret = ReadUInt16LittleEndian(bytes.AsSpan().Slice(0, l));
        ArrayPool<byte>.Shared.Return(bytes, true);
        return ret;
    }

    public static uint ReadUInt32(this Stream stream)
    {
        const int l = 4;
        var bytes = ArrayPool<byte>.Shared.Rent(l);
        var c = stream.Read(bytes, 0, l);
        if (c != l)
            throw new EndOfStreamException();
        var ret = ReadUInt32LittleEndian(bytes.AsSpan().Slice(0, l));
        ArrayPool<byte>.Shared.Return(bytes, true);
        return ret;
    }

    public static ulong ReadUInt64(this Stream stream)
    {
        const int l = 8;
        var bytes = ArrayPool<byte>.Shared.Rent(l);
        var c = stream.Read(bytes, 0, l);
        if (c != l)
            throw new EndOfStreamException();
        var ret = ReadUInt64LittleEndian(bytes.AsSpan().Slice(0, l));
        ArrayPool<byte>.Shared.Return(bytes, true);
        return ret;
    }

    public static void Write(this Stream stream, byte field, byte[] data)
    {
        stream.WriteByte(field);
        ushort size = (ushort)data.Length;
        stream.Write(size);
        stream.Write(data);
    }

    public static void Write(this Stream stream, byte field, int data)
    {
        stream.WriteByte(field);
        stream.Write(4);
        stream.Write(data);
    }

    public static void Write(this Stream stream, byte field, ushort data)
    {
        stream.WriteByte(field);
        stream.Write(2);
        stream.Write(data);
    }

    public static void Write(this Stream stream, byte field, uint data)
    {
        stream.WriteByte(field);
        stream.Write(4);
        stream.Write(data);
    }

    public static void Write(this Stream stream, byte field, ulong data)
    {
        stream.WriteByte(field);
        stream.Write(8);
        stream.Write(data);
    }

    public static void Write(this Stream stream, byte[] data)
    {
        stream.Write(data, 0, data.Length);
    }

    public static void Write(this Stream stream, short value)
    {
        var bytes = ArrayPool<byte>.Shared.Rent(2);
        WriteInt16LittleEndian(bytes, value);
        stream.Write(bytes, 0, 2);
        ArrayPool<byte>.Shared.Return(bytes, true);
    }

    public static void Write(this Stream stream, int value)
    {
        var bytes = ArrayPool<byte>.Shared.Rent(4);
        WriteInt32LittleEndian(bytes, value);
        stream.Write(bytes, 0, 4);
        ArrayPool<byte>.Shared.Return(bytes, true);
    }

    public static void Write(this Stream stream, long value)
    {
        var bytes = ArrayPool<byte>.Shared.Rent(8);
        WriteInt64LittleEndian(bytes, value);
        stream.Write(bytes, 0, 8);
        ArrayPool<byte>.Shared.Return(bytes, true);
    }

    public static void Write(this Stream stream, uint value)
    {
        var bytes = ArrayPool<byte>.Shared.Rent(4);
        WriteUInt32LittleEndian(bytes, value);
        stream.Write(bytes, 0, 4);
        ArrayPool<byte>.Shared.Return(bytes, true);
    }

    public static void Write(this Stream stream, ushort value)
    {
        var bytes = ArrayPool<byte>.Shared.Rent(2);
        WriteUInt16LittleEndian(bytes, value);
        stream.Write(bytes, 0, 2);
        ArrayPool<byte>.Shared.Return(bytes, true);
    }

    public static void Write(this Stream stream, ulong value)
    {
        var bytes = ArrayPool<byte>.Shared.Rent(8);
        WriteUInt64LittleEndian(bytes, value);
        stream.Write(bytes, 0, 8);
        ArrayPool<byte>.Shared.Return(bytes, true);
    }
}