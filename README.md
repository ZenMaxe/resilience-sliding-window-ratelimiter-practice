# resilience-sliding-window-ratelimiter-practice

⚠️ **This is a practice project**, built specifically to explore **distributed rate limiting** with Redis, using the **Sliding Window algorithm** and resilient infrastructure patterns.

---

## 🚀 Goal

The purpose of this project is to simulate a **real-world scalable rate-limiting mechanism** in a distributed .NET backend. This is part of a larger backend engineering exercise plan focused on:

- High throughput systems
- Resilience engineering
- System design mastery

> This is not a production-ready rate limiter. It’s an educational prototype.

---

## 🧠 Key Concepts Practiced

### 🧱 **Sliding Window Algorithm**
- Granular control over requests per rolling window
- Avoids burstiness compared to fixed-window
- Lua script used inside Redis for atomic operations

### 🧰 **Redis as a Shared Store**
- Centralized state tracking per user/IP
- Ensures consistency across multiple app instances

### 🧩 **Middleware-Based Enforcement**
- A custom `RateLimitMiddleware` intercepts HTTP requests
- Clean separation from business logic

### 🛡 **Resilience via Polly**
- Redis operations are wrapped in:
  - Timeout policy
  - Retry with backoff
  - Circuit breaker
- Resilience logic is extracted into `RedisPollyPolicy.cs`

---

## 📁 Folder Structure Overview

```
RateLimiter.SlidingWindowPractice
├── Core
│ ├── Interfaces/IRateLimiter.cs
│ └── Services/SlidingWindowRateLimiter.cs
│
├── Infrastructure
│ ├── Helpers/CacheKeyHelper.cs
│ ├── Storage
│ │ ├── Constants/CacheKeyConstants.cs
│ │ └── Redis
│ │ ├── RedisConnectionFactory.cs
│ │ ├── RedisRateLimitCacheStore.cs
│ │ ├── RedisScriptLoader.cs
│ │ ├── RedisScriptRegistry.cs
│ │ ├── Resilience/RedisPollyPolicy.cs
│ │ ├── Scripts/sliding_window.lua
│ │ ├── Interfaces/
│ │ └── Constants/
│ └── Web
│ ├── Middlewares/RateLimitMiddleware.cs
│ ├── Models/RequestContext.cs
│ ├── Options/ConnectionStringsOption.cs
│ └── Interfaces/IRequestContext.cs
│
├── Program.cs
├── Dockerfile
├── appsettings.json
```

## ⚙️ How It Works

1. Every incoming HTTP request passes through the `RateLimitMiddleware`.
2. The middleware uses `IRateLimiter` to decide whether to allow or block the request.
3. The actual limiting logic is implemented via `SlidingWindowRateLimiter`:
   - Generates Redis keys using `CacheKeyHelper`
   - Executes a Lua script (`sliding_window.lua`) in Redis to ensure atomicity
4. Redis interactions are made resilient with Polly (timeouts, retries, circuit breakers).
5. If rate limit is exceeded → `429 Too Many Requests`.


