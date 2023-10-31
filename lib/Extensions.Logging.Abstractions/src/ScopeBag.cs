using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GnomeStack.Extensions.Logging;

/// <summary>
/// Provides a bag of properties that can be used to enrich log messages that default to
/// using open telemetry's naming conventions.
/// </summary>
public sealed class ScopeBag
{
    private readonly Dictionary<string, object?> bag = new(StringComparer.OrdinalIgnoreCase);

    public object? this[string key]
    {
        get
        {
            if (this.bag.TryGetValue(key, out var value))
                return value;

            return null;
        }
        set => this.bag[key] = value;
    }

    public void Add(string name, object value)
    {
        this.bag[name] = value;
    }

    public void Remove(string name)
    {
        this.bag.Remove(name);
    }

    public void Remove(params string[] names)
    {
        foreach (var name in names)
        {
            this.bag.Remove(name);
        }
    }

    public ScopeBag Set(string name, object value)
    {
        this.bag[name] = value;
        return this;
    }

    public ScopeBag StartedAt()
    {
        this.bag["startedAt"] = DateTime.UtcNow;
        return this;
    }

    public ScopeBag EndedAt()
    {
        this.bag["endedAt"] = DateTime.UtcNow;
        return this;
    }

    public ScopeBag WithOs(OSPlatform platform, string? version = null, string? name = null)
    {
        this.bag["os.type"] = platform.ToString();
        if (version is not null)
            this.bag["os.version"] = version;

        if (name is not null)
            this.bag["os.name"] = name;
        return this;
    }

    public ScopeBag WithUserId<T>(T id)
    {
        this.bag["enduser.id"] = id;
        return this;
    }

    public ScopeBag WithThread(Thread thread)
    {
        this.bag["thread.id"] = thread.ManagedThreadId;
        this.bag["thread.name"] = thread.Name;
        return this;
    }

    public ScopeBag WithException(Exception exception, bool escaped = false)
    {
        this.bag["exception.type"] = exception.GetType().FullName;
        this.bag["exception.message"] = exception.Message;
        this.bag["exception.stacktrace"] = exception.StackTrace;
        this.bag["exception.escaped"] = escaped;
        return this;
    }

    public ScopeBag WithHttpResponse(HttpResponseMessage response, string method = "get")
    {
        this.bag["http.status_code"] = (int)response.StatusCode;
        this.bag["http.status"] = response.StatusCode.ToString();

        var url = response.RequestMessage?.RequestUri?.ToString() ??
            response.Headers.Location?.ToString();

        if (url is not null)
            this.bag["url.full"] = url;

        var responseSize = response.Content?.Headers.ContentLength;
        if (responseSize.HasValue)
            this.bag["http.response.body.size"] = responseSize.Value;

        var requestSize = response.RequestMessage?.Content?.Headers.ContentLength;
        if (requestSize.HasValue)
            this.bag["http.request.body.size"] = requestSize.Value;

        if (response.RequestMessage?.Headers.UserAgent != null)
            this.bag["http.user_agent"] = response.RequestMessage.Headers.UserAgent.ToString();

        var m = response.RequestMessage?.Method.ToString() ?? method;
        this.bag["http.method"] = m;

        return this;
    }

    public ScopeBag WithStopwatch(Stopwatch stopwatch)
    {
        this.bag["duration"] = stopwatch.ElapsedMilliseconds;
        return this;
    }

    public ScopeBag WithStopwatch(OperationStopwatch stopwatch)
    {
        this.bag["timespan"] = stopwatch.StartedAt;
        this.bag["duration"] = stopwatch.ElapsedMilliseconds;
        return this;
    }

    public ScopeBag WithDictionary(IDictionary<string, object?> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            this.bag[kvp.Key] = kvp.Value;
        }

        return this;
    }

    public Dictionary<string, object?> ToDictionary()
    {
        return this.bag.ToDictionary(x => x.Key, x => x.Value);
    }

    public Dictionary<string, string?> ToStringDictionary()
    {
        // useful for application insights.
        return this.bag.ToDictionary(x => x.Key, x => x.Value?.ToString());
    }
}