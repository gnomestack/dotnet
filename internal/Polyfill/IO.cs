#if NETLEGACY
using System.Buffers;

namespace System.IO;

#pragma warning disable SA1649
internal static class IOExtensions
{
    public static void Write(this Stream stream, Span<byte> buffer)
    {
        var set = ArrayPool<byte>.Shared.Rent(buffer.Length);
        buffer.CopyTo(set);
        stream.Write(set, 0, buffer.Length);
        ArrayPool<byte>.Shared.Return(set, true);
    }

    public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
    {
        var set = ArrayPool<byte>.Shared.Rent(buffer.Length);
        buffer.CopyTo(set);
        stream.Write(set, 0, buffer.Length);
        Array.Clear(set, 0, set.Length);
    }

    public static void Write(this BinaryWriter writer, Span<byte> buffer)
    {
        var set = ArrayPool<byte>.Shared.Rent(buffer.Length);
        buffer.CopyTo(set);
        writer.Write(set, 0, buffer.Length);
        ArrayPool<byte>.Shared.Return(set, true);
    }

    public static void Write(this BinaryWriter writer, ReadOnlySpan<byte> buffer)
    {
        var set = ArrayPool<byte>.Shared.Rent(buffer.Length);
        buffer.CopyTo(set);
        writer.Write(set, 0, buffer.Length);
        ArrayPool<byte>.Shared.Return(set, true);
    }

    public static void Write(this TextWriter writer, ReadOnlySpan<char> chars)
    {
        var buffer = ArrayPool<char>.Shared.Rent(chars.Length);
        try
        {
            chars.CopyTo(buffer);
            writer.Write(buffer, 0, buffer.Length);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer, true);
        }
    }

    public static Task WriteAsync(this TextWriter writer, ReadOnlyMemory<char> chars, CancellationToken cancellationToken = default)
    {
        var buffer = ArrayPool<char>.Shared.Rent(chars.Length);
        try
        {
            chars.Span.CopyTo(buffer);
            return writer.WriteAsync(buffer, 0, buffer.Length);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer, true);
        }
    }

    public static int Read(this TextReader reader, Span<char> chars)
    {
        var buffer = ArrayPool<char>.Shared.Rent(chars.Length);
        try
        {
            var read = reader.Read(buffer, 0, buffer.Length);
            if (read > 0)
                buffer.AsSpan(0, read).CopyTo(chars);

            return read;
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer, true);
        }
    }

    public static Task<int> ReadAsync(this TextReader reader, Memory<char> chars, CancellationToken cancellationToken = default)
    {
        var buffer = ArrayPool<char>.Shared.Rent(chars.Length);
        try
        {
            var read = reader.Read(buffer, 0, buffer.Length);
            if (read > 0)
                buffer.AsSpan(0, read).CopyTo(chars.Span);

            return Task.FromResult(read);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer, true);
        }
    }
}
#endif