using GnomeStack.Diagnostics;
using GnomeStack.Standard;

namespace GnomeStack.Cli.Docker;

public class DockerCmd : PsCommand
{
    public string? Config { get; set; }

    public string? Context { get; set; }

    public bool Debug { get; set; }

    public string? Host { get; set; }

    public bool Tls { get; set; }

    public string? TlsCaCert { get; set; }

    public string? TlsCert { get; set; }

    public string? TlsKey { get; set; }

    protected override string GetExecutablePath()
        => Ps.Which("docker") ?? throw new NotFoundOnPathException("docker not found on path");

    protected virtual void ApplyBaseOptions(PsArgs args)
    {
        if (!this.Config.IsNullOrWhiteSpace())
            args.Add("--config", this.Config);

        if (!this.Context.IsNullOrWhiteSpace())
            args.Add("--context", this.Context);

        if (this.Debug)
            args.Add("--debug");

        if (!this.Host.IsNullOrWhiteSpace())
            args.Add("--host", this.Host);

        if (this.Tls)
            args.Add("--tls");

        if (!this.TlsCaCert.IsNullOrWhiteSpace())
            args.Add("--tlscacert", this.TlsCaCert);

        if (!this.TlsCert.IsNullOrWhiteSpace())
            args.Add("--tlscert", this.TlsCert);

        if (!this.TlsKey.IsNullOrWhiteSpace())
            args.Add("--tlskey", this.TlsKey);
    }
}