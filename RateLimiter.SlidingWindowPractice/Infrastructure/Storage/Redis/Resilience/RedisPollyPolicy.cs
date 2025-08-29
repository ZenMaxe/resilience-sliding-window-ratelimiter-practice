using Polly;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Resilience;

public static class RedisPollyPolicy
{
    private const string PolicyKey = "RedisPolicy";


    public static IAsyncPolicy<bool> CreateRedisPolicy(
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(PolicyKey);
        
        
        var retry = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    logger.LogWarning(exception, "[Retry-{Key}] Attempt {Attempt} failed after {Delay}ms | CorrelationId: {CorrelationId}",
                        PolicyKey, attempt, timeSpan.TotalMilliseconds, context.CorrelationId);
                });

        var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(0.400)); // Timeout after 400 ms


        var circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (ex, breakTime) =>
                {
                    logger.LogError(ex, "[CircuitBreaker-{Key}] Circuit broken for {BreakTime}s due to: {Message}",
                        PolicyKey, breakTime.TotalSeconds, ex.Message);
                },
                onReset: () =>
                {
                    logger.LogInformation("[CircuitBreaker-{Key}] Circuit reset", PolicyKey);
                },
                onHalfOpen: () => {});


        var fallback = Policy<bool>
            .Handle<Exception>()
            .FallbackAsync<bool>(
                fallbackValue: true,
                onFallbackAsync: (ex, context) =>
                {
                    logger.LogWarning(ex.Exception,
                        "[Fallback-{Key}] Falling back to allowed=true | CorrelationId: {CorrelationId}",
                        PolicyKey, context.CorrelationId);
                    return Task.CompletedTask;
                });


        var policy = fallback.WrapAsync(
            Policy.WrapAsync(retry, timeout, circuitBreaker));

        return policy;
    }
}