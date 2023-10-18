using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Security.Cryptography
{
    [SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed")]
    public class MemoryProtectedText : MemoryProtectedBytes,
        IEquatable<MemoryProtectedText>,
        IEquatable<string>,
        IEquatable<byte[]>,
        IComparable<MemoryProtectedText>
    {
        private string? text;
        private bool isDisposed;
        private Encoding encoding;

        public MemoryProtectedText()
        {
            this.encoding = Encodings.Utf8NoBom;
            this.Length = 0;
        }

        public MemoryProtectedText(SecureString secureString)
            : this(secureString, Encodings.Utf8NoBom)
        {
        }

        public MemoryProtectedText(string value)
            : this(value, Encodings.Utf8NoBom)
        {
        }

        public MemoryProtectedText(SecureString secureString, Encoding encoding)
        {
            if (secureString is null)
                throw new ArgumentNullException(nameof(secureString));

            this.encoding = encoding;
            var bytes = secureString.ToBytes(this.encoding);
            this.Init(bytes, false);
            this.Length = secureString.Length;
        }

        public MemoryProtectedText(string value, Encoding encoding)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            this.text = value;
            this.encoding = encoding;
            var bytes = this.encoding.GetBytes(value);
            this.Init(bytes, false);
            this.Length = value.Length;
        }

        public MemoryProtectedText(char[] chars, Encoding encoding, bool encrypt)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));

            this.encoding = encoding;
            var bytes = this.encoding.GetBytes(chars);
            this.Init(bytes, encrypt);
            this.Length = chars.Length;
        }

        public MemoryProtectedText(byte[] bytes, Encoding encoding, bool encrypt)
            : base(bytes, encrypt)
        {
            this.encoding = encoding;
            this.Length = this.encoding.GetCharCount(bytes);
            this.Init(bytes, encrypt);
        }

        public MemoryProtectedText(ReadOnlySpan<byte> bytes, Encoding encoding, bool encrypt)
            : base(bytes, encrypt)
        {
            var byteArray = bytes.ToArray();
            this.encoding = encoding;
            this.Length = this.encoding.GetCharCount(byteArray);
            this.Init(byteArray, encrypt);
            Array.Clear(byteArray, 0, byteArray.Length);
        }

        public Encoding Encoding => this.encoding;

        public static bool operator ==(MemoryProtectedText left, MemoryProtectedText right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MemoryProtectedText left, MemoryProtectedText right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(MemoryProtectedText left, MemoryProtectedText right)
        {
            return left.CompareTo(right) == 1;
        }

        public static bool operator <(MemoryProtectedText left, MemoryProtectedText right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool operator >=(MemoryProtectedText left, MemoryProtectedText right)
        {
            return left.CompareTo(right) != -1;
        }

        public static bool operator <=(MemoryProtectedText left, MemoryProtectedText right)
        {
            return left.CompareTo(right) != 1;
        }

        public override int GetHashCode()
        {
            return this.Hash.GetHashCode() * 13;
        }

        public ReadOnlySpan<char> ToCharSpan()
            => this.ToCharSpan(Encodings.Utf8NoBom);

        // ReSharper disable once ParameterHidesMember
        public ReadOnlySpan<char> ToCharSpan(Encoding encoding)
        {
            this.CheckDisposed();
            if (this.text != null)
                return this.text.AsSpan();

            var decrypted = this.Decrypt().ToArray();
            try
            {
                char[] chars = encoding.GetChars(decrypted);
                return chars;
            }
            finally
            {
                Array.Clear(decrypted, 0, decrypted.Length);
            }
        }

        public ReadOnlySpan<byte> ToByteSpan()
        {
            this.CheckDisposed();

            return this.Decrypt().ToArray();
        }

        public string ToString(bool unprotect)
            => this.ToString(unprotect, Encodings.Utf8NoBom);

        // ReSharper disable once ParameterHidesMember
        public string ToString(bool unprotect, Encoding encoding)
        {
            this.CheckDisposed();

            if (!unprotect)
                return this.ToString();

            if (this.text != null)
                return this.text;

            var decrypted = this.Decrypt().ToArray();
            try
            {
                this.encoding = encoding;
                this.text = encoding.GetString(decrypted);
                this.IsProtected = false;
                return this.text;
            }
            finally
            {
                Array.Clear(decrypted, 0, decrypted.Length);
            }
        }

        public override string ToString()
        {
            return "********************";
        }

        public SecureString ToSecureString()
        {
            this.CheckDisposed();

            var set = new char[this.Length];
            this.CopyTo(set);
            var ss = new SecureString();
            foreach (var c in set)
                ss.AppendChar(c);

            Array.Clear(set, 0, set.Length);

            return ss;
        }

        public void CopyTo(char[] array)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));

            this.CheckDisposed();

            if (this.text != null)
            {
                var l = Math.Min(this.Length, array.Length);
                this.text.CopyTo(0, array, 0, l);
                return;
            }

            var decrypted = this.Decrypt().ToArray();
            var chars = this.GetEncoding().GetChars(decrypted);
            chars.CopyTo(array, 0);
            Array.Clear(decrypted, 0, decrypted.Length);
            Array.Clear(chars, 0, chars.Length);
        }

        public int CompareTo(MemoryProtectedText? other)
        {
            this.CheckDisposed();

            if (other is null)
                return 1;

            int c = this.Length.CompareTo(other.Length);
            if (c != 0)
                return c;

            if (this.text != null && other.text != null)
                return string.CompareOrdinal(this.text, other.text);

            // use hashes to avoid decrypting when possible.
            if (this.Hash.Equals(other.Hash))
                return 0;

            var a = new char[this.Length];
            var b = new char[this.Length];
            this.CopyTo(a);
            other.CopyTo(b);

            try
            {
                for (var i = 0; i < this.Length; i++)
                {
                    char l = a[i];
                    char r = b[i];
                    if (l == r)
                        continue;

                    if (l > r)
                        return 1;

                    return -1;
                }

                return 0;
            }
            finally
            {
                Array.Clear(a, 0, a.Length);
                Array.Clear(b, 0, a.Length);
            }
        }

        public override bool Equals(object? obj)
        {
            this.CheckDisposed();

            if (obj is MemoryProtectedText text1)
                return this.Equals(text1);

            return false;
        }

        public bool Equals(byte[]? other)
        {
            this.CheckDisposed();

            if (other is null)
                return false;

            var length = this.GetEncoding().GetCharCount(other);
            if (this.Length != length)
                return false;

            if (this.text != null)
            {
                var bytes = this.GetEncoding().GetBytes(this.text);
                return bytes.SequenceEqual(other);
            }

            var hash = EncryptionUtil.ComputeChecksum(
                other,
                HashAlgorithmName.SHA256);

            return hash.AsSpan().SequenceEqual(this.Hash.Span);
        }

        public bool Equals(MemoryProtectedText? other)
        {
            this.CheckDisposed();

            if (other is null)
                return false;

            if (this.Hash.Equals(other.Hash))
                return true;

            if (this.text != null && other.text != null)
                return this.text == other.text;

            return this.Equals((MemoryProtectedBytes)other);
        }

        public bool Equals(string? other)
        {
            this.CheckDisposed();

            if (other is null)
                return false;

            if (this.Length != other.Length)
                return false;

            if (this.text != null)
                return this.text == other;

            var bytes = this.GetEncoding().GetBytes(other);
            var hash = EncryptionUtil.ComputeChecksum(
                bytes,
                HashAlgorithmName.SHA256);

            return hash.AsSpan().SequenceEqual(this.Hash.Span);
        }

        protected override void CheckDisposed()
        {
            if (this.isDisposed)
                throw new ObjectDisposedException($"{nameof(MemoryProtectedText)} - {this.Id}");
        }

        protected override void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;

            if (disposing)
            {
                this.text = string.Empty;
            }

            this.isDisposed = true;
            base.Dispose(disposing);
        }

        private Encoding GetEncoding() => this.encoding;
    }
}