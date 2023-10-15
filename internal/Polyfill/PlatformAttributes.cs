#if NETLEGACY

// ReSharper disable once CheckNamespace
namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]

// ReSharper disable once InconsistentNaming
#pragma warning disable SA1649
internal abstract class OSPlatformAttribute : Attribute
#pragma warning restore SA1649
{
    protected OSPlatformAttribute(string platformName)
    {
        this.PlatformName = platformName;
    }

    public string PlatformName { get; }
}

[System.AttributeUsage(
    AttributeTargets.Assembly |
    AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Enum |
    AttributeTargets.Event |
    AttributeTargets.Field |
    AttributeTargets.Interface |
    AttributeTargets.Method |
    AttributeTargets.Module |
    AttributeTargets.Property |
    AttributeTargets.Struct,
    AllowMultiple = true,
    Inherited = false)]

// ReSharper disable once InconsistentNaming
internal sealed class SupportedOSPlatformAttribute : OSPlatformAttribute
{
    public SupportedOSPlatformAttribute(string platformName)
        : base(platformName)
    {
    }
}

[System.AttributeUsage(
    AttributeTargets.Assembly |
    AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Enum |
    AttributeTargets.Event |
    AttributeTargets.Field |
    AttributeTargets.Interface |
    AttributeTargets.Method |
    AttributeTargets.Module |
    AttributeTargets.Property |
    AttributeTargets.Struct,
    AllowMultiple = true,
    Inherited = false)]

// ReSharper disable once InconsistentNaming
internal sealed class UnsupportedOSPlatformAttribute : OSPlatformAttribute
{
    public UnsupportedOSPlatformAttribute(string platformName)
        : base(platformName)
    {
    }

    public UnsupportedOSPlatformAttribute(string platformName, string message)
        : base(platformName)
    {
        this.Message = message;
    }

    public string? Message { get; }
}
#endif