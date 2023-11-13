using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerBuild : DockerCmd
{
    public string Path { get; set; } = ".";

    public PsArgs? AddHost { get; set; }

    public PsArgs? Allow { get; set; }

    public PsArgs? Attach { get; set; }

    public PsArgs? BuildArg { get; set; }

    public PsArgs? BuildContext { get; set; }

    public PsArgs? CacheFrom { get; set; }

    public PsArgs? CacheTo { get; set; }

    public string? CgroupParent { get; set; }

    public string? File { get; set; }

    public string? Label { get; set; }

    public bool Load { get; set; }

    public string? MetadataFile { get; set; }

    public string? Network { get; set; }

    public bool NoCache { get; set; }

    public PsArgs? NoCacheFilter { get; set; }

    public PsArgs? Output { get; set; }

    public PsArgs? Platform { get; set; }

    public string? Progress { get; set; }

    public string? Provenance { get; set; }

    public bool Pull { get; set; }

    public bool Push { get; set; }

    public bool Quiet { get; set; }

    public string? Sbom { get; set; }

    public PsArgs? Secret { get; set; }

    public int ShmSize { get; set; }

    public PsArgs? Ssh { get; set; }

    public PsArgs? Tag { get; set; }

    public PsArgs? Target { get; set; }

    public bool Ulimit { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("build");

        if (this.AddHost?.Count > 0)
        {
            foreach (var n in this.AddHost)
                args.Add("--add-host", n);
        }

        if (this.Allow?.Count > 0)
        {
            foreach (var n in this.Allow)
                args.Add("--allow", n);
        }

        if (this.Attach?.Count > 0)
        {
            foreach (var n in this.Attach)
                args.Add("--attach", n);
        }

        if (this.BuildArg?.Count > 0)
        {
            foreach (var n in this.BuildArg)
                args.Add("--build-arg", n);
        }

        if (this.BuildContext?.Count > 0)
        {
            foreach (var n in this.BuildContext)
                args.Add("--build-context", n);
        }

        if (this.CacheFrom?.Count > 0)
        {
            foreach (var n in this.CacheFrom)
                args.Add("--cache-from", n);
        }

        if (this.CacheTo?.Count > 0)
        {
            foreach (var n in this.CacheTo)
                args.Add("--cache-to", n);
        }

        if (!this.CgroupParent.IsNullOrWhiteSpace())
            args.Add("--cgroup-parent", this.CgroupParent);

        if (!this.File.IsNullOrWhiteSpace())
            args.Add("--file", this.File);

        if (!this.Label.IsNullOrWhiteSpace())
            args.Add("--label", this.Label);

        if (this.Load)
            args.Add("--load");

        if (!this.MetadataFile.IsNullOrWhiteSpace())
            args.Add("--metadata-file", this.MetadataFile);

        if (!this.Network.IsNullOrWhiteSpace())
            args.Add("--network", this.Network);

        if (this.NoCache)
            args.Add("--no-cache");

        if (this.NoCacheFilter?.Count > 0)
        {
            foreach (var n in this.NoCacheFilter)
                args.Add("--no-cache-filter", n);
        }

        if (this.Output?.Count > 0)
        {
            foreach (var n in this.Output)
                args.Add("--output", n);
        }

        if (this.Platform?.Count > 0)
        {
            foreach (var n in this.Platform)
                args.Add("--platform", n);
        }

        if (!this.Progress.IsNullOrWhiteSpace())
            args.Add("--progress", this.Progress);

        if (!this.Provenance.IsNullOrWhiteSpace())
            args.Add("--provenance", this.Provenance);

        if (this.Pull)
            args.Add("--pull");

        if (this.Push)
            args.Add("--push");

        if (this.Quiet)
            args.Add("--quiet");

        if (!this.Sbom.IsNullOrWhiteSpace())
            args.Add("--sbom", this.Sbom);

        if (this.Secret?.Count > 0)
        {
            foreach (var n in this.Secret)
                args.Add("--secret", n);
        }

        if (this.ShmSize > 0)
            args.Add("--shm-size", this.ShmSize.ToString());

        if (this.Ssh?.Count > 0)
        {
            foreach (var n in this.Ssh)
                args.Add("--ssh", n);
        }

        if (this.Tag?.Count > 0)
        {
            foreach (var n in this.Tag)
                args.Add("--tag", n);
        }

        if (this.Target?.Count > 0)
        {
            foreach (var n in this.Target)
                args.Add("--target", n);
        }

        if (this.Ulimit)
            args.Add("--ulimit");

        args.Add(this.Path);

        return args;
    }
}