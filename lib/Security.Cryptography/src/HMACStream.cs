using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Security.Cryptography
{
    /// <summary>
    /// Hash-Based Message Authenticated Code Stream that adds authenticated codes
    /// using a given hash algorithm. This stream will write a block of bytes
    /// from the inner stream, the block size, and then write a computed
    /// hash with the given hash algorithm. The default encoding is UTF-8
    /// with no Byte Order Mark ("BOM").
    /// </summary>
    public class HmacStream : Stream
    {
        private readonly Stream innerStream;

        private readonly BinaryReader? reader;

        private readonly BinaryWriter? writer;

        private readonly byte[] endOfStreamMarker;

        private readonly HashAlgorithm signer;

        private bool endOfStream;

        private byte[]? internalBuffer;

        private int expectedPosition;

        private bool disposed;

        private int bufferOffset;

        public HmacStream(Stream innerStream)
            : this(innerStream, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HmacStream"/> class.
        /// </summary>
        /// <param name="innerStream">The stream that will be read or written to.</param>
        /// <param name="write">If true, the stream will be written to; otherwise, read from.</param>
        public HmacStream(Stream innerStream, bool write)
            : this(innerStream, write, Encodings.Utf8NoBom, HashAlgorithmName.SHA256)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HmacStream"/> class.
        /// </summary>
        /// <param name="innerStream">The stream that will be read or written to.</param>
        /// <param name="write">If true, the stream will be written to; otherwise, read from.</param>
        /// <param name="encoding">The encoding that the stream should use.</param>
        /// <param name="keyedHashAlgorithmType">The keyed hash algorithm used to create an authentication code.</param>
        /// <param name="key">The key for the keyed hash algorithm.</param>
        public HmacStream(
            Stream innerStream,
            bool write,
            Encoding encoding,
            KeyedHashAlgorithmType keyedHashAlgorithmType,
            byte[] key)
            : this(innerStream, write, encoding, HashAlgorithmName.SHA256)
        {
            var algo = keyedHashAlgorithmType.CreateKeyedHashAlgorithm();
            this.endOfStreamMarker = new byte[algo.HashSize];
            algo.Key = key;
            this.signer = algo;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HmacStream"/> class.
        /// </summary>
        /// <param name="innerStream">The stream that will be read or written to.</param>
        /// <param name="write">If true, the stream will be written to; otherwise, read from.</param>
        /// <param name="encoding">The encoding that the stream should use.</param>
        /// <param name="hashAlgorithm">The hash algorithm used to create an authentication code.</param>
        public HmacStream(Stream innerStream, bool write, Encoding encoding, HashAlgorithmName hashAlgorithm)
        {
            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            this.innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));

            if (write)
                this.writer = new BinaryWriter(innerStream, encoding);
            else
                this.reader = new BinaryReader(innerStream, encoding);

            this.signer = hashAlgorithm.CreateHashAlgorithm();
            this.endOfStreamMarker = new byte[this.signer.HashSize];
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
        /// Gets or sets the current position of the stream.
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
            if (this.writer != null)
                this.writer.Flush();
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

            while (progress < count)
            {
                if (this.internalBuffer == null)
                {
                    this.bufferOffset = 0;
                    this.internalBuffer = this.ReadNext();
                    if (this.internalBuffer == null || this.internalBuffer.Length == 0)
                        return progress;
                }

                int l = Math.Min(this.internalBuffer.Length - this.bufferOffset, count);

                Array.Copy(this.internalBuffer, this.bufferOffset, buffer, offset, l);
                offset += l;
                this.bufferOffset += l;
                progress += l;

                if (this.bufferOffset == this.internalBuffer.Length)
                    this.internalBuffer = null;
            }

            return progress;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="offset"> The offset.</param>
        /// <param name="origin"> The origin.</param>
        /// <returns>a long.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="value"> The length.</param>
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

            var hashBytes = this.signer.ComputeHash(bytes);

            this.writer.Write(hashBytes);
            this.writer.Write(length);
            this.writer.Write(bytes);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">Determines if the object is disposed manually or garbage collected.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:Do not catch general exception types", Justification = "By Design")]
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            this.disposed = true;
            if (disposing)
            {
                if (this.reader != null)
                {
                    try
                    {
                        this.reader.Dispose();
                    }
                    catch
                    {
                        // meh. coreclr throws an exception
                    }
                }

                if (this.writer != null)
                {
                    this.WriteEndOfStream();
                    this.Flush();
                    this.writer.Dispose();
                }

                this.innerStream.Dispose();
                this.signer.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Writes the end of the stream.
        /// </summary>
        protected virtual void WriteEndOfStream()
        {
            if (this.writer is null)
                throw new InvalidOperationException("The writer is null");

            this.writer.Write(this.expectedPosition);
            this.writer.Write(new byte[this.signer.HashSize]);
            this.writer.Write(0);
        }

        private byte[] ReadNext()
        {
            if (this.endOfStream)
                return Array.Empty<byte>();

            if (this.reader is null)
                throw new InvalidOperationException("Reading from the stream is not currently enabled");

            int actualPosition = this.reader.ReadInt32();
            if (this.expectedPosition != actualPosition)
                throw new IOException($"The stream's actual position {actualPosition} does not match the expected position {this.expectedPosition} ");

            this.expectedPosition++;
            byte[] expectedHash = this.reader.ReadBytes(this.signer.HashSize);
            int bufferSize = this.reader.ReadInt32();

            if (bufferSize == 0)
            {
                if (!this.endOfStreamMarker.SequenceEqual(expectedHash))
                    throw new IOException("invalid end-of-stream marker");

                this.endOfStream = true;
                return Array.Empty<byte>();
            }

            byte[] encryptedBytes = this.reader.ReadBytes(bufferSize);
            byte[] actualHash = this.signer.ComputeHash(encryptedBytes);

            if (!expectedHash.SequenceEqual(actualHash))
                throw new IOException("The file is corrupted or has been tampered with.");

            Array.Clear(expectedHash, 0, expectedHash.Length);
            Array.Clear(actualHash, 0, actualHash.Length);

            return encryptedBytes;
        }
    }
}