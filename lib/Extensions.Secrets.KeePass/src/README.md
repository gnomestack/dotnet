# GnomeStack.Extensions.Secrets.KeePass

Provides a KeePass implementation of `ISecretsProvider` for use with `GnomeStack.Extensions.Secrets`.

## Usage

```csharp
var temp = Path.GetTempPath();
var file = Path.Combine(temp, "kpsv1.kdbx");
if (File.Exists(file))
    File.Delete(file);

var pw = "p@ssw0rd";
using var vault = new KeePassSecretVault(
    new KeePassSecretVaultOptions()
    {
        KdbxFile = file,
        Password = pw,
    },
    NullLogger<KeePassSecretVault>.Instance);


// creates two groups testg and one and the entry 'Entry'.
vault.SetSecretValue("testg/one/Entry", "bad_password");

// creates one group, 'two' under the group 'testg' and the entry 'Entry'.
vault.SetSecretValue("testg/two/Entry", "bad_password2");

var e1 = vault.GetSecret("testg/one/Entry");
Console.WriteLine($"e1: {e1}");
vault.DeleteSecret("testg/one/Entry");
```

MIT License
