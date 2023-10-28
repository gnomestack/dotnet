# GnomeStack.Extensions.Configuration.KeyVault

Provides a configuration provider for Azure Key Vault that allows for providing a prefix
to the keys to be retrieved and to change the default separator characters 
from "--" to something else.

## Usage

```csharp
using Azure.Identity;
using Microsoft.Extensions.Configuration;

// elsewhere in your code
var config = new ConfigurationBuilder()
    .AddGsAzureKeyVault(
        new Uri("https://myvault.vault.azure.net/"), 
        new DefaultAzureCredential(),
        "production", // optional, defaults to null. When used, it only grabs keys with a prefix of "{prefix}-" e.g "production-"
        "-"); // optional, defaults to '-'. when used, secret keys are split on this character rather than the default of "--".
    .Build();
```

MIT License