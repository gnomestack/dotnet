using System;
using System.Linq;

namespace GnomeStack.Library;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public sealed class IgnoreAttribute : Attribute
{
    public IgnoreAttribute(string? targets, string? reason = null)
    {
        this.Targets = targets;
        this.Reason = reason;
    }

    /// <summary>
    /// Gets or sets the reason for the ignore attribute.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the targets associated with the ignore attribute
    /// so that implementations can determine to accept the ignore or not.
    /// </summary>
    public string? Targets { get; set; }
}