using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerNetworkCreate : DockerCmd
{
    public bool Attachable { get; set; }

    public string Network { get; set; } = string.Empty;

    public Dictionary<string, string>? AuxAddress { get; set; }

    public string? ConfigFrom { get; set; }

    public bool ConfigOnly { get; set; }

    public string? Driver { get; set; }

    public bool Ingress { get; set; }

    public bool Internal { get; set; }

    public PsArgs IpRange { get; set; } = new();

    public string? IpamDriver { get; set; }

    public Dictionary<string, string>? IpamOpt { get; set; }

    public bool IpV6 { get; set; }

    public PsArgs Label { get; set; } = new();

    public Dictionary<string, string>? Opt { get; set; }

    public string? Scope { get; set; }

    public PsArgs Subnet { get; set; } = new();

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("network", "create");

        if (this.Attachable)
            args.Add("--attachable");

        if (this.AuxAddress?.Count > 0)
        {
            foreach (var kvp in this.AuxAddress)
                args.Add("--aux-address", $"{kvp.Key}={kvp.Value}");
        }

        if (!this.ConfigFrom.IsNullOrWhiteSpace())
            args.Add("--config-from", this.ConfigFrom);

        if (this.ConfigOnly)
            args.Add("--config-only");

        if (!this.Driver.IsNullOrWhiteSpace())
            args.Add("--driver", this.Driver);

        if (this.Ingress)
            args.Add("--ingress");

        if (this.Internal)
            args.Add("--internal");

        foreach (var ip in this.IpRange)
            args.Add("--ip-range", ip);

        if (!this.IpamDriver.IsNullOrWhiteSpace())
            args.Add("--ipam-driver", this.IpamDriver);

        if (this.IpamOpt?.Count > 0)
        {
            foreach (var kvp in this.IpamOpt)
                args.Add("--ipam-opt", $"{kvp.Key}={kvp.Value}");
        }

        if (this.IpV6)
            args.Add("--ipv6");

        foreach (var l in this.Label)
            args.Add("--label", l);

        if (this.Opt?.Count > 0)
        {
            foreach (var kvp in this.Opt)
                args.Add("--opt", $"{kvp.Key}={kvp.Value}");
        }

        if (!this.Scope.IsNullOrWhiteSpace())
            args.Add("--scope", this.Scope);

        foreach (var s in this.Subnet)
            args.Add("--subnet", s);

        args.Add(this.Network);

        return args;
    }
}