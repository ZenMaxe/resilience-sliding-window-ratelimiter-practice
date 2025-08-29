using RateLimiter.SlidingWindowPractice.Core.Interfaces;
using RateLimiter.SlidingWindowPractice.Infrastructure.Helpers;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Constants;
using RateLimiter.SlidingWindowPractice.Infrastructure.Storage.Redis.Interfaces;
using RateLimiter.SlidingWindowPractice.Infrastructure.Web.Interfaces;
using RateLimiter.SlidingWindowPractice.Infrastructure.Web.Models;

namespace RateLimiter.SlidingWindowPractice.Core.Services;

public sealed class SlidingWindowRateLimiter : IRateLimiter
{
    private readonly IRateLimitCacheStore _rateLimitCacheStore;
    private readonly IRequestContext _requestContext;

    public SlidingWindowRateLimiter(IRateLimitCacheStore rateLimitCacheStore,
        IRequestContext requestContext)
    {
        _rateLimitCacheStore = rateLimitCacheStore;
        _requestContext = requestContext;
    }

    public async Task<bool> IsAllowedAsync(CancellationToken cancellationToken = default)
    {

        var cacheKey = CacheKeyHelper.CreateCacheKey(CacheKeyConstants.RateLimitPrefix, _requestContext.IpAddress);

        // We can extract limit and window size to make that dynamic.
        var isAllowed = await _rateLimitCacheStore
            .IsAllowedAsync(cacheKey, _requestContext.CorrelationId,
                15, TimeSpan.FromSeconds(15), cancellationToken);

        return isAllowed;
    }
}