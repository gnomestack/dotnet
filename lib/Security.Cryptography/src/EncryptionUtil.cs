using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Security.Cryptography
{
    public static class EncryptionUtil
    {
        public static ReadOnlySpan<byte> ComputeChecksum(
            this Stream stream,
            HashAlgorithmName name,
            bool resetStream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            using (var algo = CreateHashAlgorithm(name))
            {
                var result = algo.ComputeHash(stream);

                if (resetStream && stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);

                return result;
            }
        }

        public static byte[] ComputeChecksum(
            this byte[] bytes,
            HashAlgorithmName name)
        {
            using var algo = CreateHashAlgorithm(name);
            var result = algo.ComputeHash(bytes);
            Array.Clear(bytes, 0, bytes.Length);
            return result;
        }

        public static ReadOnlySpan<byte> ComputeChecksum(
            this ReadOnlySpan<byte> bytes,
            HashAlgorithmName name)
        {
            using var algo = CreateHashAlgorithm(name);
#if NETSTANDARD2_0 || NETFRAMEWORK
            var array = bytes.ToArray();
            var result = algo.ComputeHash(array);
            Array.Clear(array, 0, array.Length);
            return result;
#else
            var size = algo.HashSize >> 3;
            Span<byte> uiSpan = stackalloc byte[64];
            uiSpan = uiSpan.Slice(0, size);
            if (!algo.TryComputeHash(bytes, uiSpan, out int bytesWritten) || bytesWritten != size)
            {
                throw new CryptographicException();
            }

            return uiSpan.ToArray();
#endif
        }

        public static ReadOnlyMemory<byte> ComputeChecksum(
            this ReadOnlyMemory<byte> bytes,
            HashAlgorithmName name)
        {
            if (!MemoryMarshal.TryGetArray(bytes, out ArraySegment<byte> segment) || segment.Array is null)
            {
                throw new CryptographicException("Could not retrieve array segment from memory");
            }

            using var algo = CreateHashAlgorithm(name);
#if NETSTANDARD2_0 || NETFRAMEWORK
            return algo.ComputeHash(segment.Array);
#else
            var size = algo.HashSize >> 3;
            Span<byte> uiSpan = stackalloc byte[64];
            uiSpan = uiSpan.Slice(0, size);
            if (!algo.TryComputeHash(segment.Array, uiSpan, out int bytesWritten) || bytesWritten != size)
            {
                throw new CryptographicException();
            }

            return uiSpan.ToArray();
#endif
        }

        public static SymmetricAlgorithm CreateSymmetricAlgorithm(string symmetricAlgorithm)
        {
            if (symmetricAlgorithm is null)
                throw new ArgumentNullException(nameof(symmetricAlgorithm));

            switch (symmetricAlgorithm.ToUpperInvariant())
            {
                case "AES":
                    return Aes.Create();
                default:
                    throw new SecurityException($"Unsupported symmetric algorithm {symmetricAlgorithm}");
            }
        }

        public static HashAlgorithm CreateHashAlgorithm(this HashAlgorithmName hashAlgorithm)
            => CreateHashAlgorithm(hashAlgorithm.Name!);

        [SuppressMessage("Security", "SCS0006:Weak hashing function.")]
        public static HashAlgorithm CreateHashAlgorithm(this string hashAlgorithm)
        {
            if (hashAlgorithm is null)
                throw new ArgumentNullException(nameof(hashAlgorithm));

            switch (hashAlgorithm.ToUpperInvariant())
            {
                case "SHA1":
                    return SHA1.Create();

                case "SHA256":
                    return SHA256.Create();

                case "SHA384":
                    return SHA384.Create();

                case "SHA512":
                    return SHA512.Create();

                case "MD5":
                    return MD5.Create();
                default:
                    throw new CryptographicException("Unsupported hash algorithm");
            }
        }

        public static KeyedHashAlgorithm CreateKeyedHashAlgorithm(this KeyedHashAlgorithmType type)
            => CreateKeyedHashAlgorithm(type.ToString());

        public static KeyedHashAlgorithm CreateKeyedHashAlgorithm(string keyedHashAlgorithm)
        {
            if (keyedHashAlgorithm is null)
                throw new ArgumentNullException(nameof(keyedHashAlgorithm));

            switch (keyedHashAlgorithm.ToUpperInvariant())
            {
                case "HMACMD5":
                    return new HMACMD5();
                case "HMACSHA1":
                    return new HMACSHA1();
                case "HMACSHA256":
                    return new HMACSHA256();
                case "HMACSHA384":
                    return new HMACSHA384();
                case "HMACSHA512":
                    return new HMACSHA512();

                default:
                    throw new SecurityException($"Unable to create keyed algorithm {keyedHashAlgorithm}");
            }
        }

        public static bool SlowEquals(IList<byte> left, IList<byte> right)
        {
            uint diff = (uint)left.Count ^ (uint)right.Count;
            for (int i = 0; i < left.Count; i++)
            {
                diff |= (uint)(left[i] ^ right[i]);
            }

            return diff == 0;
        }

        public static bool SlowEquals(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            uint diff = (uint)left.Length ^ (uint)right.Length;
            for (int i = 0; i < left.Length; i++)
            {
                diff |= (uint)(left[i] ^ right[i]);
            }

            return diff == 0;
        }

        public static byte[] CreateOutputBuffer(byte[] inputBuffer, int blockSize)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer));

            var l = inputBuffer.Length;
            var actualBlockSize = blockSize / 8;
            var pad = l % actualBlockSize;
            if (pad != 0)
            {
                return new byte[l + (actualBlockSize - pad)];
            }

            return new byte[l];
        }

        public static byte[] ToBytes(this SecureString secureString, Encoding? encoding = null)
        {
            if (secureString is null)
                throw new ArgumentNullException(nameof(secureString));

            var bstr = IntPtr.Zero;
            var charArray = new char[secureString.Length];
            encoding = encoding ?? Encodings.Utf8NoBom;

            try
            {
                bstr = Marshal.SecureStringToBSTR(secureString);
                Marshal.Copy(bstr, charArray, 0, charArray.Length);

                var bytes = encoding.GetBytes(charArray);
                return bytes;
            }
            finally
            {
                if (charArray.Length > 0)
                    Array.Clear(charArray, 0, charArray.Length);

                Marshal.ZeroFreeBSTR(bstr);
            }
        }
    }
}