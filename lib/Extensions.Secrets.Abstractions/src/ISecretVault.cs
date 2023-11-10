namespace GnomeStack.Extensions.Secrets;

public interface ISecretVault
{
    /// <summary>
    /// Gets a value indicating whether this vault supports synchronous operations
    /// outside of <see cref="CreateRecord"/>.
    /// </summary>
    bool SupportsSynchronous { get; }

    /// <summary>
    /// Gets the name of the vault.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a string that identifies the type of vault.
    /// </summary>
    string Kind { get; }

    /// <summary>
    /// Lists the names of all secrets in the vault.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The an enumerable of names.</returns>
    Task<IEnumerable<string>> ListNamesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists the names of all secrets in the vault.
    /// </summary>
    /// <returns>The an enumerable of names.</returns>
    IEnumerable<string> ListNames();

    /// <summary>
    /// Creates a new secret record specific to this kind of vault.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <returns>An object that implements <see cref="ISecretRecord"/> with the name property set.</returns>
    ISecretRecord CreateRecord(string path);

    /// <summary>
    /// Gets the value of the secret with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The value of the secret; otherwise, <see langword="null" />.</returns>
    Task<string?> GetSecretValueAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the value of the secret with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <returns>The value of the secret; otherwise, <see langword="null" />.</returns>
    string? GetSecretValue(string path);

    /// <summary>
    /// Gets the secret record with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="ISecretRecord"/>; otherwise, <see langword="null"/>.</returns>
    Task<ISecretRecord?> GetSecretAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the secret record with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <returns>A <see cref="ISecretRecord"/>; otherwise, <see langword="null"/>.</returns>
    ISecretRecord? GetSecret(string path);

    /// <summary>
    /// Sets the value of the secret with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <param name="secret">The secret value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The awaitable task.</returns>
    Task SetSecretValueAsync(string path, string secret,  CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the value of the secret with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <param name="secret">The secret value.</param>
    void SetSecretValue(string path, string secret);

    /// <summary>
    /// Sets the secret record.
    /// </summary>
    /// <param name="secret">The secret record.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    Task SetSecretAsync(ISecretRecord secret, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the secret record.
    /// </summary>
    /// <param name="secret">The secret record.</param>
    void SetSecret(ISecretRecord secret);

    /// <summary>
    /// Deletes the secret with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    Task DeleteSecretAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the secret with the given name.
    /// </summary>
    /// <param name="path">The name of the secret.</param>
    void DeleteSecret(string path);
}