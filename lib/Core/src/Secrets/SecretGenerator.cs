using System.Security.Cryptography;

namespace GnomeStack.Secrets;

// ReSharper disable ParameterHidesMember

/// <summary>
/// The default implementation of <see cref="ISecretGenerator"/> that
/// uses a cryptographically secure random number generator to generate
/// new secrets.
/// </summary>
public class SecretGenerator : ISecretGenerator
{
    private readonly List<char> characters = new();

    private Func<char[], bool> validator = (password) =>
    {
#pragma warning disable S2583 // false positive
        if (password.Length == 0)
            return false;

        if (password.Length < 8)
            throw new ArgumentException("The password must be 8 characters or more", nameof(password));

        bool lower = false,
            upper = false,
            digit = false,
            special = false;

        foreach (var c in password)
        {
            if (char.IsDigit(c))
            {
                digit = true;
                continue;
            }

            if (char.IsLetter(c))
            {
                if (char.IsUpper(c))
                {
                    upper = true;
                }
                else
                {
                    lower = true;
                }

                continue;
            }

            special = true;

            if (lower && upper && digit && special)
                return true;
        }

        return lower && upper && digit && special;
    };

    public static char[] GenerateNewPassword(int length, IList<char> characters, Func<char[], bool> validator)
    {
        if (characters is null || characters.Count == 0)
            throw new ArgumentNullException(nameof(characters), "characters must not be null or empty");

        if (validator == null)
            throw new ArgumentNullException(nameof(validator));

        if (length < 1)
            throw new ArgumentOutOfRangeException(nameof(length), "length must be 1 or greater.");

        var password = new char[length];
        var bytes = new byte[length];

        using var rng = RandomNumberGenerator.Create();

        do
        {
            Array.Clear(password, 0, password.Length);
            rng.GetBytes(bytes);

            for (var i = 0; i < length; i++)
            {
                var randomIndex = bytes[i] % characters.Count;
                password[i] = characters[randomIndex];
            }
        }
        while (!validator(password));

        return password;
    }

    /// <summary>
    /// Add a character to the generator.
    /// </summary>
    /// <param name="character">The character to add.</param>
    /// <returns>The generator to chain methods.</returns>
    public ISecretGenerator Add(char character)
    {
        if (!this.characters.Contains(character))
            this.characters.Add(character);

        return this;
    }

    /// <summary>
    /// Add a character to the generator.
    /// </summary>
    /// <param name="characters">The characters to add.</param>
    /// <returns>The generator to chain methods.</returns>
    public ISecretGenerator Add(IEnumerable<char> characters)
    {
        foreach (var c in characters)
        {
            if (!this.characters.Contains(c))
                this.characters.Add(c);
        }

        return this;
    }

    /// <summary>
    /// Set the validator for the generator.
    /// </summary>
    /// <param name="validator">The validator function.</param>
    /// <returns>The generator to chain methods.</returns>
    public ISecretGenerator SetValidator(Func<char[], bool> validator)
    {
        this.validator = validator;
        return this;
    }

    /// <summary>
    /// Generates a new password and returns it as a char array.
    /// </summary>
    /// <param name="length">The length of the secret.</param>
    /// <returns>The generated secret.</returns>
    public char[] Generate(int length)
        => GenerateNewPassword(length, this.characters, this.validator);

    /// <summary>
    /// Generates a new password and returns it as a char array.
    /// </summary>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    public char[] Generate(int length, IList<char>? characters, Func<char[], bool>? validator)
        => GenerateNewPassword(
            length,
            characters ?? this.characters,
            validator ?? this.validator);
}