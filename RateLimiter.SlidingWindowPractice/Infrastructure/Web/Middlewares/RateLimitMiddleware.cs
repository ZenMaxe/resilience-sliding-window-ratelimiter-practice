using RateLimiter.SlidingWindowPractice.Core.Interfaces;

namespace RateLimiter.SlidingWindowPractice.Infrastructure.Web.Middlewares;

public sealed class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitMiddleware> _logger;

    public RateLimitMiddleware(RequestDelegate next, ILogger<RateLimitMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context,
        IRateLimiter rateLimiter)
    {
        if (!await rateLimiter.IsAllowedAsync(context.RequestAborted))
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Too Many Requests");
            return;
        }

        await _next(context);
    }
}