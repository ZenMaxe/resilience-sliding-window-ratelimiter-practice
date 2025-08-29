using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Constants;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Helpers;

public static class CacheKeyHelper
{
    public static string CreateCacheKey(string prefix, string key)
    {
        return $"{CacheKeyConstants.RateLimitPrefix}:{key}";
    }
}