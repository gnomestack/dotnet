using System.Security;
using System.Text;

namespace GnomeStack.Secrets;

public static class SecretGeneratorExtensions
{
    /// <summary>
    /// Adds the suggested default character sets to the generator.
    /// </summary>
    /// <param name="pg">The password generator.</param>
    /// <returns>The generator to chain method calls.</returns>
    public static ISecretGenerator AddDefaults(
        this ISecretGenerator pg)
    {
        return pg.Add(SecretCharacterSets.LatinAlphaUpperCase)
            .Add(SecretCharacterSets.LatinAlphaLowerCase)
            .Add(SecretCharacterSets.Digits)
            .Add(SecretCharacterSets.SpecialSafe);
    }

    /// <summary>
    /// Generates a new password and returns it as a readonly char span.
    /// </summary>
    /// <param name="pg">The password generator.</param>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    public static ReadOnlySpan<char> GenerateAsCharSpan(
        this ISecretGenerator pg,
        int length,
        IList<char>? characters = null,
        Func<char[], bool>? validator = null)
    {
        return pg.Generate(length, characters, validator);
    }

    /// <summary>
    /// Generates a new password and returns it as a string.
    /// </summary>
    /// <param name="pg">The password generator.</param>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    public static string GenerateAsString(
        this ISecretGenerator pg,
        int length,
        IList<char>? characters = null,
        Func<char[], bool>? validator = null)
    {
        var chars = pg.Generate(length, characters, validator);
        var result = new string(chars);
        Array.Clear(chars, 0, chars.Length);
        return result;
    }

    /// <summary>
    /// Generates a new password and returns it as a secure string.
    /// </summary>
    /// <param name="pg">The password generator.</param>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    public static unsafe SecureString GenerateAsSecureString(
        this ISecretGenerator pg,
        int length,
        char[]? characters = null,
        Func<char[], bool>? validator = null)
    {
        var password = pg.Generate(length, characters, validator);
        SecureString secureString;

        fixed (char* chPtr = password)
        {
            secureString = new SecureString(chPtr, password.Length);
        }

        Array.Clear(password, 0, password.Length);
        return secureString;
    }

    /// <summary>
    /// Generates a new password and returns it as a byte array.
    /// </summary>
    /// <param name="pg">The password generator.</param>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="encoding">The encoding to use to convert the password to bytes. Defaults to UTF8.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    public static byte[] GenerateAsBytes(
        this ISecretGenerator pg,
        int length,
        char[]? characters = null,
        Encoding? encoding = null,
        Func<char[], bool>? validator = null)
    {
        encoding ??= Encoding.UTF8;
        var password = pg.Generate(length, characters, validator);
        var bytes = encoding.GetBytes(password);

        Array.Clear(password, 0, password.Length);
        return bytes;
    }

    /// <summary>
    /// Generates a new password and returns it as a readonly byte span.
    /// </summary>
    /// <param name="pg">The password generator.</param>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="encoding">The encoding to use to convert the password to bytes. Defaults to UTF8.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    public static ReadOnlySpan<byte> GenerateAsByteSpan(
        this ISecretGenerator pg,
        int length,
        char[]? characters = null,
        Encoding? encoding = null,
        Func<char[], bool>? validator = null)
    {
        return pg.GenerateAsBytes(length, characters, encoding, validator);
    }
}