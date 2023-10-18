// ReSharper disable InconsistentNaming
#pragma warning disable CA1028

namespace GnomeStack.Security.Cryptography
{
    public enum KeyedHashAlgorithmType : short
    {
        /// <summary>No Algorithm.</summary>
        None = 0,

        /// <summary>HMAC MD5 Algorithm.</summary>
        HMACMD5 = 1,

        /// <summary>HMAC RIPEMD 160 Algorithm.</summary>
        HMACRIPEMD160 = 2,

        /// <summary>HMAC SHA 1 Algorithm.</summary>
        HMACSHA1 = 3,

        /// <summary>HMAC SHA 256 Algorithm.</summary>
        HMACSHA256 = 4,

        /// <summary>HMAC SHA 386 Algorithm.</summary>
        HMACSHA384 = 5,

        /// <summary>HMAC SHA 512 Algorithm.</summary>
        HMACSHA512 = 6,
    }
}