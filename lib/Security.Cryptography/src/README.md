# GnomeStack.Security.Cryptography

Provides a set of cryptographic algorithms for .NET Core such as ChaCha20, Blake2b, encryption interfaces
and a default `Aes256EncryptionProvider` and `AesGcmEncryptionProvider`. The library provides a backwards
compatible version of Rfc2898DeriveBytes for .NET Full Framework that enables more than Sha1 to be used.

The Aes256EncryptionProvider does an encrypt-then-mac approach using HMACSHA256 and uses slow equals for validating
the hash. A unique nonce and salt value is created to every encrypt call.  

The AesGcmEncryptionProvider uses the GCM mode of AES and is authenticated by the algorithm itself.

## Usage

```csharp
var options = new Aes256EncryptionProviderOptions() { Key = Encodings.Utf8NoBom.GetBytes("your-secure-key-here"), };
var provider = new Aes256EncryptionProvider(options);
var data = "Hello World";
var encrypted = provider.Encrypt(data);
var decrypted = provider.Decrypt(encrypted);
```

MIT License
