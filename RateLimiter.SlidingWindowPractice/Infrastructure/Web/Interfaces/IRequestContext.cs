namespace RateLimiter.SlidingWindowPractice.Infrastructure.Web.Interfaces;

public interface IRequestContext
{
    string IpAddress { get; }

    string CorrelationId { get; }
}