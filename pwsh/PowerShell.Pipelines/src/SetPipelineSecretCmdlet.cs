using System.Management.Automation;
using System.Security;
using System.Text;

using GnomeStack.Secrets;
using GnomeStack.Security.Cryptography;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("set_pipeline_secret")]
[OutputType(typeof(void))]
[Cmdlet(VerbsCommon.Set, "PipelineSecret")]
public class SetPipelineSecretCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string Name { get; set; } = null!;

    [Parameter(Position = 1, Mandatory = true)]
    public string Value { get; set; } = null!;

    [Parameter]
    public SecureString? Key { get; set; }

    [Parameter]
    public SwitchParameter RequireEnvFile { get; set; }

    protected override void ProcessRecord()
    {
        SecretMasker.Default.Add(this.Value);
        Env.Set(this.Name, this.Value);
        if (Util.IsTfBuild)
        {
            Console.WriteLine($"##vso[task.setvariable variable={this.Name};issecret=true]{this.Value}");
            return;
        }

        if (Util.IsGitHubActions)
        {
            Console.WriteLine($"::add-mask::{this.Value}");
            var fileName = Env.GetRequired("GITHUB_ENV");
            var file = new FileInfo(fileName);
            file.Directory!.Create();
            if (this.Value.Contains("\n"))
            {
                var text = $"""
                            {this.Name}<<EOF
                            {this.Value}
                            EOF
                            """;
                File.AppendAllText(fileName, $"{text}{Environment.NewLine}");
                return;
            }

            File.AppendAllText(fileName, $"{this.Name}={this.Value}{Environment.NewLine}");
            return;
        }

        if (Env.TryGet("PIPELINE_ENV", out var filePath))
        {
            var file = new FileInfo(filePath);
            file.Directory!.Create();

            var key = Array.Empty<byte>();
            if (this.Key is not null)
            {
                key = this.Key.ToBytes(Encoding.UTF8);
            }
            else if (Env.TryGet("PIPELINE_ENV_KEY", out var envKey) && envKey.Length > 7)
            {
                key = Encoding.UTF8.GetBytes(envKey);
            }

            if (key.Length > 0)
            {
                string text = string.Empty;
                var cipher = new Aes256EncryptionProvider(new Aes256EncryptionProviderOptions()
                {
                    Key = key,
                });

                if (file.Exists)
                {
                    var encryptedText = File.ReadAllText(filePath);
                    text = cipher.Decrypt(encryptedText);

                    File.WriteAllText(filePath, cipher.Encrypt(text));
                }

                if (this.Value.Contains("\""))
                {
                    text += $"{this.Name}='${this.Value}'{Environment.NewLine}";
                }
                else
                {
                    text += $"{this.Name}=\"{this.Value}\"{Environment.NewLine}";
                }

                File.WriteAllText(filePath, cipher.Encrypt(text));
                return;
            }

            if (this.Value.Contains("\""))
            {
                File.AppendAllText(filePath, $"{this.Name}='${this.Value}'{Environment.NewLine}");
                return;
            }

            File.AppendAllText(filePath, $"{this.Name}=\"{this.Value}\"{Environment.NewLine}");
            return;
        }

        if (this.RequireEnvFile)
        {
            var record = new ErrorRecord(
                new FileNotFoundException("PIPELINE_ENV env variable not found to store output as a dotenv file"),
                "NoEnvFile",
                ErrorCategory.ObjectNotFound,
                null);
            this.WriteError(record);
        }
    }
}