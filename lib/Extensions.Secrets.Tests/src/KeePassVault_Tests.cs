using GnomeStack.Extensions.Secrets.KeePass;

using Microsoft.Extensions.Logging.Abstractions;

namespace GnomeStack.Extensions.Secrets;

public class KeePassVault_Tests
{
    [IntegrationTest]
    public void CreateAndOpen(IAssert assert)
    {
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

        assert.True(File.Exists(file));

        vault.SetSecretValue("testg/one/Entry", "bad_password");
        vault.SetSecretValue("testg/two/Entry", "bad_password2");

        var e1 = vault.GetSecret("testg/one/Entry");
        assert.NotNull(e1);
        assert.Equal("bad_password", e1.Value);

        using var vault2 = new KeePassSecretVault(
            new KeePassSecretVaultOptions()
            {
                KdbxFile = file,
                Password = pw,
            },
            NullLogger<KeePassSecretVault>.Instance);

        var e2 = vault2.GetSecret("testg/one/Entry");
        assert.NotNull(e2);
        assert.Equal("bad_password", e2.Value);
    }

    [IntegrationTest]
    public void ApplyRequirements(IAssert assert)
    {
        var temp = Path.GetTempPath();
        var file = Path.Combine(temp, "kpsv3.kdbx");
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

        assert.True(File.Exists(file));

        var reqBuilder = new VaultRequirementsBuilder();
        var yaml = """
                   - url: testg/one/Entry
                     required: true
                     generate: true
                     length: 20
                     upper: true
                     lower: true
                     number: true
                     env: TESTG_ONE_ENTRY
                   - url: testg/two/Entry
                     required: true
                     generate: true
                     length: 20
                     env: TESTG_TWO_ENTRY
                   """;

        reqBuilder.AddYaml(yaml);
        var req = reqBuilder.Build();
        req.Apply(vault);

        var e1 = vault.GetSecret("testg/one/Entry");
        assert.NotNull(e1);
        assert.Equal(20, e1.Value.Length);
        var pw1 = e1.Value;

        var e2 = vault.GetSecret("testg/two/Entry");
        assert.NotNull(e2);
        assert.Equal(20, e2.Value.Length);
        var pw2 = e2.Value;

        var env1 = Environment.GetEnvironmentVariable("TESTG_ONE_ENTRY");
        assert.NotNull(env1);
        assert.Equal(20, env1.Length);
        assert.Equal(pw1, env1);

        Environment.SetEnvironmentVariable("TESTG_ONE_ENTRY", null);
        var env1Null = Environment.GetEnvironmentVariable("TESTG_ONE_ENTRY");
        assert.Null(env1Null);

        var env2 = Environment.GetEnvironmentVariable("TESTG_TWO_ENTRY");
        assert.NotNull(env2);
        assert.Equal(20, env2.Length);
        assert.Equal(pw2, env2);

        req.Apply(vault);
        var e1v2 = vault.GetSecret("testg/one/Entry");
        assert.NotNull(e1v2);
        assert.Equal(20, e1v2.Value.Length);
        assert.Equal(pw1, e1v2.Value);

        var env1v2 = Environment.GetEnvironmentVariable("TESTG_ONE_ENTRY");
        assert.NotNull(env1v2);
        assert.Equal(20, env1v2.Length);
        assert.Equal(pw1, env1v2);
    }
}