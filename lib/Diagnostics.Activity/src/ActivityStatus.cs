using System.Diagnostics;

namespace GnomeStack.Diagnostics;

/// <summary>
/// The activity status code for otel.
/// </summary>
public readonly struct ActivityStatus : IEquatable<ActivityStatus>
{
    public ActivityStatus(ActivityStatusCode code, string? description = null)
    {
        this.Code = code;
        this.Description = description;
    }

    public static ActivityStatus Ok { get; } = new(ActivityStatusCode.Ok);

    public static ActivityStatus Error { get; } = new(ActivityStatusCode.Error);

    public static ActivityStatus Unset { get; } = new(ActivityStatusCode.Unset);

    public ActivityStatusCode Code { get; }

    public string? Description { get; }

    public static bool operator ==(ActivityStatus left, ActivityStatus right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ActivityStatus left, ActivityStatus right)
    {
        return !left.Equals(right);
    }

    public ActivityStatus WithDescription(string? description = null)
    {
        if (this.Code != ActivityStatusCode.Error || description.IsNullOrWhiteSpace())
            return this;

        return new ActivityStatus(this.Code, description);
    }

    public bool Equals(ActivityStatus other)
    {
        return this.Code == other.Code && this.Description == other.Description;
    }

    public override bool Equals(object? obj)
    {
        return obj is ActivityStatus other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Code, this.Description);
    }

    public override string ToString()
    {
        if (this.Description.IsNullOrWhiteSpace())
            return this.Code.ToString();

        return $"{this.Code}={this.Description}";
    }
}