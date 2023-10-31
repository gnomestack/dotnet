using System;
using System.Collections.Generic;

namespace GnomeStack.Secrets;

/// <summary>
/// The contract for a secret generator that can create new secrets.
/// </summary>
public interface ISecretGenerator
{
    /// <summary>
    /// Add a character to the generator.
    /// </summary>
    /// <param name="character">The character to add.</param>
    /// <returns>The generator to chain methods.</returns>
    ISecretGenerator Add(char character);

    /// <summary>
    /// Add a character to the generator.
    /// </summary>
    /// <param name="characters">The characters to add.</param>
    /// <returns>The generator to chain methods.</returns>
    ISecretGenerator Add(IEnumerable<char> characters);

    /// <summary>
    /// Set the validator for the generator.
    /// </summary>
    /// <param name="validator">The validator function.</param>
    /// <returns>The generator to chain methods.</returns>
    ISecretGenerator SetValidator(Func<char[], bool> validator);

    /// <summary>
    /// Generates a new password and returns it as a char array.
    /// </summary>
    /// <param name="length">The length of the secret.</param>
    /// <returns>The generated secret.</returns>
    char[] Generate(int length);

    /// <summary>
    /// Generates a new password and returns it as a char array.
    /// </summary>
    /// <param name="length">The length of the secret.</param>
    /// <param name="characters">The allowed set of characters to use.</param>
    /// <param name="validator">The function used to validate the new secret.</param>
    /// <returns>The generated secret.</returns>
    char[] Generate(int length, IList<char>? characters, Func<char[], bool>? validator);
}