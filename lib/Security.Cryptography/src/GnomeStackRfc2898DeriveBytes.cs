// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
#pragma warning disable SA1405

namespace GnomeStack.Security.Cryptography
{
    /// <summary>
    /// Implementation of PBKDF2 that allows a <see cref="HashAlgorithmName" />
    /// to be provided that solves the issue for FxCop warning CA5379, which is
    /// around the limitation of SHA1 for <see cref="System.Security.Cryptography.Rfc2898DeriveBytes" />.
    /// </summary>
    public class GnomeStackRfc2898DeriveBytes : DeriveBytes
    {
        private const int MinimumSaltSize = 8;

        private readonly int blockSize;

        private readonly byte[] password;

        private readonly HMAC hmac;

        private byte[] salt;

        private uint iterations;

        private byte[] buffer;

        private uint block;

        private int startIndex;

        private int endIndex;

        public GnomeStackRfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
            : this(password, salt, iterations, HashAlgorithmName.SHA1)
        {
        }

        public GnomeStackRfc2898DeriveBytes(
            ReadOnlySpan<byte> password,
            ReadOnlySpan<byte> salt,
            int iterations,
            HashAlgorithmName hashAlgorithm)
        {
            if (salt.Length < MinimumSaltSize)
                throw new ArgumentException($"Salt minimum length is {MinimumSaltSize}", nameof(salt));

            if (iterations <= 0)
                throw new ArgumentOutOfRangeException(nameof(iterations), "iterations must be a positive number");

            this.salt = new byte[salt.Length + sizeof(uint)];
            salt.CopyTo(this.salt);
            this.iterations = (uint)iterations;

            this.password = new byte[password.Length];
            password.CopyTo(this.password);

            this.HashAlgorithm = hashAlgorithm;
            this.hmac = this.OpenHmac();

            // this.blockSize is in bytes, HashSize is in bits.
            this.blockSize = this.hmac.HashSize >> 3;
            this.buffer = Array.Empty<byte>();

            this.Initialize();
        }

        public GnomeStackRfc2898DeriveBytes(byte[] password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm)
        {
            if (salt == null)
                throw new ArgumentNullException(nameof(salt));

            if (salt.Length < MinimumSaltSize)
                throw new ArgumentException($"salt minimum length is {MinimumSaltSize}", nameof(salt));

            if (iterations <= 0)
                throw new ArgumentOutOfRangeException(nameof(iterations), "iterations must be a positive number");

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            this.salt = new byte[salt.Length + sizeof(uint)];
            salt.AsSpan().CopyTo(this.salt);
            this.iterations = (uint)iterations;

            this.password = (byte[])password.Clone();

            this.HashAlgorithm = hashAlgorithm;
            this.hmac = this.OpenHmac();

            // this.blockSize is in bytes, HashSize is in bits.
            this.blockSize = this.hmac.HashSize >> 3;
            this.buffer = Array.Empty<byte>();
            this.Initialize();
        }

        public GnomeStackRfc2898DeriveBytes(string password, byte[] salt)
             : this(password, salt, 1000)
        {
        }

        public GnomeStackRfc2898DeriveBytes(string password, byte[] salt, int iterations)
            : this(password, salt, iterations, HashAlgorithmName.SHA1)
        {
        }

        public GnomeStackRfc2898DeriveBytes(string password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm)
            : this(Encoding.UTF8.GetBytes(password), salt, iterations, hashAlgorithm)
        {
        }

        public GnomeStackRfc2898DeriveBytes(string password, int saltSize)
            : this(password, saltSize, 1000)
        {
        }

        public GnomeStackRfc2898DeriveBytes(string password, int saltSize, int iterations)
            : this(password, saltSize, iterations, HashAlgorithmName.SHA1)
        {
        }

        public GnomeStackRfc2898DeriveBytes(string password, int saltSize, int iterations, HashAlgorithmName hashAlgorithm)
        {
            switch (saltSize)
            {
                case < 0:
                    throw new ArgumentOutOfRangeException(nameof(saltSize), "saltSize must a positive number");
                case < MinimumSaltSize:
                    throw new ArgumentException($"salt minimum length is {MinimumSaltSize}", nameof(saltSize));
            }

            if (iterations <= 0)
                throw new ArgumentOutOfRangeException(nameof(iterations), "iterations must be a positive number");

            this.salt = new byte[saltSize + sizeof(uint)];
            using (var rng = new Csrng())
            {
                rng.GetBytes(this.salt);
            }

            this.iterations = (uint)iterations;
            this.password = Encoding.UTF8.GetBytes(password);
            this.HashAlgorithm = hashAlgorithm;
            this.hmac = this.OpenHmac();

            // this.blockSize is in bytes, HashSize is in bits.
            this.blockSize = this.hmac.HashSize >> 3;
            this.buffer = Array.Empty<byte>();
            this.Initialize();
        }

        public HashAlgorithmName HashAlgorithm { get; }

        public int IterationCount
        {
            get => (int)this.iterations;

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "value must a positive number");
                this.iterations = (uint)value;

                this.Initialize();
            }
        }

        public byte[] Salt
        {
            get => this.salt.AsSpan(0, this.salt.Length - sizeof(uint)).ToArray();

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Length < MinimumSaltSize)
                    throw new ArgumentException($"salt is must be {MinimumSaltSize}");

                this.salt = new byte[value.Length + sizeof(uint)];
                value.AsSpan().CopyTo(this.salt);
                this.Initialize();
            }
        }

        public override byte[] GetBytes(int cb)
        {
            Debug.Assert(this.blockSize > 0);

            if (cb <= 0)
                throw new ArgumentOutOfRangeException(nameof(cb), "cb must be positive number");
            var nextBytes = new byte[cb];

            var offset = 0;
            var size = this.endIndex - this.startIndex;
            if (size > 0)
            {
                if (cb >= size)
                {
                    Buffer.BlockCopy(this.buffer, this.startIndex, nextBytes, 0, size);
                    this.startIndex = this.endIndex = 0;
                    offset += size;
                }
                else
                {
                    Buffer.BlockCopy(this.buffer, this.startIndex, nextBytes, 0, cb);
                    this.startIndex += cb;
                    return nextBytes;
                }
            }

            Debug.Assert(this.startIndex == 0 && this.endIndex == 0, "Invalid start or end index in the internal buffer.");

            while (offset < cb)
            {
                this.Func();
                var remainder = cb - offset;
                if (remainder >= this.blockSize)
                {
                    Buffer.BlockCopy(this.buffer, 0, nextBytes, offset, this.blockSize);
                    offset += this.blockSize;
                }
                else
                {
                    Buffer.BlockCopy(this.buffer, 0, nextBytes, offset, remainder);
                    this.startIndex = remainder;
                    this.endIndex = this.buffer.Length;
                    return nextBytes;
                }
            }

            return nextBytes;
        }

        [SuppressMessage("", "IDE0060", Justification = "Required")]
        public byte[] CryptDeriveKey(string algname, string alghashname, int keySize, byte[] rgbIV)
        {
            // If this were to be implemented here, CAPI would need to be used (not CNG) because of
            // unfortunate differences between the two. Using CNG would break compatibility. Since this
            // assembly currently doesn't use CAPI it would require non-trivial additions.
            // In addition, if implemented here, only Windows would be supported as it is intended as
            // a thin wrapper over the corresponding native API.
            // Note that this method is implemented in PasswordDeriveBytes (in the Csp assembly) using CAPI.
            throw new PlatformNotSupportedException();
        }

        public override void Reset()
        {
            this.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.hmac.Dispose();
                Array.Clear(this.buffer, 0, this.buffer.Length);
                Array.Clear(this.password, 0, this.password.Length);
                Array.Clear(this.salt, 0, this.salt.Length);
            }

            base.Dispose(disposing);
        }

        private static void WriteInt(uint i, byte[] arr, int offset)
        {
            #pragma warning disable CS8602, S2259
            unchecked
            {
                Debug.Assert(arr != null);
                Debug.Assert(arr.Length >= offset + sizeof(uint));

                arr[offset] = (byte)(i >> 24);
                arr[offset + 1] = (byte)(i >> 16);
                arr[offset + 2] = (byte)(i >> 8);
                arr[offset + 3] = (byte)i;
            }
        }

        private HMAC OpenHmac()
        {
            Debug.Assert(this.password != null);

            HashAlgorithmName hashAlgorithm = this.HashAlgorithm;

            if (string.IsNullOrEmpty(hashAlgorithm.Name))
                throw new CryptographicException("HashAlgorithm is null or empty");

            if (hashAlgorithm == HashAlgorithmName.SHA1)
                return new HMACSHA1(this.password);
            if (hashAlgorithm == HashAlgorithmName.SHA256)
                return new HMACSHA256(this.password);
            if (hashAlgorithm == HashAlgorithmName.SHA384)
                return new HMACSHA384(this.password);
            if (hashAlgorithm == HashAlgorithmName.SHA512)
                return new HMACSHA512(this.password);

            throw new CryptographicException($"Unknown Algorithm {hashAlgorithm.Name}");
        }

        private void Initialize()
        {
            Array.Clear(this.buffer, 0, this.buffer.Length);

            this.buffer = new byte[this.blockSize];
            this.block = 1;
            this.startIndex = this.endIndex = 0;
        }

#if NETLEGACY
        private void Func()
        {
            WriteInt(this.block, this.salt, this.salt.Length - sizeof(uint));
            byte[] temp = this.hmac.ComputeHash(this.salt);
            Debug.Assert(temp.Length == this.blockSize);

            Buffer.BlockCopy(temp, 0, this.buffer, 0, temp.Length);

            for (int i = 2; i <= this.iterations; i++)
            {
                temp = this.hmac.ComputeHash(temp);
                Debug.Assert(temp.Length == this.blockSize);

                for (var j = this.buffer.Length - 1; j >= 0; j--)
                {
                    this.buffer[j] ^= temp[j];
                }
            }

            // increment the block count.
            this.block++;
        }
#else
        private void Func()
        {
            WriteInt(this.block, this.salt, this.salt.Length - sizeof(uint));
            Debug.Assert(this.blockSize == this.buffer.Length);

            // The biggest this.blockSize we have is from SHA512, which is 64 bytes.
            // Since we have a closed set of supported hash algorithms (OpenHmac())
            // we can know this always fits.
            Span<byte> uiSpan = stackalloc byte[64];
            uiSpan = uiSpan.Slice(0, this.blockSize);

            if (!this.hmac.TryComputeHash(this.salt, uiSpan, out int bytesWritten) || bytesWritten != this.blockSize)
            {
                throw new CryptographicException();
            }

            uiSpan.CopyTo(this.buffer);

            for (var i = 2; i <= this.iterations; i++)
            {
                if (!this.hmac.TryComputeHash(uiSpan, uiSpan, out bytesWritten) || bytesWritten != this.blockSize)
                {
                    throw new CryptographicException();
                }

                for (var j = this.buffer.Length - 1; j >= 0; j--)
                {
                    this.buffer[j] ^= uiSpan[j];
                }
            }

            // increment the block count.
            this.block++;
        }
#endif
    }
}