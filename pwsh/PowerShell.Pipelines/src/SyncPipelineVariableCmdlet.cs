using System.Management.Automation;
using System.Security;
using System.Text;

using GnomeStack.Security.Cryptography;
using GnomeStack.Standard;
using GnomeStack.Text.DotEnv;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("sync_pipeline_variables")]
[OutputType(typeof(void))]
[Cmdlet(VerbsData.Sync, "PipelineVariable")]
public class SyncPipelineVariableCmdlet : PSCmdlet
{
    [Parameter]
    public SwitchParameter Quiet { get; set; }

    [Parameter]
    public SecureString? Key { get; set; }

    protected override void ProcessRecord()
    {
        if (Util.IsTfBuild || Util.IsGitHubActions)
            return;

        if (Env.TryGet("PIPELINE_ENV", out var filePath))
        {
            if (!Fs.Exists(filePath))
            {
                if (!this.Quiet.ToBool())
                    this.WriteWarning($"PIPELINE_ENV file not found: {filePath}");
                else
                    this.WriteDebug("PIPELINE_ENV file not found.");
                return;
            }

            var key = Array.Empty<byte>();
            if (this.Key is not null)
            {
                key = this.Key.ToBytes(Encoding.UTF8);
            }
            else if (Env.TryGet("PIPELINE_ENV_KEY", out var envKey) && envKey.Length > 0)
            {
                key = Encoding.UTF8.GetBytes(envKey);
            }

            if (key.Length > 0)
            {
                var cipher = new Aes256EncryptionProvider(new Aes256EncryptionProviderOptions()
                {
                    Key = key,
                });
                var text = File.ReadAllText(filePath);
                var decrypted = cipher.Decrypt(text);
                DotEnv.Load(new DotEnvLoadOptions()
                {
                    Expand = true,
                    Content = decrypted,
                    AllowBackticks = true,
                    AllowJson = true,
                    AllowYaml = true,
                    OverrideEnvironment = true,
                });
            }
            else
            {
                DotEnv.Load(new DotEnvLoadOptions()
                {
                    Expand = true,
                    Files = new[] { filePath },
                    AllowBackticks = true,
                    AllowJson = true,
                    AllowYaml = true,
                });
            }
        }
    }
}