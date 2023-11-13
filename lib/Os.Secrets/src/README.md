# GnomeStack.Os.Secrets

Provides KeyTar like support for libsecret, Windows Credential Manager, 
and macOS Keychain for .NET.

The main class `OsSecretVault` does not yet support listing credential as the Darwin ("MacOS")
implementation does not yet implement it.  It does support setting, getting, and deleting
credentials.

Similar to node-keytar, the linux implementation uses libsecret. It requires libsecret
and the gnome-keyring schema to be installed. No promises on supporting other schemas. 

- Debian/Ubuntu: `sudo apt-get install libsecret-1-dev`
- Red Hat-based: `sudo yum install libsecret-devel`
- Arch Linux: `sudo pacman -S libsecret`

The macOS implementation uses the Keychain API and windows uses the Credential Manager API.
The windows implementation uses Enterprise persistence by default.  If you want to use
local, you'll need to use the `WinCredManager` class directly.

## Usage

```csharp
// basically service, account, password
OsSecretVault.SetSecret("myapp", "myuser", "mypassword"); // you can also pass in a byte[] instead of a string
var password = OsSecretVault.GetSecret("myapp", "myuser");
// various options for getting the password
// as bytes and chars can be cleared from memory while strings 
// cannot since they are immutable.  SecureString is provided
// to support powershell despite Microsoft wanting 
to deprecate SecureString.
var pwBytes = OsSecretVault.GetSecretAsBytes("myapp", "myuser");
var pwChars = OsSecretVault.GetSecretAsChars("myapp", "myuser");
var secureString = OsSecretVault.GetSecretAsSecureString("myapp", "myuser");
OsSecretVault.DeleteSecret("myapp", "myuser");

// you may call the os specific implementations directly
LibSecret.SetSecret("unit", "test", "password");
WinCredManager.SetSecret("unit", "test", "password");
KeyChain.SetSecret("unit", "test", "password");
```

MIT License
