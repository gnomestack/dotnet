using System.ComponentModel;
using System.Diagnostics;

using Polly;
using Polly.Retry;

namespace GnomeStack.Data;

public static class SqlRetryPolicy
{
    private static Lazy<ResiliencePipeline> s_policy = new(() => Create());

    [CLSCompliant(false)]
    public static ResiliencePipeline Default
    {
        get => s_policy.Value;
        set
        {
            if (s_policy.IsValueCreated)
                throw new InvalidOperationException("Policy is already created and cannot be changed");

            s_policy = new Lazy<ResiliencePipeline>(() => value);
        }
    }

    public static ResiliencePipeline Create(
        int maxTries = 10,
        TimeSpan? delay = null,
        Func<PredicateBuilder, PredicateBuilder>? configure = null,
        Func<OnRetryArguments<object>, ValueTask>? onRetry = null)
    {
        var pb = new PredicateBuilder();
        configure?.Invoke(pb);
        delay ??= new TimeSpan(0, 0, 0, 10);
        onRetry ??= (data) =>
        {
            Debug.WriteLine($"Retry {data.AttemptNumber}. Exception: {data.Outcome.Exception}");
#if NETLEGACY
            return default;
#else
            return ValueTask.CompletedTask;
#endif
        };

        var b = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
            {
                MaxRetryAttempts = maxTries,
                Delay = delay.Value,
                ShouldHandle = pb.Handle<TimeoutException>()
                    .Handle<Win32Exception>(o =>
                    {
                        switch (o.NativeErrorCode)
                        {
                            // Timeout expired
                            case 0x102:
                            // Semaphore timeout expired
                            case 0x121:
                                return true;
                            default:
                                return false;
                        }
                    }),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = onRetry,
            });

        return b.Build();
    }
}