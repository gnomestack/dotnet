using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography
{
    // https://blake2.net/blake2.pdf
    [SuppressMessage("", "CA1819:", Justification = "Byte[] for properties are required in crypto.")]
    public sealed class Blake2B : HashAlgorithm
    {
        private const int Rounds = 12;

        private const int BufferSize = 128;

        private const int OutBytes = 64;

        // ReSharper disable once InconsistentNaming
        private static readonly ulong[] IV = new[]
        {
            0x6A09E667F3BCC908UL, 0xBB67AE8584CAA73BUL, 0x3C6EF372FE94F82BUL, 0xA54FF53A5F1D36F1UL,
            0x510E527FADE682D1UL, 0x9B05688C2B3E6C1FUL, 0x1F83D9ABFB41BD6BUL, 0x5BE0CD19137E2179UL,
        };

        private static readonly int[] Sigma = new[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            14, 10, 4, 8, 9, 15, 13, 6, 1, 12, 0, 2, 11, 7, 5, 3,
            11, 8, 12, 0, 5, 2, 15, 13, 10, 14, 3, 6, 7, 1, 9, 4,
            7, 9, 3, 1, 13, 12, 11, 14, 2, 6, 5, 10, 4, 0, 15, 8,
            9, 0, 5, 7, 2, 4, 10, 15, 14, 1, 11, 12, 6, 8, 3, 13,
            2, 12, 6, 10, 0, 11, 8, 3, 4, 13, 7, 5, 15, 14, 1, 9,
            12, 5, 1, 15, 14, 13, 4, 10, 0, 7, 6, 3, 9, 2, 8, 11,
            13, 11, 7, 14, 12, 1, 3, 9, 5, 0, 15, 4, 8, 6, 2, 10,
            6, 15, 14, 9, 11, 3, 0, 8, 12, 2, 13, 7, 1, 4, 10, 5,
            10, 2, 8, 4, 7, 6, 1, 5, 15, 11, 9, 14, 3, 12, 13, 0,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            14, 10, 4, 8, 9, 15, 13, 6, 1, 12, 0, 2, 11, 7, 5, 3,
        };

        private readonly ulong[] h = new ulong[8]; // config

        private readonly ulong[] t = new ulong[2]; // counters

        private readonly ulong[] f = new ulong[2]; // finalizer flags

        private readonly ulong[] m = new ulong[16];

        private readonly ulong[] v = new ulong[16];

        private readonly byte[] buffer = new byte[BufferSize];

        private readonly int hashLength;

        private byte[]? personalization;

        private byte[]? key;

        private byte[]? salt;

        private int bufferOffset;

        public Blake2B(
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> salt,
            ReadOnlySpan<byte> personalization,
            int hashLength = OutBytes)
            : this(hashLength)
        {
            if (!key.IsEmpty)
            {
                if (key.Length > BufferSize)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(key),
                        $"key must not exceed {BufferSize} in length.");
                }

                this.key = new byte[key.Length];
                key.CopyTo(this.key);
            }

            if (!salt.IsEmpty)
            {
                if (salt.Length != 16)
                {
                    throw new ArgumentOutOfRangeException(
                       nameof(salt),
                       $"key must be only 16 bytes in length.");
                }

                this.salt = new byte[salt.Length];
                key.CopyTo(this.salt);
            }

            if (!personalization.IsEmpty)
            {
                if (personalization.Length != 16)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(personalization),
                        $"personalization must be only 16 bytes in length.");
                }

                this.personalization = new byte[personalization.Length];
                key.CopyTo(this.personalization);
            }

            if (hashLength is < 0 or > OutBytes)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hashLength),
                    $"hashLength ({hashLength}) must not exceed the total max size in bytes ({OutBytes})");
            }

            this.hashLength = hashLength;
            this.HashSizeValue = hashLength * 8;
            this.Initialize();
        }

        public Blake2B()
            : this(OutBytes)
        {
        }

        public Blake2B(int hashLength)
        {
            if (hashLength is < 0 or > OutBytes)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hashLength),
                    $"hashLength ({hashLength}) must not exceed the total max size in bytes ({OutBytes})");
            }

            this.hashLength = hashLength;
            this.HashSizeValue = hashLength * 8;
            this.Initialize();
        }

        public byte[] Salt
        {
            get
            {
                if (this.salt is null)
                    return Array.Empty<byte>();

                return this.salt.AsSpan().ToArray();
            }

            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(this.Salt));

                if (value.Length != 16)
                    throw new ArgumentOutOfRangeException(nameof(this.Salt), "Salt must have a length of 16");

                if (this.salt?.SequenceEqual(value) == true)
                    return;

                this.salt = new byte[16];
                value.AsSpan().CopyTo(this.salt);
                this.Initialize();
            }
        }

        public byte[] Key
        {
            get
            {
                if (this.key is null)
                    return Array.Empty<byte>();

                return this.key.AsSpan().ToArray();
            }

            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(this.Key));

                if (value.Length > BufferSize)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(this.Key),
                        $"Key must not exceed {BufferSize} in length.");
                }

                if (this.key?.SequenceEqual(value) == true)
                    return;

                this.key = new byte[value.Length];
                value.AsSpan(0, BufferSize).CopyTo(this.key);

                this.Initialize();
            }
        }

        public byte[] Personalization
        {
            get
            {
                if (this.personalization is null)
                    return Array.Empty<byte>();

                return this.personalization.AsSpan().ToArray();
            }

            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(this.Personalization));

                if (value.Length != 16)
                    throw new ArgumentOutOfRangeException(nameof(this.Personalization), "Personalization must have a length of 16");

                if (this.personalization?.SequenceEqual(value) == true)
                    return;

                this.personalization = new byte[16];
                value.AsSpan().CopyTo(this.personalization);
                this.Initialize();
            }
        }

        public override void Initialize()
        {
            this.h[0] = IV[0];
            this.h[1] = IV[1];
            this.h[2] = IV[2];
            this.h[3] = IV[3];
            this.h[4] = IV[4];
            this.h[5] = IV[5];
            this.h[6] = IV[6];
            this.h[7] = IV[7];

            unsafe
            {
                var config = stackalloc ulong[8];
                config[0] |= (uint)this.hashLength;

                if (this.key is { Length: > 0 })
                    config[0] |= (uint)this.key.Length << 8;

                // currently only supports sequential hashing
                // FanOut
                config[0] |= 1U << 16;

                // Depth
                config[0] |= 1U << 24;

                // Leaf length
                // config[0] |= 0L << 32;
                // config[2] |= (uint)0 intermediate hash is not supported
                if (this.salt is { Length: 16 })
                {
                    config[4] = BytesToUInt64(this.salt, 0);
                    config[5] = BytesToUInt64(this.salt, 8);
                }

                if (this.personalization is { Length: 16 })
                {
                    config[6] = BytesToUInt64(this.personalization, 0);
                    config[7] = BytesToUInt64(this.personalization, 8);
                }

                for (var i = 0; i < 8; i++)
                    this.h[i] ^= config[i];
            }

            Array.Clear(this.t, 0, this.t.Length);
            Array.Clear(this.f, 0, this.f.Length);
            Array.Clear(this.m, 0, this.m.Length);
            Array.Clear(this.v, 0, this.v.Length);
            Array.Clear(this.buffer, 0, this.buffer.Length);

            if (this.key != null)
                this.HashCore(this.key, 0, this.key.Length);
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (this.bufferOffset + cbSize > OutBytes)
            {
                int l = OutBytes - this.bufferOffset;
                if (l > 0)
                    Array.Copy(array, ibStart, this.buffer, this.bufferOffset, l);

                this.IncrementCount(OutBytes);
                this.Compress(this.buffer, 0);

                this.bufferOffset = 0;
                cbSize -= l;
                ibStart += l;

                while (cbSize > OutBytes)
                {
                    this.IncrementCount(OutBytes);
                    this.Compress(array, ibStart);
                    cbSize -= OutBytes;
                    ibStart += OutBytes;
                }
            }

            if (cbSize > 0)
            {
                Array.Copy(array, ibStart, this.buffer, this.bufferOffset, cbSize);
                this.bufferOffset += cbSize;
            }
        }

        protected override byte[] HashFinal()
        {
            if (this.f[0] != 0)
                throw new CryptographicException();

            this.f[0] = ulong.MaxValue;
            int l = OutBytes - this.bufferOffset;
            if (l > 0)
                Array.Clear(this.buffer, this.bufferOffset, l);

            this.IncrementCount((ulong)this.bufferOffset);
            this.Compress(this.buffer, 0);

            byte[] result = new byte[OutBytes];
            for (var i = 0; i < 8; ++i)
            {
                UInt64ToBytes(this.h[i], result, i << 3);
            }

            if (this.hashLength == OutBytes)
                return result;

            return result.AsSpan(0, this.hashLength).ToArray();
        }

        private static ulong BytesToUInt64(byte[] buf, int offset)
        {
            return (ulong)buf[offset + 7] << (7 * 8) |
                ((ulong)buf[offset + 6] << (6 * 8)) |
                ((ulong)buf[offset + 5] << (5 * 8)) |
                ((ulong)buf[offset + 4] << (4 * 8)) |
                ((ulong)buf[offset + 3] << (3 * 8)) |
                ((ulong)buf[offset + 2] << (2 * 8)) |
                ((ulong)buf[offset + 1] << (1 * 8)) |
                buf[offset];
        }

        private static void UInt64ToBytes(ulong value, byte[] buf, int offset)
        {
            buf[offset + 7] = (byte)(value >> (7 * 8));
            buf[offset + 6] = (byte)(value >> (6 * 8));
            buf[offset + 5] = (byte)(value >> (5 * 8));
            buf[offset + 4] = (byte)(value >> (4 * 8));
            buf[offset + 3] = (byte)(value >> (3 * 8));
            buf[offset + 2] = (byte)(value >> (2 * 8));
            buf[offset + 1] = (byte)(value >> (1 * 8));
            buf[offset] = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void IncrementCount(ulong blocks)
        {
            this.t[0] += blocks;
            if (this.t[0] < blocks)
                this.t[1]++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void G(int a, int b, int c, int d, int r, int i)
        {
            int p = (r << 4) + i;
            int p0 = Sigma[p];
            int p1 = Sigma[p + 1];
            var v = this.v;
            var m = this.m;

            v[a] += v[b] + m[p0];
            v[d] = BitOperations.RotateRight(v[d] ^ v[a], 32);
            v[c] += v[d];
            v[b] = BitOperations.RotateRight(v[b] ^ v[c], 24);
            v[a] += v[b] + m[p1];
            v[d] = BitOperations.RotateRight(v[d] ^ v[a], 16);
            v[c] += v[d];
            v[b] = BitOperations.RotateLeft(v[b] ^ v[c], 63);
        }

        private void Compress(byte[] bytes, int offset)
        {
            for (int i = 0; i < 16; ++i)
                this.m[i] = BytesToUInt64(bytes, offset + (i << 3));

            Array.Copy(this.h, this.v, 8);

            this.v[8] = IV[0];
            this.v[9] = IV[1];
            this.v[10] = IV[2];
            this.v[11] = IV[3];
            this.v[12] = IV[4] ^ this.t[0]; // counter 0
            this.v[13] = IV[5] ^ this.t[1]; // counter 1
            this.v[14] = IV[6] ^ this.f[0]; // finalization flag 1
            this.v[15] = IV[7] ^ this.f[1]; // finalization flag 2

            for (int r = 0; r < Rounds; ++r)
            {
                this.G(0, 4, 8, 12, r, 0);
                this.G(1, 5, 9, 13, r, 2);
                this.G(2, 6, 10, 14, r, 4);
                this.G(3, 7, 11, 15, r, 6);
                this.G(3, 4, 9, 14, r, 14);
                this.G(2, 7, 8, 13, r, 12);
                this.G(0, 5, 10, 15, r, 8);
                this.G(1, 6, 11, 12, r, 10);
            }

            for (int i = 0; i < 8; ++i)
                this.h[i] ^= this.v[i] ^ this.v[i + 8];
        }
    }
}