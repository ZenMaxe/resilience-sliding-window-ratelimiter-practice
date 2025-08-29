namespace RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Interfaces;

public interface IRateLimitCacheStore
{
    Task<bool> IsAllowedAsync(string key, string randomId, int limit, TimeSpan window, CancellationToken cancellationToken);
}