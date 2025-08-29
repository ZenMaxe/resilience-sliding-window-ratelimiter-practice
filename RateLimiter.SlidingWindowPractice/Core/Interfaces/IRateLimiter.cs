namespace RateLimiter.SlidingWindowPractice.Core.Interfaces;

public interface IRateLimiter
{
    Task<bool> IsAllowedAsync(CancellationToken cancellationToken = default);
}