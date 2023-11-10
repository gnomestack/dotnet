using GnomeStack.Diagnostics;

namespace GnomeStack.Cli.Docker;

public class DockerRun : DockerCmd
{
    public string Command { get; set; } = string.Empty;

    public PsArgs Args { get; set; } = new();

    public PsArgs? AddHost { get; set; }

    public Dictionary<string, string>? Annotations { get; set; }

    public PsArgs? Attach { get; set; }

    public int? BlkioWeight { get; set; }

    public PsArgs? BlkioWeightDevice { get; set; }

    public PsArgs? CapAdd { get; set; }

    public PsArgs? CapDrop { get; set; }

    public string? CgroupParent { get; set; }

    public string? Cgroupns { get; set; }

    public string? Cidfile { get; set; }

    public int? CpuPeriod { get; set; }

    public int? CpuQuota { get; set; }

    public int? CpuRtPeriod { get; set; }

    public int? CpuRtRuntime { get; set; }

    public int? CpuShares { get; set; }

    public decimal? Cpus { get; set; }

    public string? CpusetCpus { get; set; }

    public string? CpusetMems { get; set; }

    public bool Detach { get; set; }

    public string? DetachKeys { get; set; }

    public PsArgs? Device { get; set; }

    public PsArgs? DeviceCgroupRule { get; set; }

    public PsArgs? DeviceReadBps { get; set; }

    public PsArgs? DeviceReadIops { get; set; }

    public PsArgs? DeviceWriteBps { get; set; }

    public PsArgs? DeviceWriteIops { get; set; }

    public bool DisableContentTrust { get; set; }

    public PsArgs? Dns { get; set; }

    public PsArgs? DnsOption { get; set; }

    public PsArgs? DnsSearch { get; set; }

    public string? Domainname { get; set; }

    public string? EntryPoint { get; set; }

    public Dictionary<string, string>? Env { get; set; }

    public PsArgs? EnvFile { get; set; }

    public PsArgs? Expose { get; set; }

    public PsArgs? Gpu { get; set; }

    public PsArgs? GroupAdd { get; set; }

    public string? HealthCmd { get; set; }

    public string? HealthInterval { get; set; }

    public int? HealthRetries { get; set; }

    public string? HealthStartPeriod { get; set; }

    public string? HealthTimeout { get; set; }

    public string? Hostname { get; set; }

    public bool Init { get; set; }

    public bool Interactive { get; set; }

    public string? Ip { get; set; }

    public string? Ip6 { get; set; }

    public string? Ipc { get; set; }

    public string? Isolation { get; set; }

    public string? KernelMemory { get; set; }

    public Dictionary<string, string>? Label { get; set; }

    public PsArgs? LabelFile { get; set; }

    public string? Link { get; set; }

    public PsArgs? LinkLocalIp { get; set; }

    public string? LogDriver { get; set; }

    public PsArgs? LogOpt { get; set; }

    public string? MacAddress { get; set; }

    public string? Memory { get; set; }

    public string? MemoryReservation { get; set; }

    public string? MemorySwap { get; set; }

    public int? MemorySwappiness { get; set; }

    public string? Mount { get; set; }

    public string? Name { get; set; }

    public string? Network { get; set; }

    public PsArgs? NetworkAlias { get; set; }

    public bool NoHealthcheck { get; set; }

    public bool OomKillDisable { get; set; }

    public int? OomScoreAdj { get; set; }

    public string? Pid { get; set; }

    public int? PidsLimit { get; set; }

    public string? Platform { get; set; }

    public bool Privileged { get; set; }

    public PsArgs Publish { get; set; } = new();

    public PsArgs? PublishAll { get; set; }

    public string? Pull { get; set; }

    public bool Quiet { get; set; }

    public bool ReadOnly { get; set; }

    public string? Restart { get; set; }

    public bool Rm { get; set; }

    public string? Runtime { get; set; }

    public PsArgs? SecurityOpt { get; set; }

    public string? ShmSize { get; set; }

    public bool SigProxy { get; set; }

    public string? StopSignal { get; set; }

    public int? StopTimeout { get; set; }

    public PsArgs? StorageOpt { get; set; }

    public Dictionary<string, string>? Sysctl { get; set; }

    public PsArgs? Tmpfs { get; set; }

    public bool Tty { get; set; }

    public string? Ulimit { get; set; }

    public string? User { get; set; }

    public string? Userns { get; set; }

    public PsArgs? Uts { get; set; }

    public PsArgs? Volume { get; set; }

    public string? VolumeDriver { get; set; }

    public PsArgs? VolumesFrom { get; set; }

    public string? Workdir { get; set; }

    protected override PsArgs BuildPsArgs()
    {
        var args = new PsArgs();
        this.ApplyBaseOptions(args);
        args.Add("run");

        if (this.AddHost?.Count > 0)
        {
            foreach (var n in this.AddHost)
                args.Add("--add-host", n);
        }

        if (this.Annotations?.Count > 0)
        {
            foreach (var kvp in this.Annotations)
                args.Add("--allow", $"{kvp.Key}='{kvp.Value}'");
        }

        if (this.Attach?.Count > 0)
        {
            foreach (var n in this.Attach)
                args.Add("--attach", n);
        }

        if (this.BlkioWeight.HasValue)
            args.Add("--blkio-weight", this.BlkioWeight.Value.ToString());

        if (this.BlkioWeightDevice?.Count > 0)
        {
            foreach (var n in this.BlkioWeightDevice)
                args.Add("--blkio-weight-device", n);
        }

        if (this.CapAdd?.Count > 0)
        {
            foreach (var n in this.CapAdd)
                args.Add("--cap-add", n);
        }

        if (this.CapDrop?.Count > 0)
        {
            foreach (var n in this.CapDrop)
                args.Add("--cap-drop", n);
        }

        if (!this.CgroupParent.IsNullOrWhiteSpace())
            args.Add("--cgroup-parent", this.CgroupParent);

        if (!this.Cgroupns.IsNullOrWhiteSpace())
            args.Add("--cgroupns", this.Cgroupns);

        if (!this.Cidfile.IsNullOrWhiteSpace())
            args.Add("--cidfile", this.Cidfile);

        if (this.CpuPeriod.HasValue)
            args.Add("--cpu-period", this.CpuPeriod.Value.ToString());

        if (this.CpuQuota.HasValue)
            args.Add("--cpu-quota", this.CpuQuota.Value.ToString());

        if (this.CpuRtPeriod.HasValue)
            args.Add("--cpu-rt-period", this.CpuRtPeriod.Value.ToString());

        if (this.CpuRtRuntime.HasValue)
            args.Add("--cpu-rt-runtime", this.CpuRtRuntime.Value.ToString());

        if (this.CpuShares.HasValue)
            args.Add("--cpu-shares", this.CpuShares.Value.ToString());

        if (this.Cpus.HasValue)
            args.Add("--cpus", this.Cpus.ToString());

        if (!this.CpusetCpus.IsNullOrWhiteSpace())
            args.Add("--cpuset-cpus", this.CpusetCpus);

        if (!this.CpusetMems.IsNullOrWhiteSpace())
            args.Add("--cpuset-mems", this.CpusetMems);

        if (this.Detach)
            args.Add("--detach");

        if (!this.DetachKeys.IsNullOrWhiteSpace())
            args.Add("--detach-keys", this.DetachKeys);

        if (this.Device?.Count > 0)
        {
            foreach (var n in this.Device)
                args.Add("--device", n);
        }

        if (this.DeviceCgroupRule?.Count > 0)
        {
            foreach (var n in this.DeviceCgroupRule)
                args.Add("--device-cgroup-rule", n);
        }

        if (this.DeviceReadBps?.Count > 0)
        {
            foreach (var n in this.DeviceReadBps)
                args.Add("--device-read-bps", n);
        }

        if (this.DeviceReadIops?.Count > 0)
        {
            foreach (var n in this.DeviceReadIops)
                args.Add("--device-read-iops", n);
        }

        if (this.DeviceWriteBps?.Count > 0)
        {
            foreach (var n in this.DeviceWriteBps)
                args.Add("--device-write-bps", n);
        }

        if (this.DeviceWriteIops?.Count > 0)
        {
            foreach (var n in this.DeviceWriteIops)
                args.Add("--device-write-iops", n);
        }

        if (this.DisableContentTrust)
            args.Add("--disable-content-trust");

        if (this.Dns?.Count > 0)
        {
            foreach (var n in this.Dns)
                args.Add("--dns", n);
        }

        if (this.DnsOption?.Count > 0)
        {
            foreach (var n in this.DnsOption)
                args.Add("--dns-option", n);
        }

        if (this.DnsSearch?.Count > 0)
        {
            foreach (var n in this.DnsSearch)
                args.Add("--dns-search", n);
        }

        if (!this.Domainname.IsNullOrWhiteSpace())
            args.Add("--domainname", this.Domainname);

        if (!this.EntryPoint.IsNullOrWhiteSpace())
            args.Add("--entry-point", this.EntryPoint);

        if (this.Env?.Count > 0)
        {
            foreach (var kvp in this.Env)
                args.Add("--env", $"{kvp.Key}='{kvp.Value}'");
        }

        if (this.EnvFile?.Count > 0)
        {
            foreach (var n in this.EnvFile)
                args.Add("--env-file", n);
        }

        if (this.Expose?.Count > 0)
        {
            foreach (var n in this.Expose)
                args.Add("--expose", n);
        }

        if (this.Gpu?.Count > 0)
        {
            foreach (var n in this.Gpu)
                args.Add("--gpu", n);
        }

        if (this.GroupAdd?.Count > 0)
        {
            foreach (var n in this.GroupAdd)
                args.Add("--group-add", n);
        }

        if (!this.HealthCmd.IsNullOrWhiteSpace())
            args.Add("--health-cmd", this.HealthCmd);

        if (!this.HealthInterval.IsNullOrWhiteSpace())
            args.Add("--health-interval", this.HealthInterval);

        if (this.HealthRetries.HasValue)
            args.Add("--health-retries", this.HealthRetries.Value.ToString());

        if (!this.HealthStartPeriod.IsNullOrWhiteSpace())
            args.Add("--health-start-period", this.HealthStartPeriod);

        if (!this.HealthTimeout.IsNullOrWhiteSpace())
            args.Add("--health-timeout", this.HealthTimeout);

        if (!this.Hostname.IsNullOrWhiteSpace())
            args.Add("--hostname", this.Hostname);

        if (this.Init)
            args.Add("--init");

        if (this.Interactive)
            args.Add("--interactive");

        if (!this.Ip.IsNullOrWhiteSpace())
            args.Add("--ip", this.Ip);

        if (!this.Ip6.IsNullOrWhiteSpace())
            args.Add("--ip6", this.Ip6);

        if (!this.Ipc.IsNullOrWhiteSpace())
            args.Add("--ipc", this.Ipc);

        if (!this.Isolation.IsNullOrWhiteSpace())
            args.Add("--isolation", this.Isolation);

        if (!this.KernelMemory.IsNullOrWhiteSpace())
            args.Add("--kernel-memory", this.KernelMemory);

        if (this.Label?.Count > 0)
        {
            foreach (var kvp in this.Label)
                args.Add("--label", $"{kvp.Key}='{kvp.Value}'");
        }

        if (this.LabelFile?.Count > 0)
        {
            foreach (var n in this.LabelFile)
                args.Add("--label-file", n);
        }

        if (!this.Link.IsNullOrWhiteSpace())
            args.Add("--link", this.Link);

        if (this.LinkLocalIp?.Count > 0)
        {
            foreach (var n in this.LinkLocalIp)
                args.Add("--link-local-ip", n);
        }

        if (!this.LogDriver.IsNullOrWhiteSpace())
            args.Add("--log-driver", this.LogDriver);

        if (this.LogOpt?.Count > 0)
        {
            foreach (var kvp in this.LogOpt)
                args.Add("--log-opt", kvp);
        }

        if (!this.MacAddress.IsNullOrWhiteSpace())
            args.Add("--mac-address", this.MacAddress);

        if (!this.Memory.IsNullOrWhiteSpace())
            args.Add("--memory", this.Memory);

        if (!this.MemoryReservation.IsNullOrWhiteSpace())
            args.Add("--memory-reservation", this.MemoryReservation);

        if (!this.MemorySwap.IsNullOrWhiteSpace())
            args.Add("--memory-swap", this.MemorySwap);

        if (this.MemorySwappiness.HasValue)
            args.Add("--memory-swappiness", this.MemorySwappiness.Value.ToString());

        if (!this.Mount.IsNullOrWhiteSpace())
            args.Add("--mount", this.Mount);

        if (!this.Name.IsNullOrWhiteSpace())
            args.Add("--name", this.Name);

        if (!this.Network.IsNullOrWhiteSpace())
            args.Add("--network", this.Network);

        if (this.NetworkAlias?.Count > 0)
        {
            foreach (var n in this.NetworkAlias)
                args.Add("--network-alias", n);
        }

        if (this.NoHealthcheck)
            args.Add("--no-healthcheck");

        if (this.OomKillDisable)
            args.Add("--oom-kill-disable");

        if (this.OomScoreAdj.HasValue)
            args.Add("--oom-score-adj", this.OomScoreAdj.Value.ToString());

        if (!this.Pid.IsNullOrWhiteSpace())
            args.Add("--pid", this.Pid);

        if (this.PidsLimit.HasValue)
            args.Add("--pids-limit", this.PidsLimit.Value.ToString());

        if (!this.Platform.IsNullOrWhiteSpace())
            args.Add("--platform", this.Platform);

        if (this.Privileged)
            args.Add("--privileged");

        if (this.Publish?.Count > 0)
        {
            foreach (var n in this.Publish)
                args.Add("--publish", n);
        }

        if (this.PublishAll?.Count > 0)
        {
            foreach (var n in this.PublishAll)
                args.Add("--publish-all", n);
        }

        if (!this.Pull.IsNullOrWhiteSpace())
            args.Add("--pull", this.Pull);

        if (this.Quiet)
            args.Add("--quiet");

        if (this.ReadOnly)
            args.Add("--read-only");

        if (!this.Restart.IsNullOrWhiteSpace())
            args.Add("--restart", this.Restart);

        if (this.Rm)
            args.Add("--rm");


        if (this.SecurityOpt?.Count > 0)
        {
            foreach (var n in this.SecurityOpt)
                args.Add("--security-opt", n);
        }

        if (!this.ShmSize.IsNullOrWhiteSpace())
            args.Add("--shm-size", this.ShmSize);

        if (this.SigProxy)
            args.Add("--sig-proxy");

        if (!this.StopSignal.IsNullOrWhiteSpace())
            args.Add("--stop-signal", this.StopSignal);

        if (this.StopTimeout.HasValue)
            args.Add("--stop-timeout", this.StopTimeout.Value.ToString());

        if (this.StorageOpt?.Count > 0)
        {
            foreach (var kvp in this.StorageOpt)
                args.Add("--storage-opt", kvp);
        }

        if (this.Sysctl?.Count > 0)
        {
            foreach (var kvp in this.Sysctl)
                args.Add("--sysctl", $"{kvp.Key}='{kvp.Value}'");
        }

        if (this.Tmpfs?.Count > 0)
        {
            foreach (var n in this.Tmpfs)
                args.Add("--tmpfs", n);
        }

        if (this.Tty)
            args.Add("--tty");

        if (!this.Ulimit.IsNullOrWhiteSpace())
            args.Add("--ulimit", this.Ulimit);

        if (!this.User.IsNullOrWhiteSpace())
            args.Add("--user", this.User);

        if (!this.Userns.IsNullOrWhiteSpace())
            args.Add("--userns", this.Userns);

        if (this.Uts?.Count > 0)
        {
            foreach (var n in this.Uts)
                args.Add("--uts", n);
        }

        if (this.Volume?.Count > 0)
        {
            foreach (var n in this.Volume)
                args.Add("--volume", n);
        }

        if (!this.VolumeDriver.IsNullOrWhiteSpace())
            args.Add("--volume-driver", this.VolumeDriver);

        if (this.VolumesFrom?.Count > 0)
        {
            foreach (var n in this.VolumesFrom)
                args.Add("--volumes-from", n);
        }

        if (!this.Workdir.IsNullOrWhiteSpace())
            args.Add("--workdir", this.Workdir);

        args.Add(this.Command);
        args.AddRange(this.Args);

        return args;
    }
}