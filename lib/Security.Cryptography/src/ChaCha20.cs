using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace GnomeStack.Security.Cryptography
{
    public sealed class ChaCha20 : SymmetricAlgorithm
    {
        private static readonly KeySizes[] ChaChaLegalBlockSizes = new[] { new KeySizes(64, 64, 0) };
        private static readonly KeySizes[] ChaChaLegalKeySizes = new[] { new KeySizes(128, 256, 128) };
        private readonly Csrng rng;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ChaCha20"/> class.
        /// </summary>
        public ChaCha20()
        {
#if NETSTANDARD2_0 || NETFRAMEWORK
            this.LegalBlockSizesValue = ChaChaLegalBlockSizes;
            this.LegalKeySizesValue = ChaChaLegalKeySizes;
#endif
            this.BlockSize = 64;
            this.KeySize = 256;
            this.rng = new Csrng();
        }

        /// <summary>
        /// Gets the block sizes, in bits, that are supported by the symmetric algorithm.
        /// </summary>
        public override KeySizes[] LegalBlockSizes
        {
            get
            {
                return ChaChaLegalBlockSizes;
            }
        }

        /// <summary>
        /// Gets the key sizes, in bits, that are supported by the symmetric algorithm.
        /// </summary>
        public override KeySizes[] LegalKeySizes
        {
            get
            {
                return ChaChaLegalKeySizes;
            }
        }

        public bool SkipXor { get; set; }

        public int Counter { get; set; }

        public ChaChaRound Rounds { get; set; } = ChaChaRound.Twenty;

#pragma warning disable CS0109

        /// <summary>
        /// Creates a new instance of <see cref="GnomeStack.Security.Cryptography.ChaCha20" /> class.
        /// </summary>
        /// <returns>A new instance of <see cref="ChaCha20"/>.</returns>
        public static new ChaCha20 Create()
        {
            return new ChaCha20();
        }
#pragma warning restore CS0109

        /// <summary>
        /// Creates a symmetric decryptor object with the <paramref name="rgbKey"/> and initialization vector <paramref name="rgbIV"/>.
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric decryptor object.</returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            if (rgbIV is null)
            {
                this.GenerateIV();
                rgbIV = this.IV;
            }

            return new ChaCha20Transform(rgbKey, rgbIV, this.Rounds, this.SkipXor, this.Counter);
        }

        /// <summary>
        /// Creates a symmetric encryptor object with the <paramref name="rgbKey"/> and initialization vector <paramref name="rgbIV"/>.
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric encryptor object.</returns>
        [SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations", Justification = "Both are the same")]
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            if (rgbIV is null)
            {
                this.GenerateIV();
                rgbIV = this.IV;
            }

            return new ChaCha20Transform(rgbKey, rgbIV, this.Rounds, this.SkipXor, this.Counter);
        }

        public override void GenerateIV()
        {
            this.IV = GetRandomBytes(this.rng, this.BlockSize / 8);
        }

        public override void GenerateKey()
        {
            this.Key = GetRandomBytes(this.rng, this.KeySize / 8);
        }

        private static byte[] GetRandomBytes(Csrng rng, int byteCount)
        {
            return rng.NextBytes(byteCount);
        }
    }
}