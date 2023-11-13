namespace GnomeStack.Extensions.Secrets;

public abstract class SecretVaultOptions
{
    public abstract Type SecretVaultType { get; }

    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vault name. The vault name is used
    /// to identify the vault in the UI.
    /// </summary>
    public string VaultName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path delimiter used to transverse hierarchical
    /// password vaults or emulation of hierarchical vaults. The default is '/'.
    /// </summary>
    public virtual char[] Delimiter { get; set; } = new[] { '/' };
}