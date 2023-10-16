using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography
{
    /// <summary>
    /// The ChaCha20 implementation.
    /// </summary>
    /// <remarks>https://cr.yp.to/chacha/chacha-20080128.pdf .</remarks>
    internal class ChaCha20Transform : ICryptoTransform
    {
        // https://dotnetfiddle.net/Bh4ijW
        private static readonly uint[] Sigma = new uint[] { 0x61707865, 0x3320646E, 0x79622D32, 0x6B206574 };

        private static readonly uint[] Tau = new uint[] { 0x61707865, 0x3120646E, 0x79622D36, 0x6B206574 };

        private readonly int rounds;

        private readonly bool skipXor;

        private readonly uint[] state;

        private readonly uint[] stateBuffer = new uint[16];

        private readonly byte[] bitSet = new byte[64];

        private bool isDisposed;

        private int bytesRemaining;

        public ChaCha20Transform(byte[] key, byte[] iv, ChaChaRound rounds, bool skipXor = false, int counter = 0)
        {
            this.skipXor = skipXor;
            this.rounds = (int)rounds;
            this.state = CreateState(key, iv, counter);
        }

        ~ChaCha20Transform()
        {
            this.Dispose(false);
        }

        public bool CanReuseTransform => false;

        public bool CanTransformMultipleBlocks => true;

        public int InputBlockSize => 64;

        public int OutputBlockSize => 64;

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            this.CheckDisposed();

            int bytesTransformed = 0;

            while (inputCount > 0)
            {
                if (this.bytesRemaining == 0)
                {
                    AddXorRotate(this.rounds, this.state, this.stateBuffer, this.bitSet);
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static uint[] CreateState(byte[] key, byte[] iv, int counter)
        {
            var state = new uint[16];
            var constants = key.Length == 32 ? Sigma : Tau;
            var offset = key.Length - 16;

            state[0] = constants[0];
            state[1] = constants[1];
            state[2] = constants[2];
            state[3] = constants[3];

            var keySpan = key.AsSpan();
            var ivSpan = iv.AsSpan();

            state[4] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(0, 4));
            state[5] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(4, 4));
            state[6] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(8, 4));
            state[7] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(12, 4));

            state[8] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset, 4));
            state[9] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset + 4, 4));
            state[10] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset + 8, 4));
            state[11] = BinaryPrimitives.ReadUInt32LittleEndian(keySpan.Slice(offset + 12, 4));

            state[12] = (uint)counter;
            switch (iv.Length)
            {
                // the test cases only uses 8
                // https://github.com/secworks/chacha_testvectors/blob/master/src/gen_chacha_testvectors.c
                case 8:
                    state[13] = 0;
                    state[14] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(0, 4));
                    state[15] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(4, 4));
                    break;
                case 12:
                    state[13] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(0, 4));
                    state[14] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(4, 4));
                    state[15] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(8, 4));
                    break;
                case 16:
                    // overwrites the counter
                    state[12] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(0, 4));
                    state[13] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(4, 4));
                    state[14] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(8, 4));
                    state[15] = BinaryPrimitives.ReadUInt32LittleEndian(ivSpan.Slice(12, 4));
                    break;
            }

            return state;
        }

        internal static void AddXorRotate(int rounds, uint[] state, uint[] buffer, byte[] output)
        {
            Array.Copy(state, buffer, state.Length);

            for (int i = rounds; i > 0; i -= 2)
            {
                QuarterRound(buffer, 0, 4, 8, 12);
                QuarterRound(buffer, 1, 5, 9, 13);
                QuarterRound(buffer, 2, 6, 10, 14);
                QuarterRound(buffer, 3, 7, 11, 15);

                QuarterRound(buffer, 0, 5, 10, 15);
                QuarterRound(buffer, 1, 6, 11, 12);
                QuarterRound(buffer, 2, 7, 8, 13);
                QuarterRound(buffer, 3, 4, 9, 14);
            }

            for (int i = 16; i-- > 0;)
            {
                buffer[i] += state[i];

                // converts unit to little endian using an offset.
                output[i << 2] = (byte)buffer[i];
                output[(i << 2) + 1] = (byte)(buffer[i] >> 8);
                output[(i << 2) + 2] = (byte)(buffer[i] >> 16);
                output[(i << 2) + 3] = (byte)(buffer[i] >> 24);
            }

            state[12] = unchecked(state[12] + 1);
            if (state[12] <= 0)
            {
                state[13] = unchecked(state[13] + 1);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;

            if (disposing)
            {
                this.isDisposed = true;
                Array.Clear(this.bitSet, 0, this.bitSet.Length);
                Array.Clear(this.state, 0, this.state.Length);
                Array.Clear(this.stateBuffer, 0, this.stateBuffer.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("Major Bug", "S4143:Collection elements should not be replaced unconditionally")]
        private static void QuarterRound(uint[] set, uint a, uint b, uint c, uint d)
        {
            set[a] = unchecked(set[a] + set[b]);
            set[d] = BitOperations.RotateLeft(set[d] ^ set[a], 16);

            set[c] = unchecked(set[c] + set[d]);
            set[b] = BitOperations.RotateLeft(set[b] ^ set[c], 12);

            set[a] = unchecked(set[a] + set[b]);
            set[d] = BitOperations.RotateLeft(set[d] ^ set[a], 8);

            set[c] = unchecked(set[c] + set[d]);
            set[b] = BitOperations.RotateLeft(set[b] ^ set[c], 7);
        }

        private void CheckDisposed()
        {
            if (this.isDisposed)
                throw new ObjectDisposedException("ICryptoTransform is already disposed");
        }
    }
}