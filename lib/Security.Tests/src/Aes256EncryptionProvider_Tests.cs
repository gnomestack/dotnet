using System.Runtime.Versioning;
using System.Text;

using GnomeStack;
using GnomeStack.Security.Cryptography;
using GnomeStack.Text;

namespace Tests;

public class Aes256EncryptionProvider_Tests
{
    [IntegrationTest]
    public void Verify_EncryptDecrypt(IAssert assert)
    {
        var options = new Aes256EncryptionProviderOptions() { Key = Encodings.Utf8NoBom.GetBytes("ASDFw23d@12sdk"), };
        var provider = new Aes256EncryptionProvider(options);
        var data = "Hello World";
        var encrypted = provider.Encrypt(data);
        assert.NotEmpty(encrypted);
        assert.NotEqual(data, encrypted);
        assert.True(encrypted.Length > data.Length);

        var decrypted = provider.Decrypt(encrypted);
        assert.Equal(data, decrypted);
    }

    [UnitTest]
    public void Verify_Descrypt(IAssert assert)
    {
        // ensure that various versions of .net can decrypt the same encrypted value
        var encrypted = "AQAEAAAAAABg6gAACAAIAAucKSxGCvwsdG0AAnI5mTZfer83g41fBfzbdVQyAo8bxmToh6rXBfSj8abx0FbpbW5OmfkOZOAAMwl3nAIEf96HCstET9USOUP41rC1tDAK";
        var options = new Aes256EncryptionProviderOptions() { Key = Encodings.Utf8NoBom.GetBytes("ASDFw23d@12sdk"), };
        var provider = new Aes256EncryptionProvider(options);
        var decrypted = provider.Decrypt(encrypted);
        assert.Equal("Hello World", decrypted);
    }
}