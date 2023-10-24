namespace GnomeStack.KeePass.Package;

public enum KpHeaderFields : byte
{
    EndOfHeader = 0,
    Comment = 1,

    /// <summary>
    /// The UUID of the Cipher for the database.
    /// </summary>
    CipherId = 2,

    /// <summary>
    /// The database compression type (normal or gzipped).
    /// </summary>
    CompressionType = 3,

    /// <summary>
    /// The seed used to generate the Cipher key.
    /// </summary>
    CipherKeySeed = 4,

    /// <summary>
    /// The set used to generate bytes for the Cipher key.
    /// </summary>
    AesKeySeed = 5,

    /// <summary>
    /// The number of iterations for the Encryption engine to execute.
    /// </summary>
    AesIterations = 6,
    CipherIV = 7,
    CsrngKey = 8,
    HeaderByteMark = 9,
    CsrngType = 10,
    DerivedKeyParams = 11,
    KpMap = 12,
}

// version 4 or greater.
public enum KpRngHeaderFields : byte
{
    EndOfHeader = 0,
    RngId = 1,
    RngKey = 2,
    ProtectedFlags = 3,
}

public enum ProtectionType : byte
{
    None = 0,
    Protected = 1,
}