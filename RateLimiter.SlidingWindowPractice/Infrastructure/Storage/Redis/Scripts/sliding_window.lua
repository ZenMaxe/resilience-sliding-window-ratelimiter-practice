-- Sliding Window Rate Limiter
-- KEYS[1]    = Redis ZSET key (e.g., "ratelimit:user:123")
-- ARGV[1]    = current timestamp (in seconds)
-- ARGV[2]    = window size
-- ARGV[3]    = limit
-- ARGV[4]    = unique request ID

redis.call('ZREMRANGEBYSCORE', @key, 0, tonumber(@now) - tonumber(@window))

local count = redis.call('ZCARD', @key)

if tonumber(count) >= tonumber(@limit) then
    return 0
end

redis.call('ZADD', @key, @now, @uuid)

redis.call('EXPIRE', @key, @window)
return 1


--redis.call('ZREMRANGEBYSCORE', KEYS[1], 0, ARGV[1] - ARGV[2])  -- Remove Outdated.
--
--
--local count = redis.call('ZCARD', KEYS[1]) -- Count
--
--
--if tonumber(count) >= tonumber(ARGV[3]) then
--    return 0 -- Denied: Too many requests
--end
--
--redis.call('ZADD', KEYS[1], ARGV[1], ARGV[4]) -- Add to ZSET
--
--
--redis.call('EXPIRE', KEYS[1], ARGV[2]) -- Set Expiration
--
--
--return 1