using System;

namespace GnomeStack.Security.Cryptography
{
    /// <summary>
    /// A delegate for encrypting or decrypting binary data, so that encryption methods can
    /// be swapped out.
    /// </summary>
    /// <param name="bytes">The binary data that will be encrtyped or decrypted.</param>
    /// <param name="state">state that helps with the encryption / decryption process.</param>
    /// <param name="action">The type of action for the delegate to perform.</param>
    /// <returns>Binary data that is encrypted or decrypted.</returns>
    public delegate ReadOnlySpan<byte> MemoryProtectionAction(
        ReadOnlySpan<byte> bytes,
        object state,
        MemoryProtectionActionType action);
}