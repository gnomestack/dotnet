using System.Security.Cryptography;
using System.Text;

using GnomeStack.Extra.Arrays;

namespace GnomeStack.KeePass.Cryptography;

public class HmacStream : System.IO.Stream
{
    private readonly Stream innerStream;

    private readonly BinaryReader? reader;

    private readonly BinaryWriter? writer;

    private readonly byte[] endOfStreamMarker = new byte[32];

    private readonly Func<HashAlgorithm> hashFactory;

    private bool endOfStream = false;

    private int expectedPosition = 0;

    private bool disposed;

    private int bufferOffset = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="HmacStream"/> class.
    /// </summary>
    /// <param name="innerStream">The stream that will be read or written to.</param>
    /// <param name="write">If true, the stream will be written to; otherwise, read from.</param>
    public HmacStream(Stream innerStream, bool write = true)
        : this(innerStream, write, Utils.Utf8NoBom, null)
    {
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="HmacStream"/> class.
    /// </summary>
    /// <param name="innerStream">The stream that will be read or written to.</param>
    /// <param name="write">If true, the stream will be written to; otherwise, read from.</param>
    /// <param name="encoding">The encoding that the stream should use.</param>
    /// <param name="hashFactory">The <see cref="System.Security.Cryptography.HashAlgorithm"/> that should be used to verify the encrypted data.</param>
    public HmacStream(Stream innerStream, bool write, Encoding? encoding = null, Func<HashAlgorithm>? hashFactory = null)
    {
        encoding ??= Utils.Utf8NoBom;
        hashFactory ??= SHA256.Create;
        if (innerStream is null)
            throw new ArgumentNullException(nameof(innerStream));

        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));

        this.innerStream = innerStream;
        this.hashFactory = hashFactory;

        if (write)
            this.writer = new BinaryWriter(innerStream, encoding);
        else
            this.reader = new BinaryReader(innerStream, encoding);

        this.hashFactory = hashFactory;
    }

    /// <summary>
    /// Gets a value indicating whether consumers of this stream can read from it.
    /// </summary>
    public override bool CanRead => this.writer == null;

    /// <summary>
    /// Gets a value indicating whether consumers of this stream can seek. Always False.
    /// </summary>
    public override bool CanSeek => false;

    /// <summary>
    /// Gets a value indicating whether consumers of this stream can write to it.
    /// </summary>
    public override bool CanWrite => this.writer != null;

    /// <summary>
    /// Gets the length of the stream.
    /// </summary>
    public override long Length => this.innerStream.Length;

    /// <summary>
    /// Gets or sets the current position of the stream. Set is not supported.
    /// </summary>
    public override long Position
    {
        get
        {
            return this.innerStream.Position;
        }

        set
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Flushes the write stream, if there is one.
    /// </summary>
    public override void Flush()
    {
        this.writer?.Flush();
    }

    /// <summary>
    /// Reads a give number of bytes (<paramref name="count"/>) starting at the given <paramref name="offset"/>.
    /// </summary>
    /// <param name="buffer">The buffer that filled with bytes.</param>
    /// <param name="offset">The offset from the position of the stream.</param>
    /// <param name="count">The number of bytes to read to the buffer.</param>
    /// <returns>The number of bytes read.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (this.reader == null)
            throw new InvalidOperationException("HMACStream cannot read");

        int progress = 0;

        var buf = Array.Empty<byte>();

        while (progress < count)
        {
            if (buf.Length == 0)
            {
                this.bufferOffset = 0;
                buf = this.ReadNext();
                if (buf.Length == 0)
                    return progress;
            }

            int l = Math.Min(buf.Length - this.bufferOffset, count);
            Array.Copy(buf, this.bufferOffset, buffer, offset, l);
            offset += l;
            this.bufferOffset += l;
            progress += l;

            if (this.bufferOffset == buf.Length)
                buf = Array.Empty<byte>();
        }

        return progress;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Writes the number of bytes from the buffer at the specified offset.
    /// </summary>
    /// <param name="buffer">That data to write to the stream.</param>
    /// <param name="offset">The offset from the position of the inner stream.</param>
    /// <param name="count">The number of bytes that should be written.</param>
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (this.writer == null)
            throw new InvalidOperationException($"HMACStream cannot write.");

        this.writer.Write(this.expectedPosition);
        this.expectedPosition++;

        var length = count - offset;
        var bytes = new byte[length];
        Array.Copy(buffer, bytes, length);

        using (var hash = this.hashFactory())
        {
            var hashBytes = hash.ComputeHash(bytes);
            this.writer.Write(hashBytes);
        }

        this.writer.Write(length);
        this.writer.Write(bytes);
    }

    /// <summary>
    /// Disposes the object.
    /// </summary>
    /// <param name="disposing">Determines if the object is disposed manually or gced.</param>
    protected override void Dispose(bool disposing)
    {
        if (this.disposed)
            return;

        this.disposed = true;
        if (disposing)
        {
            this.reader?.Dispose();
            if (this.writer is not null)
            {
                this.WriteEndOfStream();
                this.Flush();
            }

            this.writer?.Dispose();
            this.innerStream.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Writes the end of the stream.
    /// </summary>
    protected virtual void WriteEndOfStream()
    {
        if (this.writer is null)
            throw new InvalidOperationException("HmacStream is read only and cannot be written to.");

        this.writer.Write(this.expectedPosition);
        this.writer.Write(new byte[32]);
        this.writer.Write(0);
    }

    private byte[] ReadNext()
    {
        if (this.endOfStream || this.reader == null)
            return Array.Empty<byte>();

        int actualPosition = this.reader.ReadInt32();
        if (this.expectedPosition != actualPosition)
            throw new CryptographicException($"The stream's actual position {actualPosition} does not match the expected position {this.expectedPosition} ");

        this.expectedPosition++;
        byte[] expectedHash = this.reader.ReadBytes(32);
        int bufferSize = this.reader.ReadInt32();

        if (bufferSize == 0)
        {
            if (!this.endOfStreamMarker.SequenceEqual(expectedHash))
                throw new CryptographicException("invalid EOF marker");

            this.endOfStream = true;
            return Array.Empty<byte>();
        }

        byte[] decryptedBytes = this.reader.ReadBytes(bufferSize);

        using var hash = this.hashFactory();
        byte[] actualHash = hash.ComputeHash(decryptedBytes);
        if (!expectedHash.SequenceEqual(actualHash))
            throw new CryptographicException("The file is corrupted or has been tampered with.");

        expectedHash.Clear();
        actualHash.Clear();

        return decryptedBytes;
    }
}