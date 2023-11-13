# GnomeStack.Extensions.Secrets.Abstractions

Provides abstractions for secret vaults to enable switching out vaults in an application and provides a default
null provider.

## Usage

```csharp
// add GnomeStack.Extensions.Secrets.KeePass to your project.

var services = new ServiceCollection();
services.AddLogging();
services.AddSecrets(new KeePassSecretVaultOptions()
    {
        KdbxFile = file,
        Password = pw,
    });

var sp = services.BuildServiceProvider();

var vault = sp.GetRequiredService<ISecretsVault>();
vault.SetSecretValue("testg/one/Entry", "example_password");
var e1 = vault.GetSecret("testg/one/Entry");
Console.WriteLine($"e1: {e1}");
```

MIT License
