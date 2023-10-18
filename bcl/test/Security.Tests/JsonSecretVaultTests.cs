using GnomeStack.Extensions.Secrets;
using GnomeStack.Security.Cryptography;

namespace Tests;

public class JsonSecretVaultTests
{
    private static byte[]? s_key;

    [IntegrationTest]
    public void Ctor(IAssert assert)
    {
        using var vault = new JsonSecretVault(GetOptions());
        assert.NotNull(vault);
    }

    [IntegrationTest]
    public void SetAndGetSecrets(IAssert assert)
    {
        var file = "get-set-secrets.json";
        var vaultPath = GetVaultPath(file);
        try
        {
            using var vault = CreateVault(file);

            var nullSecret = vault.GetSecretValue("BAD_SECRET");
            assert.Null(nullSecret);

            vault.SetSecretValue("TEST_VALUE", "TEST_VALUE1");

            var testValue = vault.GetSecretValue("TEST_VALUE");
            assert.NotNull(testValue);
            assert.Equal("TEST_VALUE1", testValue);
        }
        finally
        {
            if (File.Exists(vaultPath))
                File.Delete(vaultPath);
        }
    }

    [IntegrationTest]
    public void Delete(IAssert assert)
    {
        var file = "delete.json";
        var vaultPath = GetVaultPath(file);
        try
        {
            using var vault = CreateVault(file);

            var nullSecret = vault.GetSecretValue("BAD_SECRET");
            assert.Null(nullSecret);

            vault.SetSecretValue("TEST_VALUE", "TEST_VALUE1");

            var testValue = vault.GetSecretValue("TEST_VALUE");
            assert.NotNull(testValue);

            vault.DeleteSecret("TEST_VALUE");
            nullSecret = vault.GetSecretValue("TEST_VALUE");
            assert.Null(nullSecret);
        }
        finally
        {
            if (File.Exists(vaultPath))
                File.Delete(vaultPath);
        }
    }

    [IntegrationTest]
    public void List(IAssert assert)
    {
        var file = "list.json";
        var vaultPath = GetVaultPath(file);
        try
        {
            using var vault = CreateVault(file);

            vault.SetSecretValues(new Dictionary<string, string>()
            {
                ["TEST1"] = "TEST1",
                ["TEST2"] = "TEST2",
                ["TEST3"] = "TEST3",
            });

            var secrets = vault.ListNames().OrderBy(o => o);

            assert.Collection(
                secrets,
                o => assert.Equal("TEST1", o),
                o => assert.Equal("TEST2", o),
                o => assert.Equal("TEST3", o));
        }
        finally
        {
            if (File.Exists(vaultPath))
                File.Delete(vaultPath);
        }
    }

    private static JsonSecretVault CreateVault(string fileName = "vault_test.json")
    {
        return new JsonSecretVault(GetOptions(fileName));
    }

    private static JsonSecretVaultOptions GetOptions(string fileName = "vault_test.json")
    {
        var options = new JsonSecretVaultOptions()
        {
            Key = GetOrCreateKey(),
            Path = GetVaultPath(fileName),
        };

        return options;
    }

    private static string GetVaultPath(string fileName = "vault_test.json")
    {
        var tmp = Path.GetTempPath();
        return Path.Combine(tmp, "GomeStack_extensions_secrets", fileName);
    }

    private static byte[] GetOrCreateKey()
    {
        if (s_key is null || s_key.Length == 0)
        {
            using var rng = new Csrng();
            s_key = rng.NextBytes(32);
        }

        return s_key;
    }
}