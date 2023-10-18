using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography;

/// <summary>
/// An implementation of Salsa20, a stream cipher proposed by Daniel J. Bernstein available for
/// use in the public domain.
/// </summary>
public sealed class Salsa20 : SymmetricAlgorithm
{
    private KeySizes[]? legalBlockSizes;

    private KeySizes[]? legalKeySizes;

    private Csrng? rng;

    /// <summary>
    /// Initializes a new instance of the <see cref="Salsa20"/> class.
    /// </summary>
    private Salsa20()
    {
#if NETSTANDARD2_0 || NETFRAMEWORK
        this.LegalBlockSizesValue = new[] { new KeySizes(64, 64, 0), };
        this.LegalKeySizesValue = new[] { new KeySizes(128, 256, 128) };
#endif
        this.BlockSize = 64;
        this.KeySize = 256;
        this.Rounds = SalsaRounds.Twenty;
    }

    /// <summary>
    /// Gets or sets the number of rounds that should be used.
    /// </summary>
    public SalsaRounds Rounds { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether or skip a XOR operation during the transform block.
    /// </summary>
    public bool SkipXor { get; set; }

    /// <summary>
    /// Gets the block sizes, in bits, that are supported by the symmetric algorithm.
    /// </summary>
    public override KeySizes[] LegalBlockSizes
    {
        get
        {
            this.legalBlockSizes ??= new[] { new KeySizes(64, 64, 0), };

            return this.legalBlockSizes;
        }
    }

    /// <summary>
    /// Gets the key sizes, in bits, that are supported by the symmetric algorithm.
    /// </summary>
    public override KeySizes[] LegalKeySizes
    {
        get
        {
            this.legalKeySizes ??= new[] { new KeySizes(128, 256, 128) };
            return this.legalKeySizes;
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="Salsa20" />.
    /// </summary>
    /// <returns>A new instance of <see cref="Salsa20"/>.</returns>
    public static new Salsa20 Create()
    {
        return new Salsa20();
    }

    public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
    {
        rgbIV ??= this.IV;
        return new Salsa20CryptoTransform(rgbKey, rgbIV, (int)this.Rounds, this.SkipXor);
    }

    public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        => this.CreateDecryptor(rgbKey, rgbIV);

    public override void GenerateIV()
    {
        this.rng ??= new Csrng();
        this.IV = GetRandomBytes(this.rng, this.BlockSize / 8);
    }

    public override void GenerateKey()
    {
        this.rng ??= new Csrng();
        this.Key = GetRandomBytes(this.rng, this.KeySize / 8);
    }

    private static byte[] GetRandomBytes(RandomNumberGenerator rng, int byteCount)
    {
        byte[] bytes = new byte[byteCount];
        rng.GetBytes(bytes);
        return bytes;
    }

    private class Salsa20CryptoTransform : ICryptoTransform
    {
        // https://dotnetfiddle.net/Bh4ijW
        private static readonly uint[] Sigma = { 0x61707865, 0x3320646E, 0x79622D32, 0x6B206574 };

        private static readonly uint[] Tau = { 0x61707865, 0x3120646E, 0x79622D36, 0x6B206574 };

        private readonly byte[] bitSet = new byte[64];

        private readonly uint[] state;

        private readonly uint[] stateBuffer = new uint[16];

        private readonly bool skipXor;

        private readonly int rounds;

        private bool isDisposed;

        private int bytesRemaining;

        public Salsa20CryptoTransform(byte[] key, byte[] iv, int rounds, bool skipXor)
        {
            this.state = CreateState(key, iv);
            this.rounds = rounds;
            this.skipXor = skipXor;
        }

        ~Salsa20CryptoTransform()
        {
            this.Dispose(false);
        }

        public bool CanReuseTransform
        {
            get
            {
                return false;
            }
        }

        public bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        public int InputBlockSize
        {
            get
            {
                return 64;
            }
        }

        public int OutputBlockSize
        {
            get
            {
                return 64;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int TransformBlock(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount,
            byte[] outputBuffer,
            int outputOffset)
        {
            this.CheckDisposed();

            int bytesTransformed = 0;

            while (inputCount > 0)
            {
                if (this.bytesRemaining == 0)
                {
                    AddRotateXor(this.rounds, this.state, this.stateBuffer, this.bitSet);
                    this.bytesRemaining = 64;
                }

                var length = Math.Min(this.bytesRemaining, inputCount);
                if (this.skipXor)
                {
                    Array.Copy(this.bitSet, inputOffset, outputBuffer, outputOffset, length);
                }
                else
                {
                    for (int i = 0; i < length; i++)
                        outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ this.bitSet[i]);
                }

                this.bytesRemaining -= length;
                bytesTransformed += length;
                inputCount -= length;
                outputOffset += length;
                inputOffset += length;
            }

            return bytesTransformed;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            this.CheckDisposed();

            byte[] output = new byte[inputCount];
            this.TransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
            return output;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                return;

            Array.Clear(this.stateBuffer, 0, this.stateBuffer.Length);
            Array.Clear(this.state, 0, this.state.Length);
            this.isDisposed = true;
        }

        private static void AddRotateXor(int rounds, uint[] state, uint[] buffer, byte[] output)
        {
            Array.Copy(state, buffer, state.Length);

            // 8 quarter rounds = 2 rounds
            for (var i = rounds; i > 0; i -= 2)
            {
                QuarterRound(buffer, 0, 4, 8, 12); // column 1
                QuarterRound(buffer, 5, 9, 13, 1);
                QuarterRound(buffer, 10, 14, 2, 6);
                QuarterRound(buffer, 15, 3, 7, 11);

                QuarterRound(buffer, 0, 1, 2, 3); // column 1
                QuarterRound(buffer, 5, 6, 7, 4);
                QuarterRound(buffer, 10, 11, 8, 9);
                QuarterRound(buffer, 15, 12, 13, 14);

                /*
                buffer[4] ^= BitOperations.RotateLeft(buffer[0] + buffer[12], 7);
                buffer[8] ^= BitOperations.RotateLeft(buffer[4] + buffer[0], 9);
                buffer[12] ^= BitOperations.RotateLeft(buffer[8] + buffer[4], 13);
                buffer[0] ^= BitOperations.RotateLeft(buffer[12] + buffer[8], 18);

                buffer[9] ^= BitOperations.RotateLeft(buffer[5] + buffer[1], 7);
                buffer[13] ^= BitOperations.RotateLeft(buffer[9] + buffer[5], 9);
                buffer[1] ^= BitOperations.RotateLeft(buffer[13] + buffer[9], 13);
                buffer[5] ^= BitOperations.RotateLeft(buffer[1] + buffer[13], 18);

                buffer[14] ^= BitOperations.RotateLeft(buffer[10] + buffer[6], 7);
                buffer[2] ^= BitOperations.RotateLeft(buffer[14] + buffer[10], 9);
                buffer[6] ^= BitOperations.RotateLeft(buffer[2] + buffer[14], 13);
                buffer[10] ^= BitOperations.RotateLeft(buffer[6] + buffer[2], 18);

                buffer[3] ^= BitOperations.RotateLeft(buffer[15] + buffer[11], 7);
                buffer[7] ^= BitOperations.RotateLeft(buffer[3] + buffer[15], 9);
                buffer[11] ^= BitOperations.RotateLeft(buffer[7] + buffer[3], 13);
                buffer[15] ^= BitOperations.RotateLeft(buffer[11] + buffer[7], 18);

                buffer[1] ^= BitOperations.RotateLeft(buffer[0] + buffer[3], 7);
                buffer[2] ^= BitOperations.RotateLeft(buffer[1] + buffer[0], 9);
                buffer[3] ^= BitOperations.RotateLeft(buffer[2] + buffer[1], 13);
                buffer[0] ^= BitOperations.RotateLeft(buffer[3] + buffer[2], 18);

                buffer[6] ^= BitOperations.RotateLeft(buffer[5] + buffer[4], 7);
                buffer[7] ^= BitOperations.RotateLeft(buffer[6] + buffer[5], 9);
                buffer[4] ^= BitOperations.RotateLeft(buffer[7] + buffer[6], 13);
                buffer[5] ^= BitOperations.RotateLeft(buffer[4] + buffer[7], 18);

                buffer[11] ^= BitOperations.RotateLeft(buffer[10] + buffer[9], 7);
                buffer[8] ^= BitOperations.RotateLeft(buffer[11] + buffer[10], 9);
                buffer[9] ^= BitOperations.RotateLeft(buffer[8] + buffer[11], 13);
                buffer[10] ^= BitOperations.RotateLeft(buffer[9] + buffer[8], 18);

                buffer[12] ^= BitOperations.RotateLeft(buffer[15] + buffer[14], 7);
                buffer[13] ^= BitOperations.RotateLeft(buffer[12] + buffer[15], 9);
                buffer[14] ^= BitOperations.RotateLeft(buffer[13] + buffer[12], 13);
                buffer[15] ^= BitOperations.RotateLeft(buffer[14] + buffer[13], 18);
                */
            }

            for (int i = 0; i < 16; i++)
            {
                buffer[i] += state[i];
                var index = i << 2;
                var value = buffer[i];

                output[index] = (byte)value;
                output[index + 1] = (byte)(value >> 8);
                output[index + 2] = (byte)(value >> 16);
                output[index + 3] = (byte)(value >> 24);
            }

            state[8]++;
            if (state[8] == 0)
                state[9]++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("Major Bug", "S4143:Collection elements should not be replaced unconditionally")]
        private static void QuarterRound(uint[] set, uint a, uint b, uint c, uint d)
        {
            set[b] ^= BitOperations.RotateLeft(unchecked(set[a] + set[d]), 7);
            set[c] ^= BitOperations.RotateLeft(unchecked(set[b] + set[a]), 9);
            set[d] ^= BitOperations.RotateLeft(unchecked(set[c] + set[b]), 13);
            set[a] ^= BitOperations.RotateLeft(unchecked(set[d] + set[c]), 18);
        }

        private static uint[] CreateState(byte[] key, byte[] iv)
        {
            int offset = key.Length - 16;
            uint[] expand = key.Length == 32 ? Sigma : Tau;
            uint[] state = new uint[16];

            var keySpan = key.AsSpan();
            var ivSpan = iv.AsSpan();

            // key
            state[1] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(0, 4));
            state[2] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(4, 4));
            state[3] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(8, 4));
            state[4] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(12, 4));

            // key offset
            state[11] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset, 4));
            state[12] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset + 4, 4));
            state[13] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset + 8, 4));
            state[14] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset + 12, 4));

            // sigma / tau
            state[0] = expand[0];
            state[5] = expand[1];
            state[10] = expand[2];
            state[15] = expand[3];

            state[6] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(0, 4));
            state[7] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(4, 4));
            state[8] = 0;
            state[9] = 0;

            return state;
        }

        private void CheckDisposed()
        {
            if (this.isDisposed)
                throw new ObjectDisposedException("ICryptoTransform is already disposed");
        }
    }
}